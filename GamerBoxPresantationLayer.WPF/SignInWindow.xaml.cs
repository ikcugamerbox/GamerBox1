using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Extensions.DependencyInjection; // GetRequiredService için gerekli
using System;
using System.Windows;
using System.Windows.Input;

namespace GamerBoxPresantationLayer.WPF
{
    public partial class SignInWindow : Window
    {
        private readonly IUserService _userService;

        // Constructor'da servisi istiyoruz (Dependency Injection)
        public SignInWindow(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (this.Owner != null)
                this.Owner.Opacity = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = txtEmail.Text;
                string password = txtPass.Password; // PasswordBox'tan şifre alma

                // Business Layer'daki Login metodu çağrılıyor
                User user = _userService.Login(email, password);

                // Giriş Başarılı!
                MessageBox.Show($"Hoşgeldin, {user.Username}!", "Giriş Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);

                // Ana pencereye kullanıcı bilgisini gönder
                if (this.Owner is MainWindow mainWindow)
                {
                    mainWindow.IsLoggedIn = true;
                    mainWindow.UserName = user.Username;
                    // Eğer avatar yolu veritabanında varsa burayı da güncelleyebilirsiniz
                    // mainWindow.AvatarPath = user.AvatarUrl; 

                    // UI güncellemesi için DataContext'i yenile
                    mainWindow.DataContext = null;
                    mainWindow.DataContext = mainWindow;

                    mainWindow.Opacity = 1; // Ana pencereyi tekrar görünür yap
                }

                this.Close();
            }
            catch (Exception ex)
            {
                // Hata mesajı (Şifre yanlış vb.)
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignUp_Click(object sender, MouseButtonEventArgs e)
        {
            // SignUp penceresini de servisten alıyoruz (O da IUserService kullanacak çünkü)
            var signUp = App.ServiceProvider.GetRequiredService<SignUpWindow>();
            signUp.Owner = this.Owner; // Ana pencereyi sahibi yapıyoruz

            this.Close(); // Giriş penceresini kapat
            signUp.ShowDialog(); // Kayıt penceresini aç
        }

        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}