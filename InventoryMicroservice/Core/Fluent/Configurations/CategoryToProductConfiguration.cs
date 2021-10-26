using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryMicroservice.Core.Fluent.Configurations
{
    public class CategoryToProductConfiguration : IEntityTypeConfiguration<CategoryToProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryToProduct> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(a => a.CategoryId).IsRequired();
            modelBuilder.Property(a => a.ProductId).IsRequired();

            modelBuilder.ToTable("CategoriesToProducts");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.CategoryId).HasColumnName("CategoryId");
            modelBuilder.Property(a => a.ProductId).HasColumnName("ProductId");
        }
    }
}
