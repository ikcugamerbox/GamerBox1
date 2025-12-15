using GamerBox.EntitiesLayer.Concrete;
using GamerBoxPresantationLayer.WPF.Classes;
using GamerBoxPresantationLayer.WPF.Views.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace GamerBoxPresantationLayer.WPF
{
    // INotifyPropertyChanged: Arayüzün (UI) değişkenlerdeki değişimi (Giriş yaptı/yaptı) algılamasını sağlar.
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _isLoggedIn;
        private string _userName = "Misafir";
        private User? _currentUser;

        // --- BINDING ÖZELLİKLERİ ---
        // Bu özellikler değiştiğinde arayüz otomatik güncellenir.

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set { _isLoggedIn = value; OnPropertyChanged(); }
        }

        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        public User? CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            // ESKİ KOD: MainContent.Content = new UCHome(); -> Bu artık çalışmaz.

            // YENİ KOD: UCHome'u servis sağlayıcıdan istiyoruz.
            // O da bize içine ViewModel'i yerleştirilmiş hazır bir sayfa veriyor.
            if (App.ServiceProvider != null)
            {
                MainContent.Content = App.ServiceProvider.GetService<UCHome>();
            }
        }

        // --- PENCERE KONTROLLERİ ---

        // Pencereyi sürükleme
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Sadece sol tık ile sürükleme yapılsın
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        // Kapat
        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Simge Durumu
        private void btnStbClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Tam Ekran / Normal
        private void btnFullScClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        // --- NAVİGASYON (MENÜ BUTONLARI) ---

        private void btnHomeClick(object sender, RoutedEventArgs e)
        {
            // UCHome'u ServiceProvider üzerinden alıyoruz, böylece ViewModel de içine otomatik enjekte ediliyor.
            MainContent.Content = App.ServiceProvider.GetService<UCHome>();
        }

        private void btnRvsClick(object sender, RoutedEventArgs e)
        {
            if (App.ServiceProvider != null)
            {
                MainContent.Content = App.ServiceProvider.GetService<UCReviews>();
            }
        }

        private void btnListClick(object sender, RoutedEventArgs e)
        {
            if (App.ServiceProvider != null)
            {
                MainContent.Content = App.ServiceProvider.GetService<UCLists>();
            }
        }

        private void btnWtListClick(object sender, RoutedEventArgs e)
        {
            // Watchlist sayfasını servisten istiyoruz
            if (App.ServiceProvider != null)
            {
                MainContent.Content = App.ServiceProvider.GetService<UCWatchtLists>();
            }
        }

        private void btnProfileClick(object sender, RoutedEventArgs e)
        {
 

            if (!IsLoggedIn)
            {
                CustomMessageBox.Show("Profilinizi görüntülemek için lütfen giriş yapın.", "Erişim Reddedildi");
                return;
            }

            if (App.ServiceProvider != null)
            {
                // Sayfayı çağırmayı dene
                try
                {
                    var profilePage = App.ServiceProvider.GetService<UCProfile>();
                    if (profilePage == null)
                    {
                        MessageBox.Show("HATA: UCProfile servisten NULL döndü!");
                    }
                    else
                    {
                        MainContent.Content = profilePage;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sayfa açılırken hata oluştu: {ex.Message}");
                }
            }
        }

        // --- GİRİŞ / ÇIKIŞ İŞLEMLERİ ---

        private void btnSignInCli(object sender, RoutedEventArgs e)
        {
            // SignIn penceresini aç
            // App.ServiceProvider üzerinden alıyoruz ki bağımlılıkları (UserService) otomatik gelsin.
            var signInWindow = App.ServiceProvider.GetService(typeof(SignInWindow)) as SignInWindow;

            if (signInWindow != null)
            {
                signInWindow.Owner = this;
                this.Opacity = 0.4; // Arka planı biraz karart
                signInWindow.ShowDialog();
                // Pencere kapandığında SignInWindow içindeki kodlar bu pencerenin Opacity'sini düzeltecek.
            }
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            

            bool answer = CustomMessageBox.Show("Çıkış yapmak istediğinize emin misiniz?", "Çıkış", true);

            if (answer) // Eğer Evet'e bastıysa
            {
                IsLoggedIn = false;
                CurrentUser = null;
                UserName = "Misafir";
                if (App.ServiceProvider != null)
                {
                    MainContent.Content = App.ServiceProvider.GetService<UCHome>();
                }
            }
        }

        // --- INotifyPropertyChanged ARAYÜZ GÜNCELLEME ---
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void btnThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            Classes.ThemeHelper.ToggleTheme();
        }
    }
}