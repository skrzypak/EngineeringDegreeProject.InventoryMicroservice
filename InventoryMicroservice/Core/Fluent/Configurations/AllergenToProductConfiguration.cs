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
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(a => a.AllergenId).IsRequired();
            modelBuilder.Property(a => a.ProductId).IsRequired();

            modelBuilder.ToTable("AllergensToProducts");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.AllergenId).HasColumnName("AllergenId");
            modelBuilder.Property(a => a.ProductId).HasColumnName("ProductId");
        }
    }
}
