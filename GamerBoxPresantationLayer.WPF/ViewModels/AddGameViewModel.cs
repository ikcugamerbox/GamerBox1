using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Win32; // Dosya seçimi için
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows; // MessageBox için (Şimdilik)

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class AddGameViewModel : ObservableObject
    {
        private readonly IGameService _gameService;
        private int _userId; // Oyunu ekleyen kullanıcının ID'si

        // Pencereyi kapatmak için bir olay (Action) tanımlıyoruz
        public Action RequestClose { get; set; }

        // --- Özellikler (Properties) ---
        [ObservableProperty] private string title;
        [ObservableProperty] private string genre;
        [ObservableProperty] private string description;
        [ObservableProperty] private string priceText = "0"; // TextBox'tan string gelir
        [ObservableProperty] private string imageUrl = "/Resources/treasure-chest.png"; // Varsayılan resim
        [ObservableProperty] private string selectedImagePath; // Dosya yolunu tutar

        public AddGameViewModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        // Kullanıcı ID'sini dışarıdan almak için bir metod
        public void SetUserId(int userId)
        {
            _userId = userId;
        }

        // Resim Seçme Komutu
        [RelayCommand]
        private void SelectImage()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Oyun Görseli Seç";
            op.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";

            if (op.ShowDialog() == true)
            {
                SelectedImagePath = op.FileName;
                ImageUrl = op.FileName; // Ekranda önizlemeyi güncelle
            }
        }

        // Kaydetme Komutu
        [RelayCommand]
        private async Task SaveAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Genre))
                {
                    // true/false parametresi vermezsek varsayılan olarak "Tamam" butonu çıkar.
                    CustomMessageBox.Show("Lütfen oyun adı ve türünü giriniz.", "Eksik Bilgi");
                    return;
                }

                // 1. Resmi Kopyala (Varsa)
                string finalImagePath = "/Resources/treasure-chest.png";
                if (!string.IsNullOrEmpty(SelectedImagePath) && File.Exists(SelectedImagePath))
                {
                    string appPath = AppDomain.CurrentDomain.BaseDirectory;
                    string targetFolder = Path.Combine(appPath, "Images");
                    if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(SelectedImagePath);
                    string destPath = Path.Combine(targetFolder, fileName);

                    File.Copy(SelectedImagePath, destPath, true);
                    finalImagePath = destPath;
                }

                // 2. Oyunu Oluştur
                decimal.TryParse(PriceText, out decimal price);

                var game = new Game
                {
                    Title = Title,
                    Genre = Genre,
                    Description = Description,
                    Price = price,
                    ImageUrl = finalImagePath,
                    ReleaseDate = DateTime.Now,
                    UserId = _userId
                };

                // 3. Servise Gönder
                await _gameService.AddGameAsyncB(game);

                CustomMessageBox.Show("Oyun başarıyla eklendi!", "Başarılı");

                // 4. Pencereyi Kapat (Action Tetikle)
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}