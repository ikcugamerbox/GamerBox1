using GamerBoxPresantationLayer.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCProfile : UserControl
    {
        public ProfileViewModel ViewModel { get; }

        public UCProfile(ProfileViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;

            // 1. KONTROL: Sayfa oluşturuldu mu?
            // MessageBox.Show("UCProfile Constructor Çalıştı! (Sayfa oluşturuluyor)", "Debug 1");

            this.Loaded += UCProfile_Loaded;
        }

        private async void UCProfile_Loaded(object sender, RoutedEventArgs e)
        {
            // 2. KONTROL: Sayfa yüklendi olayı tetiklendi mi?
            // MessageBox.Show("UCProfile_Loaded Tetiklendi!", "Debug 2");

            if (Application.Current.MainWindow is MainWindow mainWin)
            {
                if (mainWin.CurrentUser != null)
                {
                    // 3. KONTROL: Kullanıcı bilgisi var, veri yüklemeye gidiyoruz.
                    await ViewModel.LoadUserDataAsync(mainWin.CurrentUser.Id);
                }
                else
                {
                    MessageBox.Show("HATA: MainWindow.CurrentUser NULL görünüyor! Giriş yapılmamış gibi algılanıyor.", "Debug Hata");
                }
            }
        }
    }
}