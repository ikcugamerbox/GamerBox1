using GamerBox.EntitiesLayer.Concrete;
using GamerBoxPresantationLayer.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace GamerBoxPresantationLayer.WPF
{
    public partial class SignInWindow : Window
    {
        public SignInViewModel ViewModel { get; }

        public SignInWindow(SignInViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;

            // ViewModel eventlerini dinle
            ViewModel.OnLoginSuccess += HandleLoginSuccess;
            ViewModel.OnLoginFailed += (msg) => CustomMessageBox.Show(msg, "Hata");
        }

        private void HandleLoginSuccess(User user)
        {
            CustomMessageBox.Show($"Hoşgeldin, {user.Username}!", "Giriş Başarılı");

            if (this.Owner is MainWindow mainWindow)
            {
                mainWindow.IsLoggedIn = true;
                mainWindow.UserName = user.Username;
                mainWindow.CurrentUser = user;
                // UI güncellemesini tetikle
                mainWindow.DataContext = null;
                mainWindow.DataContext = mainWindow;
                mainWindow.Opacity = 1;
            }
            this.Close();
        }

        // Pencere kapatma vb. görsel işlemler burada kalabilir
        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (this.Owner != null) this.Owner.Opacity = 1;
        }

        // SignUp penceresine geçiş UI işlemidir, 
 
        private void SignUp_Click(object sender, MouseButtonEventArgs e)
        {
            var signUp = App.ServiceProvider.GetRequiredService<SignUpWindow>();
            signUp.Owner = this.Owner;
            this.Close();
            signUp.ShowDialog();
        }

        // SignInWindow.xaml.cs içine eklenecek metod:

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter tuşuna basıldığında odağı şifre kutusuna (txtPass) kaydır
            if (e.Key == Key.Enter)
            {
                txtPass.Focus();
                e.Handled = true;
            }
        }
    }
}