using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Description)
               .HasMaxLength(500);

        builder.Property(x => x.Genre)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.ReleaseDate)
               .IsRequired();

        // Para birimi hassasiyet ayarı
        builder.Property(x => x.Price)
               .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ImageUrl)
               .HasMaxLength(300);

    
        // Kullanıcı silindiğinde, eğer oyunları varsa silme işlemini engelle (Restrict).
        // Bu sayede "User -> Game -> Post" zinciri kırılır.
        builder.HasOne(x => x.User)
               .WithMany() // User tarafında "Games" listesi tanımlamadığımız için boş bırakıyoruz
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        // Rating İlişkisi
        builder.HasMany(x => x.Ratings)
               .WithOne(x => x.Game)
               .HasForeignKey(x => x.GameId)
               .OnDelete(DeleteBehavior.Cascade);

        // Post İlişkisi
        builder.HasMany(x => x.Posts)
               .WithOne(x => x.Game)
               .HasForeignKey(x => x.GameId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}