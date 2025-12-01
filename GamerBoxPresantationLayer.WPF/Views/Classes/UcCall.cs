using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.Classes
{
    public class UcCall
    {
        public void AddUc(Grid grd, UserControl usc)
        {
            if (grd.Children.Count > 0)
            {
                grd.Children.Clear();
                grd.Children.Add(usc);
            }else {grd.Children.Add(usc); }
        }
    }
}
