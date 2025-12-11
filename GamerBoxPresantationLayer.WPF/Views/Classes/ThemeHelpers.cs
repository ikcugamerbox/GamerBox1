using System;
using System.Windows;

namespace GamerBoxPresantationLayer.WPF.Classes
{
    public static class ThemeHelper
    {
        public static bool IsDarkTheme { get; private set; } = true;

        public static void ToggleTheme()
        {
            // Mevcut temayı kaldır
            Application.Current.Resources.MergedDictionaries.Clear();

            // Yeni temayı belirle
            string themeFile = IsDarkTheme ? "Themes/LightTheme.xaml" : "Themes/DarkTheme.xaml";

            // Yeni ResourceDictionary'i oluştur ve ekle
            ResourceDictionary newTheme = new ResourceDictionary
            {
                Source = new Uri(themeFile, UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries.Add(newTheme);

            // Durumu güncelle
            IsDarkTheme = !IsDarkTheme;
        }
    }
}