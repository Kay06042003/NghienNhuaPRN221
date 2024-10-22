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
            var loginView = ServiceProvider.GetRequiredService<LoginView>();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false)
                {
                    var mainView = ServiceProvider.GetRequiredService<MainView>();
                    mainView.Show();
                    loginView.Close();
                }
            };
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Đăng ký các service, repository, DAO mà bạn đã có sẵn
            services.AddTransient<IAccountServices, AccountServices>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IStaffServices, StaffService>();


            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IStaffRepository, StaffRepository>();

            services.AddTransient<AccountDAO>();
            services.AddTransient<UserDAO>();
            services.AddTransient<StaffDAO>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<StaffViewModel>();

            // Đăng ký MainWindow
            services.AddSingleton<MainView>();
            services.AddSingleton<LoginView>();

        }

    }

}
