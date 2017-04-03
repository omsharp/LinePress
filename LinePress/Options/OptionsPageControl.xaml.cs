using System.Collections.Generic;
using System.Linq;

namespace LinePress.Options
{
   public partial class OptionsPageControl
   {
      public Settings Settings { get; } = new Settings();

      public OptionsPageControl()
      {
         InitializeComponent();
      }

      public void Refresh()
      {
         Settings.Copy(SettingsManager.CurrentSettings);
         DataContext = Settings;
      }
   }
}
