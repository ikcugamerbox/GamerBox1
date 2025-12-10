using System.Collections.Generic;

namespace GamerBox.EntitiesLayer.Concrete
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Bio { get; set; }
        public string ThemePreference { get; set; }



        public string? Theme { get; set; }//light or dark
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Rating>? Ratings { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime? LastLoginUtc { get; set; }

        public ICollection<User> Following { get; set; } = new List<User>();// takip edilen kullanıcılar

        public ICollection<User> Followers { get; set; } // takipçiler
        public ICollection<User> FollowingUsers { get; set; } // takip edilen kullanıcılar

        public string FavoriteGenres { get; set; } = string.Empty; // favori türler

        public List<string> PreferredCategories { get; set; }

        public ICollection<User> Friends { get; set; } = new List<User>();







    }
}