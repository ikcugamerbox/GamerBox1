using GamerBoxPresantationLayer.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCReviews : UserControl
    {
        public ReviewsViewModel ViewModel { get; }

        // Dependency Injection ile ViewModel geliyor
        public UCReviews(ReviewsViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;

            this.Loaded += async (s, e) => await ViewModel.LoadPostsAsync();
        }
    }
}