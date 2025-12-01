using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.EntitiesLayer.Concrete
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string? Bio { get; set; }
        public string? Theme { get; set; }//light or dark
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
    }
}
