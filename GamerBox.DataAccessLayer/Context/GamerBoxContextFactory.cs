using GamerBox.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GamerBox.DataAccessLayer
{
    // Bu sınıf sadece "Update-Database" komutu çalıştırılırken devreye girer.
    public class GamerBoxContextFactory : IDesignTimeDbContextFactory<GamerBoxContext>
    {
        public GamerBoxContext CreateDbContext(string[] args)
        {
            // 1. Yapılandırma dosyasını (appsettings.json) bul ve inşa et
            // Directory.GetCurrentDirectory() genellikle startup projesinin (WPF) olduğu yeri gösterir.
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2. Bağlantı dizesini yapılandırmadan oku
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // 3. DbContextOptionsBuilder'ı hazırla
            var optionsBuilder = new DbContextOptionsBuilder<GamerBoxContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new GamerBoxContext(optionsBuilder.Options);
        }
    }
}