using CSVParserLibrary.Models;

using MongoDB.Bson;

using MVVMLibrary;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Parsers.DigiKey;
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
      private ObservableCollection<PartModel> _parts = new();
      private SupplierType? _supplierType = null;
      private bool _isAddedToParts = false;
      public IEnumerable<string> PartIDs { get; set; }
      #endregion

      #region Constructors
      public InvoiceModel() { }

      public InvoiceModel(OrderCSVPage order)
      {
         Parts = new(ParseOrder(order));
      }
      #endregion

      #region Methods
      private static IEnumerable<PartModel> ParseOrder(OrderCSVPage order)
      {
         List<PartModel> parts = new();
         foreach (var line in order.Lines)
         {
            parts.Add(ParsePart(line, order.Props));
         }
         return parts;
      }
      private static PartModel ParsePart(ILine line, IList<string> props)
      {
         PartModel part = new();
         for (int i = 0; i < props.Count; i++)
         {
            part.ParseProp(props[i], line.Data[i]);
         }
         return part;
      }

      public static InvoiceModel Create()
      {
         return new InvoiceModel()
         {
            Id = ObjectId.GenerateNewId().ToString()
         };
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
