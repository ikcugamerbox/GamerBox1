using System.Windows;

namespace GamerBoxPresantationLayer.WPF.Views.Windows
{
    public partial class EditPostWindow : Window
    {
        public string UpdatedContent { get; private set; }

        // currentContent parametresi, gönderinin "eski halini" taşır.
        public EditPostWindow(string currentContent)
        {
            InitializeComponent();

            // 1. Eski metni kutucuğa dolduruyoruz
            txtContent.Text = currentContent;

            // 2. Pencere yüklendiğinde yapılacaklar (UX İyileştirmesi)
            Loaded += (s, e) =>
            {
                // Kutucuğa odaklan (Kullanıcı hemen yazabilsin)
                txtContent.Focus();

                // İmleci metnin en sonuna taşı (Kullanıcı silmekle uğraşmasın, devamını yazabilsin)
                txtContent.CaretIndex = txtContent.Text.Length;
            };
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Güncellenmiş metni değişkene alıp pencereyi kapat
            UpdatedContent = txtContent.Text;
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}