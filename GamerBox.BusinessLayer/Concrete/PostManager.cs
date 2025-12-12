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

        public async Task AddAsyncB(Post entity)
        {
            ValidatePostContent(entity.Content);
            await _postDal.AddAsync(entity);
        }

        public async Task UpdateAsyncB(Post entity)
        {
            ValidatePostContent(entity.Content);
            await _postDal.UpdateAsync(entity);
        }

        public async Task DeleteAsyncB(Post entity)
        {
            await _postDal.DeleteAsync(entity);
        }

        public async Task<Post> GetByIdAsyncB(int id)
        {
            return await _postDal.GetByIdAsync(id);
        }

        public async Task<List<Post>> GetAllAsyncB()
        {
            return await _postDal.GetAllAsync();
        }

        // --- IPostService Özel Metotları ---

        public async Task<Post> CreatePostAsyncB(int userId, int? gameId, string content)
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

            await _postDal.AddAsync(post);
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

        public async Task<List<Post>> GetByUserIdAsyncB(int userId)
        {
            return await _postDal.GetPostsByUserAsync(userId); // DAL'da hazır metot varken onu kullanmak daha performanslıdır
        }

        public async Task<List<Post>> GetByGameIdAsyncB(int gameId)
        {
            // DAL'da GetPostsByGameId yoksa GetAll ile filtreleriz, varsa onu kullanırız.
            // Şimdilik GetAll üzerinden filtreleyelim:
            return await _postDal.GetPostsByGameIdAsync(gameId);
        }

        // --- Yardımcı Metotlar (Private) ---

        private void ValidatePostContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Post content cannot be empty.");

            if (content.Length > MaxLength)
                throw new InvalidOperationException($"Post cannot exceed {MaxLength} characters.");
        }

        public async Task<List<Post>> GetRecentPostsAsyncB(int count)
        {
            return await _postDal.GetRecentPostsAsync(count);
        }
    }
}