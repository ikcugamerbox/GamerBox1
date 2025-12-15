using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using GamerBoxPresantationLayer.WPF.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows; // MessageBox için (İsteğe bağlı, MVVM'de servis kullanılmalı ama şimdilik kalsın)

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class WatchlistViewModel : ObservableObject
    {
        private readonly IUserService _userService;


        // Şimdilik yapıyı bozmamak için Entity bırakıyorum.
        public ObservableCollection<GameDisplayModel> MyGames { get; } = new ObservableCollection<GameDisplayModel>();

        public WatchlistViewModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task LoadLibraryAsync(int? userId)
        {
            MyGames.Clear();
            if (userId.HasValue)
            {
                var games = await _userService.GetUserGamesAsyncB(userId.Value);
                foreach (var game in games)
                {
                    // Entity -> Model Dönüşümü (Mapping)
                    MyGames.Add(new GameDisplayModel
                    {
                        Id = game.Id,
                        Title = game.Title,
                        Genre = game.Genre,
                        // Rating null gelebilir, kontrol ediyoruz
                        Rating = game.AverageRating.HasValue ? $"{game.AverageRating:0.0}" : "N/A",
                        Year = game.ReleaseDate.Year.ToString(),
                        // Resim yoksa varsayılanı kullan
                        Poster = string.IsNullOrEmpty(game.ImageUrl) ? "/Resources/treasure-chest.png" : game.ImageUrl,
                        // Fiyat formatlama
                        PriceTag = game.Price == 0 ? "Ücretsiz" : $"{game.Price:0.##} ₺"
                    });
                }
            }
        }
    }
}