using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBoxPresantationLayer.WPF.Models;
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


        // --- Özellikler (Properties) ---
        // [ObservableProperty] attribute'u sayesinde arka planda 
        // "SearchText" propertysini ve değişim bildirimlerini otomatik oluşturur.

        [ObservableProperty] private string searchText="";
        [ObservableProperty] private string selectedGenre = "Tümü";
        [ObservableProperty] private int selectedRatingIndex = 0;
        [ObservableProperty] private int selectedPriceIndex = 0;
        [ObservableProperty] private int selectedSortIndex = 0;
        [ObservableProperty] private string resultCountText;

        // ComboBox ve ListBox kaynakları
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

            // 1. TURLERİ VERİTABANINDAN ÇEK (Sadece 1 kere yapılır)
            // Eğer liste zaten doluysa tekrar çekmeye gerek yok (Performans)
            if (GenreList.Count == 0)
            {
                var genres = await _gameService.GetGenresAsyncB();

                GenreList.Clear();
                GenreList.Add("Tümü"); // Varsayılan seçenek
                foreach (var genre in genres)
                {
                    GenreList.Add(genre);
                }
            }

            // 2. OYUNLARI VERİTABANINDAN FİLTRELEYEREK ÇEK
            await ApplyFilters();
        }

        // Property'ler her değiştiğinde filtrelemeyi otomatik tetikle
        partial void OnSearchTextChanged(string value) => _ = ApplyFilters();
        partial void OnSelectedGenreChanged(string value) => _ = ApplyFilters();
        partial void OnSelectedRatingIndexChanged(int value) => _ = ApplyFilters();
        partial void OnSelectedPriceIndexChanged(int value) => _ = ApplyFilters();
        partial void OnSelectedSortIndexChanged(int value) => _ = ApplyFilters();

        [RelayCommand]
        private async Task ApplyFilters()
        {
            // Veritabanına parametreleri gönderiyoruz, SQL orada çalışıp sadece sonucu dönüyor.
            var resultList = await _gameService.GetFilteredGamesAsyncB(
                SearchText,
                SelectedGenre,
                SelectedRatingIndex,
                SelectedPriceIndex,
                SelectedSortIndex
            );

            // Sonuç sayısını güncelle
            ResultCountText = $"{resultList.Count} oyun bulundu.";

            // UI Listesini Temizle ve Doldur (Entity -> Model Dönüşümü)
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
                    // Resim yoksa varsayılan resmi kullan
                    Poster = string.IsNullOrEmpty(g.ImageUrl) ? "/Resources/treasure-chest.png" : g.ImageUrl,
                    // Fiyat formatlama
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