using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GamerBox.BusinessLayer.Concrete
{
    public class PostManager : IPostService
    {
        private readonly IPostDal _postDal;
        private const int MaxLength = 280;

        public PostManager(IPostDal postDal)
        {
            _postDal = postDal;
        }

        // --- IGenericService İmplementasyonları ---

        public void Add(Post entity)
        {
            ValidatePostContent(entity.Content);
            _postDal.Add(entity);
        }

        public void Update(Post entity)
        {
            ValidatePostContent(entity.Content);
            _postDal.Update(entity);
        }

        public void Delete(Post entity)
        {
            _postDal.Delete(entity);
        }

        public Post GetById(int id)
        {
            return _postDal.GetById(id);
        }

        public List<Post> GetAll()
        {
            return _postDal.GetAll();
        }

        // --- IPostService Özel Metotları ---

        public Post CreatePost(int userId, int? gameId, string content)
        {
            ValidatePostContent(content);

            var hashtags = ExtractHashtags(content);

            var post = new Post
            {
                UserId = userId,
                GameId = gameId,
                Content = content.Trim(),
                Hashtags = hashtags, // List<string> olarak atanıyor
                CreatedAt = DateTime.Now, // CreatedAtUtc yerine CreatedAt kullandım (Entity ile uyumlu olsun diye)
                CreatedAtUtc = DateTime.UtcNow // Entity'de ikisi de varsa ikisini de doldurabiliriz
            };

            _postDal.Add(post);
            return post;
        }

        public List<string> ExtractHashtags(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return new List<string>();

            var matches = Regex.Matches(content, @"#([A-Za-z0-9_]+)");

            return matches
                .Select(m => m.Groups[1].Value.ToLowerInvariant())
                .Distinct()
                .ToList();
        }

        public List<Post> GetByUserId(int userId)
        {
            return _postDal.GetPostsByUser(userId); // DAL'da hazır metot varken onu kullanmak daha performanslıdır
        }

        public List<Post> GetByGameId(int gameId)
        {
            // DAL'da GetPostsByGameId yoksa GetAll ile filtreleriz, varsa onu kullanırız.
            // Şimdilik GetAll üzerinden filtreleyelim:
            return _postDal.GetAll()
                           .Where(p => p.GameId == gameId)
                           .ToList();
        }

        // --- Yardımcı Metotlar (Private) ---

        private void ValidatePostContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Post content cannot be empty.");

            if (content.Length > MaxLength)
                throw new InvalidOperationException($"Post cannot exceed {MaxLength} characters.");
        }
    }
}