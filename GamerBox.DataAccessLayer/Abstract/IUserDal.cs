using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IUserDal : IGenericDal<User>
    {
        User GetUserByEmail(string email); 
        List<User> GetFollowers(int userId); 
        List<User> GetFollowing(int userId); 
        List<string> GetUserPreferredCategories(int userId); 
    }
}
