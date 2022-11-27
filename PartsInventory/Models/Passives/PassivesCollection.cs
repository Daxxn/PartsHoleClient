using MVVMLibrary;

using PartsInventory.Models.Enums;
using PartsInventory.Models.Passives.Book;

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

      private ObservableCollection<PassiveBookModel> _books = new();
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

      public void Add(IPassive model, PassiveType type)
      {
         switch (type)
         {
            case PassiveType.Resistor:
                  Add((Resistor)model);
               break;
            case PassiveType.Capacitor:
               Add((Capacitor)model);
               break;
            case PassiveType.Inductor:
               Add((Inductor)model);
               break;
            default:
               break;
         }
      }

      public void AddRange<T>(IEnumerable<T> data) where T : IPassive
      {
         if (data is IEnumerable<Resistor> resistors)
         {
            foreach (Resistor res in resistors)
            {
               Add(res);
            }
         }
         else if (data is IEnumerable<Capacitor> capacitors)
         {
            foreach (var cap in capacitors)
            {
               Add(cap);
            }
         }
         else if (data is IEnumerable<Inductor> inductors)
         {
            foreach (var ind in inductors)
            {
               Add(ind);
            }
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

      public ObservableCollection<PassiveBookModel> Books
      {
         get => _books;
         set
         {
            _books = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
