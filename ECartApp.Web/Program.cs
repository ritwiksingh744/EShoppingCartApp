using ECartApp.DAL;
using ECartApp.Models;
using ECartApp.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECartApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ECartContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ECartContext>().AddDefaultTokenProviders();

            // Add services to the container.
            builder.Services.AddMvc();
            builder.Services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            builder.Services.AddScoped<IGenericRepository<SubCategory>, GenericRepository<SubCategory>>();
            builder.Services.AddScoped<IGenericRepository<Items>, GenericRepository<Items>>();
            builder.Services.AddScoped<IItemRepository, ItemRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Category}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}