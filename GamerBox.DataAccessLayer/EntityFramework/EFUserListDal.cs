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
    public class EFUserListDal : GenericRepository<UserList>, IUserListDal
    {
        public EFUserListDal(GamerBoxContext context) : base(context) { }

        public async Task<List<UserList>> GetListsByUserIdAsync(int userId)
        {
            return await _context.UserLists
                .Include(x => x.Games) // Listeyi çekerken içindeki oyunları da getir
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task AddGameToList(int listId, int gameId)
        {
            var list = await _context.UserLists.Include(x => x.Games).FirstOrDefaultAsync(x => x.Id == listId);
            var game = await _context.Games.FindAsync(gameId);

            if (list != null && game != null && !list.Games.Contains(game))
            {
                list.Games.Add(game);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoveGameFromList(int listId, int gameId)
        {
            // Listeyi çek, ama Games tablosundan SADECE silmek istediğimiz oyun ID'sine sahip olanı getir.
            // SQL tarafında "WHERE GameId = @gameId" çalışır, diğer oyunları getirmez.
            var list = await _context.UserLists
                                     .Include(x => x.Games.Where(g => g.Id == gameId))
                                     .FirstOrDefaultAsync(x => x.Id == listId);

            if (list != null)
            {
                var game = list.Games.FirstOrDefault();

                if (game != null)
                {

                    list.Games.Remove(game);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}