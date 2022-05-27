using MVVMLibrary;
using PartsInventory.Models;
using PartsInventory.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels
{
   public class PartsInventoryViewModel : ViewModel
   {
      #region Local Props
      private PartsCollection? _partsCollection = null;

      #region commands

      #endregion
      #endregion

      #region Constructors
      public PartsInventoryViewModel() { }
      #endregion

      #region Methods
      public void NewPartsEventHandler(object sender, AddInvoiceToPartsEventArgs e)
      {
         if (PartsCollection is null) PartsCollection = new();
         PartsCollection.AddParts(e.InvoiceID, e.NewParts);
      }
      #endregion

      #region Full Props
      public PartsCollection? PartsCollection
      {
         get => _partsCollection;
         set
         {
            _partsCollection = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
