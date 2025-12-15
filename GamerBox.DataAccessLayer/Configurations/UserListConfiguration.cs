using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserListConfiguration : IEntityTypeConfiguration<UserList>
{
    public void Configure(EntityTypeBuilder<UserList> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

        // Kullanıcı silinirse listeleri de silinsin
        builder.HasOne(x => x.User)
               .WithMany() // User tarafına koleksiyon eklemedim, gerekirse ekleriz
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        // Bir listede çok oyun, bir oyun çok listede olabilir.
        builder.HasMany(x => x.Games)
               .WithMany();
    }
}