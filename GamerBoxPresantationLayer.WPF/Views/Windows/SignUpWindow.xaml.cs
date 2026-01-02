using GamerBoxPresantationLayer.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GamerBoxPresantationLayer.WPF
{
    public partial class SignUpWindow : Window
    {
        public SignUpViewModel ViewModel { get; }

        public SignUpWindow(SignUpViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;

            ViewModel.RequestClose += () => this.Close();
           
            ViewModel.GoToSignInAction += OpenSignInWindow;
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
          
            var passwordBoxes = new object[] { txtPass, txtPassConfirm };
 
            if (ViewModel.RegisterCommand.CanExecute(passwordBoxes))
            {
                await ViewModel.RegisterCommand.ExecuteAsync(passwordBoxes);
            }
        }

        private void GoSignIn_Click(object sender, RoutedEventArgs e)
        {
            OpenSignInWindow();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (this.Owner != null) this.Owner.Opacity = 1;
        }

        // Giriş Ekranını Açan Yardımcı Metot
        private void OpenSignInWindow()
        {
            var signIn = App.ServiceProvider.GetRequiredService<SignInWindow>();

            if (this.Owner != null)
                signIn.Owner = this.Owner;

            this.Close();
            signIn.ShowDialog();
        }
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
                
                if (ViewModel.RegisterCommand.CanExecute(txtPass))
                {
                    await ViewModel.RegisterCommand.ExecuteAsync(txtPass);
                }
            }
        }
    }
}