using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;

using Microsoft.Extensions.Options;

using MVVMLibrary;

using PartsInventory.Models.API.Buffer;
using PartsInventory.Models.Enums;
using PartsInventory.Resources.Settings;

namespace PartsInventory.Utils.Messager;

public class MessageService : Model, IMessageService
{
   #region Local Props
   private static readonly Dictionary<Severity, SolidColorBrush> MessageColors = new()
   {
      { Severity.Info, new(Color.FromRgb(100,100,255)) },
      { Severity.Warning, new(Color.FromRgb(255,255,100)) },
      { Severity.Error, new(Color.FromRgb(255,100,100)) }
   };
   private static readonly SolidColorBrush _transparent = new(Color.FromArgb(0,0,0,0));
   private readonly Timer _timer;
   private SolidColorBrush _severeColor = _transparent;
   private string? _message;

   private Queue<Message> _messageQueue = new();
   #endregion

   #region Constructors
   public MessageService(IOptions<GeneralSettings> settings)
   {
      _timer = new()
      {
         AutoReset = true,
         Enabled = true,
         Interval = settings.Value.MessageInterval
      };
      _timer.Elapsed += Timer_Elapsed;
   }
   #endregion

   #region Methods
   private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
   {
      if (MessageQueue.Count == 0)
      {
         Message = null;
         SeverityColor = _transparent;
         _timer.Stop();
      }
      else
      {
         var msg = MessageQueue.Dequeue();
         Message = msg.Text;
         SeverityColor = MessageColors[msg.Severity];
      }
   }

   public void AddMessage(string msg, Severity severity)
   {
      if (Message is null)
      {
         Message = msg;
         SeverityColor = MessageColors[severity];
      }
      else
      {
         MessageQueue.Enqueue(new(msg, severity));
      }
      _timer.Restart();
   }
   #endregion

   #region Full Props
   public string? Message
   {
      get => _message;
      set
      {
         _message = value;
         OnPropertyChanged();
      }
   }
   public SolidColorBrush SeverityColor
   {
      get => _severeColor;
      set
      {
         _severeColor = value;
         OnPropertyChanged();
      }
   }
   public Queue<Message> MessageQueue
   {
      get => _messageQueue;
      set
      {
         _messageQueue = value;
         _timer.Start();
         OnPropertyChanged();
      }
   }
   #endregion
}
