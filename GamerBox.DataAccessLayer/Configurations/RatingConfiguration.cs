
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Score)
               .IsRequired();

        builder.Property(x => x.RatedAt)
               .IsRequired();

        
        builder.HasOne(x => x.User)
               .WithMany(x => x.Ratings)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        
        builder.HasOne(x => x.Game)
               .WithMany(x => x.Ratings)
               .HasForeignKey(x => x.GameId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}