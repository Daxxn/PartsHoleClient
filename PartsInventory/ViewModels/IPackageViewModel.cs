using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PartsInventory.Models.Inventory;

namespace PartsInventory.ViewModels;

public interface IPackageViewModel
{
   IMainViewModel MainVM { get; }
}
