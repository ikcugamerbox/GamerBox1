using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Repositories;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.DataAccessLayer.EntityFramework
{
    public class EfUserDal : GenericRepository<User>, IUserDal
    {
        public List<User> GetFollowers(int userId)
        {
            throw new NotImplementedException();
        }

        public List<User> GetFollowing(int userId)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public List<string> GetUserPreferredCategories(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
