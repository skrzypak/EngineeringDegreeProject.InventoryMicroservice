using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryMicroservice.Core.Fluent.Configurations
{
    public class InventoryOperationConfiguration : IEntityTypeConfiguration<InventoryOperation>
    {
        public void Configure(EntityTypeBuilder<InventoryOperation> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(a => a.InventoryId).IsRequired();

            modelBuilder.Property(a => a.Quantity).HasDefaultValue(0).IsRequired();
            modelBuilder.Property(a => a.Operation).HasConversion<int>().IsRequired();
            modelBuilder.Property(a => a.Description).HasMaxLength(3000).IsRequired(false);

            modelBuilder.ToTable("InventoriesOperations");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.InventoryId).HasColumnName("InventoryId");
            modelBuilder.Property(a => a.Quantity).HasColumnName("Quantity");
            modelBuilder.Property(a => a.Operation).HasColumnName("Operation");
            modelBuilder.Property(a => a.Description).HasColumnName("Description");
            modelBuilder.Property(a => a.Date).HasColumnName("Date");
        }
    }
}
