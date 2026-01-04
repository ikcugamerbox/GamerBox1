using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input; // Eğer tıklayınca profile gitmek istersen
using GamerBoxPresantationLayer.WPF.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace GamerBoxPresantationLayer.WPF.ViewModels
{
    public partial class UserListViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title; // Pencere başlığı (örn: "Takipçiler")

        // Listede gösterilecek kullanıcılar
        public ObservableCollection<UserDisplayModel> Users { get; set; } = new ObservableCollection<UserDisplayModel>();

        public UserListViewModel(string pageTitle, IEnumerable<UserDisplayModel> users)
        {
            Title = pageTitle;
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        // Opsiyonel: Listeden bir kullanıcıya tıklayınca yapılacak işlem
        [RelayCommand]
        private void GoToUserProfile(UserDisplayModel user)
        {
            // İleride buraya tıklanan kullanıcının profiline gitme kodu eklenebilir.
            // Şimdilik sadece mesaj gösterelim.
            MessageBox.Show($"{user.Username} profiline gidiliyor...");
        }
    }
}