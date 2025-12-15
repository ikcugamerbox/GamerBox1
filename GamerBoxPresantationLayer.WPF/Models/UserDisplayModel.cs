namespace GamerBoxPresantationLayer.WPF.Models
{
    public class UserDisplayModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Bio { get; set; }

        // Arayüzde göstermek için kısa bir özet (Örn: Bio'nun ilk 50 karakteri)
        public string ShortBio => !string.IsNullOrEmpty(Bio) && Bio.Length > 50
                                  ? Bio.Substring(0, 50) + "..."
                                  : Bio;

        // Varsayılan resim mantığı (Resim yoksa emoji veya default png)
        public string DisplayImage => string.IsNullOrEmpty(ProfilePictureUrl)
                                      ? ""
                                      : ProfilePictureUrl;
    }
}