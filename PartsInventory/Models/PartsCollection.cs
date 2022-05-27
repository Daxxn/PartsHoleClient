using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class PartsCollection : Model
   {
      #region Local Props
      private ObservableCollection<PartModel> _parts = new();
      private ObservableCollection<uint> _invoiceIDs = new();
      #endregion

      #region Constructors
      public PartsCollection() { }
      #endregion

      #region Methods
      public void AddParts(IList<uint> invoiceIDs, IList<PartModel> parts)
      {
         foreach (var id in invoiceIDs)
         {
            InvoiceIDs.Add(id);
         }
         foreach (var part in parts)
         {
            Parts.Add(part);
         }
      }
      #endregion

      #region Full Props
      public ObservableCollection<PartModel> Parts
      {
         get => _parts;
         set
         {
            _parts = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<uint> InvoiceIDs
      {
         get => _invoiceIDs;
         set
         {
            _invoiceIDs = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
