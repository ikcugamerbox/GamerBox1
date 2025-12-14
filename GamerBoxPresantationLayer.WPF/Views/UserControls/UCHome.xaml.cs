using GamerBoxPresantationLayer.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Classes
{
    public partial class UCHome : UserControl
    {
        // ViewModel'e erişim (Gerekirse code-behind'dan özel bir şey çağırmak için)
        public HomeViewModel ViewModel { get; }

        // Constructor Injection: ViewModel otomatik olarak buraya gelecek
        public UCHome(HomeViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel; // Veri bağlamını ViewModel yapıyoruz

            // Sayfa yüklendiğinde verileri çek
            this.Loaded += async (s, e) => await ViewModel.LoadDataAsync();
        }

        // İnceleme butonu arayüz olayı olduğu için burada kalabilir veya
        // CommandParameter ile ViewModel'e taşınabilir. Şimdilik burada kalsın, basitleştirelim.
        private void btnReview_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.Tag is int gameId)
            {
                var mainWin = Application.Current.MainWindow as MainWindow;

                if (mainWin == null || !mainWin.IsLoggedIn || mainWin.CurrentUser == null)
                {
                    CustomMessageBox.Show("İnceleme yazmak için giriş yapmalısınız.","Erişim Reddedildi");
                    return;
                }

                // Not: _allGames'e buradan erişemediğimiz için başlığı basitçe "Oyun" geçiyoruz
                // İleride bunu da düzelteceğiz.
                AddReviewWindow win = new AddReviewWindow(gameId, "Seçilen Oyun", mainWin.CurrentUser.Id);
                win.Owner = mainWin;
                win.ShowDialog();
            }
        }

        private void btnToggleFilter_Click(object sender, RoutedEventArgs e)
        {
            if (FilterColumn.Width.Value == 0)
                FilterColumn.Width = new GridLength(220);
            else
                FilterColumn.Width = new GridLength(0);
        }

        // Temizle butonu artık ViewModel içindeki bir Command'a bağlanacak (XAML'da yapacağız)
        // O yüzden btnClearFilters_Click metodunu buradan silebilirsin.
    }
}