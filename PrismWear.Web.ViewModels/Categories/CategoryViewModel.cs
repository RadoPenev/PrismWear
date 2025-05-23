﻿using System.ComponentModel.DataAnnotations;

namespace PrismWear.Web.ViewModels.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters.")]
        public string Name { get; set; }
    }
}
