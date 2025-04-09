using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrismWear.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace PrismWear.Data.Data.Configuration
{
    public class ProductsConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(SeedProducts());
        }

        private List<Product> SeedProducts()
        {
            List<Product> products = new List<Product>();
            var product = new Product();
            product = new Product
            {
               Id = 1,
               Name = "The Nun",
               Description = "95% памук, 5% еластан",
               Price = 29.99,
               CategoryId = 1,
               AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);


            product = new Product
            {
                Id = 2,
                Name = "Stylish Hoodie",
                Description = "95% памук, 5% еластан",
                Price = 29.99,
                CategoryId = 1,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);
            product = new Product
            {
                Id = 3,
                Name = "Elegant Dress",
                Description = "95% памук, 5% еластан",
                Price = 49.99,
                CategoryId = 1,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);
            product = new Product
            {
                Id = 4,
                Name = "s",
                Description = "95% памук, 5% еластан",
                Price = 29.99,
                CategoryId = 1,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);


            product = new Product
            {
                Id = 5,
                Name = "d",
                Description = "95% памук, 5% еластан",
                Price = 29.99,
                CategoryId = 1,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);
            product = new Product
            {
                Id = 6,
                Name = "f",
                Description = "95% памук, 5% еластан",
                Price = 49.99,
                CategoryId = 1,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);

            product = new Product
            {
                Id = 7,
                Name = "v",
                Description = "95% памук, 5% еластан",
                Price = 49.99,
                CategoryId = 2,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);

            product = new Product
            {
                Id = 8,
                Name = "v",
                Description = "95% памук, 5% еластан",
                Price = 49.99,
                CategoryId = 2,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);

            product = new Product
            {
                Id = 9,
                Name = "v",
                Description = "95% памук, 5% еластан",
                Price = 49.99,
                CategoryId = 2,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);

            product = new Product
            {
                Id = 10,
                Name = "v",
                Description = "95% памук, 5% еластан",
                Price = 49.99,
                CategoryId = 2,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);

            product = new Product
            {
                Id = 11,
                Name = "v",
                Description = "95% памук, 5% еластан",
                Price = 49.99,
                CategoryId = 2,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);

            product = new Product
            {
                Id = 12,
                Name = "v",
                Description = "95% памук, 5% еластан",
                Price = 49.99,
                CategoryId = 2,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);

            product = new Product
            {
                Id = 13,
                Name = "v",
                Description = "95% памук, 5% еластан",
                Price = 49.99,
                CategoryId = 2,
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            products.Add(product);

            return products;
        }
    }
}
