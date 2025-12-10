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
using System.Text.RegularExpressions;

namespace GamerBox.BusinessLayer.Concrete
{
    public class PostManager : IPostService
    {
        private readonly IPostDal _postDal; //post verilerini databasede yöneten DAL class ı.
        private const int MaxLength = 280;

        public PostManager(IPostDal postDal) //dışarıdan bir IPostDal nesnesi alır. dependency Injection.
        {
            _postDal = postDal;
        }

        // BASIC CRUD OPERATIONS (IGenericService<Post>)

        public void Add(Post entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Content))
                throw new InvalidOperationException("Post cannot be empty.");

            if (entity.Content.Length > MaxLength)
                throw new InvalidOperationException($"Post cannot exceed {MaxLength} characters.");

            _postDal.Add(entity); //uygunsa database e ekler.
        }

        public void Update(Post entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Content))
                throw new InvalidOperationException("Post cannot be empty.");

            if (entity.Content.Length > MaxLength)
                throw new InvalidOperationException($"Post cannot exceed {MaxLength} characters.");

            _postDal.Update(entity); // uygunsa database de post güncellenir.
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
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Post cannot be empty .");
            if (content.Length > MaxLength)
                throw new InvalidOperationException($"Post cannot exceed {MaxLength} characters.");

        public void TInsert(Post entity) => Add(entity); //TInsert çağrıldığında Add çalışır.
        public void TDelete(Post entity) => Delete(entity);
        public Post TGetById(int id) => GetById(id);
        public List<Post> TGetAll() => GetAll();

        // CUSTOM BUSİNESS METHODS 
        public Post CreatePost(int userId, int? gameId, string content) //Yeni bir post oluşturur. Hashtag analizini otomatik yapar. gameId verilebilir, verilmezse null olur.

        {
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Post cannot be empty.");

            if (content.Length > MaxLength)
                throw new InvalidOperationException($"Post cannot exceed {MaxLength} characters.");

            var hashtags = ExtractHashtags(content);//hastagleri çıkar.

            var post = new Post //yeni post nesnesi oluştur.
            {
                UserId = userId,
                GameId = gameId,
                Content = content.Trim(),
                Hashtags = hashtags,
                CreatedAtUtc = DateTime.UtcNow
            };

            _postDal.Add(post); //database e ekle.
            return post;
        }

        public List<string> ExtractHashtags(string content)  //İçerikten tüm hashtagleri çıkarır.
        {
            var matches = Regex.Matches(content, @"#([A-Za-z0-9_]+)"); // hastagleri bulmak için regex kullanılır.

            return matches  // sadece kelimeleri alır, küçük harfe çevirir, tekrarları kaldırır ve listeler.
                .Select(m => m.Groups[1].Value.ToLowerInvariant())
                .Distinct()
                .ToList();
        }

        public List<Post> GetByUserId(int userId)  // Belirli kullanıcıya ait postları getirir.
        {
            return _postDal.GetAll()
                           .Where(p => p.UserId == userId)
                           .ToList();
        }

        public List<Post> GetByGameId(int gameId)    // Belirli oyuna ait tüm postları getirir.
        {
            return _postDal.GetAll()
                           .Where(p => p.GameId == gameId)
                           .ToList();
        }

        public void Delete(Post entity) => _postDal.Delete(entity);
        public Post GetById(int id) => _postDal.GetById(id);
        public List<Post> GetAll() => _postDal.GetAll();
    }
}