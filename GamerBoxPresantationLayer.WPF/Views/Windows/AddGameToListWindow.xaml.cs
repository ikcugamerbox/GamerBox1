using GamerBox.BusinessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace GamerBoxPresantationLayer.WPF
{
    public partial class AddGameToListWindow : Window
    {
        private readonly IUserListService _userListService;
        private readonly int _userId;
        private readonly int _gameId;

        public AddGameToListWindow(int userId, int gameId)
        {
            InitializeComponent();
            _userId = userId;
            _gameId = gameId;

            // Servisi al
            if (App.ServiceProvider != null)
            {
                _userListService = App.ServiceProvider.GetService<IUserListService>();
                LoadLists();
            }
        }

        private async void LoadLists()
        {
            // Kullanıcının listelerini çek ve ComboBox'a doldur
            List<UserList> lists = await _userListService.GetUserListsAsyncB(_userId);
            cmbLists.ItemsSource = lists;

            if (lists.Count > 0)
                cmbLists.SelectedIndex = 0; // İlkini seçili yap
            else
            {
                CustomMessageBox.Show("Henüz hiç listeniz yok. Önce 'Listeler' sayfasından bir liste oluşturun.", "Uyarı");
                this.Close();
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbLists.SelectedValue == null) return;

            int listId = (int)cmbLists.SelectedValue;

            try
            {
                await _userListService.AddGameToListAsyncB(listId, _gameId);
                CustomMessageBox.Show("Oyun listeye eklendi!", "Başarılı");
                this.Close();
            }
            catch (System.Exception ex)
            {
                CustomMessageBox.Show($"Hata: {ex.Message}", "Hata");
            }
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