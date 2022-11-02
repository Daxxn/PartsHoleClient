using JsonReaderLibrary;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PartsInventory.Models;
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
               config.AddJsonFile(@".\Resources\Settings\Settings.json");
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
               ConnectViewSevices(services);
               ConnectViewModelServices(services);
            })
            .Build();
      }
      protected override async void OnStartup(StartupEventArgs e)
      {
         await AppHost!.StartAsync();

         var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
         mainWindow.Show();
         //MainViewModel.Instance.Open();
         DatasheetFinder.OnStartup();
         base.OnStartup(e);
      }
      protected override async void OnExit(ExitEventArgs e)
      {
         //MainViewModel.Instance.Save();
         Settings.Default.Save();
         PathSettings.Default.Save();
         await AppHost!.StopAsync();
         base.OnExit(e);
      }

      private void ConnectViewSevices(IServiceCollection services)
      {
         services.AddSingleton<MainWindow>();
         services.AddSingleton<PassivesView>();
         services.AddSingleton<InvoiceParserView>();
         services.AddSingleton<PackageView>();
         services.AddSingleton<PartNumberGeneratorView>();
         services.AddSingleton<PartsInventoryView>();
         services.AddSingleton<ProjectBOMView>();
         services.AddTransient<PartNumberTemplateDialog>();
         services.AddTransient<PartSearchDialog>();
         services.AddTransient<PassiveBookDialog>();
      }

      private void ConnectViewModelServices(IServiceCollection services)
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
      }
   }
}
