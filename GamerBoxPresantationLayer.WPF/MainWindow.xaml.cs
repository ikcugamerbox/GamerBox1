using GamerBoxPresantationLayer.WPF.Classes;
using GamerBoxPresantationLayer.WPF.Views.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;

namespace GamerBoxPresantationLayer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsLoggedIn { get; set; } = false;
        public string UserName { get; set; } = "Eren";
        public string AvatarPath { get; set; } // = "/Assets/avatar.png";
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new UCHome();
            DataContext = this;
            // this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            // this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; //if you want to see bar iin maximized
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnStbClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnHomeClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCHome();

        }

        private void btnRvsClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCReviews();
        }

        private void btnListClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCLists();

        }

        private void btnWtListClick(object sender, RoutedEventArgs e)
        { 
            MainContent.Content = new UCWatchtLists();

        }

        private void btnProfileClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCProfile();
        }

        private void brdTopRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btnFullScClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else { this.WindowState = WindowState.Normal;}
        }


        private void btnSignInCli(object sender, RoutedEventArgs e)
        {
            // 1. DI Konteynerinden SignInWindow'u talep et (Otomatik olarak IUserService içine konulur)
            var signInWindow = App.ServiceProvider.GetRequiredService<SignInWindow>();

            // 2. Sahipliği ayarla
            signInWindow.Owner = this;

            // 3. Arka planı karart
            this.Opacity = 0.4;

            // 4. Pencereyi aç
            signInWindow.ShowDialog();

            // Not: ShowDialog bittiğinde (pencere kapandığında) kod buradan devam eder.
            // Opacity düzeltmesini SignInWindow içinde yaptık ama garanti olsun diye buraya da koyabiliriz.
            this.Opacity = 1;
        }
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}