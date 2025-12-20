using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerBox.BusinessLayer.Abstract;
using GamerBoxPresantationLayer.WPF.Models;
using GamerBoxPresantationLayer.WPF.Services;
using System.Collections.ObjectModel;
using System.IO;
using GamerBoxPresantationLayer.WPF.Views.Windows;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private int _currentUserId; 
        private readonly IDialogService _dialogService;

        // UI'da gösterilecek veriler
        [ObservableProperty] private string username = "Misafir";
        [ObservableProperty] private string email = "-";
        [ObservableProperty] private string bio = "Henüz bir biyografi yok.";
        [ObservableProperty] private int followersCount = 0;
        [ObservableProperty] private int followingCount = 0;
        [ObservableProperty] private string profilePictureUrl = "/Resources/default_avatar.png";

        // Post Listesi
        public ObservableCollection<PostDisplayModel> UserPosts { get; } = new ObservableCollection<PostDisplayModel>();
        public ObservableCollection<UserDisplayModel> FollowersList { get; } = new ObservableCollection<UserDisplayModel>();

        public ProfileViewModel(IUserService userService, IPostService postService, IDialogService dialogService)
        {
            _userService = userService;
            _postService = postService;
            _dialogService = dialogService;
        }

        // Veriyi yükleyen metod
        public async Task LoadUserDataAsync(int userId)
        {
            _currentUserId = userId;
            try
            {
                // Kullanıcı bilgilerini çek
                var user = await _userService.GetByIdAsyncB(userId);
                if (user != null)
                {
                    Username = user.Username;
                    Email = user.Email;
                    Bio = string.IsNullOrEmpty(user.Bio) ? "Henüz bir biyografi yok." : user.Bio;
                    FollowersCount = user.Followers?.Count ?? 0;
                    FollowingCount = user.Following?.Count ?? 0;

                    if (!string.IsNullOrEmpty(user.ProfilePictureUrl) && File.Exists(user.ProfilePictureUrl))
                    {
                        ProfilePictureUrl = user.ProfilePictureUrl;
                    }
                    else
                    {
                        // Varsayılan bir ikon veya proje içindeki bir kaynak
                        ProfilePictureUrl = ""; // XAML'da boşsa emoji gösterecek şekilde ayarlayacağız.
                    }

                    // Postları çek
                    UserPosts.Clear();
                    var posts = await _postService.GetByUserIdAsyncB(userId);

                    foreach (var p in posts)
                    {
                        UserPosts.Add(new PostDisplayModel
                        {
                            Id = p.Id,
                            Content = p.Content,
                            DateStr = p.CreatedAt.ToString("dd MMM yyyy HH:mm"),
                            HashtagsStr = p.Hashtags != null ? string.Join(" ", p.Hashtags.Select(h => "#" + h)) : ""
                        });
                    }
                }
                else
                {
                    // Eğer kullanıcı null gelirse
                    Username = "Kullanıcı Bulunamadı";
                }
            }
            catch (System.Exception ex)
            {
                // Hatayı ekrana bas (Bu sayede output penceresindeki hatanın sebebini göreceğiz)
                System.Windows.MessageBox.Show($"Profil yüklenirken hata oluştu:\n{ex.Message}\n\nDetay: {ex.InnerException?.Message}", "Hata");
            }
            /*
    var followers = await _userService.GetFollowersAsyncB(userId);
    FollowersList.Clear();
    foreach(var f in followers)
    {
        FollowersList.Add(new UserDisplayModel 
        {
            Id = f.Id,
            Username = f.Username,
            ProfilePictureUrl = f.ProfilePictureUrl,
            Bio = f.Bio
        });
    }
*/

        }

        [RelayCommand]
        private async Task ChangeProfilePictureAsync()
        {
            // 1. Dosya Seçme Penceresi
            string? selectedFilePath = _dialogService.OpenFile("Profil Fotoğrafı Seç", "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp");

            if (selectedFilePath != null)
            {
                try
                {
                    // 2. Klasör Oluşturma (UserImages)
                    string appPath = AppDomain.CurrentDomain.BaseDirectory;
                    string targetFolder = Path.Combine(appPath, "UserImages");

                    if (!Directory.Exists(targetFolder))
                        Directory.CreateDirectory(targetFolder);

                    // 3. Dosyayı Kopyalama (Benzersiz isimle)
                    string extension = Path.GetExtension(selectedFilePath);
                    string newFileName = $"{_currentUserId}_{Guid.NewGuid()}{extension}"; // Örn: 1_aksjdfh-123.jpg
                    string destPath = Path.Combine(targetFolder, newFileName);

                    File.Copy(selectedFilePath, destPath, true);

                    // 4. Veritabanını Güncelleme
                    var user = await _userService.GetByIdAsyncB(_currentUserId);
                    if (user != null)
                    {
                        user.ProfilePictureUrl = destPath;

                        // UserManager.UpdateAsyncB çağırıyoruz (GenericService üzerinden)
                        await _userService.UpdateAsyncB(user);

                        // 5. Arayüzü Güncelle
                        ProfilePictureUrl = destPath;
                        _dialogService.ShowMessage("Profil fotoğrafınız güncellendi!", "Başarılı");
                    }
                }
                catch (Exception ex)
                {
                    _dialogService.ShowMessage($"Fotoğraf yüklenirken hata oluştu: {ex.Message}", "Hata");
                }
            }
        }
        // Fotoğrafı Kaldırma Komutu
        [RelayCommand]
        private async Task RemoveProfilePictureAsync()
        {
            if (string.IsNullOrEmpty(ProfilePictureUrl)) return; // Zaten resim yoksa işlem yapma

            try
            {
                bool result = _dialogService.ShowConfirmation("Profil fotoğrafını kaldırmak istediğine emin misin?", "Fotoğrafı Kaldır");

                if (result)
                {
                    var user = await _userService.GetByIdAsyncB(_currentUserId);
                    if (user != null)
                    {
                        // 1. Veritabanından yolu sil
                        user.ProfilePictureUrl = null;
                        await _userService.UpdateAsyncB(user);

                        // 2. Arayüzü güncelle (Boş yapınca otomatik emojiye dönecek)
                        ProfilePictureUrl = "";

                        _dialogService.ShowMessage("Profil fotoğrafı kaldırıldı.", "Bilgi");
                    }
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage("Hata: " + ex.Message, "Hata");
            }
        }
        [RelayCommand]
        private async Task DeletePostAsync(PostDisplayModel postModel)
        {
            if (postModel == null) return;

            bool confirm = _dialogService.ShowConfirmation("Bu gönderiyi silmek istediğinize emin misiniz?", "Sil");
            if (confirm)
            {
                try
                {
                    var post = await _postService.GetByIdAsyncB(postModel.Id);
                    if (post != null)
                    {
                        await _postService.DeleteAsyncB(post);
                        UserPosts.Remove(postModel); // Listeden de kaldır
                    }
                }
                catch (Exception ex)
                {
                    _dialogService.ShowMessage($"Silme işlemi başarısız: {ex.Message}", "Hata");
                }
            }
        }

        [RelayCommand]
        private async Task EditPostAsync(PostDisplayModel postModel)
        {
            if (postModel == null) return;

            // Yeni pencereyi aç
            var editWindow = new EditPostWindow(postModel.Content);
            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    string newContent = editWindow.UpdatedContent;

                    // Veritabanından orijinal nesneyi çek
                    var post = await _postService.GetByIdAsyncB(postModel.Id);
                    if (post != null)
                    {
                        post.Content = newContent;

                        await _postService.UpdateAsyncB(post);

                        // Arayüzü güncelle
                        postModel.Content = newContent;

                     
                    }
                }
                catch (Exception ex)
                {
                    _dialogService.ShowMessage($"Güncelleme başarısız: {ex.Message}", "Hata");
                }
            }
        }

    }
}