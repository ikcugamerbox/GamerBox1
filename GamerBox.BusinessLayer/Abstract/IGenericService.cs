using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {

        Task AddAsyncB(T entity);
        Task UpdateAsyncB(T entity);
        Task DeleteAsyncB(T entity);
        Task<T> GetByIdAsyncB(int id);
        Task<List<T>> GetAllAsyncB();
    }

}
