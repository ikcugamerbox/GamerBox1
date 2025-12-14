using GamerBoxPresantationLayer.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCProfile : UserControl
    {
        // View sadece ViewModel'ini tanır
        public ProfileViewModel ViewModel { get; }

        public UCProfile(ProfileViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel; // Bağlamı ViewModel'e ata

            this.Loaded += UCProfile_Loaded;
        }

        private async void UCProfile_Loaded(object sender, RoutedEventArgs e)
        {
            // MainWindow'dan sadece ID bilgisini alıp ViewModel'e "Yükle" emri veriyoruz.
            if (Application.Current.MainWindow is MainWindow mainWin && mainWin.CurrentUser != null)
            {
                await ViewModel.LoadUserDataAsync(mainWin.CurrentUser.Id);
            }
        }
    }
}