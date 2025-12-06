using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.Email)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(x => x.Email)
               .IsUnique();

        builder.Property(x => x.PasswordHash)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Bio)
               .HasMaxLength(300);

        builder.Property(x => x.ThemePreference)
               .HasMaxLength(50);

        builder.Property(x => x.FavoriteGenres)
               .HasMaxLength(200);

        
        builder.HasMany(x => x.Posts)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        
        builder.HasMany(x => x.Ratings)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        
        builder
            .HasMany(u => u.Followers)
            .WithMany(u => u.Following)
            .UsingEntity<Dictionary<string, object>>(
                "UserFollowers",
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("FollowerId")
                    .OnDelete(DeleteBehavior.Restrict),   

                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("FollowingId")
                    .OnDelete(DeleteBehavior.Restrict),   

                j =>
                {
                    j.HasKey("FollowerId", "FollowingId");
                    j.ToTable("UserFollowers");
                }
            );
    }
}