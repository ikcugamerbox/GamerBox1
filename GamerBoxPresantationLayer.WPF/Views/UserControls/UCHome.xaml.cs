using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using GamerBoxPresantationLayer.WPF; // App sınıfına erişim için
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    }

    // XAML'da Binding için kullanılan yardımcı sınıf
    public class GameDisplayModel
    {
        public string Title { get; set; }
        public string Rating { get; set; }
        public string Year { get; set; }
        public string Poster { get; set; }
    }
}