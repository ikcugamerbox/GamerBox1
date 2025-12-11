using GamerBox.EntitiesLayer.Concrete;
using GamerBoxPresantationLayer.WPF.Classes;
using GamerBoxPresantationLayer.WPF.Views.UserControls;
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
            this.DataContext = this; // Binding işlemleri için bağlamı kendisi yapıyoruz.

            // Uygulama açılışında varsayılan sayfa Home olsun
            MainContent.Content = new UCHome();
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
            MainContent.Content = new UCHome();
        }

        private void btnRvsClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCReviews();
        }

        private void btnListClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCLists();
        }

        private void btnWtListClick(object sender, RoutedEventArgs e)
        {
            // İsterseniz burada giriş kontrolü yapabilirsiniz
            MainContent.Content = new UCWatchtLists();
        }

        private void btnProfileClick(object sender, RoutedEventArgs e)
        {
            if (!IsLoggedIn)
            {
                MessageBox.Show("Profilinizi görüntülemek için lütfen giriş yapın.", "Erişim Reddedildi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MainContent.Content = new UCProfile();
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
            if (MessageBox.Show("Çıkış yapmak istediğinize emin misiniz?", "Çıkış", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // Bilgileri sıfırla
                IsLoggedIn = false;
                CurrentUser = null;
                UserName = "Misafir";

                // Ana sayfaya yönlendir
                MainContent.Content = new UCHome();
            }
        }

        // --- INotifyPropertyChanged ARAYÜZ GÜNCELLEME ---
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}