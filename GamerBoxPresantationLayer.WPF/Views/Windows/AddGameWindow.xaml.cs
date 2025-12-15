using GamerBoxPresantationLayer.WPF.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace GamerBoxPresantationLayer.WPF
{
    public partial class AddGameWindow : Window
    {
        public AddGameViewModel ViewModel { get; }

        // Constructor Injection
        public AddGameWindow(AddGameViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;

            // ViewModel "Kapat" dediğinde pencereyi kapat
            ViewModel.RequestClose += () => this.Close();
        }

        // Kullanıcı ID'sini ayarlamak için yardımcı metod
        public void Initialize(int userId)
        {
            ViewModel.SetUserId(userId);
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}