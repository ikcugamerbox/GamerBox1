using GamerBoxPresantationLayer.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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


        private void btnReview_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.Tag is int gameId)
            {
                var mainWin = Application.Current.MainWindow as MainWindow;

                if (mainWin != null)
                {
                    // 1. Detay sayfasını servisten al
                    var detailPage = App.ServiceProvider.GetService<GamerBoxPresantationLayer.WPF.Views.UserControls.UCGameDetail>();

                    if (detailPage != null)
                    {
                        // 2. Sayfayı başlat (Misafir veya Üye olarak)
                        detailPage.Initialize(gameId);

                        // 3. Sayfayı değiştir
                        mainWin.MainContent.Content = detailPage;
                    }
                }
            }
        }

        private void btnToggleFilter_Click(object sender, RoutedEventArgs e)
        {
            if (FilterColumn.Width.Value == 0)
                FilterColumn.Width = new GridLength(220);
            else
                FilterColumn.Width = new GridLength(0);
        }
        private void btnAddToList_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.Tag is int gameId)
            {
                var mainWin = Application.Current.MainWindow as MainWindow;

                if (mainWin == null || !mainWin.IsLoggedIn || mainWin.CurrentUser == null)
                {
                    CustomMessageBox.Show("Listeye oyun eklemek için giriş yapmalısınız.", "Erişim Reddedildi");
                    return;
                }

                // Yeni pencereyi aç
                AddGameToListWindow win = new AddGameToListWindow(mainWin.CurrentUser.Id, gameId);
                win.Owner = mainWin;
                win.ShowDialog();
            }
        }


    }
}