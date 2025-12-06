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
        public string FavoriteGenres { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Rating> Ratings { get; set; }

        public ICollection<User> Followers { get; set; }
        public ICollection<User> Following { get; set; }
    }
}
