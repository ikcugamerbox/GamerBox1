using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

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
        // Veritabanından okurken (v) eğer null gelirse boş liste dön, değilse Split yap.
        builder.Property(e => e.Hashtags)
               .HasConversion(
                 v => string.Join(',', v), // Yazarken: Listeyi virgülle birleştirip string yap
                 v => v == null ? new List<string>() : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()) // Okurken: Null kontrolü yap!
               .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                 (c1, c2) => c1.SequenceEqual(c2),
                 c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                 c => c.ToList()));

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