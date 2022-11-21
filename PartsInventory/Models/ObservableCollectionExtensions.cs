using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PartsInventory.Models.Extensions;

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
      foreach (var item in changedList)
      {
         int index = 0;
         for (int i = 0; i < list.Count; i++)
         {
            if (predicate(item, list[i]))
            {
               index = i;
               break;
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

   /// <summary>
   /// If the item 
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="list"></param>
   /// <param name="changedList"></param>
   public static void MergeAdd<T>(this ObservableCollection<T> list, IEnumerable<T> changedList)
   {
      foreach (var item in changedList)
      {
         if (list.Any(x => x?.Equals(item) != true))
         {
            list.Add(item);
         }
      }
   }
}
