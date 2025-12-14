using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBoxPresantationLayer.WPF.Models; // Modeli buradan çekiyoruz
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class ListsViewModel : ObservableObject
    {
        private readonly IGameService _gameService;

        [ObservableProperty]
        private bool isGuest = true;

        public ObservableCollection<GameDisplayModel> TopRatedGames { get; } = new ObservableCollection<GameDisplayModel>();
        public ObservableCollection<GameDisplayModel> RecommendedGames { get; } = new ObservableCollection<GameDisplayModel>();

        public ListsViewModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        // Kullanıcı ID'sini parametre olarak alıyoruz (Giriş yapmamışsa null gelir)
        public async Task LoadDataAsync(int? userId)
        {
            // 1. En Çok Puan Alanlar
            TopRatedGames.Clear();
            var topGames = await _gameService.GetByRatingAsyncB(10);
            foreach (var g in topGames)
            {
                TopRatedGames.Add(new GameDisplayModel
                {
                    Title = g.Title,
                    Rating = $"⭐ {g.AverageRating:0.0}",
                    Genre = g.Genre,
                    Poster = "/Resources/treasure-chest.png" // Varsayılan resim
                });
            }

            // 2. Önerilenler
            RecommendedGames.Clear();
            if (userId.HasValue)
            {
                IsGuest = false;
                try
                {
                    var recGames = await _gameService.RecommendByCategoriesAsyncB(userId.Value, 10);
                    foreach (var g in recGames)
                    {
                        RecommendedGames.Add(new GameDisplayModel
                        {
                            Title = g.Title,
                            Rating = $"{g.AverageRating:0.0}",
                            Genre = g.Genre,
                            Poster = "/Resources/treasure-chest.png"
                        });
                    }
                }
                catch
                {
                    // Öneri yoksa boş kalsın
                }
            }
            else
            {
                IsGuest = true;
            }
        }
    }
}