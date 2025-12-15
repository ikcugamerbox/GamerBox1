using GamerBoxPresantationLayer.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace GamerBoxPresantationLayer.WPF.Views.UserControls
{
    public partial class UCListDetails : UserControl
    {
        public ListDetailsViewModel ViewModel { get; }

        public UCListDetails()
        {
            InitializeComponent();
            if (App.ServiceProvider != null)
            {
                ViewModel = App.ServiceProvider.GetService<ListDetailsViewModel>();
                DataContext = ViewModel;
            }
        }

        public async void Initialize(int listId, int userId, string listName)
        {
            await ViewModel.LoadDataAsync(listId, userId, listName);
        }
    }
}