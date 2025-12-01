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
    /// SignInWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class SignInWindow : Window
    {
        public SignInWindow()
        {
            InitializeComponent();
            

        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            
            this.Close();
            this.Owner.Opacity = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // if()
            // this.Close();
            // call auth security algo
        }

        private void SignUp_Click(object sender, MouseButtonEventArgs e)
        {
            SignUpWindow signUp = new SignUpWindow();
            signUp.Owner = this;
            this.Hide();
            signUp.Show();

            
        }
    }
}
