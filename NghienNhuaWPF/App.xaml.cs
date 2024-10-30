using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.DAO;
using Microsoft.Extensions.DependencyInjection;
using NghienNhuaWPF.View;
using NghienNhuaWPF.ViewModels;
using Repository;
using Repository.Interfaces;
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
            // Đăng ký các service
            services.AddTransient<IAccountServices, AccountServices>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IStaffServices, StaffService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IKeyboardServices, KeyboardServices>();
            services.AddTransient<IKeycapServices, KeycapServices>();
            services.AddTransient<ISwitchServices, SwitchServices>();
            services.AddTransient<IKitServices, KitServices>();
            services.AddTransient<IMouseServices, MouseServices>();
            services.AddTransient<IEarphoneServices, EarphoneServices>();
            services.AddTransient<IOrderServices, OrderServices>();

            // Đăng ký các Repository
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IStaffRepository, StaffRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IKeyboardRepository, KeyboardRepository>();
            services.AddTransient<IKeycapRepository, KeycapRepository>();
            services.AddTransient<ISwitchRepository, SwitchRepository>();
            services.AddTransient<IKitRepository, KitRepository>();
            services.AddTransient<IMouseRepository, MouseRepository>();
            services.AddTransient<IEarphoneRepository, EarphoneRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();


            // Đăng ký các DAO
            services.AddTransient<AccountDAO>();
            services.AddTransient<UserDAO>();
            services.AddTransient<StaffDAO>();
            services.AddTransient<KeyboardDAO>();
            services.AddTransient<ProductDAO>();
            services.AddTransient<KeycapDAO>();
            services.AddTransient<SwitchDAO>();
            services.AddTransient<KitDAO>();
            services.AddTransient<MouseDAO>();
            services.AddTransient<EarphoneDAO>();
            services.AddTransient<OrderDAO>();


            //Đăng ký các ViewModel
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<StaffViewModel>();
            services.AddTransient<KeyboardViewModel>();
            services.AddTransient<KeycapViewModel>();
            services.AddTransient<SwitchViewModel>();
            services.AddTransient<KitViewModel>();
            services.AddTransient<MouseViewModel>();
            services.AddTransient<EarphoneViewModel>();
            services.AddTransient<HomeViewModel>();

            // Đăng ký MainWindow
            services.AddSingleton<MainView>();
            services.AddSingleton<LoginView>();

        }

    }

}
