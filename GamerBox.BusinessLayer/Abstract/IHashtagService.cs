using GamerBox.EntitiesLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IHashtagService : IGenericService<Hashtag>
    {
        Task<List<Hashtag>> GetOrCreateHashtagsAsyncB(List<string> tags);
    }
}