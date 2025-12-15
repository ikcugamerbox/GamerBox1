using GamerBox.EntitiesLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IUserListDal : IGenericDal<UserList>
    {
        Task<List<UserList>> GetListsByUserIdAsync(int userId);
        Task AddGameToList(int listId, int gameId);
        Task RemoveGameFromList(int listId, int gameId);
    }
}