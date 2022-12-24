using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using PartsInventory.Models.Enums;

namespace PartsInventory.Models.Inventory.Main
{
   public class InvoiceModel : BaseModel, IModel
   {
      #region Local Props
      private uint _orderNumber = 0;
      private string _path = "";
      private ObservableCollection<InvoicePartModel> _parts = new();
      private SupplierType _supplierType = SupplierType.NA;
      private bool _isAddedToParts = false;
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
      [BsonIgnore]
      [JsonIgnore]
      public ObservableCollection<InvoicePartModel> PartModels
      {
         get => _parts;
         set
         {
            _parts = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SubTotal));
         }
      }

      public List<InvoicePartModel> Parts
      {
         get => _parts.ToList();
         set
         {
            _parts = new(value);
            OnPropertyChanged(nameof(PartModels));
         }
      }

      public SupplierType SupplierType
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

      [BsonIgnore]
      [JsonIgnore]
      public decimal SubTotal
      {
         get
         {
            if (Parts is null)
               return 0;
            if (!Parts.Any())
               return 0;
            return Parts.Sum(p => p.ExtendedPrice);
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
