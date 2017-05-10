
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

   public class CompressionScaleToSliderValueConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return 100 - (double.Parse(value.ToString()) * 100);
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return (100 - int.Parse(value.ToString())) / 100;
      }
   }
}
