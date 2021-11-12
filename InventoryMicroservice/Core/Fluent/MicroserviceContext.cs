using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Configurations;
using InventoryMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryMicroservice.Core.Fluent
{
    public class MicroserviceContext : DbContext
    {
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<AllergenToProduct> AllergensToProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryOperation> InventoriesOperations { get; set; }
        public DbSet<Product> Products { get; set; }

        public MicroserviceContext(DbContextOptions options) : base(options)
        {
        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.AddProperty("CreatedDate", typeof(DateTime));
                entity.AddProperty("LastUpdatedDate", typeof(DateTime?));
            }

            modelBuilder.ApplyConfiguration(new AllergenConfiguration());
            modelBuilder.ApplyConfiguration(new AllergenToProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryOperationConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
        #endregion
    }
}
