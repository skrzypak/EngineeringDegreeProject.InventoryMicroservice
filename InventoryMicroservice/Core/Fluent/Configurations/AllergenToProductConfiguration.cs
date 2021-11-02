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
            modelBuilder.HasKey(a => new { a.AllergenId, a.ProductId });

            modelBuilder.Property(a => a.AllergenId).IsRequired();
            modelBuilder.Property(a => a.ProductId).IsRequired();

            modelBuilder.ToTable("AllergensToProducts");
            modelBuilder.Property(a => a.AllergenId).HasColumnName("AllergenId");
            modelBuilder.Property(a => a.ProductId).HasColumnName("ProductId");
        }
    }
}
