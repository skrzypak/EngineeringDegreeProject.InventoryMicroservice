using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryMicroservice.Core.Fluent.Configurations
{
    public class AllergenToProductConfiguration : IEntityTypeConfiguration<AllergenToProduct>
    {
        public void Configure(EntityTypeBuilder<AllergenToProduct> modelBuilder)
        {
            modelBuilder.HasKey(a => new { a.AllergenId, a.ProductId, a.EspId });

            modelBuilder.Property(a => a.AllergenId).IsRequired();
            modelBuilder.Property(a => a.ProductId).IsRequired();

            modelBuilder
                .HasOne(a2p => a2p.Allergen)
                .WithMany(a => a.AllergensToProducts)
                .HasForeignKey(a2p => new { a2p.AllergenId, a2p.EspId })
                .HasPrincipalKey(a => new { a.Id, a.EspId });

            modelBuilder
                .HasOne(a2p => a2p.Product)
                .WithMany(p => p.AllergensToProducts)
                .HasForeignKey(a2p => new { a2p.ProductId, a2p.EspId })
                .HasPrincipalKey(p => new { p.Id, p.EspId });

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("AllergensToProducts");
            modelBuilder.Property(a => a.AllergenId).HasColumnName("AllergenId");
            modelBuilder.Property(a => a.ProductId).HasColumnName("ProductId");
        }
    }
}
