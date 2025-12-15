using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamerBox.BusinessLayer.Concrete
{
    public class UserListManager : IUserListService
    {
        private readonly IUserListDal _dal;

        public UserListManager(IUserListDal dal)
        {
            _dal = dal;
        }

        // Generic Metotlar
        public async Task AddAsyncB(UserList entity) => await _dal.AddAsync(entity);
        public async Task UpdateAsyncB(UserList entity) => await _dal.UpdateAsync(entity);
        public async Task DeleteAsyncB(UserList entity) => await _dal.DeleteAsync(entity);
        public async Task<UserList> GetByIdAsyncB(int id) => await _dal.GetByIdAsync(id);
        public async Task<List<UserList>> GetAllAsyncB() => await _dal.GetAllAsync();

        // Özel Metotlar
        public async Task<UserList> CreateListAsyncB(int userId, string listName)
        {
            if (string.IsNullOrWhiteSpace(listName)) throw new Exception("Liste adı boş olamaz.");

            var newList = new UserList
            {
                UserId = userId,
                Name = listName,
                CreatedAt = DateTime.Now
            };

            await _dal.AddAsync(newList);
            return newList;
        }

        public async Task<List<UserList>> GetUserListsAsyncB(int userId)
        {
            return await _dal.GetListsByUserIdAsync(userId);
        }

        public async Task AddGameToListAsyncB(int listId, int gameId)
        {
            await _dal.AddGameToList(listId, gameId);
        }
        public async Task RemoveGameFromListAsyncB(int listId, int gameId) => await _dal.RemoveGameFromList(listId, gameId);
    }
}