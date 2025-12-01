using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Context;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal; 
        }

        public void TDelete(User entity)
        {
            _userDal.Delete(entity);
        }

        public List<User> TGetAll()
        {
            return _userDal.GetAll();
        }

        public User? TGetById(int id)
        {
            return _userDal.GetById(id);   
        }

        public void TInsert(User entity)
        {
            _userDal.Insert(entity);
        }

        public void TUpdate(User entity)
        {
           _userDal.Update(entity);
        }
    }
}
