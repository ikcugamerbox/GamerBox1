using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        protected readonly GamerBoxContext _context; // İsimlendirmeyi standart hale getirdim (_context)
        private readonly DbSet<T> _object;


        public GenericRepository(GamerBoxContext context)
        {
            _context = context;
            _object = _context.Set<T>();
        }
        public void Delete(T entity)
        {
            var deletedEntity = _context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public List<T> GetAll()
        {
            return _object.ToList();
        }

        public T GetById(int id) => _object.Find(id);

        public void Insert(T entity)
        {
            var addedEntity = _context.Entry(entity); 
            addedEntity.State = EntityState.Added;
            _context.SaveChanges();

        }

        public void Update(T entity)
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
