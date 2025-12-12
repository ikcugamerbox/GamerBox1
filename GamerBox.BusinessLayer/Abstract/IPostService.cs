using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
//abstractta interfaceler bulunur.
{
    public interface IPostService : IGenericService<Post>
    //IPostService IGenericService den miras alır.
    {
        Task<List<Post>> GetByUserIdAsyncB(int userId);
        //bir kullanıcının yaptığı tüm postları döner.
        Task<List<Post>> GetByGameIdAsyncB(int gameId);
        //bir oyuna ait tüm postları döner. 
        Task<Post> CreatePostAsyncB(int userId, int? gameId, string content);
        //yeni bir post oluşturur.bir oyuna bağlı gönderiyse oyun id si de verilir .content postun içerik metni. döndürdüğü değer oluşturulan post .
        List<string> ExtractHashtags(string content);
        //paylaşım metnindeki hastagleri bulup liste olarak döner .
        Task<List<Post>> GetRecentPostsAsyncB(int count);

    }
}