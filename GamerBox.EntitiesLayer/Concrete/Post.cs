using System;

namespace GamerBox.EntitiesLayer.Concrete
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }

        public int? GameId { get; set; }

        public Game? Game { get; set; }
        public List<string> Hashtags { get; set; }

        public DateTime CreatedAtUtc { get; set; }





    }
}