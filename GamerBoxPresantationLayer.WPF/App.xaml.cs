using GamerBox.BusinessLayer.Abstract;
using GamerBox.BusinessLayer.Concrete;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.EntityFramework;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GamerBox.DataAccessLayer.Context;


namespace GamerBoxPresantationLayer.WPF
{
   
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
   
                                         

    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; } // Konfigürasyon özelliği

        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += (s, args) =>
            {
                MessageBox.Show($"Beklenmedik bir hata oluştu:\n{args.Exception.Message}\n\nDetay:\n{args.Exception.InnerException?.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true; // Uygulamanın çökmesini engelle
            };
            // Konfigürasyonu inşa et
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Bu blok, uygulama başlarken veritabanını kontrol eder ve tablolar yoksa oluşturur.
            using (var scope = ServiceProvider.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<GamerBoxContext>();
                    // 1. Veritabanını oluştur/güncelle
                    context.Database.Migrate();

                    // 2. TEST VERİLERİNİ EKLE (Yeni Eklenen Kısım)
                    GamerBox.DataAccessLayer.Dataseeds.SeedData.Initialize(context);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Veritabanı oluşturulurken hata oluştu:\n{ex.Message}", "Kritik Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Veritabanı Bağlantısı (Dependency Injection)
            services.AddDbContext<GamerBoxContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Repository ve Servisler
            services.AddTransient<IUserDal, EFUserDal>();
            services.AddTransient<IGameDal, EFGameDal>();
            services.AddTransient<IPostDal, EFPostDal>();
            services.AddTransient<IRatingDal, EFRatingDal>();
            // IPostDal, IRatingDal vb. eksikse ekleyin

            services.AddTransient<IUserService, UserManager>();
            services.AddTransient<IGameService, GameManager>();
            services.AddTransient<IPostService, PostManager>();
            services.AddTransient<IRatingService, RatingManager>();
            // IGameService, IPostService vb. eksikse ekleyin

            // Pencereler
            services.AddTransient<MainWindow>();
            services.AddTransient<SignInWindow>();
            services.AddTransient<SignUpWindow>();

            // ViewModels
            services.AddTransient<GamerBoxPresantationLayer.WPF.ViewModels.HomeViewModel>();
            services.AddTransient<GamerBoxPresantationLayer.WPF.ViewModels.ListsViewModel>(); 
            services.AddTransient<GamerBoxPresantationLayer.WPF.ViewModels.WatchlistViewModel>();
            services.AddTransient<GamerBoxPresantationLayer.WPF.ViewModels.AddGameViewModel>();
            services.AddTransient<GamerBoxPresantationLayer.WPF.ViewModels.ReviewsViewModel>();
            services.AddTransient<GamerBoxPresantationLayer.WPF.ViewModels.ProfileViewModel>();
            services.AddTransient<GamerBoxPresantationLayer.WPF.ViewModels.SignInViewModel>();
            services.AddTransient<GamerBoxPresantationLayer.WPF.ViewModels.SignUpViewModel>();
            // Views (Inject edilebilir olması için)
            services.AddTransient<GamerBoxPresantationLayer.WPF.Classes.UCHome>();
            services.AddTransient<GamerBoxPresantationLayer.WPF.Views.UserControls.UCLists>(); 
            services.AddTransient<GamerBoxPresantationLayer.WPF.Views.UserControls.UCWatchtLists>();
            services.AddTransient<GamerBoxPresantationLayer.WPF.Views.UserControls.UCReviews>();
            services.AddTransient<AddGameWindow>();
            services.AddTransient<GamerBoxPresantationLayer.WPF.Views.UserControls.UCProfile>();
            services.AddTransient<GamerBoxPresantationLayer.WPF.Views.UserControls.UCReviews>();
        }
    }
}

