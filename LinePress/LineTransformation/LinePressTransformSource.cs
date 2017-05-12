using LinePress.Options;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace LinePress
{
   public class LinePressTransformSource : ILineTransformSource
   {
      private readonly LineTransform defaultTransform = new LineTransform(0.0, 0.0, 1.0);

      private LineTransform emptyLineTransform;
      private LineTransform customTokensTransform;

      private readonly IWpfTextView textView;
      private readonly LinePressSettings settings = new LinePressSettings();

      private LinePressTransformSource(IWpfTextView view)
      {
         textView = view;
         SettingsStore.LoadSettings(settings);
         SettingsStore.SettingsChanged += OnSettingsChanged;
         SetTransforms();
      }

      private void OnSettingsChanged()
      {
         SettingsStore.LoadSettings(settings);

         SetTransforms();

         var firstLine = textView.TextViewLines.FirstVisibleLine;

         textView.DisplayTextLineContainingBufferPosition(firstLine.Start,
                                                          firstLine.Top - textView.ViewportTop,
                                                          ViewRelativePosition.Top);
      }

      private void SetTransforms()
      {
         emptyLineTransform = new LineTransform(0.0, 0.0, (100d - settings.EmptyLineScale) / 100d);
         customTokensTransform = new LineTransform(0.0, 0.0, (100d - settings.CustomTokensScale) / 100d);
      }

      public static LinePressTransformSource Create(IWpfTextView view)
      {
         return view.Properties.GetOrCreateSingletonProperty(() => new LinePressTransformSource(view));
      }

      public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
      {
         if (!settings.CompressEmptyLines && !settings.CompressCustomTokens)
            return defaultTransform;

         if (line.Length > 100
            || line.End > line.Start.GetContainingLine().End
            || !line.IsFirstTextViewLineForSnapshotLine
            || !line.IsLastTextViewLineForSnapshotLine)
         {
            // Ignore long and wrapped lines.
            return defaultTransform;
         }

         var lineText = line.Snapshot.GetText(line.Start, line.Length).Trim();

         if (settings.CompressEmptyLines && string.IsNullOrWhiteSpace(lineText))
         {
            return emptyLineTransform;
         }
         else if (settings.CompressCustomTokens && settings.CustomTokens.Contains(lineText))
         {
            return customTokensTransform;
         }

         return defaultTransform;
      }
   }
}
