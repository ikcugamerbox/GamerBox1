using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.EntitiesLayer.Concrete
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double? AverageRating { get; set; }

        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<Post>? Posts { get; set; }
    }
}
