using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32; // FileDialog için
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GamerBoxPresantationLayer.WPF
{
    public partial class AddGameWindow : Window
    {
        private readonly IGameService _gameService;
        private readonly int _userId;
        private string _selectedImagePath;

        public AddGameWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;

            if (App.ServiceProvider != null)
            {
                _gameService = App.ServiceProvider.GetService<IGameService>();
            }
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Oyun Görseli Seç";
            op.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";

            if (op.ShowDialog() == true)
            {
                // Önizleme göster
                imgPreview.Source = new BitmapImage(new Uri(op.FileName));
                txtImageUrl.Text = op.FileName;
                _selectedImagePath = op.FileName;
            }
        }
        
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtGenre.Text))
                {
                    MessageBox.Show("Lütfen oyun adı ve türünü giriniz.");
                    return;
                }

                // 1. Resmi Uygulama Klasörüne Kopyala
                string finalImagePath = "/Resources/treasure-chest.png"; // Varsayılan

                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    // "Images" klasörü oluştur
                    string appPath = AppDomain.CurrentDomain.BaseDirectory;
                    string targetFolder = Path.Combine(appPath, "Images");
                    if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

                    // Dosyaya benzersiz isim ver
                    string fileName = Guid.NewGuid() + Path.GetExtension(_selectedImagePath);
                    string destPath = Path.Combine(targetFolder, fileName);

                    File.Copy(_selectedImagePath, destPath, true);
                    finalImagePath = destPath; // Mutlak yol kaydediyoruz (Basitlik için)
                }

                // 2. Oyunu Oluştur
                var game = new Game
                {
                    Title = txtTitle.Text,
                    Genre = txtGenre.Text,
                    Description = txtDesc.Text,
                    Price = decimal.TryParse(txtPrice.Text, out decimal p) ? p : 0,
                    ImageUrl = finalImagePath,
                    ReleaseDate = DateTime.Now,
                    UserId = _userId
                };

                 await _gameService.AddGameAsyncB(game);
                MessageBox.Show("Oyun başarıyla eklendi!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}