using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.EntitiesLayer.Concrete
{
    public class Hashtag
    {
        public int Id { get; set; }
        public string Tag { get; set; } // Örn: "rpg", "fps"

        //Bir hashtag birçok postta olabilir
        public ICollection<Post> Posts { get; set; }
    }
}