using GamerBoxPresantationLayer.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection; // GetService için
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCGameDetail : UserControl
    {
        public GameDetailViewModel ViewModel { get; }

        // Parametre almıyoruz, Dependency Injection ile ViewModel'i alıyoruz
        public UCGameDetail()
        {
            InitializeComponent();

            if (App.ServiceProvider != null)
            {
                ViewModel = App.ServiceProvider.GetService<GameDetailViewModel>();
                this.DataContext = ViewModel;
            }
        }

        // Bu metodu dışarıdan (Home'dan) çağıracağız
        public async void Initialize(int gameId)
        {
            var mainWin = Application.Current.MainWindow as MainWindow;

            // Sadece mainWin kontrolü yapıyoruz, CurrentUser kontrolünü esnetiyoruz
            if (mainWin != null && ViewModel != null)
            {
                // Kullanıcı varsa ID'sini, yoksa null gönder
                int? userId = mainWin.CurrentUser?.Id;

                await ViewModel.LoadPageAsync(gameId, userId);
            }
        }
    }
}