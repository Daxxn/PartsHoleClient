using CSVParserLibrary;

using JsonReaderLibrary;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PartsInventory.Models;
using PartsInventory.Models.API;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;
using PartsInventory.ViewModels;
#if IMPL == MAIN
using PartsInventory.ViewModels.Main;
#elif IMPL == TESTING
using PartsInventory.ViewModels.Testing;
#elif IMPL == MOCK
using PartsInventory.ViewModels.Mock;
#endif
using PartsInventory.Views;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

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
      }

      private static void ConnectModelServices(IServiceCollection services)
      {
         services.AddSingleton<IAPIController, APIController>();
         services.AddSingleton<IUserModel, UserModel>();
         services.AddAbstractFactory<ICSVParser, CSVParser>();
         services.AddSingleton<ICSVParserOptions, CSVParserOptions>();

         // OMG !! this is gonna suck...
         // It would require replacing everything with factories.
         // Probably isnt worth it.
         //services.AddTransient<IPartModel, PartModel>();
         //services.AddTransient<IPartsCollection, PartsCollection>();
         //services.AddTransient<IInvoiceModel, InvoiceModel>();
         //services.AddTransient<IBinModel, BinModel>();
         //services.AddTransient<IDatasheet, Datasheet>();
         //services.AddTransient<IPartNumber, PartNumber>();
      }
   }
}
