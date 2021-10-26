﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Models.Dto.Category
{
    public abstract class CategoryBasicDto
    {
        [MaxLength(6)]
        public string Code { get; set; }
        [MinLength(3), MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(3000)]
        public string Description { get; set; }
    }
}
