using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryMicroservice.Core.Fluent.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> modelBuilder)
        {
            modelBuilder.HasKey(i => new { i.SupplierId, i.DocumentId, i.DocumentToProductId, i.EspId });

            modelBuilder.Property(i => i.SupplierId).IsRequired();
            modelBuilder.Property(i => i.DocumentId).IsRequired();
            modelBuilder.Property(i => i.DocumentToProductId).IsRequired();
            modelBuilder.Property(i => i.ProductId).IsRequired();

            modelBuilder
                .HasOne(i => i.Product)
                .WithMany(p => p.AsInventoryItem)
                .HasForeignKey(i => new { i.ProductId, i.EspId })
                .HasPrincipalKey(p => new { p.Id, p.EspId });

            modelBuilder.Property(i => i.Id).ValueGeneratedOnAdd().IsRequired();
            modelBuilder.HasIndex(i => i.Id).IsUnique();

            modelBuilder.Property(i => i.NumOfAvailable).HasDefaultValue(0).IsRequired();
            modelBuilder.Property(i => i.NumOfSettled).HasDefaultValue(0).IsRequired();
            modelBuilder.Property(i => i.NumOfSpoiled).HasDefaultValue(0).IsRequired();
            modelBuilder.Property(i => i.NumOfDamaged).HasDefaultValue(0).IsRequired();

            modelBuilder.Property(i => i.Quantity).IsRequired();
            modelBuilder.Property(i => i.UnitMeasureValue).IsRequired();
            modelBuilder.Property(i => i.UnitNetPrice).HasPrecision(13, 3).IsRequired();
            modelBuilder.Property(i => i.PercentageVat).HasPrecision(4, 2).IsRequired();
            modelBuilder.Property(i => i.GrossValue).HasPrecision(13, 3).IsRequired();
            modelBuilder.Property(i => i.ExpirationDate).IsRequired(false);

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("Inventories");
            modelBuilder.Property(i => i.Id).HasColumnName("Id");
            modelBuilder.Property(i => i.ProductId).HasColumnName("ProductId");
            modelBuilder.Property(i => i.SupplierId).HasColumnName("SupplierId");
            modelBuilder.Property(i => i.DocumentId).HasColumnName("DocumentId");
            modelBuilder.Property(i => i.DocumentToProductId).HasColumnName("DocumentToProductId");
            modelBuilder.Property(i => i.NumOfAvailable).HasColumnName("NumOfAvailable");
            modelBuilder.Property(i => i.NumOfSettled).HasColumnName("NumOfSettled");
            modelBuilder.Property(i => i.NumOfSpoiled).HasColumnName("NumOfSpoiled");
            modelBuilder.Property(i => i.NumOfDamaged).HasColumnName("NumOfDamaged");
            modelBuilder.Property(i => i.Quantity).HasColumnName("Quantity");
            modelBuilder.Property(i => i.UnitMeasureValue).HasColumnName("UnitMeasureValue");
            modelBuilder.Property(i => i.UnitNetPrice).HasColumnName("UnitNetPrice");
            modelBuilder.Property(i => i.PercentageVat).HasColumnName("PercentageVat");
            modelBuilder.Property(i => i.GrossValue).HasColumnName("GrossValue");
            modelBuilder.Property(i => i.ExpirationDate).HasColumnName("ExpirationDate");
        }
    }
}
