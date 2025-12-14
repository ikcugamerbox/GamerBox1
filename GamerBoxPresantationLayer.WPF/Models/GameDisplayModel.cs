using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBoxPresantationLayer.WPF.Models
{
    public class GameDisplayModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Rating { get; set; }
        public string Year { get; set; }
        public string Poster { get; set; }
        public string PriceTag { get; set; }
    }
}
