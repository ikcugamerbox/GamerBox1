using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
//abstractta interfaceler bulunur.
{
    public interface IPostService : IGenericService<Post>
    //IPostService IGenericService den miras alır.
    {
        List<Post> GetByUserId(int userId);
        //bir kullanıcının yaptığı tüm postları döner.
        List<Post> GetByGameId(int gameId);
        //bir oyuna ait tüm postları döner. 
        Post CreatePost(int userId, int? gameId, string content);
        //yeni bir post oluşturur.bir oyuna bağlı gönderiyse oyun id si de verilir .content postun içerik metni. döndürdüğü değer oluşturulan post .
        List<string> ExtractHashtags(string content);
        //paylaşım metnindeki hastagleri bulup liste olarak döner .

        void Add(Post post);
    }
}