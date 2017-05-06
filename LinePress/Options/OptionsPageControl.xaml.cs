
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
   }
}
