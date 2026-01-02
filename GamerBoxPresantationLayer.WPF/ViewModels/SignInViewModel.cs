using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Threading.Tasks;
using System.Windows.Controls; // PasswordBox için

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        [ObservableProperty]
        private string email;


        // Pencerenin kapatılması veya Login başarısı için event
        public event Action<User> OnLoginSuccess;
        public event Action<string> OnLoginFailed;

        public SignInViewModel(IUserService userService)
        {
            _userService = userService;
        }

        [RelayCommand]
        public async Task LoginAsync(object parameter)
        {
            // PasswordBox güvenliği nedeniyle parametre olarak gönderilir
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox?.Password;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                OnLoginFailed?.Invoke("Lütfen tüm alanları doldurun.");
                return;
            }

            try
            {
                var user = await _userService.LoginAsyncB(Email, password);
                // Başarılı olursa View'a haber ver
                OnLoginSuccess?.Invoke(user);
            }
            catch (Exception ex)
            {
                OnLoginFailed?.Invoke(ex.Message);
            }
        }
    }
}