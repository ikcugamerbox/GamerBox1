using GamerBox.DataAccessLayer.Context;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography; // BU KÜTÜPHANE ÖNEMLİ
using System.Text;

namespace GamerBox.DataAccessLayer.Dataseeds
{
    public static class SeedData
    {
        public static void Initialize(GamerBoxContext context)
        {
            // Veritabanı zaten doluysa işlem yapma
            if (context.Users.Any()) return;

            // --- UserManager'daki Güvenli Şifreleme Mantığının Aynısı ---
            string HashPass(string password)
            {
                using var rng = RandomNumberGenerator.Create();
                var salt = new byte[16];
                rng.GetBytes(salt);

                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
                var key = pbkdf2.GetBytes(32);

                return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
            }
            // ------------------------------------------------------------

            // 1. KULLANICILAR
            var user1 = new User
            {
                Username = "Eren",
                Email = "eren@gamerbox.com",
                PasswordHash = HashPass("123456"), // Artık doğru formatta şifrelenecek
                Bio = "Full-stack developer ve hardcore oyuncu.",
                CreatedAtUtc = DateTime.UtcNow,
                ThemePreference = "dark",
                FavoriteGenres = "RPG,FPS"
            };

            var user2 = new User
            {
                Username = "GamerGirl",
                Email = "gamer@test.com",
                PasswordHash = HashPass("123456"),
                Bio = "FPS oyunlarının kraliçesi.",
                CreatedAtUtc = DateTime.UtcNow,
                ThemePreference = "light",
                FavoriteGenres = "FPS,MOBA"
            };

            context.Users.AddRange(user1, user2);
            context.SaveChanges();

            // 2. OYUNLAR
            var games = new List<Game>
            {
                new Game
                {
                    Title = "The Witcher 3: Wild Hunt",
                    Description = "Bir canavar avcısının hikayesi.",
                    Genre = "RPG",
                    Price = 59.99m,
                    ReleaseDate = new DateTime(2015, 5, 19),
                    UserId = user1.Id,
                    ImageUrl = "/Resources/treasure-chest.png"
                },
                new Game
                {
                    Title = "Cyberpunk 2077",
                    Description = "Gelecekte geçen açık dünya aksiyon.",
                    Genre = "RPG",
                    Price = 49.99m,
                    ReleaseDate = new DateTime(2020, 12, 10),
                    UserId = user1.Id,
                    ImageUrl = "/Resources/treasure-chest.png"
                },
                new Game
                {
                    Title = "Valorant",
                    Description = "Taktiksel FPS oyunu.",
                    Genre = "FPS",
                    Price = 0,
                    ReleaseDate = new DateTime(2020, 6, 2),
                    UserId = user1.Id,
                    ImageUrl = "/Resources/treasure-chest.png"
                },
                new Game
                {
                    Title = "Elden Ring",
                    Description = "Zorlu bir açık dünya macerası.",
                    Genre = "RPG",
                    Price = 69.99m,
                    ReleaseDate = new DateTime(2022, 2, 25),
                    UserId = user1.Id,
                    ImageUrl = "/Resources/treasure-chest.png"
                }
            };

            context.Games.AddRange(games);
            context.SaveChanges();

            // 3. PUANLAR
            var ratings = new List<Rating>
            {
                new Rating { UserId = user1.Id, GameId = games[0].Id, Score = 5, RatedAt = DateTime.Now, CreatedAtUtc = DateTime.UtcNow },
                new Rating { UserId = user1.Id, GameId = games[3].Id, Score = 5, RatedAt = DateTime.Now, CreatedAtUtc = DateTime.UtcNow },
                new Rating { UserId = user2.Id, GameId = games[0].Id, Score = 5, RatedAt = DateTime.Now, CreatedAtUtc = DateTime.UtcNow },
                new Rating { UserId = user2.Id, GameId = games[2].Id, Score = 4, RatedAt = DateTime.Now, CreatedAtUtc = DateTime.UtcNow }
            };

            context.Ratings.AddRange(ratings);
            context.SaveChanges();

            // 4. GÖNDERİLER
            var posts = new List<Post>
            {
                new Post
                {
                    UserId = user1.Id,
                    GameId = games[0].Id,
                    Content = "Witcher 3 hala oynadığım en iyi oyun! #rpg #masterpiece",
                    Hashtags = new List<string> { "rpg", "masterpiece" },
                    CreatedAt = DateTime.Now.AddDays(-2),
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-2)
                },
                new Post
                {
                    UserId = user2.Id,
                    GameId = games[2].Id,
                    Content = "Valorant'ta bugün harika bir maç attık. #fps #valorant",
                    Hashtags = new List<string> { "fps", "valorant" },
                    CreatedAt = DateTime.Now.AddHours(-5),
                    CreatedAtUtc = DateTime.UtcNow.AddHours(-5)
                },
                new Post
                {
                    UserId = user1.Id,
                    GameId = games[3].Id,
                    Content = "Bu oyun gerçekten çok zor ama bir o kadar da keyifli. #souls #hardcore",
                    Hashtags = new List<string> { "souls", "hardcore" },
                    CreatedAt = DateTime.Now,
                    CreatedAtUtc = DateTime.UtcNow
                }
            };

            context.Posts.AddRange(posts);
            context.SaveChanges();
        }
    }
}