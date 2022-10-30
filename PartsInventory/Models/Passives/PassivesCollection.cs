using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Passives
{

   public class PassivesCollection : Model
   {
      #region Local Props
      private ObservableCollection<Resistor> _resistors = new();
      private ObservableCollection<Capacitor> _capacitors = new();
      private ObservableCollection<Inductor> _inductors = new();
      #endregion

      #region Constructors
      public PassivesCollection() { }
      #endregion

      #region Methods
      public IEnumerable<IPassive> GetPassivesList(int index)
      {
         if (index == 0) return Resistors;
         else if (index == 1) return Capacitors;
         else if (index == 2) return Inductors;
         else throw new IndexOutOfRangeException("Index doesnt map onto any list.");
      }

      public void Add<T>(T passive) where T : IPassive
      {
         if (passive is Resistor res)
         {
            if (Resistors.FirstOrDefault(r => r.ManufacturerNumber == res.ManufacturerNumber) != null) return;
            Resistors.Add(res);
         }
         else if (passive is Capacitor cap)
         {
            if (Resistors.FirstOrDefault(c => c.ManufacturerNumber == cap.ManufacturerNumber) != null) return;
            Capacitors.Add(cap);
         }
         else if (passive is Inductor ind)
         {
            if (Resistors.FirstOrDefault(l => l.ManufacturerNumber == ind.ManufacturerNumber) != null) return;
            Inductors.Add(ind);
         }
      }
      #endregion

      #region Full Props
      public ObservableCollection<Resistor> Resistors
      {
         get => _resistors;
         set
         {
            _resistors = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<Capacitor> Capacitors
      {
         get => _capacitors;
         set
         {
            _capacitors = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<Inductor> Inductors
      {
         get => _inductors;
         set
         {
            _inductors = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
