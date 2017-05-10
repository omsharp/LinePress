using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System;

namespace LinePress.Options
{
   [ClassInterface(ClassInterfaceType.AutoDual)]
   [ComVisible(true)]
   [Guid("e137d6a4-53b9-4995-bb49-bbbb20088b48")]
   public class OptionsPage : UIElementDialogPage
   {
      private OptionsPageControl pageControl;

      protected override UIElement Child
      {
         get { return pageControl ?? (pageControl = new OptionsPageControl()); }
      }

      protected override void OnActivate(CancelEventArgs e)
      {
         base.OnActivate(e);
      }

      protected override void OnApply(PageApplyEventArgs e)
      {
         if (e.ApplyBehavior == ApplyKind.Apply)
            SettingsStore.SaveSettings(pageControl.Settings);
         
         base.OnApply(e);
      }

      protected override void OnClosed(EventArgs e)
      {
         pageControl.Clear();

         base.OnClosed(e);
      }
   }
}