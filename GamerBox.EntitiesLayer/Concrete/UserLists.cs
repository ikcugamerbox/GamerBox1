using System;
using System.Collections.Generic;

namespace GamerBox.EntitiesLayer.Concrete
{
    public class UserList
    {
        public int Id { get; set; }
        public string Name { get; set; } // Örn: "Favoriler", "RPG Koleksiyonum"
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Hangi kullanıcıya ait?
        public int UserId { get; set; }
        public User User { get; set; }

        // Listede hangi oyunlar var? (Many-to-Many ilişkisi)
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}