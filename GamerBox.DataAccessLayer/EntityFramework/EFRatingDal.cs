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
    public class EFRatingDal : GenericRepository<Rating>,IRatingDal
    {
    
    }
}
