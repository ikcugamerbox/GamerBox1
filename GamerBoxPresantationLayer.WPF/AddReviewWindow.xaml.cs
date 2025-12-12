using GamerBox.BusinessLayer.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF
{
    public partial class AddReviewWindow : Window
    {
        private readonly IPostService _postService;
        private readonly IRatingService _ratingService;
        private readonly int _gameId;
        private readonly int _userId;

        public AddReviewWindow(int gameId, string gameTitle, int userId)
        {
            InitializeComponent();
            _gameId = gameId;
            _userId = userId;
            txtGameTitle.Text = gameTitle;

            if (App.ServiceProvider != null)
            {
                _postService = App.ServiceProvider.GetService<IPostService>();
                _ratingService = App.ServiceProvider.GetService<IRatingService>();
            }
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string content = txtContent.Text;
                int score = cmbRating.SelectedIndex + 1; // ComboBox 0'dan başlar

                // 1. Post Oluştur
                await _postService.CreatePostAsyncB(_userId, _gameId, content);

                // 2. Puan Ver (Hata verirse kullanıcı daha önce puan vermiş demektir, yakalayalım)
                try
                {
                   await _ratingService.HasUserRatedAsyncB(_userId, _gameId);
                }
                catch (Exception)
                {
                    // Puan zaten verilmiş olabilir, sorun değil, postu attık.
                }

                MessageBox.Show("İncelemeniz paylaşıldı!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}