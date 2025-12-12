using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Extensions.DependencyInjection; // Servis sağlayıcı için
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCReviews : UserControl
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IGameService _gameService;

        public ObservableCollection<ReviewDisplayModel> RecentPosts { get; set; } = new ObservableCollection<ReviewDisplayModel>();

        public UCReviews()
        {
            InitializeComponent();

            // Servisleri App.ServiceProvider üzerinden alıyoruz
            if (App.ServiceProvider != null)
            {
                _postService = App.ServiceProvider.GetService<IPostService>();
                _userService = App.ServiceProvider.GetService<IUserService>();
                _gameService = App.ServiceProvider.GetService<IGameService>();

                LoadPosts();
            }

            this.DataContext = this;
        }

        private async void LoadPosts()
        {
            // Son 20 postu getir (DAL'da GetRecentPosts metodu vardı)
            var posts = await _postService.GetRecentPostsAsyncB(20);

            foreach (var post in posts)
            {
                var user = await _userService.GetByIdAsyncB(post.UserId);
                var game = post.GameId.HasValue
                     ? await _gameService.GetGameByIdAsyncB(post.GameId.Value)
                     : null;
                RecentPosts.Add(new ReviewDisplayModel
                {
                    UserName = user?.Username ?? "Unknown",
                    GameName = game != null ? $"Playing: {game.Title}" : "",
                    Content = post.Content,
                    DateStr = post.CreatedAt.ToString("g"),
                    HashtagsStr = post.Hashtags != null ? string.Join(" ", post.Hashtags.Select(h => "#" + h)) : ""
                });
            }
        }
    }

    public class ReviewDisplayModel
    {
        public string UserName { get; set; }
        public string GameName { get; set; }
        public string Content { get; set; }
        public string DateStr { get; set; }
        public string HashtagsStr { get; set; }
    }
}