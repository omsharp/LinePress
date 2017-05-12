
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;


namespace LinePress.Options
{
   public partial class OptionsPageControl
   {
      public LinePressSettings Settings { get; private set; } = new LinePressSettings();

      public OptionsPageControl()
      {
         SettingsStore.LoadSettings(Settings);
         DataContext = Settings;

         InitializeComponent();
      }

      private void NewTokenTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
      {
         e.Handled = e.Key == Key.Space;
      }

      public void Clear()
      {
         NewTokenTextBox.Text = string.Empty;
      }
   }
}
