using GamerBox.EntitiesLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IUserListService : IGenericService<UserList>
    {
        Task<UserList> CreateListAsyncB(int userId, string listName);
        Task<List<UserList>> GetUserListsAsyncB(int userId);
        Task AddGameToListAsyncB(int listId, int gameId);
        Task RemoveGameFromListAsyncB(int listId, int gameId);
    }
}