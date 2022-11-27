using System;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Extensions.Options;

using PartsInventory.Models;
using PartsInventory.Resources.Settings;

namespace PartsInventory.Resources.Packages.Passives
{
   public partial class SmdDisplay : UserControl
   {
      public string PackageString
      {
         get { return (string)GetValue(PackageStringProperty); }
         set
         {
            SetValue(PackageStringProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for PackageString.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty PackageStringProperty =
    DependencyProperty.Register("PackageString", typeof(string), typeof(SmdDisplay), new PropertyMetadata("0402"));

      public double PackageWidth
      {
         get { return (double)GetValue(PackageWidthProperty); }
         set { SetValue(PackageWidthProperty, value); }
      }

      // Using a DependencyProperty as the backing store for PackageWidth.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty PackageWidthProperty =
         DependencyProperty.Register("PackageWidth", typeof(double), typeof(SmdDisplay), new PropertyMetadata(default(double)));

      public double PackageHeight
      {
         get { return (double)GetValue(PackageHeightProperty); }
         set { SetValue(PackageHeightProperty, value); }
      }

      // Using a DependencyProperty as the backing store for PackageHeight.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty PackageHeightProperty =
         DependencyProperty.Register("PackageHeight", typeof(double), typeof(SmdDisplay), new PropertyMetadata(default(double)));

      public double PadWidth
      {
         get { return (double)GetValue(PadWidthProperty); }
         set { SetValue(PadWidthProperty, value); }
      }

      // Using a DependencyProperty as the backing store for PadWidth.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty PadWidthProperty =
         DependencyProperty.Register("PadWidth", typeof(double), typeof(SmdDisplay), new PropertyMetadata(default(double)));

      private readonly IOptions<GeneralSettings> _generalSettings;
      public SmdDisplay()
      {
         var settings = (IOptions<GeneralSettings>)App.AppHost!.Services.GetService(typeof(IOptions<GeneralSettings>))!;

         _generalSettings = settings;
         InitializeComponent();
         //CalcSize();
      }

      private void courtyardRect_LayoutUpdated(object sender, EventArgs e)
      {
         CalcSize();
      }

      private void CalcSize()
      {
         if (PackageString.Length == 4)
         {
            if (double.TryParse(PackageString[0..2], out double width))
            {
               if (double.TryParse(PackageString[2..], out double height))
               {
                  CalcRealSize(width, height);
               }
            }
         }
         if (PackageWidth == 0)
            PadWidth = 0;
         else
            PadWidth = PackageWidth / 3;
      }

      private void CalcRealSize(double width, double height)
      {
         var dpi = SystemParameters.WorkArea.Width / MonitorSizeToWidth();

         PackageWidth = (width * 0.01) * dpi;
         PackageHeight = (height * 0.01) * dpi;
      }

      private double MonitorSizeToWidth()
      {
         if (Constants.StandardAspectRatios.ContainsKey(_generalSettings.Value.AspectRatio))
         {
            return
               Constants.StandardAspectRatios[_generalSettings.Value.AspectRatio].aspectRatio
                  *
               (_generalSettings.Value.MonitorSize / Constants.StandardAspectRatios[_generalSettings.Value.AspectRatio].aspectSqrt);
         }
         return
            Constants.StandardAspectRatios["16/9"].aspectRatio
               *
            (_generalSettings.Value.MonitorSize / Constants.StandardAspectRatios["16/9"].aspectSqrt);
      }

      private void Root_Loaded(object sender, RoutedEventArgs e)
      {
         CalcSize();
      }
   }
}
