using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.BusinessLayer.Concrete
{
    public class PostManager : IPostService
    {
        private readonly IPostDal _postDal;

        public PostManager(IPostDal postDal)
        {
            _postDal = postDal;
        }

        public void TDelete(Post entity)
        {
            _postDal.Delete(entity);
        }
        public List<Post> TGetAll()
        {
            return _postDal.GetAll();
        }

        public Post TGetById(int id)
        {
            return _postDal.GetById(id);
        }

        public void TInsert(Post entity)
        {
            _postDal.Insert(entity);
        }

        public void TUpdate(Post entity)
        {
            if (entity != null && entity.Id != 0 && entity.Content != "")
            {//validation
                _postDal.Update(entity);
            }
            else { }
        }
    }
}
