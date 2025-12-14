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

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validasyon Kontrolleri
            if (txtPass.Password != txtPassConfirm.Password)
            {
                CustomMessageBox.Show("Şifreler uyuşmuyor!", "Hata");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPass.Password))
            {
                CustomMessageBox.Show("Lütfen tüm zorunlu alanları doldurun.", "Uyarı");
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
                await _userService.RegisterAsyncB(user, txtPass.Password);

                CustomMessageBox.Show("Kayıt başarılı! Şimdi giriş yapabilirsiniz.", "Başarılı");

                // 4. Giriş Ekranına Yönlendirme
                GoToSignIn();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.Message, "Kayıt Hatası");
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

        //  Giriş Ekranına Dönüş
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