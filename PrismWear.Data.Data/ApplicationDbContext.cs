﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrismWear.Data.Common.Models;
using PrismWear.Data.Models;
using System.Reflection.Emit;

namespace PrismWear.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<ProductDetail> ProductDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(IDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ApplicationDbContext)
                        .GetMethod(nameof(SetGlobalQueryFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(null, new object[] { builder });
                }
            }

            builder.Entity<Category>().HasData(
                new Category { Id=1,Name = "Men" },
                new Category { Id=2,Name = "Women" }
                );


            builder.Entity<Product>(entity =>
            {
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(p => p.User)
                      .WithMany()
                      .HasForeignKey(p => p.AddedByUser)
                      .OnDelete(DeleteBehavior.Restrict); 
            });

            builder.Entity<CartItem>()
                   .HasOne(ci => ci.Cart)
                   .WithMany(c => c.CartItems)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
                   .HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                   .HasOne(ci => ci.Product)
                   .WithMany(p => p.CartItems)
                   .HasForeignKey(ci => ci.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

        }

        private static void SetGlobalQueryFilter<T>(ModelBuilder builder) where T : class
        {
            builder.Entity<T>().HasQueryFilter(e => !EF.Property<bool>(e, "IsDeleted"));
        }

    }
}
