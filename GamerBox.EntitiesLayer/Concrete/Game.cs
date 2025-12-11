using System;
using System.Collections.Generic;

namespace GamerBox.EntitiesLayer.Concrete
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } // Name yerine Title
        public string Description { get; set; }
        public string Genre { get; set; } // Category yerine Genre
        public DateTime ReleaseDate { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }

        // Admin veya oyunu ekleyen kişi (Opsiyonel)
        public int UserId { get; set; }
        public User User { get; set; }

        public double? AverageRating { get; set; } // Hesaplanmış alan

        // İlişkiler
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}