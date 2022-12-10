using System.Windows.Media;

using PartsInventory.Models.Enums;

namespace PartsInventory.Utils.Messager
{
   public interface IMessageService
   {
      string? Message { get; set; }
      SolidColorBrush SeverityColor { get; set; }

      void AddMessage(string message, Severity severity);
   }
}