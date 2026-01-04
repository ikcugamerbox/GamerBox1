namespace GamerBoxPresantationLayer.WPF.Messages
{
    // Bu mesaj, herhangi bir yerden "Şu kullanıcının profilini aç" demek için kullanılacak.
    public class OpenProfileMessage
    {
        public int UserId { get; }
        public OpenProfileMessage(int userId) => UserId = userId;
    }
}