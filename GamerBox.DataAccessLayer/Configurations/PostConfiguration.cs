
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Content)
               .IsRequired()
               .HasMaxLength(300);

        builder.Property(x => x.CreatedAt)
               .IsRequired();

        builder.Property(x => x.Hashtags)
               .HasMaxLength(200);

        
        builder.HasOne(x => x.User)
               .WithMany(x => x.Posts)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        
        builder.HasOne(x => x.Game)
               .WithMany(x => x.Posts)
               .HasForeignKey(x => x.GameId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
