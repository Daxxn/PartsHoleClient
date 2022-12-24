
using Microsoft.Win32;

public class FolderBrowserDialog : FileDialog
{
   #region Local Props

   #endregion

   #region Constructors
   public FolderBrowserDialog() { }
   #endregion

   #region Methods
   public override bool? ShowDialog()
   {
      return base.ShowDialog();
   }
   #endregion

   #region Full Props

   #endregion
}