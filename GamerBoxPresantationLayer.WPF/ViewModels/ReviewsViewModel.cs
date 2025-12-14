using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class ReviewsViewModel : ObservableObject
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IGameService _gameService;

        public ObservableCollection<ReviewDisplayModel> RecentPosts { get; } = new ObservableCollection<ReviewDisplayModel>();

        public ReviewsViewModel(IPostService postService, IUserService userService, IGameService gameService)
        {
            _postService = postService;
            _userService = userService;
            _gameService = gameService;
        }

        [RelayCommand]
        public async Task LoadPostsAsync()
        {
            RecentPosts.Clear();
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
                    HashtagsStr = post.Hashtags != null ? string.Join(" ", post.Hashtags.Select(h => "#" + h)) : "",
                    ProfilePictureUrl = user?.ProfilePictureUrl ?? ""
                });
            }
        }
    }

    // Model sınıfını buraya veya Models klasörüne koyabilirsin
    public class ReviewDisplayModel
    {
        public string UserName { get; set; }
        public string GameName { get; set; }
        public string Content { get; set; }
        public string DateStr { get; set; }
        public string HashtagsStr { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}