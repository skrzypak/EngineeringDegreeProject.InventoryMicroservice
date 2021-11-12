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
            modelBuilder.HasKey(i => new { i.Id, i.EspId });
            modelBuilder.Property(i => i.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.HasOne(i => i.Inventory)
                .WithMany(inv => inv.InventoryOperations)
                .HasForeignKey(i => new { i.InventoryId , i.EspId})
                .HasPrincipalKey(inv => new { inv.Id, inv.EspId });

            modelBuilder.Property(i => i.InventoryId).IsRequired();

            modelBuilder.Property(i => i.Quantity).HasDefaultValue(0).IsRequired();
            modelBuilder.Property(i => i.Operation).HasConversion<int>().IsRequired();
            modelBuilder.Property(i => i.Description).HasMaxLength(3000).IsRequired(false);

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("InventoriesOperations");
            modelBuilder.Property(i => i.Id).HasColumnName("Id");
            modelBuilder.Property(i => i.InventoryId).HasColumnName("InventoryId");
            modelBuilder.Property(i => i.Quantity).HasColumnName("Quantity");
            modelBuilder.Property(i => i.Operation).HasColumnName("Operation");
            modelBuilder.Property(i => i.Description).HasColumnName("Description");
            modelBuilder.Property(i => i.Date).HasColumnName("Date");
        }
    }
}
