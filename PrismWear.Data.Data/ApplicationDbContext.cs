using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrismWear.Data.Common.Models;
using PrismWear.Data.Data.Configuration;
using PrismWear.Data.Models;

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

            const string adminRoleId = "45989dba-9350-4d78-b2f4-afef33936dbe";
            const string userRoleId = "c696fe2c-f184-4adb-839c-4607ba5c0dc5";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "df2b7237-93f2-4cd8-bdd3-5c452677b935"
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "ab6414c3-2391-42f5-bd5c-2a64267dc709"
                }
            );

            var user = new IdentityUser
            {
                Id = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
                UserName = "admin@myapp.com",
                NormalizedUserName = "ADMIN@MYAPP.COM",
                Email = "admin@myapp.com",
                NormalizedEmail = "ADMIN@MYAPP.COM",
                EmailConfirmed = true,
                SecurityStamp = "ab6414c3-2391-42f5-bd5c-2a64267dc709",
                PasswordHash = "AQAAAAIAAYagAAAAEBheA/UaJzgCE+mtgDRxqzUD0/Wk07hacMTEcg3eAC7kl0UtLlHxfyvTnI/vZkSiNQ==",
    ConcurrencyStamp = "d61ae36a-b528-4d0f-b1c1-b5546a7098f9",
            };

            builder.Entity<IdentityUser>().HasData(user);

            var userRole = new IdentityUserRole<string>
            {
                UserId = "f45b8455-8f1d-4b28-96a1-7321cd9829de",
                RoleId = "45989dba-9350-4d78-b2f4-afef33936dbe"
            };

            builder.Entity<IdentityUserRole<string>>().HasData(userRole);

            builder.ApplyConfiguration(new ProductsConfiguration());

            builder.ApplyConfiguration(new ProductDetailConfiguration());

            builder.ApplyConfiguration(new ImageConfiguration());


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
