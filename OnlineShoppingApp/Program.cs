using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services.Classes;
using OnlineShoppingApp.Services.Interfaces;

namespace OnlineShoppingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // 1.Adding dbcontext to the container
            builder.Services.AddDbContext<ShoppingContext>(op =>
                  op.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"))
            );

            // 2.Adding identity to the container
            builder.Services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ShoppingContext>()
                .AddDefaultTokenProviders();


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // 3. adding google authentication to the container
            builder.Services.AddAuthentication()
                  .AddGoogle(options =>
                  {
                      options.ClientId = builder.Configuration["GoogleKeys:ClientId"];
                      options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"]; ;
                  });


            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // 5. add authentication
           app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
