using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        [ObservableProperty] private string fullName; // Bio yerine kullanıyoruz
        [ObservableProperty] private string username;
        [ObservableProperty] private string email;

        public Action GoToSignInAction { get; set; }
        public Action RequestClose { get; set; }

        public SignUpViewModel(IUserService userService)
        {
            _userService = userService;
        }

        // Parametre olarak object[] veya benzeri bir yapı ile iki şifreyi de alabiliriz 
        // ama basitlik adına PasswordBox'ları View'den alacağız.
        [RelayCommand]
        private async Task RegisterAsync(object parameter)
        {
            // View'den object array olarak 2 passwordBox gelecek
            var values = (object[])parameter;
            var passBox = values[0] as PasswordBox;
            var confirmBox = values[1] as PasswordBox;

            string password = passBox?.Password;
            string confirmPass = confirmBox?.Password;

            // 1. Validasyon
            if (password != confirmPass)
            {
                CustomMessageBox.Show("Şifreler uyuşmuyor!", "Hata");
                return;
            }

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                CustomMessageBox.Show("Lütfen tüm zorunlu alanları doldurun.", "Uyarı");
                return;
            }

            try
            {
                var user = new User
                {
                    Username = Username,
                    Email = Email,
                    Bio = FullName ?? "",
                    ThemePreference = "dark"
                };

                await _userService.RegisterAsyncB(user, password);
                CustomMessageBox.Show("Kayıt başarılı! Şimdi giriş yapabilirsiniz.", "Başarılı");

                // Giriş sayfasına yönlendir
                GoToSignInAction?.Invoke();
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.Message, "Kayıt Hatası");
            }
        }

        [RelayCommand]
        private void NavigateToSignIn()
        {
            GoToSignInAction?.Invoke();
            RequestClose?.Invoke();
        }
    }
}