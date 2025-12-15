using System.Windows;

namespace GamerBoxPresantationLayer.WPF
{
    public partial class CustomMessageBox : Window
    {
        // Kullanıcının cevabını tutacak değişken (True: Evet/Tamam, False: Hayır/İptal)
        public bool Result { get; private set; } = false;

        // Constructor'ı private  sadece Show metoduyla açılabilsin
        private CustomMessageBox(string message, string title, bool isConfirmation)
        {
            InitializeComponent();
            lblTitle.Text = title;
            lblMessage.Text = message;

            if (isConfirmation)
            {
                // Soru soruyorsak (Evet/Hayır)
                btnOk.Content = "Evet";
                btnCancel.Content = "Hayır";
                btnCancel.Visibility = Visibility.Visible;
            }
            else
            {
                // Sadece bilgi veriyorsak (Tamam)
                btnOk.Content = "Tamam";
                btnCancel.Visibility = Visibility.Collapsed;
            }
        }


        // Bu static metot sayesinde pencereyi tek satır ile çağırabiliriz
        public static bool Show(string message, string title = "Bilgi", bool isConfirmation = false)
        {
            var msgBox = new CustomMessageBox(message, title, isConfirmation);

            // Eğer ana pencere açıksa, mesaj kutusunu onun ortasında aç
            if (Application.Current.MainWindow != null && Application.Current.MainWindow.IsVisible)
            {
                msgBox.Owner = Application.Current.MainWindow;
                msgBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            msgBox.ShowDialog();
            return msgBox.Result;
        }

        // Pencereyi sürükleyebilmek için
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            this.Close();
        }
    }
}