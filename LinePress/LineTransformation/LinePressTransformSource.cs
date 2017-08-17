using LinePress.Options;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace LinePress
{
   public class LinePressTransformSource : ILineTransformSource
   {
      private LineTransform defaultTransform;

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
         defaultTransform = new LineTransform(settings.TopSpace, settings.BottomSpace, 1d);
         emptyLineTransform = new LineTransform(settings.TopSpace, settings.BottomSpace, (100d - settings.EmptyLineScale) / 100d);
         customTokensTransform = new LineTransform(settings.TopSpace, settings.BottomSpace, (100d - settings.CustomTokensScale) / 100d);
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
         
         if (string.IsNullOrWhiteSpace(lineText) && settings.CompressEmptyLines)
         {
            return emptyLineTransform;
         }
         else if (settings.CustomTokens.Contains(lineText) && settings.CompressCustomTokens)
         {
            return customTokensTransform;
         }

         return defaultTransform;
      }
   }
}
