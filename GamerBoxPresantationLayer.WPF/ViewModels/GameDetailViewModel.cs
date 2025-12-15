using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBoxPresantationLayer.WPF.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class GameDetailViewModel : ObservableObject
    {
        private readonly IGameService _gameService;
        private readonly IPostService _postService;
        private readonly IRatingService _ratingService;
        private readonly IUserService _userService;

        private int _gameId;
        private int? _currentUserId;

        // --- Oyun Bilgileri ---
        [ObservableProperty] private string title;
        [ObservableProperty] private string genre;
        [ObservableProperty] private string description;
        [ObservableProperty] private string priceText;
        [ObservableProperty] private string imageUrl;
        [ObservableProperty] private string ratingText;

        // --- Yorum Yapma Kısmı ---
        [ObservableProperty] private string newCommentContent;
        [ObservableProperty] private int newRatingScore = 5; // Varsayılan 5 yıldız

        // --- Yorumlar Listesi ---
        public ObservableCollection<ReviewDisplayModel> Reviews { get; } = new ObservableCollection<ReviewDisplayModel>();

        // Constructor
        public GameDetailViewModel(IGameService gameService, IPostService postService, IRatingService ratingService, IUserService userService)
        {
            _gameService = gameService;
            _postService = postService;
            _ratingService = ratingService;
            _userService = userService;
        }

        // Sayfa açılınca verileri yükle
        public async Task LoadPageAsync(int gameId, int? userId)
        {
            _gameId = gameId;
            _currentUserId = userId;

            // 1. Oyun Detaylarını Çek
            var game = await _gameService.GetGameByIdAsyncB(gameId);
            if (game != null)
            {
                Title = game.Title;
                Genre = game.Genre;
                Description = game.Description;
                ImageUrl = string.IsNullOrEmpty(game.ImageUrl) ? "/Resources/treasure-chest.png" : game.ImageUrl;
                PriceText = game.Price == 0 ? "Ücretsiz" : $"{game.Price:0.##} ₺";

                // Güncel puanı hesapla
                double avgRating = await _ratingService.GetAverageRatingAsyncB(gameId);
                RatingText = avgRating > 0 ? $"{avgRating:0.0} / 5" : "Henüz Puan Yok";
            }

            // 2. Yorumları Çek
            await LoadReviewsAsync();
        }

        private async Task LoadReviewsAsync()
        {
            Reviews.Clear();
            var posts = await _postService.GetByGameIdAsyncB(_gameId);

            foreach (var post in posts)
            {
                var user = await _userService.GetByIdAsyncB(post.UserId);
                Reviews.Add(new ReviewDisplayModel
                {
                    UserName = user?.Username ?? "Gizli Kullanıcı",
                    Content = post.Content,
                    DateStr = post.CreatedAt.ToString("dd MMM yyyy"),
                    ProfilePictureUrl = user?.ProfilePictureUrl ?? "",
                    // Hashtagleri de gösterebiliriz
                    HashtagsStr = post.Hashtags != null ? string.Join(" ", post.Hashtags.Select(h => "#" + h)) : ""
                });
            }
        }

        [RelayCommand]
        public async Task SubmitReviewAsync()
        {
            if (_currentUserId == null || _currentUserId <= 0)
            {
                CustomMessageBox.Show("Yorum yapabilmek için lütfen giriş yapınız.", "Erişim Kısıtlı");
                return;
            }
            if (string.IsNullOrWhiteSpace(NewCommentContent))
            {
                CustomMessageBox.Show("Lütfen bir yorum yazın.", "Uyarı");
                return;
            }

            try
            {
                // 1. Postu Kaydet
               
                await _postService.CreatePostAsyncB(_currentUserId.Value, _gameId, NewCommentContent);

                // 2. Puanı Kaydet (Hata verirse daha önce puan vermiş demektir, yoksayalım)
                try
                {
                    bool hasRated = await _ratingService.HasUserRatedAsyncB(_currentUserId.Value, _gameId);
                    if (!hasRated)
                        await _ratingService.RateGameAsyncB(_currentUserId.Value, _gameId, NewRatingScore);
                }
                catch { }

                CustomMessageBox.Show("Yorumunuz paylaşıldı!", "Teşekkürler");
                NewCommentContent = ""; // Kutuyu temizle

                // Listeyi Yenile
                await LoadPageAsync(_gameId, _currentUserId);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Hata: " + ex.Message, "Hata");
            }
        }

        [RelayCommand]
        public void GoBack()
        {
            // Ana pencereye eriş ve Home'a dön
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin != null)
            {
                // Home sayfasını geri getir
                mainWin.MainContent.Content = App.ServiceProvider.GetService<GamerBoxPresantationLayer.WPF.Classes.UCHome>();
            }
        }
    }
}