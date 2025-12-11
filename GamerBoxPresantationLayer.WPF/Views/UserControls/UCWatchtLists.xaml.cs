using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCWatchtLists : UserControl
    {
        private readonly IUserService _userService;
        public ObservableCollection<Game> MyGames { get; set; } = new ObservableCollection<Game>();

        public UCWatchtLists()
        {
            InitializeComponent();

            if (App.ServiceProvider != null)
            {
                _userService = App.ServiceProvider.GetService<IUserService>();
                LoadLibrary();
            }

            this.DataContext = this;
        }

        private void LoadLibrary()
        {
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin != null && mainWin.CurrentUser != null)
            {
                var games = _userService.GetUserGames(mainWin.CurrentUser.Id);
                MyGames.Clear();
                foreach (var game in games)
                {
                    MyGames.Add(game);
                }
            }
        }
        private void btnAddGame_Click(object sender, RoutedEventArgs e)
        {
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin != null && mainWin.CurrentUser != null)
            {
                // Pencereyi aç
                AddGameWindow win = new AddGameWindow(mainWin.CurrentUser.Id);
                win.Owner = mainWin;
                win.ShowDialog();

                // Pencere kapandığında listeyi yenile
                LoadLibrary();
            }
            else
            {
                MessageBox.Show("Oyun eklemek için giriş yapmalısınız.");
            }
        }
    }
}