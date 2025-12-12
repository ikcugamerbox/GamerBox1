using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore; // ToListAsync vb. için gerekli
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamerBox.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        protected readonly GamerBoxContext _context;
        private readonly DbSet<T> _object;

        public GenericRepository(GamerBoxContext context)
        {
            _context = context;
            _object = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _object.AddAsync(entity);
            await _context.SaveChangesAsync(); // Değişiklikleri asenkron kaydet
        }

        public async Task UpdateAsync(T entity)
        {
            _object.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _object.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            // Veritabanını bloklamadan listeyi çeker
            return await _object.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _object.FindAsync(id);
        }
    }
}