using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace LinePress.Options
{
   public static class SettingsManager
   {
      private const string COLLECTION_PATH = "LinePress";

      private static readonly WritableSettingsStore settingsStore
          = new ShellSettingsManager(ServiceProvider.GlobalProvider)
              .GetWritableSettingsStore(SettingsScope.UserSettings);

      public static Settings CurrentSettings { get; } = new Settings();

      public static EventHandler SettingsSaved;

      static SettingsManager()
      {
         Load();
      }

      public static void Save(Settings settings)
      {
         try
         {
            if (!settingsStore.CollectionExists(COLLECTION_PATH))
               settingsStore.CreateCollection(COLLECTION_PATH);

            settingsStore.SetBoolean(COLLECTION_PATH, nameof(Settings.CompressEmptyLines), settings.CompressEmptyLines);
            settingsStore.SetInt32(COLLECTION_PATH, nameof(Settings.EmptyLineCompressionRate), settings.EmptyLineCompressionRate);
            settingsStore.SetBoolean(COLLECTION_PATH, nameof(Settings.CompressCurlyBraces), settings.CompressCurlyBraces);
            settingsStore.SetInt32(COLLECTION_PATH, nameof(Settings.CurlyBraceCompressionRate), settings.CurlyBraceCompressionRate);
            
            CurrentSettings.Copy(settings);

            SettingsSaved?.Invoke(null, EventArgs.Empty);
         }
         catch (Exception ex)
         {
            Debug.Fail(ex.Message);
         }
      }
      
      private static void Load()
      {
         try
         {
            if (!settingsStore.CollectionExists(COLLECTION_PATH))
               return;

            if (settingsStore.PropertyExists(COLLECTION_PATH, nameof(Settings.CompressEmptyLines)))
               CurrentSettings.CompressEmptyLines = settingsStore.GetBoolean(COLLECTION_PATH, nameof(Settings.CompressEmptyLines));

            if (settingsStore.PropertyExists(COLLECTION_PATH, nameof(Settings.EmptyLineCompressionRate)))
               CurrentSettings.EmptyLineCompressionRate = settingsStore.GetInt32(COLLECTION_PATH, nameof(Settings.EmptyLineCompressionRate));

            if (settingsStore.PropertyExists(COLLECTION_PATH, nameof(Settings.CompressCurlyBraces)))
               CurrentSettings.CompressCurlyBraces = settingsStore.GetBoolean(COLLECTION_PATH, nameof(Settings.CompressCurlyBraces));

            if (settingsStore.PropertyExists(COLLECTION_PATH, nameof(Settings.CurlyBraceCompressionRate)))
               CurrentSettings.CurlyBraceCompressionRate = settingsStore.GetInt32(COLLECTION_PATH, nameof(Settings.CurlyBraceCompressionRate));
         }
         catch (Exception ex)
         {
            Debug.Fail(ex.Message);
         }
      }
   }
}