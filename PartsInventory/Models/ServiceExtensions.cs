using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace PartsInventory.Models.Extensions;

public static class ServiceExtensions
{
   /// <summary>
   /// Register a factory with DI.
   /// <para/>
   /// The factory will build the <typeparamref name="TImplementation"/> model when requested by the consuming dependency.
   /// </summary>
   /// <typeparam name="TInterface">Interface of model</typeparam>
   /// <typeparam name="TImplementation">Implementation of the model that the factory will build</typeparam>
   /// <param name="services">DI service builder</param>
   public static void AddAbstractFactory<TInterface, TImplementation>(this IServiceCollection services)
      where TInterface : class
      where TImplementation : class, TInterface
   {
      services.AddTransient<TInterface, TImplementation>();
      services.AddSingleton<Func<TInterface>>(x => () => x.GetService<TInterface>()!);
      services.AddSingleton<IAbstractFactory<TInterface>, AbstractFactory<TInterface>>();
   }
}
