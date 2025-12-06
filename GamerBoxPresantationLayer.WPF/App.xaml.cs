using GamerBox.BusinessLayer.Abstract;
using GamerBox.BusinessLayer.Concrete;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.EntityFramework;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GamerBoxPresantationLayer.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1️⃣ Service Collection (kayıt listesi) oluştur
            var services = new ServiceCollection();

            // 2️⃣ Katmanları bağla
            ConfigureServices(services);

            // 3️⃣ Servis sağlayıcıyı oluştur
            ServiceProvider = services.BuildServiceProvider();

            // 4️⃣ MainWindow’u DI üzerinden başlat
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Bağımlılık eşleştirmeleri
            services.AddTransient<IUserDal, EFUserDal>();
            services.AddTransient<IUserService, UserManager>();
            services.AddTransient<IGameDal, EFGameDal>();

            // WPF pencereleri
            services.AddTransient<MainWindow>();
            services.AddTransient<SignInWindow>();
            services.AddTransient<SignUpWindow>();
        }
    }
}

