using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GamerBoxPresantationLayer.WPF
{
    /// <summary>
    /// SignUpWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void GoSignIn_Click(object sender, RoutedEventArgs e)
        {

            this.Owner.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Owner.Opacity = 1;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
