using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    public interface IGenericService<T>
    {
        void TInsert(T entity);
        void TUpdate(T entity);
        void TDelete(T entity);
        List<T> TGetAll();
        T TGetById(int id);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetById(int id);
        List<T> GetAll();
    }
}
