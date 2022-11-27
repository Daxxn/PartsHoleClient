using MVVMLibrary;

using PartsInventory.Models.Inventory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels.Main
{
   public class PackageViewModel : ViewModel, IPackageViewModel
   {
      #region Local Props
      private readonly IMainViewModel _mainViewModel;

      #region Commands
      public Command TestCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PackageViewModel(IMainViewModel mainViewModel)
      {
         _mainViewModel = mainViewModel;
         TestCmd = new(Test);
      }
      #endregion

      #region Methods
      private void Test()
      {

      }
      #endregion

      #region Full Props
      public IMainViewModel MainVM
      {
         get => _mainViewModel;
      }
      #endregion
   }
}
