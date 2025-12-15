namespace GamerBoxPresantationLayer.WPF.Models
{
    public class UserListDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GameCount { get; set; }

        // Arayüzde göstermek için hazır metin
        public string DisplayText => $"{Name} ({GameCount} Oyun)";
    }
}