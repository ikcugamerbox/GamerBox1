using GamerBox.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GamerBox.DataAccessLayer
{
    // Bu sınıf sadece "Update-Database" komutu çalıştırılırken devreye girer.
    public class GamerBoxContextFactory : IDesignTimeDbContextFactory<GamerBoxContext>
    {
        public GamerBoxContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GamerBoxContext>();

            // Migration işlemleri için veritabanı bağlantı adresini buraya da yazıyoruz.
            // (appsettings.json ile aynı adres olmalı)
            optionsBuilder.UseSqlServer("Server=EREN\\SQLEXPRESS;Database=GamerBoxDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new GamerBoxContext(optionsBuilder.Options);
        }
    }
}