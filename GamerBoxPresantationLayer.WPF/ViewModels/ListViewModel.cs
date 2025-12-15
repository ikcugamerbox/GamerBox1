using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBoxPresantationLayer.WPF.Models;
using GamerBoxPresantationLayer.WPF.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class ListsViewModel : ObservableObject
    {
        private readonly IGameService _gameService;
        private readonly IUserListService _userListService;
        private readonly IDialogService _dialogService;

        [ObservableProperty] private bool isGuest = true;
        [ObservableProperty] private string newListName = "";
        [ObservableProperty] private bool isLoggedIn = false;


        public ObservableCollection<GameDisplayModel> TopRatedGames { get; } = new ObservableCollection<GameDisplayModel>();
        public ObservableCollection<GameDisplayModel> RecommendedGames { get; } = new ObservableCollection<GameDisplayModel>();
        public ObservableCollection<UserListDisplayModel> MyCustomLists { get; } = new ObservableCollection<UserListDisplayModel>();

        public ListsViewModel(IGameService gameService, IUserListService userListService, IDialogService dialogService)
        {
            _gameService = gameService;
            _userListService = userListService;
            _dialogService = dialogService;
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
            // Özel Listeleri Yükle
            MyCustomLists.Clear();
            if (userId.HasValue)
            {
                var entityLists = await _userListService.GetUserListsAsyncB(userId.Value);
                foreach (var entity in entityLists)
                {
                    // 2. Entity -> Model Dönüşümü (MAPPING)
                    MyCustomLists.Add(new UserListDisplayModel
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        GameCount = entity.Games?.Count ?? 0
                    });
                }
            }
        }
        [RelayCommand]
        public async Task CreateNewListAsync()
        {
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin?.CurrentUser == null) return;

            if (!string.IsNullOrWhiteSpace(NewListName))
            {
                try
                {
                    await _userListService.CreateListAsyncB(mainWin.CurrentUser.Id, NewListName);
                    NewListName = ""; // Kutuyu temizle
                    _dialogService.ShowMessage("Liste oluşturuldu!", "Başarılı");
                    await LoadDataAsync(mainWin.CurrentUser.Id); // Listeyi yenile
                }
                catch (System.Exception ex)
                {
                    _dialogService.ShowMessage(ex.Message, "Hata");
                    
                }
            }
        }
    }
}