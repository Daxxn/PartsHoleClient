using MVVMLibrary;
using PartsInventory.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels
{
   public class DatasheetViewModel : ViewModel
   {
      #region Local Props
      public event EventHandler<Stream> DocLoadedEvent = (s,e) => { };
      private Stream? _docStream = null;
      private PartModel? _currentPart = null;
      #endregion

      #region Constructors
      public DatasheetViewModel() { }
      #endregion

      #region Methods
      public void OpenDatasheetEventHandler(object sender, PartModel part)
      {
         CurrentPart = part;
         if (File.Exists(part.Datasheet))
         {
            DocStream = File.OpenRead(part.Datasheet);
            DocLoadedEvent?.Invoke(this, DocStream);
         }
      }
      #endregion

      #region Full Props
      public PartModel? CurrentPart
      {
         get => _currentPart;
         set
         {
            _currentPart = value;
            OnPropertyChanged();
         }
      }

      public Stream? DocStream
      {
         get => _docStream;
         set
         {
            _docStream = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
