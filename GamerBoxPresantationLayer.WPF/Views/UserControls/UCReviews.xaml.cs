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
        private readonly IPostDal _postDal;
        private readonly IUserDal _userDal;
        private readonly IGameDal _gameDal;

        public ObservableCollection<ReviewDisplayModel> RecentPosts { get; set; } = new ObservableCollection<ReviewDisplayModel>();

        public UCReviews()
        {
            InitializeComponent();

            // Servisleri App.ServiceProvider üzerinden alıyoruz
            if (App.ServiceProvider != null)
            {
                _postDal = App.ServiceProvider.GetService<IPostDal>();
                _userDal = App.ServiceProvider.GetService<IUserDal>();
                _gameDal = App.ServiceProvider.GetService<IGameDal>();

                LoadPosts();
            }

            this.DataContext = this;
        }

        private void LoadPosts()
        {
            // Son 20 postu getir (DAL'da GetRecentPosts metodu vardı)
            var posts = _postDal.GetRecentPosts(20);

            foreach (var post in posts)
            {
                // İlişkili verileri manuel yükleme (Lazy Loading kapalıysa gerekebilir)
                var user = _userDal.GetById(post.UserId);
                var game = post.GameId.HasValue ? _gameDal.GetById(post.GameId.Value) : null;

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