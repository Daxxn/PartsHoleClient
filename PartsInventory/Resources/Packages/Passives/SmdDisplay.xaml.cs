using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Newtonsoft.Json.Linq;

using PartsInventory.Models;

namespace PartsInventory.Resources.Packages.Passives
{
   /// <summary>
   /// Interaction logic for SmdDisplay.xaml
   /// </summary>
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

      public SmdDisplay()
      {
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
         if (Constants.StandardAspectRatios.ContainsKey(Settings.Default.AspectRatio))
         {
            return
               Constants.StandardAspectRatios[Settings.Default.AspectRatio].aspectRatio
                  *
               (Settings.Default.MonitorSize / Constants.StandardAspectRatios[Settings.Default.AspectRatio].aspectSqrt);
         }
         return
            Constants.StandardAspectRatios["16/9"].aspectRatio
               *
            (Settings.Default.MonitorSize / Constants.StandardAspectRatios["16/9"].aspectSqrt);
      }

      private void Root_Loaded(object sender, RoutedEventArgs e)
      {
         CalcSize();
      }
   }
}
