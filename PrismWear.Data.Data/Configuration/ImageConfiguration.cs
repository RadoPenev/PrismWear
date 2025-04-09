using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrismWear.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismWear.Data.Data.Configuration
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasData(SeedImages());
        }
        private List<Image> SeedImages()
        {
            var images = new List<Image>();
            var image = new Image();
            image =
                new Image
                {
                    Id = "Мъжка-1",
                    ProductId = 1,
                    Extension = "jpg",
                    AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
                };
            images.Add(image);
            image =
                new Image
                {
                    Id = "Мъжка-г-1",
                    ProductId = 1,
                    Extension = "jpg",
                    AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
                };
            images.Add(image);
            image =
               new Image
               {
                   Id = "Мъжка-п-2",
                   ProductId = 2,
                   Extension = "jpg",
                   AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
               };
            images.Add(image);
            image =
              new Image
              {
                  Id = "Мъжка-2",
                  ProductId = 2,
                  Extension = "jpg",
                  AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
              };
            images.Add(image);
            image =
             new Image
             {
                 Id = "Мъжка-3",
                 ProductId = 3,
                 Extension = "jpg",
                 AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
             };
            images.Add(image);
            image =
            new Image
            {
                Id = "Мъжка-г-3",
                ProductId = 3,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);

            image =
            new Image
            {
                Id = "Мъжка-4",
                ProductId = 4,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);
            image =
            new Image
            {
                Id = "Мъжка-г-4",
                ProductId = 4,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);

            image =
            new Image
            {
                Id = "Мъжка-6",
                ProductId = 5,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);
            image =
            new Image
            {
                Id = "Мъжка-г-6",
                ProductId = 5,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);

            image =
            new Image
            {
                Id = "Мъжка-7",
                ProductId = 6,
                Extension = "jpeg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);
            image =
            new Image
            {
                Id = "Мъжка-г-7",
                ProductId = 6,
                Extension = "jpeg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);


            image =
           new Image
           {
               Id = "Женска-1",
               ProductId = 7,
               Extension = "jpg",
               AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
           };
            images.Add(image);
            image =
            new Image
            {
                Id = "Женска-г-1",
                ProductId = 7,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);

            image =
          new Image
          {
              Id = "Женска-2",
              ProductId = 8,
              Extension = "jpg",
              AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
          };
            images.Add(image);
            image =
            new Image
            {
                Id = "Женска-г-2",
                ProductId = 8,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);

            image =
          new Image
          {
              Id = "Женска-3",
              ProductId = 9,
              Extension = "jpg",
              AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
          };
            images.Add(image);
            image =
            new Image
            {
                Id = "Женска-г-3",
                ProductId = 9,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);

            image =
          new Image
          {
              Id = "Женска-4",
              ProductId = 10,
              Extension = "jpeg",
              AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
          };
            images.Add(image);
            image =
            new Image
            {
                Id = "Женска-г-4",
                ProductId = 10,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);

            image =
          new Image
          {
              Id = "Женска-5",
              ProductId = 11,
              Extension = "jpg",
              AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
          };
            images.Add(image);
            image =
            new Image
            {
                Id = "Женска-г-5",
                ProductId = 11,
                Extension = "jpeg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);

            image =
          new Image
          {
              Id = "Женска-6",
              ProductId = 12,
              Extension = "jpeg",
              AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
          };
            images.Add(image);
            image =
            new Image
            {
                Id = "Женска-г-6",
                ProductId = 12,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);

            image =
          new Image
          {
              Id = "Женска-7",
              ProductId = 13,
              Extension = "jpg",
              AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
          };
            images.Add(image);
            image =
            new Image
            {
                Id = "Женска-г-7",
                ProductId = 13,
                Extension = "jpg",
                AddedByUser = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
            };
            images.Add(image);


            return images;
        }
    }
    
}
