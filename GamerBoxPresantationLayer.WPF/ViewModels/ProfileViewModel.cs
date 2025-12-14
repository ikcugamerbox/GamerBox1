using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System.Linq;
using GamerBoxPresantationLayer.WPF.Models;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        // UI'da gösterilecek veriler
        [ObservableProperty] private string username = "Misafir";
        [ObservableProperty] private string email = "-";
        [ObservableProperty] private string bio = "Henüz bir biyografi yok.";
        [ObservableProperty] private int followersCount = 0;
        [ObservableProperty] private int followingCount = 0;

        // Post Listesi
        public ObservableCollection<PostDisplayModel> UserPosts { get; } = new ObservableCollection<PostDisplayModel>();

        public ProfileViewModel(IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;
        }

        // Veriyi yükleyen metod
        public async Task LoadUserDataAsync(int userId)
        {
            var user = await _userService.GetByIdAsyncB(userId);
            if (user != null)
            {
                Username = user.Username;
                Email = user.Email;
                Bio = user.Bio;
                FollowersCount = user.Followers?.Count ?? 0;
                FollowingCount = user.Following?.Count ?? 0;

                // Postları Yükle
                UserPosts.Clear();
                var posts = await _postService.GetByUserIdAsyncB(userId);
                foreach (var p in posts)
                {
                    UserPosts.Add(new PostDisplayModel
                    {
                        Content = p.Content,
                        DateStr = p.CreatedAt.ToString("dd MMM yyyy HH:mm"),
                        HashtagsStr = p.Hashtags != null ? string.Join(" ", p.Hashtags.Select(h => "#" + h)) : ""
                    });
                }
            }
        }
    }

   
   
}