using GamerBoxPresantationLayer.WPF.ViewModels;
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
            // Kullanıcı ID'sini MainWindow'dan alıp ViewModel'e veriyoruz
            var mainWin = Application.Current.MainWindow as MainWindow;
            int? userId = null;

            if (mainWin != null && mainWin.IsLoggedIn && mainWin.CurrentUser != null)
            {
                userId = mainWin.CurrentUser.Id;
            }

            await ViewModel.LoadDataAsync(userId);
        }
    }
}