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
    public class RatingManager : IRatingService
    {
        private readonly IRatingDal _ratingDal;

        public RatingManager(IRatingDal rating)
        {
            _ratingDal = rating;
        }

        public void TDelete(Rating entity)
        {
            throw new NotImplementedException();
        }

        public List<Rating> TGetAll()
        {
            throw new NotImplementedException();
        }

        public Rating TGetById(int id)
        {
            throw new NotImplementedException();
        }

        public void TInsert(Rating entity)
        {
            throw new NotImplementedException();
        }

        public void TUpdate(Rating entity)
        {
            throw new NotImplementedException();
        }
    }
}
