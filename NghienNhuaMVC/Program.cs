using Models;
using DataAccess.DAO;
using BusinessLogic.Interfaces;
using Repository.Interfaces;
using BusinessLogic.Services;
using Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using NghienNhuaMVC.Services;
using NghienNhuaMVC.Middleware;
using Microsoft.AspNetCore.Authentication.Google;
namespace NghienNhuaMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                {
                    // Cấu hình tùy chọn cho Newtonsoft.Json
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            // send email
            builder.Services.AddTransient<ISendEmail, SendEmail>();

            // add session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            // add authentication
            builder.Services.AddAuthentication(
                options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                .AddCookie("Cookies")
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        builder.Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                });

            // add services and repository - Account
            builder.Services.AddScoped<IAccountServices, AccountServices>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<AccountDAO>();
            // add services and repository - Product
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ProductDAO>();
            // add services and repository - User
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<UserDAO>();
            // add services and repository - Cart
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<CartDAO>();
            // add services and repository - Order
            builder.Services.AddScoped<IOrderServices, OrderServices>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<OrderDAO>();
            // add services VNPay
            builder.Services.AddScoped<IVnPayServices, VnPayServices>();
            // add filter
            builder.Services.AddScoped<UserAuthorizationFilter>();

            builder.WebHost.ConfigureKestrel(options => { options.ListenLocalhost(5001, listenOptions => { listenOptions.UseHttps(); }); });
            // add razer page
            builder.Services.AddRazorPages();
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
            app.UseSession();

            app.UseAuthorization();
            app.UseMiddleware<LoginMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
