using System.Windows.Input;


namespace LinePress.Options
{
   public partial class OptionsPageControl
   {
      public LinePressSettings Settings { get; private set; } 
         = new LinePressSettings();

      public OptionsPageControl()
      {
         Settings.TokenAdded += Clear;

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

      private void NewTokenTextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
      {
         e.Handled = e.Command == ApplicationCommands.Paste;
      }
   }
}
