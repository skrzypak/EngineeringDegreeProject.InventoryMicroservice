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
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(a => a.ProductId).IsRequired();
            modelBuilder.Property(a => a.InvoicingSupplierId).IsRequired();
            modelBuilder.Property(a => a.InvoicingDocumentId).IsRequired();
            modelBuilder.Property(a => a.InvoicingDocumentToProductId).IsRequired();

            modelBuilder.Property(a => a.NumOfAvailable).HasDefaultValue(0).IsRequired();
            modelBuilder.Property(a => a.NumOfSettled).HasDefaultValue(0).IsRequired();
            modelBuilder.Property(a => a.NumOfSpoiled).HasDefaultValue(0).IsRequired();
            modelBuilder.Property(a => a.ExpirationDate).IsRequired(false);

            modelBuilder.ToTable("Inventories");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.ProductId).HasColumnName("ProductId");
            modelBuilder.Property(a => a.InvoicingSupplierId).HasColumnName("SupplierId");
            modelBuilder.Property(a => a.InvoicingDocumentId).HasColumnName("DocumentId");
            modelBuilder.Property(a => a.InvoicingDocumentToProductId).HasColumnName("DocumentToProductId");
            modelBuilder.Property(a => a.NumOfAvailable).HasColumnName("NumOfAvailable");
            modelBuilder.Property(a => a.NumOfSettled).HasColumnName("NumOfSettled");
            modelBuilder.Property(a => a.NumOfSpoiled).HasColumnName("NumOfSpoiled");
            modelBuilder.Property(a => a.ExpirationDate).HasColumnName("ExpirationDate");
        }
    }
}
