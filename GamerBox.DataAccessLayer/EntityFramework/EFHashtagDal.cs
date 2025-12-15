using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Context;
using GamerBox.DataAccessLayer.Repositories;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamerBox.DataAccessLayer.EntityFramework
{
    public class EFHashtagDal : GenericRepository<Hashtag>, IHashtagDal
    {
        public EFHashtagDal(GamerBoxContext context) : base(context) { }

        public async Task<List<Hashtag>> GetByTagsAsync(List<string> tags)
        {
            // SQL'deki "WHERE Tag IN ('rpg', 'fps')" sorgusunu yapar.
            return await _context.Hashtags
                                 .Where(h => tags.Contains(h.Tag))
                                 .ToListAsync();
        }
    }
}