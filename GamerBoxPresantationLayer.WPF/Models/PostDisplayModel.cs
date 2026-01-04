using CommunityToolkit.Mvvm.ComponentModel;

namespace GamerBoxPresantationLayer.WPF.Models
{
    // 'partial' ve 'ObservableObject' eklemeyi unutma
    public partial class PostDisplayModel : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        private string content;
        public string DateStr { get; set; }
        public string HashtagsStr { get; set; }

    }
}