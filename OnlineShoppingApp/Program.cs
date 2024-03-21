using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services;
using OnlineShoppingApp.Services.Classes;
using OnlineShoppingApp.Services.Interfaces;
using Stripe;

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
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<ICategoriesRepo, CategoryRepo>();
            builder.Services.AddScoped<IBrandRepo, BrandRepo>();
            builder.Services.AddScoped<ICommentsRepo, CommentsRepo>();
            builder.Services.AddScoped<IRateRepo, RateRepo>();
            builder.Services.AddScoped<IBuyerRepo, BuyerRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();


            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();
            builder.Services.AddScoped<IDeliveryMethodsRepo, DeliveryMethodRepo>();
            builder.Services.AddScoped<IAddressRepo, AddressRepo>();
            builder.Services.AddScoped<ISellerRepo, SellerRepo>();


            // 2.Adding identity to the container
            builder.Services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ShoppingContext>()
                .AddDefaultTokenProviders();


            // Add services to the container.
            builder.Services.AddControllersWithViews();
           builder.Services.AddHttpContextAccessor();
           builder.Services.AddScoped<CartService>();
           


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

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
               



            // 5. add authentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
               name: "CatchAll",
               pattern: "{*url}",
               defaults: new { controller = "Home", action = "NotFound" }
           );

            app.Run();
        }
    }
}
