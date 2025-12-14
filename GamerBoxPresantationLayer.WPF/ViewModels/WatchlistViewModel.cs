using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows; // MessageBox için (İsteğe bağlı, MVVM'de servis kullanılmalı ama şimdilik kalsın)

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class WatchlistViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        // Burada direkt Entity (Game) kullanmışız, ileride GameDisplayModel'e çevirebilirsin.
        // Şimdilik yapıyı bozmamak için Entity bırakıyorum.
        public ObservableCollection<Game> MyGames { get; } = new ObservableCollection<Game>();

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
                    MyGames.Add(game);
                }
            }
        }
    }
}