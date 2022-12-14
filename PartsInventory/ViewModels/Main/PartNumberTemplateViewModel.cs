using MVVMLibrary;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels.Main
{
   public class PartNumberTemplateViewModel : ViewModel, IPartNumberTemplateViewModel
   {
      #region Local Props
      public event EventHandler<PartNumber> CreatePartNumber;

      private PartNumber _partNum = null!;

      #region Commands
      public Command CreatePartNumCmd { get; init; }
      public Command SelectTypeNumCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PartNumberTemplateViewModel()
      {
         CreatePartNumCmd = new(CreatePartNum);
         CreatePartNumber += CreatePartNumberEvent;
         SelectTypeNumCmd = new(SelectTypeNum);
      }
      #endregion

      #region Methods
      public void CreatePartNum()
      {
         CreatePartNumber?.Invoke(this, PartNumber);
      }

      public void SelectTypeNum(object param)
      {
         if (param is string str)
         {
            if (uint.TryParse(str, out uint val))
            {
               PartNumber.Category = val;
            }
         }
      }

      private void CreatePartNumberEvent(object? sender, PartNumber e)
      {
         // TODO - replace with DI interfaces.
         PartNumber = new PartNumber();
      }

      #endregion

      #region Full Props
      public PartNumber PartNumber
      {
         get => _partNum;
         set
         {
            _partNum = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
