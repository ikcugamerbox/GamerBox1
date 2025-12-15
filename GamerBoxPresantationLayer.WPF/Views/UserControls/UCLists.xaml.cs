using GamerBoxPresantationLayer.WPF.Models; 
using GamerBoxPresantationLayer.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCLists : UserControl
    {
        public ListsViewModel ViewModel { get; }

        public UCLists(ListsViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;
            this.Loaded += UCLists_Loaded;
        }

        private async void UCLists_Loaded(object sender, RoutedEventArgs e)
        {
            var mainWin = Application.Current.MainWindow as MainWindow;
            int? userId = null;

            if (mainWin != null && mainWin.IsLoggedIn && mainWin.CurrentUser != null)
            {
                userId = mainWin.CurrentUser.Id;
            }

            await ViewModel.LoadDataAsync(userId);
        }
        private void BtnOpenList_Click(object sender, RoutedEventArgs e)
        {
            // 1. Tıklanan butonu ve içindeki veriyi (Tag) alıyoruz
            if (sender is Button btn && btn.Tag is UserListDisplayModel listModel)
            {
                var mainWin = Application.Current.MainWindow as MainWindow;

                if (mainWin != null && mainWin.CurrentUser != null)
                {
                    // 2. Detay Sayfasını Servisten İste
                    var detailsPage = App.ServiceProvider.GetService<UCListDetails>();

                    if (detailsPage != null)
                    {
                        // 3. Sayfayı Başlat (Verileri Yükle)
                        detailsPage.Initialize(listModel.Id, mainWin.CurrentUser.Id, listModel.Name);

                        // 4. Ana Pencerede Sayfayı Değiştir (Navigasyon)
                        mainWin.MainContent.Content = detailsPage;
                    }
                }
            }
        }
    }
}