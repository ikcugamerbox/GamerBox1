using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Context;
using GamerBox.DataAccessLayer.Repositories;
using GamerBox.EntitiesLayer.Concrete;
using System.Linq;

namespace GamerBox.DataAccessLayer.EntityFramework
{
    public class EFRatingDal : GenericRepository<Rating>, IRatingDal
    {
        

        public EFRatingDal(GamerBoxContext context) : base(context)
        {
            
        }





        public double GetAverageRatingForGame(int gameId)
        {
            var ratings = _context.Ratings
                .Where(r => r.GameId == gameId)
                .ToList();

            if (ratings.Count == 0)
                return 0.0;

            return ratings.Average(r => r.Score);
        }


        public bool HasUserRatedGame(int userId, int gameId)
        {
            return _context.Ratings
                .Any(r => r.UserId == userId && r.GameId == gameId);
        }
    }
}