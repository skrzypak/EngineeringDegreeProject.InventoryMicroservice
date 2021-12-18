using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryMicroservice.Core.Fluent.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> modelBuilder)
        {
            modelBuilder.HasKey(a => new { a.Id, a.EspId });
            modelBuilder.Property(a => a.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(a => a.CategoryId).IsRequired();

            modelBuilder
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => new { p.CategoryId, p.EspId })
                .HasPrincipalKey(c => new { c.Id, c.EspId });

            modelBuilder.Property(a => a.Code).HasMaxLength(6).IsRequired(false);
            modelBuilder.Property(a => a.Name).HasMaxLength(300).IsRequired();
            modelBuilder.Property(a => a.Unit).HasConversion<string>().HasMaxLength(10).IsRequired();
            modelBuilder.Property(a => a.Description).HasMaxLength(3000).IsRequired(false);

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.Property(a => a.Calories).HasDefaultValue<int?>(null).IsRequired(false);
            modelBuilder.Property(a => a.Proteins).HasDefaultValue<float?>(null).IsRequired(false);
            modelBuilder.Property(a => a.Carbohydrates).HasDefaultValue<float?>(null).IsRequired(false);
            modelBuilder.Property(a => a.Fats).HasDefaultValue<float?>(null).IsRequired(false);

            modelBuilder.ToTable("Products");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.CategoryId).HasColumnName("CategoryId");
            modelBuilder.Property(a => a.Name).HasColumnName("Name");
            modelBuilder.Property(a => a.Unit).HasColumnName("Unit");
            modelBuilder.Property(a => a.Description).HasColumnName("Description");

            modelBuilder.Property(a => a.Calories).HasColumnName("Calories");
            modelBuilder.Property(a => a.Proteins).HasColumnName("Proteins");
            modelBuilder.Property(a => a.Carbohydrates).HasColumnName("Carbohydrates");
            modelBuilder.Property(a => a.Fats).HasColumnName("Fats");
        }
    }
}
