using GamerBoxPresantationLayer.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCWatchtLists : UserControl
    {
        public WatchlistViewModel ViewModel { get; }

        public UCWatchtLists(WatchlistViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;

            this.Loaded += UCWatchtLists_Loaded;
        }

        private async void UCWatchtLists_Loaded(object sender, RoutedEventArgs e)
        {
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin != null && mainWin.CurrentUser != null)
            {
                await ViewModel.LoadLibraryAsync(mainWin.CurrentUser.Id);
            }
        }

        private void btnAddGame_Click(object sender, RoutedEventArgs e)
        {
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin != null && mainWin.CurrentUser != null)
            {
                // 1. Pencereyi servisten iste (ViewModel içinde hazır gelir)
                var addGameWin = App.ServiceProvider.GetService<AddGameWindow>();

                if (addGameWin != null)
                {
                    // 2. Kullanıcı ID'sini aktar
                    addGameWin.Initialize(mainWin.CurrentUser.Id);
                    addGameWin.Owner = mainWin;

                    // 3. Pencereyi aç
                    addGameWin.ShowDialog();

                    // 4. Listeyi yenile
                    _ = ViewModel.LoadLibraryAsync(mainWin.CurrentUser.Id);
                }
            }
            else
            {
                CustomMessageBox.Show("Oyun eklemek için giriş yapmalısınız.","Erişim Reddedildi");
            }
        }
    }
}