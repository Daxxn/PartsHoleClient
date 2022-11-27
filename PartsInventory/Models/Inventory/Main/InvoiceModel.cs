using CSVParserLibrary.Models;

using MongoDB.Bson;

using MVVMLibrary;
using PartsInventory.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Inventory.Main
{
   public class InvoiceModel : BaseModel
   {
      #region Local Props
      private uint _orderNumber = 0;
      private string _path = "";
      private decimal _subTotal = 0;
      private ObservableCollection<DigiKeyPartModel> _parts = new();
      private SupplierType? _supplierType = null;
      private bool _isAddedToParts = false;
      public IEnumerable<string> PartIDs { get; set; }
      #endregion

      #region Constructors
      public InvoiceModel() { }
      #endregion

      #region Methods
      public static InvoiceModel Create()
      {
         return new InvoiceModel()
         {
            Id = ObjectId.GenerateNewId().ToString()
         };
      }
      #endregion

      #region Full Props

      public ObservableCollection<DigiKeyPartModel> Parts
      {
         get => _parts;
         set
         {
            _parts = value;
            OnPropertyChanged();
         }
      }

      public SupplierType? SupplierType
      {
         get => _supplierType;
         set
         {
            _supplierType = value;
            OnPropertyChanged();
         }
      }

      public string Path
      {
         get => _path;
         set
         {
            _path = value;
            OnPropertyChanged();
         }
      }

      public uint OrderNumber
      {
         get => _orderNumber;
         set
         {
            _orderNumber = value;
            OnPropertyChanged();
         }
      }

      public decimal SubTotal
      {
         get => _subTotal;
         set
         {
            _subTotal = value;
            OnPropertyChanged();
         }
      }

      public bool IsAddedToParts
      {
         get => _isAddedToParts;
         set
         {
            _isAddedToParts = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
