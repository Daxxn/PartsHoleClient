using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels
{
   public class MainViewModel : ViewModel
   {
      #region Local Props
      private DatasheetViewModel _datasheetVM = new();
      private PartsInventoryViewModel _partsInventoryVM = new();
      private InvoiceParserViewModel _invoiceParserVM = new();

      #region Commands

      #endregion
      #endregion

      #region Constructors
      public MainViewModel()
      {
         InvoiceParserVM.AddToPartsEvent += PartsInventoryVM.NewPartsEventHandler;
      }
      #endregion

      #region Methods
      public ViewModel GetViewModel(int index)
      {
         if (index == 0)
         {
            return PartsInventoryVM;
         }
         else if (index == 1)
         {
            return InvoiceParserVM;
         }
         else if (index == 2)
         {
            return DatasheetVM;
         }
         else throw new ArgumentException("Index is not a valid view model.");
      }
      #endregion

      #region Full Props
      public DatasheetViewModel DatasheetVM
      {
         get => _datasheetVM;
         set
         {
            _datasheetVM = value;
            OnPropertyChanged();
         }
      }

      public PartsInventoryViewModel PartsInventoryVM
      {
         get => _partsInventoryVM;
         set
         {
            _partsInventoryVM = value;
            OnPropertyChanged();
         }
      }

      public InvoiceParserViewModel InvoiceParserVM
      {
         get => _invoiceParserVM;
         set
         {
            _invoiceParserVM = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
