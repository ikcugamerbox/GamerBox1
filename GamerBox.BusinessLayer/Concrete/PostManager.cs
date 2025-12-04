using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;

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

        public Post CreatePost(int userId, int? gameId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Post cannot be empty .");
            if (content.Length > MaxLength)
                throw new InvalidOperationException($"Post cannot exceed {MaxLength} characters.");

            var hashtags = ExtractHashtags(content);

            var post = new Post
            {
                UserId = userId,
                GameId = gameId,
                Content = content.Trim(),
                Hashtags = hashtags,
                CreatedAtUtc = DateTime.UtcNow
            };

            _postDal.Add(post);
            return post;
        }

        public List<string> ExtractHashtags(string content)
        {
            var matches = Regex.Matches(content, @"#([A-Za-z0-9_]+)");
            return matches.Select(m => m.Groups[1].Value.ToLowerInvariant()).Distinct().ToList();
        }

        public List<Post> GetByUserId(int userId) =>
            _postDal.GetAll().Where(p => p.UserId == userId).ToList();

        public List<Post> GetByGameId(int gameId) =>
            _postDal.GetAll().Where(p => p.GameId == gameId).ToList();

        // IGenericService<Post> implementasyonları
        public void Add(Post entity) => _postDal.Add(entity);

        public void Update(Post entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Content))
                throw new InvalidOperationException("Post cannot be empty.");
            if (entity.Content.Length > MaxLength)
                throw new InvalidOperationException($"Post cannot exceed {MaxLength} characters.");
            _postDal.Update(entity);
        }

        public void Delete(Post entity) => _postDal.Delete(entity);
        public Post GetById(int id) => _postDal.GetById(id);
        public List<Post> GetAll() => _postDal.GetAll();
    }
}