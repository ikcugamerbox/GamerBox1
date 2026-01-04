using System.Windows;

namespace GamerBoxPresantationLayer.WPF.Views.Windows
{
    public partial class UserListWindow : Window
    {
        public UserListWindow(object viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}