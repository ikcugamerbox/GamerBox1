using GamerBox.EntitiesLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IHashtagDal : IGenericDal<Hashtag>
    {
        Task<List<Hashtag>> GetByTagsAsync(List<string> tags);
    }
}