using System;
using System.Collections.Generic;

namespace GamerBox.EntitiesLayer.Concrete
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
