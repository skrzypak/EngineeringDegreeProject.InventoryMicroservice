using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Exceptions;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Fluent.Entities;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Mappers;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.ViewModel.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly MicroserviceContext _context;

        public CategoryService(ILogger<CategoryService> logger, MicroserviceContext context)
        {
            _logger = logger;
            _context = context;
        }

        public CategoryViewModel Get(int id)
        {
            var categoryViewModel = _context
                .Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                { 
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Description = c.Description
                })
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (categoryViewModel is null)
            {
                throw new NotFoundException($"Category with ID {id} NOT FOUND");
            }

            return categoryViewModel;
        }

        public int Create(CategoryBasicDto dto)
        {
            throw new NotImplementedException();
        }

        public void Update(CategoryBasicDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var category = new Category() { Id = id };
            _context.Categories.Attach(category);
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

    }
}
