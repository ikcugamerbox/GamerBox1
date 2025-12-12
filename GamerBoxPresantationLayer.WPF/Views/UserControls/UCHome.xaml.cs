using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using GamerBoxPresantationLayer.WPF;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Classes
{
    public partial class UCHome : UserControl, INotifyPropertyChanged
    {
        private readonly IGameService _gameService;

        // --- Değişkenler ---
        private List<Game> _allGames; // Tüm oyunların ham listesi
        private ObservableCollection<GameDisplayModel> _filteredGames;
        private ObservableCollection<string> _genreList;

        // Filtre Durumları
        private string _searchText;
        private string _selectedGenre;
        private int _selectedRatingIndex = 0; // 0: Tümü
        private int _selectedPriceIndex = 0;  // 0: Tümü
        private int _selectedSortIndex = 0;   // 0: Varsayılan
        private string _resultCountText;

        public UCHome()
        {
            InitializeComponent();

            if (App.ServiceProvider != null)
            {
                _gameService = App.ServiceProvider.GetService<IGameService>();
                LoadGames();
            }

            this.DataContext = this;
        }

        // --- Binding Özellikleri (UI ile bağlantılı) ---

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public string SelectedGenre
        {
            get => _selectedGenre;
            set { _selectedGenre = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public int SelectedRatingIndex
        {
            get => _selectedRatingIndex;
            set { _selectedRatingIndex = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public int SelectedPriceIndex
        {
            get => _selectedPriceIndex;
            set { _selectedPriceIndex = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public int SelectedSortIndex
        {
            get => _selectedSortIndex;
            set { _selectedSortIndex = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public string ResultCountText
        {
            get => _resultCountText;
            set { _resultCountText = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> GenreList
        {
            get => _genreList;
            set { _genreList = value; OnPropertyChanged(); }
        }

        public ObservableCollection<GameDisplayModel> FilteredGames
        {
            get => _filteredGames;
            set { _filteredGames = value; OnPropertyChanged(); }
        }

        // --- Metotlar ---

        private async void LoadGames()
        {
            if (_gameService == null) return;

            // 1. Veritabanından tüm oyunları çek
            _allGames = await _gameService.GetAllGamesAsyncB();

            // 2. Türleri (Genres) dinamik olarak doldur
            var genres = _allGames.Select(g => g.Genre).Distinct().OrderBy(g => g).ToList();
            genres.Insert(0, "Tümü"); // En başa "Tümü" seçeneği ekle
            GenreList = new ObservableCollection<string>(genres);
            SelectedGenre = "Tümü"; // Varsayılan seç

            // 3. İlk listeleme
            ApplyFilters();
        }

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
            // 0: Tümü, 1: 4+, 2: 3+, 3: 2+
            if (SelectedRatingIndex == 1) query = query.Where(g => (g.AverageRating ?? 0) >= 4);
            else if (SelectedRatingIndex == 2) query = query.Where(g => (g.AverageRating ?? 0) >= 3);
            else if (SelectedRatingIndex == 3) query = query.Where(g => (g.AverageRating ?? 0) >= 2);

            // 4. Fiyat Filtresi
            // 0: Tümü, 1: Ücretsiz, 2: Ücretli
            if (SelectedPriceIndex == 1) query = query.Where(g => g.Price == 0);
            else if (SelectedPriceIndex == 2) query = query.Where(g => g.Price > 0);

            // 5. Sıralama
            switch (SelectedSortIndex)
            {
                case 1: // Yeni Eklenenler (Tarih - Yeni > Eski)
                    query = query.OrderByDescending(g => g.ReleaseDate);
                    break;
                case 2: // Puanı Yüksek
                    query = query.OrderByDescending(g => g.AverageRating ?? 0);
                    break;
                case 3: // Fiyatı Düşük
                    query = query.OrderBy(g => g.Price);
                    break;
                default: // Varsayılan (ID veya İsim)
                    query = query.OrderBy(g => g.Title);
                    break;
            }

            var resultList = query.ToList();

            // 6. Sonuç Sayısını Güncelle
            ResultCountText = $"{resultList.Count} oyun bulundu.";

            // 7. Görüntüleme Modeline Dönüştür
            var displayList = resultList.Select(g => new GameDisplayModel
            {
                Id = g.Id,
                Title = g.Title,
                Genre = g.Genre, // Yeni eklenen alan
                Rating = g.AverageRating.HasValue ? g.AverageRating.Value.ToString("0.0") : "N/A",
                Year = g.ReleaseDate.Year.ToString(),
                Poster = string.IsNullOrEmpty(g.ImageUrl) ? "/Resources/treasure-chest.png" : g.ImageUrl,
                PriceTag = g.Price == 0 ? "Ücretsiz" : $"{g.Price:0.##} ₺" // Yeni eklenen alan
            }).ToList();

            FilteredGames = new ObservableCollection<GameDisplayModel>(displayList);
        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchText = "";
            SelectedGenre = "Tümü";
            SelectedRatingIndex = 0;
            SelectedPriceIndex = 0;
            SelectedSortIndex = 0;
        }

        // --- İnceleme Butonu ---
        private void btnReview_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.Tag is int gameId)
            {
                var mainWin = Application.Current.MainWindow as MainWindow;

                if (mainWin == null || !mainWin.IsLoggedIn || mainWin.CurrentUser == null)
                {
                    MessageBox.Show("İnceleme yazmak için giriş yapmalısınız.");
                    return;
                }

                var game = _allGames.FirstOrDefault(g => g.Id == gameId);
                string title = game?.Title ?? "Oyun";

                AddReviewWindow win = new AddReviewWindow(gameId, title, mainWin.CurrentUser.Id);
                win.Owner = mainWin;
                win.ShowDialog();
            }
        }

        // --- PropertyChanged Helper ---
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        // ... (Diğer metotlarınız burada duruyor: LoadGames, ApplyFilters vb.)

        private void btnToggleFilter_Click(object sender, RoutedEventArgs e)
        {
            // Eğer genişlik 0 ise (kapalıysa), 220 yap (aç).
            if (FilterColumn.Width.Value == 0)
            {
                FilterColumn.Width = new GridLength(220);
            }
            else
            {
                // Açıksa, 0 yap (kapat).
                FilterColumn.Width = new GridLength(0);
            }
        }

        // ...
    }

    // XAML Binding Sınıfı
    public class GameDisplayModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; } // Yeni
        public string Rating { get; set; }
        public string Year { get; set; }
        public string Poster { get; set; }
        public string PriceTag { get; set; } // Yeni
    }
}