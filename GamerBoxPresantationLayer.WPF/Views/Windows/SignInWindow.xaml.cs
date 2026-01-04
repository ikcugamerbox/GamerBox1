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

            // ViewModel eventlerini dinle (Hata mesajları artık Business katmanından filtrelenmiş geliyor)
            ViewModel.OnLoginSuccess += HandleLoginSuccess;
            ViewModel.OnLoginFailed += (msg) => CustomMessageBox.Show(msg, "Giriş Hatası");
        }

        private void HandleLoginSuccess(User user)
        {
            CustomMessageBox.Show($"Hoşgeldin, {user.Username}!", "Giriş Başarılı");

            // MainWindow'u güncelle
            if (this.Owner is MainWindow mainWindow)
            {
                mainWindow.IsLoggedIn = true;
                mainWindow.UserName = user.Username;
                mainWindow.CurrentUser = user;
                mainWindow.DataContext = null;//:)
                mainWindow.DataContext = mainWindow;

                mainWindow.Opacity = 1;
            }
            this.Close();
        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (this.Owner != null) this.Owner.Opacity = 1;
        }

        private void SignUp_Click(object sender, MouseButtonEventArgs e)
        {
            var signUp = App.ServiceProvider.GetRequiredService<SignUpWindow>();
            signUp.Owner = this.Owner;
            this.Close();
            signUp.ShowDialog();
        }

        // Enter tuşu desteği
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtPass.Focus();
                e.Handled = true;
            }
        }

        // Şifre kutusunda Enter'a basınca giriş yap
        private async void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Login Command'ini çalıştır
                if (ViewModel.LoginCommand.CanExecute(txtPass))
                {
                    await ViewModel.LoginCommand.ExecuteAsync(txtPass);
                }
            }
        }
    }
}