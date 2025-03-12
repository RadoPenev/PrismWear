using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrismWear.Data;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Data.Repositories;
using PrismWear.Data.Repositories;
using PrismWear.Services.Data;

namespace PrismWear
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
          

            

            builder.Services.AddTransient<ICategoriesService, CategoriesService>();
            builder.Services.AddTransient<IProductsService, ProductsService>();
            builder.Services.AddTransient<ICartService, CartService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
            app.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
