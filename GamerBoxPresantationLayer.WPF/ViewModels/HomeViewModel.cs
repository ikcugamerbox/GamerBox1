using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBoxPresantationLayer.WPF.Models; // Modeli buradan çekiyoruz
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    // ObservableObject, INotifyPropertyChanged özelliklerini otomatik sağlar.
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IGameService _gameService;
        private List<GamerBox.EntitiesLayer.Concrete.Game> _allGames; // Ham veri

        // --- Özellikler (Properties) ---
        // [ObservableProperty] attribute'u sayesinde arka planda 
        // "SearchText" propertysini ve değişim bildirimlerini otomatik oluşturur.

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private string selectedGenre = "Tümü";

        [ObservableProperty]
        private int selectedRatingIndex = 0;

        [ObservableProperty]
        private int selectedPriceIndex = 0;

        [ObservableProperty]
        private int selectedSortIndex = 0;

        [ObservableProperty]
        private string resultCountText;

        public ObservableCollection<string> GenreList { get; } = new ObservableCollection<string>();
        public ObservableCollection<GameDisplayModel> FilteredGames { get; } = new ObservableCollection<GameDisplayModel>();

        // Constructor Injection ile servisi alıyoruz
        public HomeViewModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        // Sayfa yüklendiğinde çalışacak komut
        [RelayCommand]
        public async Task LoadDataAsync()
        {
            if (_gameService == null) return;

            _allGames = await _gameService.GetAllGamesAsyncB();

            // Türleri doldur
            var genres = _allGames.Select(g => g.Genre).Distinct().OrderBy(g => g).ToList();
            GenreList.Clear();
            GenreList.Add("Tümü");
            foreach (var genre in genres)
            {
                GenreList.Add(genre);
            }

            ApplyFilters();
        }

        // Özellikler değiştiğinde otomatik filtreleme yapması için tetikleyici metodlar
        // CommunityToolkit.Mvvm isimlendirme kuralı: On[Property]Changed
        partial void OnSearchTextChanged(string value) => ApplyFilters();
        partial void OnSelectedGenreChanged(string value) => ApplyFilters();
        partial void OnSelectedRatingIndexChanged(int value) => ApplyFilters();
        partial void OnSelectedPriceIndexChanged(int value) => ApplyFilters();
        partial void OnSelectedSortIndexChanged(int value) => ApplyFilters();

        // Filtreleme Mantığı (Eski kodunun aynısı)
        [RelayCommand]
        private void ApplyFilters()
        {
            if (_allGames == null) return;

            var query = _allGames.AsEnumerable();

            // 1. Metin Arama
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(g => g.Title.ToLower().Contains(SearchText.ToLower()));
            }

            // 2. Tür Filtresi
            if (!string.IsNullOrEmpty(SelectedGenre) && SelectedGenre != "Tümü")
            {
                query = query.Where(g => g.Genre == SelectedGenre);
            }

            // 3. Puan Filtresi
            if (SelectedRatingIndex == 1) query = query.Where(g => (g.AverageRating ?? 0) >= 4);
            else if (SelectedRatingIndex == 2) query = query.Where(g => (g.AverageRating ?? 0) >= 3);
            else if (SelectedRatingIndex == 3) query = query.Where(g => (g.AverageRating ?? 0) >= 2);

            // 4. Fiyat Filtresi
            if (SelectedPriceIndex == 1) query = query.Where(g => g.Price == 0);
            else if (SelectedPriceIndex == 2) query = query.Where(g => g.Price > 0);

            // 5. Sıralama
            switch (SelectedSortIndex)
            {
                case 1: query = query.OrderByDescending(g => g.ReleaseDate); break;
                case 2: query = query.OrderByDescending(g => g.AverageRating ?? 0); break;
                case 3: query = query.OrderBy(g => g.Price); break;
                default: query = query.OrderBy(g => g.Title); break;
            }

            var resultList = query.ToList();
            ResultCountText = $"{resultList.Count} oyun bulundu.";

            FilteredGames.Clear();
            foreach (var g in resultList)
            {
                FilteredGames.Add(new GameDisplayModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    Genre = g.Genre,
                    Rating = g.AverageRating.HasValue ? g.AverageRating.Value.ToString("0.0") : "N/A",
                    Year = g.ReleaseDate.Year.ToString(),
                    Poster = string.IsNullOrEmpty(g.ImageUrl) ? "/Resources/treasure-chest.png" : g.ImageUrl,
                    PriceTag = g.Price == 0 ? "Ücretsiz" : $"{g.Price:0.##} ₺"
                });
            }
        }

        // Temizle Butonu için Komut
        [RelayCommand]
        private void ClearFilters()
        {
            SearchText = "";
            SelectedGenre = "Tümü";
            SelectedRatingIndex = 0;
            SelectedPriceIndex = 0;
            SelectedSortIndex = 0;
            ApplyFilters(); // Properties değişince zaten tetiklenir ama garanti olsun
        }
    }
}