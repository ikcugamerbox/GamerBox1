using System;

namespace GamerBox.EntitiesLayer.Concrete
{
    public class Rating
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime RatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}