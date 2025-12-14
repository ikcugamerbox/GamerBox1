using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GamerBox.EntitiesLayer.Concrete
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }

        // "Theme" yerine sadece bunu kullanıyoruz
        public string ThemePreference { get; set; } = "dark";

        public DateTime CreatedAtUtc { get; set; }
        public DateTime? LastLoginUtc { get; set; }

        // İlişkiler
        public ICollection<Post> Posts { get; set; }
        public ICollection<Rating> Ratings { get; set; }

        // Takip Sistemi (Many-to-Many)
        public virtual ICollection<User> Following { get; set; } = new List<User>(); // Takip ettikleri
        public virtual ICollection<User> Followers { get; set; } = new List<User>(); // Takipçileri

        // Arkadaşlık sistemi (Opsiyonel, şimdilik kapalı tutabilir veya ayrı bir tablo yapabilirsiniz, 
        // ancak EF Core karmaşasını önlemek için Follow sistemi üzerinden gidiyoruz)
        // public ICollection<User> Friends { get; set; } = new List<User>();

        // Kategoriler (Veritabanında string olarak, kod tarafında List olarak)
        public string FavoriteGenres { get; set; } = string.Empty;

        [NotMapped]
        public List<string> PreferredCategories
        {
            get => string.IsNullOrEmpty(FavoriteGenres) ? new List<string>() : FavoriteGenres.Split(',').ToList();
            set => FavoriteGenres = value != null ? string.Join(",", value) : string.Empty;
        }
    }
}