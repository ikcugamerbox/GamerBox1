using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using GamerBoxPresantationLayer.WPF.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;

        [ObservableProperty] private string fullName; // Bio yerine kullanıyoruz
        [ObservableProperty] private string username;
        [ObservableProperty] private string email;
        [ObservableProperty] private string selectedTheme = "dark";

        public Action GoToSignInAction { get; set; }
        public Action RequestClose { get; set; }

        public SignUpViewModel(IUserService userService,IDialogService dialogService)
        {
            _userService = userService;
            _dialogService = dialogService;
        }

        
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
                _dialogService.ShowMessage("Şifreler uyuşmuyor!", "Hata");
                return;
            }

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                _dialogService.ShowMessage("Lütfen tüm zorunlu alanları doldurun.", "Uyarı"); 
                return;
            }

            try
            {
                var user = new User
                {
                    Username = Username,
                    Email = Email,
                    Bio = FullName ?? "",
                    ThemePreference = SelectedTheme
                };

                await _userService.RegisterAsyncB(user, password);
                _dialogService.ShowMessage("Kayıt başarılı! Şimdi giriş yapabilirsiniz.", "Başarılı");
             
                // Giriş sayfasına yönlendir
                GoToSignInAction?.Invoke();
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message, "Kayıt Hatası");
           
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