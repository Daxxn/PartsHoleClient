using System.Reflection;
using System.Windows;

using CSVParserLibrary;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PartsInventory.Models;
using PartsInventory.Models.API;
using PartsInventory.Models.Extensions;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;
using PartsInventory.ViewModels;
using PartsInventory.ViewModels.Main;
using PartsInventory.Views;

namespace PartsInventory
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App : Application
   {
      public static IHost? AppHost { get; private set; }
      public App()
      {
         AppHost = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((config) =>
            {
               config.AddJsonFile(@".\APIEndpoints.json");
               config.AddJsonFile(@".\Settings.json");
               config.AddUserSecrets(Assembly.GetExecutingAssembly());
            })
            .ConfigureLogging((hostContext, logBuilder) =>
            {
               if (hostContext.HostingEnvironment.IsDevelopment())
               {
                  logBuilder.AddConsole();
               }
            })
            .ConfigureServices((hostContext, services) =>
            {
               services.Configure<DirSettings>(hostContext.Configuration.GetSection("Dirs"));
               services.Configure<GeneralSettings>(hostContext.Configuration.GetSection("Settings"));
               services.Configure<APISettings>(hostContext.Configuration.GetSection("API"));
               ConnectViewSevices(services);
               ConnectViewModelServices(services);
               ConnectModelServices(services);
            })
            .Build();
      }
      protected override async void OnStartup(StartupEventArgs e)
      {
         await AppHost!.StartAsync();

         var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
         mainWindow.Show();

         DatasheetFinder.OnStartup();
         base.OnStartup(e);
      }
      protected override async void OnExit(ExitEventArgs e)
      {
         Settings.Default.Save();
         PathSettings.Default.Save();
         await AppHost!.StopAsync();
         base.OnExit(e);
      }

      private static void ConnectViewSevices(IServiceCollection services)
      {
         // Main Window
         services.AddSingleton<MainWindow>();

         // Main Window Child Views
         services.AddSingleton<PassivesView>();
         services.AddSingleton<InvoiceParserView>();
         services.AddSingleton<PackageView>();
         services.AddSingleton<PartNumberGeneratorView>();
         services.AddSingleton<PartsInventoryView>();
         services.AddSingleton<ProjectBOMView>();
         services.AddSingleton<BinsView>();

         // Dialog Windows
         services.AddSingleton<PartNumberTemplateDialog>();
         services.AddSingleton<PartSearchDialog>();
         services.AddSingleton<PassiveBookDialog>();
         services.AddSingleton<NewPartView>();
      }

      private static void ConnectViewModelServices(IServiceCollection services)
      {
         services.AddSingleton<IMainViewModel, MainViewModel>();
         services.AddSingleton<IInvoiceParserViewModel, InvoiceParserViewModel>();
         services.AddSingleton<IPartNumberGeneratorViewModel, PartNumberGeneratorViewModel>();
         services.AddSingleton<IPartNumberTemplateViewModel, PartNumberTemplateViewModel>();
         services.AddSingleton<IPartSearchViewModel, PartSearchViewModel>();
         services.AddSingleton<IPartsInventoryViewModel, PartsInventoryViewModel>();
         services.AddSingleton<IPassiveBookViewModel, PassiveBookViewModel>();
         services.AddSingleton<IPassivesViewModel, PassivesViewModel>();
         services.AddSingleton<IProjectBOMViewModel, ProjectBOMViewModel>();
         services.AddSingleton<IPackageViewModel, PackageViewModel>();
         services.AddSingleton<INewPartViewModel, NewPartViewModel>();
         services.AddSingleton<IBinsViewModel, BinsViewModel>();
      }

      private static void ConnectModelServices(IServiceCollection services)
      {
         services.AddSingleton<IAPIController, APIController>();
         services.AddSingleton<IUserModel, UserModel>();
         services.AddAbstractFactory<ICSVParser, CSVParser>();
         services.AddSingleton<ICSVParserOptions, CSVParserOptions>();
      }
   }
}
