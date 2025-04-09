using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using PrismWear.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismWear.Data.Data.Configuration
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
          builder.HasData(SeedProductDetails());
        }

        private List<ProductDetail> SeedProductDetails()
        {
            var products = new List<ProductDetail>();
            var productDetail = new ProductDetail();
            productDetail =
                new ProductDetail
                {
                    Id = 1,
                    ProductId = 1,
                    Size = "S",
                    Quantity = 10,
                };

            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 2,
                ProductId = 1,
                Size = "M",
                Quantity = 8,
            };
            products.Add(productDetail);

            productDetail =
               new ProductDetail
               {
                   Id = 3,
                   ProductId = 1,
                   Size = "L",
                   Quantity = 5,
               };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 4,
                ProductId = 1,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 5,
                ProductId = 2,
                Size = "S",
                Quantity = 4,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 6,
                ProductId = 2,
                Size = "M",
                Quantity = 4,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 7,
                ProductId = 2,
                Size = "L",
                Quantity = 3,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 8,
                ProductId = 2,
                Size = "XL",
                Quantity = 3,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 9,
                ProductId = 3,
                Size = "S",
                Quantity = 6,
            };
              products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 10,
                ProductId = 3,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 11,
                ProductId = 3,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 12,
                ProductId = 3,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 13,
                ProductId = 4,
                Size = "S",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 14,
                ProductId = 4,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 15,
                ProductId = 4,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 16,
                ProductId = 4,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 17,
                ProductId = 5,
                Size = "S",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 18,
                ProductId = 5,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 19,
                ProductId = 5,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 20,
                ProductId = 5,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 21,
                ProductId = 6,
                Size = "S",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 22,
                ProductId = 6,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 23,
                ProductId = 6,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 24,
                ProductId = 6,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 25,
                ProductId = 7,
                Size = "S",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 26,
                ProductId = 7,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 27,
                ProductId = 7,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 28,
                ProductId = 8,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 29,
                ProductId = 8,
                Size = "S",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 30,
                ProductId = 8,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 31,
                ProductId = 8,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 32,
                ProductId = 8,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 33,
                ProductId = 9,
                Size = "S",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 34,
                ProductId = 9,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 35,
                ProductId = 9,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 36,
                ProductId = 9,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 37,
                ProductId = 10,
                Size = "S",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 38,
                ProductId = 10,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 39,
                ProductId = 10,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 40,
                ProductId = 10,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 41,
                ProductId = 11,
                Size = "S",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 42,
                ProductId = 11,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 43,
                ProductId = 11,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 44,
                ProductId = 11,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 45,
                ProductId = 12,
                Size = "S",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 46,
                ProductId = 12,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 47,
                ProductId = 12,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 48,
                ProductId = 12,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);

            productDetail = new ProductDetail
            {
                Id = 49,
                ProductId = 13,
                Size = "S",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 50,
                ProductId = 13,
                Size = "M",
                Quantity = 5,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 51,
                ProductId = 13,
                Size = "L",
                Quantity = 6,
            };
            products.Add(productDetail);
            productDetail = new ProductDetail
            {
                Id = 52,
                ProductId = 13,
                Size = "XL",
                Quantity = 6,
            };
            products.Add(productDetail);


















            return products;
        }
    }
}
