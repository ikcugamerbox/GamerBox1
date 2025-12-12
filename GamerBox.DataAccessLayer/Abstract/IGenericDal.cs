using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IGenericDal<T> where T : class
    {
        // CRUD İşlemleri - Task dönerler (void yerine)
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        // Veri Çekme İşlemleri
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
    }
}