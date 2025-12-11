using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace GamerBoxPresantationLayer.WPF
{
    public partial class SignUpWindow : Window
    {
        private readonly IUserService _userService;

        public SignUpWindow(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validasyon Kontrolleri
            if (txtPass.Password != txtPassConfirm.Password)
            {
                MessageBox.Show("Şifreler uyuşmuyor!", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPass.Password))
            {
                MessageBox.Show("Lütfen tüm zorunlu alanları doldurun.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 2. Kullanıcı Nesnesini Oluşturma
                var user = new User
                {
                    Username = txtUsername.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Bio = txtName.Text.Trim(), // Entity'de Ad Soyad olmadığı için Bio'ya atadık
                    ThemePreference = "dark" // Varsayılan tema
                };

                // 3. Kayıt İşlemi (Business Layer)
                _userService.Register(user, txtPass.Password);

                MessageBox.Show("Kayıt başarılı! Şimdi giriş yapabilirsiniz.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);

                // 4. Giriş Ekranına Yönlendirme
                GoToSignIn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Kayıt Hatası", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoSignIn_Click(object sender, RoutedEventArgs e)
        {
            GoToSignIn();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            // Eğer bir ana pencere tarafından açıldıysa onun opaklığını düzelt
            if (this.Owner != null)
                this.Owner.Opacity = 1;
        }

        // Yardımcı Metot: Giriş Ekranına Dönüş
        private void GoToSignIn()
        {
            var signIn = App.ServiceProvider.GetRequiredService<SignInWindow>();

            // Eğer bu pencerenin sahibi varsa (MainWindow), yeni açılan login penceresinin de sahibi yap
            if (this.Owner != null)
                signIn.Owner = this.Owner;

            this.Close();
            signIn.ShowDialog();
        }
    }
}