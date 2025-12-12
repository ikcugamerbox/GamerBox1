using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCLists : UserControl
    {
        private readonly IGameService _gameService;

        public ObservableCollection<GameDisplay> TopRatedGames { get; set; } = new ObservableCollection<GameDisplay>();
        public ObservableCollection<GameDisplay> RecommendedGames { get; set; } = new ObservableCollection<GameDisplay>();
        public bool IsGuest { get; set; } = true;

        public UCLists()
        {
            InitializeComponent();

            if (App.ServiceProvider != null)
            {
                _gameService = App.ServiceProvider.GetService<IGameService>();
                LoadLists();
            }

            this.DataContext = this;
        }

        private async void LoadLists()
        {
            // 1. En Çok Puan Alanlar
            var topGames = await  _gameService.GetByRatingAsyncB(10);
            foreach (var g in topGames)
            {
                TopRatedGames.Add(new GameDisplay
                {
                    Title = g.Title,
                    RatingStr = $"⭐ {g.AverageRating:0.0}",
                    Genre = g.Genre
                });
            }

            // 2. Önerilenler (Kullanıcı Giriş Yaptıysa)
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin != null && mainWin.IsLoggedIn && mainWin.CurrentUser != null)
            {
                IsGuest = false;
                // Kullanıcının favori kategorilerine göre öneri getir
                // (Not: Kullanıcı henüz kategori seçmediyse boş gelebilir)
                try
                {
                    var recGames = await _gameService.RecommendByCategoriesAsyncB(mainWin.CurrentUser.Id, 10);
                    foreach (var g in recGames)
                    {
                        RecommendedGames.Add(new GameDisplay
                        {
                            Title = g.Title,
                            RatingStr = $"{g.AverageRating:0.0}",
                            Genre = g.Genre
                        });
                    }
                }
                catch
                {
                    // Öneri servisi hata verirse (örn. kategori seçilmemişse) sessizce geç
                }
            }
        }
    }

    public class GameDisplay
    {
        public string Title { get; set; }
        public string RatingStr { get; set; }
        public string Genre { get; set; }
    }
}