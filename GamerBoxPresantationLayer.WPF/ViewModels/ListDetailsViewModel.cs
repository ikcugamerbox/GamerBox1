using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBoxPresantationLayer.WPF.Models;
using GamerBoxPresantationLayer.WPF.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class ListDetailsViewModel : ObservableObject
    {
        private readonly IUserListService _userListService;
        private readonly IDialogService _dialogService;
        private int _listId;
        private int _userId;

        [ObservableProperty] private string listName;

        // Listede gösterilecek oyunlar
        public ObservableCollection<GameDisplayModel> Games { get; } = new ObservableCollection<GameDisplayModel>();

        public ListDetailsViewModel(IUserListService userListService, IDialogService dialogService)
        {
            _userListService = userListService;
            _dialogService = dialogService;
        }

        public async Task LoadDataAsync(int listId, int userId, string name)
        {
            _listId = listId;
            _userId = userId;
            ListName = name;
            await RefreshListAsync();
        }

        private async Task RefreshListAsync()
        {
            Games.Clear();
            // Kullanıcının listelerini çekip, ilgili ID'ye sahip olanı buluyoruz
            // (Performans için ileride GetListByIdAsync yazılabilir ama şimdilik bu yeterli)
            var allLists = await _userListService.GetUserListsAsyncB(_userId);
            var currentList = allLists.FirstOrDefault(x => x.Id == _listId);

            if (currentList != null && currentList.Games != null)
            {
                foreach (var g in currentList.Games)
                {
                    Games.Add(new GameDisplayModel
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Genre = g.Genre,
                        Rating = g.AverageRating.HasValue ? $"{g.AverageRating:0.0}" : "N/A",
                        Year = g.ReleaseDate.Year.ToString(),
                        Poster = string.IsNullOrEmpty(g.ImageUrl) ? "/Resources/treasure-chest.png" : g.ImageUrl,
                        PriceTag = g.Price == 0 ? "Ücretsiz" : $"{g.Price:0.##} ₺"
                    });
                }
            }
        }

        // Listeden Çıkarma Komutu
        [RelayCommand]
        private async Task RemoveGameAsync(int gameId)
        {
            var result = _dialogService.ShowConfirmation("Bu oyunu listeden kaldırmak istediğine emin misin?", "Onay");
            if (result)
            {
                await _userListService.RemoveGameFromListAsyncB(_listId, gameId);
                await RefreshListAsync(); // Listeyi yenile
            }
        }

        // Detay Sayfasına Gitme (İncele) Komutu
        [RelayCommand]
        private void GoToDetail(int gameId)
        {
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin != null)
            {
                var detailPage = App.ServiceProvider.GetService<GamerBoxPresantationLayer.WPF.Views.UserControls.UCGameDetail>();
                detailPage.Initialize(gameId);
                mainWin.MainContent.Content = detailPage;
            }
        }

        // Geri Dön Komutu
        [RelayCommand]
        private void GoBack()
        {
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin != null)
            {
                mainWin.MainContent.Content = App.ServiceProvider.GetService<GamerBoxPresantationLayer.WPF.Views.UserControls.UCLists>();
            }
        }
    }
}