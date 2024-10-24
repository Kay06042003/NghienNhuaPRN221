using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.DAO;
using Microsoft.Extensions.DependencyInjection;
using NghienNhuaWPF.View;
using NghienNhuaWPF.ViewModels;
using Repository;
using Repository.Interfaces;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace NghienNhuaWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            var Window = ServiceProvider.GetRequiredService<GetListOrderConfirm>();
            Window.Show();
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IOrderServices, OrderServices>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<OrderDAO>();
            services.AddTransient<OrderConfirmViewModel>();
            services.AddTransient<OrderUpdateViewModel>();
            services.AddSingleton<GetListOrderConfirm>();
            services.AddSingleton<GetListOrderUpdate>();
            // Đăng ký các service, repository, DAO mà bạn đã có sẵn
            services.AddTransient<IAccountServices, AccountServices>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IStaffServices, StaffService>();
            services.AddTransient<IKeyboardServices, KeyboardServices>();
            services.AddTransient<IProductService, ProductService>();



            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IStaffRepository, StaffRepository>();
            services.AddTransient<IKeyboardRepository, KeyboardRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();

            services.AddTransient<AccountDAO>();
            services.AddTransient<UserDAO>();
            services.AddTransient<StaffDAO>();
            services.AddTransient<KeyboardDAO>();
            services.AddTransient<ProductDAO>();


            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<StaffViewModel>();
            services.AddTransient<KeyboardViewModel>();

            // Đăng ký MainWindow
            services.AddSingleton<MainView>();
            services.AddSingleton<LoginView>();

        }
    }

}
