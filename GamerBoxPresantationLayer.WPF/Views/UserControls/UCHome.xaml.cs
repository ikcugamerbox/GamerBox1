using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using GamerBoxPresantationLayer.WPF; // App sınıfına erişim için
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Classes // Namespace düzeltildi
{
    public partial class UCHome : UserControl, INotifyPropertyChanged
    {
        private readonly IGameService _gameService;
        private string _searchText;
        private ObservableCollection<GameDisplayModel> _filteredGames;
        private List<Game> _allGames;

        public UCHome()
        {
            InitializeComponent();

            // Servisi App.xaml.cs'deki Provider'dan çekiyoruz
            if (App.ServiceProvider != null)
            {
                _gameService = App.ServiceProvider.GetService<IGameService>();
                LoadGames();
            }

            this.DataContext = this;
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterGames();
            }
        }

        public ObservableCollection<GameDisplayModel> FilteredGames
        {
            get => _filteredGames;
            set
            {
                _filteredGames = value;
                OnPropertyChanged();
            }
        }

        private void LoadGames()
        {
            if (_gameService == null) return;

            _allGames = _gameService.GetAllGames();
            FilterGames();
        }

        private void FilterGames()
        {
            if (_allGames == null) return;

            var query = _allGames.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(g => g.Title.ToLower().Contains(SearchText.ToLower()));
            }

            var displayList = query.Select(g => new GameDisplayModel
            {
                Id = g.Id, // EKLENDİ
                Title = g.Title,
                Rating = g.AverageRating.HasValue ? g.AverageRating.Value.ToString("0.0") : "N/A",
                Year = g.ReleaseDate.Year.ToString(),
                Poster = string.IsNullOrEmpty(g.ImageUrl) ? "/Resources/treasure-chest.png" : g.ImageUrl
            }).ToList();

            FilteredGames = new ObservableCollection<GameDisplayModel>(displayList);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void btnReview_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.Tag is int gameId) // Tag üzerinden ID alacağız
            {
                var mainWin = Application.Current.MainWindow as MainWindow;

                if (mainWin == null || !mainWin.IsLoggedIn || mainWin.CurrentUser == null)
                {
                    MessageBox.Show("İnceleme yazmak için giriş yapmalısınız.");
                    return;
                }

                // Oyun adını bul (Listeden basitçe çekiyoruz)
                var game = _allGames.FirstOrDefault(g => g.Id == gameId);
                string title = game?.Title ?? "Oyun";

                AddReviewWindow win = new AddReviewWindow(gameId, title, mainWin.CurrentUser.Id);
                win.Owner = mainWin;
                win.ShowDialog();
            }
        }
    }

    // XAML'da Binding için kullanılan yardımcı sınıf
    public class GameDisplayModel
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Rating { get; set; }
        public string Year { get; set; }
        public string Poster { get; set; }
    }
}