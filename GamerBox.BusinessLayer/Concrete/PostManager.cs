using GamerBox.BusinessLayer.Abstract;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public void TDelete(Post entity)
        public Post CreatePost(int userId, int? gameId, string content)
        {
            _postDal.Delete(entity);
        }
        public List<Post> TGetAll()
        {
            return _postDal.GetAll();
        }
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Post cannot be empty .");
            if (content.Length > MaxLength)
                throw new InvalidOperationException($"Post cannot exceed {MaxLength} characters.");

        public Post TGetById(int id)
        {
            return _postDal.GetById(id);
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

        public void TInsert(Post entity)
        public List<string> ExtractHashtags(string content)
        {
            _postDal.Insert(entity);
            var matches = Regex.Matches(content, @"#([A-Za-z0-9_]+)");
            return matches.Select(m => m.Groups[1].Value.ToLowerInvariant()).Distinct().ToList();
        }

        public void TUpdate(Post entity)
        public List<Post> GetByUserId(int userId) =>
            _postDal.GetAll().Where(p => p.UserId == userId).ToList();

        public List<Post> GetByGameId(int gameId) =>
            _postDal.GetAll().Where(p => p.GameId == gameId).ToList();

        // IGenericService<Post> implementasyonları
        public void Add(Post entity) => _postDal.Add(entity);

        public void Update(Post entity)
        {
            if (entity != null && entity.Id != 0 && entity.Content != "")
            {//validation
                _postDal.Update(entity);
            }
            else { }
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
