using GamerBoxPresantationLayer.WPF.ViewModels;
using System.Windows;

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

    
    }
}