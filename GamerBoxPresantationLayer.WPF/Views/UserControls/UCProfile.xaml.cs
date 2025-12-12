using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCProfile : UserControl, INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private User _currentUser;

        // Binding Properties
        public string Username => _currentUser?.Username ?? "Misafir";
        public string Email => _currentUser?.Email ?? "-";
        public string Bio => _currentUser?.Bio ?? "Henüz bir biyografi yok.";
        public int FollowersCount => _currentUser?.Followers?.Count ?? 0;
        public int FollowingCount => _currentUser?.Following?.Count ?? 0;

        public ObservableCollection<PostDisplayModel> UserPosts { get; set; } = new ObservableCollection<PostDisplayModel>();

        public UCProfile()
        {
            InitializeComponent();

            // Servisleri Al
            if (App.ServiceProvider != null)
            {
                _userService = App.ServiceProvider.GetService<IUserService>();
                _postService = App.ServiceProvider.GetService<IPostService>();

                LoadUserData();
            }

            this.DataContext = this;
        }

        private async void LoadUserData()
        {
            // MainWindow'dan aktif kullanıcıyı al
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin != null && mainWin.CurrentUser != null)
            {
                // Veritabanından güncel halini çek (Takipçi sayıları vs. için)
                _currentUser = await _userService.GetByIdAsyncB(mainWin.CurrentUser.Id);

                // Gönderileri Çek
                var posts = await _postService.GetByUserIdAsyncB(_currentUser.Id);
                foreach (var p in posts)
                {
                    UserPosts.Add(new PostDisplayModel
                    {
                        Content = p.Content,
                        DateStr = p.CreatedAt.ToString("dd MMM yyyy HH:mm"),
                        HashtagsStr = p.Hashtags != null ? string.Join(" ", p.Hashtags.Select(h => "#" + h)) : ""
                    });
                }

                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Bio));
                OnPropertyChanged(nameof(FollowersCount));
                OnPropertyChanged(nameof(FollowingCount));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class PostDisplayModel
    {
        public string Content { get; set; }
        public string DateStr { get; set; }
        public string HashtagsStr { get; set; }
    }
}