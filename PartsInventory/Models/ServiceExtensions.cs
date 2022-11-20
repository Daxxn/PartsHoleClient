using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace PartsInventory.Models
{
    public static class ServiceExtensions
    {
      public static void AddAbstractFactory<TInterface, TImplementation>(this IServiceCollection services)
         where TInterface: class
         where TImplementation : class, TInterface
      {
         services.AddTransient<TInterface, TImplementation>();
         services.AddSingleton<Func<TInterface>>(x => () => x.GetService<TInterface>()!);
         services.AddSingleton<IAbstractFactory<TInterface>, AbstractFactory<TInterface>>();
      }
    }
}
