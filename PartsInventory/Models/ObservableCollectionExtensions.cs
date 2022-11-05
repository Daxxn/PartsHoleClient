using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PartsInventory.Models
{
   public static class ObservableCollectionExtensions
   {
      public static void Replace<T>(this ObservableCollection<T> list, Func<T, bool> predicate, T replacement)
      {
         for (int i = 0; i < list.Count; i++)
         {
            if (predicate(list[i]))
            {
               list[i] = replacement;
            }
         }
      }

      public static void MergeAdd<T>(this ObservableCollection<T> list, Func<T, T, bool> predicate, IEnumerable<T> changedList)
      {
         var add = new List<T>();

         foreach (var item in changedList)
         {
            int index = 0;
            for (int i = 0; i < list.Count; i++)
            {
               if (predicate(item, list[i]))
               {
                  index = i;
               }
            }
            if (index != 0)
            {
               list[index] = item;
            }
            else
            {
               list.Add(item);
            }
         }
      }
   }
}
