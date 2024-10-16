using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.DAO;
using Microsoft.Extensions.DependencyInjection;
using NghienNhuaWPF.ViewModels;
using Repository;
using Repository.Interfaces;
using System.Configuration;
using System.Data;
using System.Windows;

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
            var mainWindow = ServiceProvider.GetRequiredService<GetListOrderConfirm>();
            mainWindow.Show();
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddTransient<OrderDAO>();
            services.AddTransient<OrderViewModel>();

            services.AddSingleton<GetListOrderConfirm>();
        }
    }

}
