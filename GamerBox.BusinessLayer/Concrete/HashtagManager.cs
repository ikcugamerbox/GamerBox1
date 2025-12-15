using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamerBox.BusinessLayer.Concrete
{
    public class HashtagManager : IHashtagService
    {
        private readonly IHashtagDal _hashtagDal;

        public HashtagManager(IHashtagDal hashtagDal)
        {
            _hashtagDal = hashtagDal;
        }

        // Generic Metotlar (Boş bırakıyorum, GenericService'den implemente edersin)
        public async Task AddAsyncB(Hashtag entity) => await _hashtagDal.AddAsync(entity);
        public async Task UpdateAsyncB(Hashtag entity) => await _hashtagDal.UpdateAsync(entity);
        public async Task DeleteAsyncB(Hashtag entity) => await _hashtagDal.DeleteAsync(entity);
        public async Task<Hashtag> GetByIdAsyncB(int id) => await _hashtagDal.GetByIdAsync(id);
        public async Task<List<Hashtag>> GetAllAsyncB() => await _hashtagDal.GetAllAsync();

        //  ASIL 
        public async Task<List<Hashtag>> GetOrCreateHashtagsAsyncB(List<string> tags)
        {
            if (tags == null || !tags.Any()) return new List<Hashtag>();

            // 1. Veritabanında ZATEN VAR OLANLARI çek
            var existingTags = await _hashtagDal.GetByTagsAsync(tags);

            // 2. LİSTEDE OLUP VERİTABANINDA OLMAYANLARI bul (Yeni eklenecekler)
            var existingTagNames = existingTags.Select(e => e.Tag).ToList();
            var newTagNames = tags.Except(existingTagNames).ToList();

            var finalHashtagList = new List<Hashtag>(existingTags);

            // 3. Yeni olanları oluştur (ama veritabanına hemen kaydetmiyoruz, Post ile kaydedilecek)
            foreach (var tagName in newTagNames)
            {
                var newTag = new Hashtag { Tag = tagName };
                // Veritabanına eklemiyoruz, çünkü EF Core Post'u eklerken
                // ilişkili yeni nesneleri de otomatik ekler (Cascade Insert).
                finalHashtagList.Add(newTag);
            }

            return finalHashtagList;
        }
    }
}