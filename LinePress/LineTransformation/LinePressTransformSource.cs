using System.Text.RegularExpressions;
using LinePress.Options;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace LinePress
{
   public class LinePressTransformSource : ILineTransformSource
   {
      private LineTransform defaultTransform;
      private LineTransform spacedTransform;

      private LineTransform emptyLineTransform;
      private LineTransform spacedEmptyLineTransform;

      private LineTransform customTokensTransform;
      private LineTransform spacedCustomTokensTransform;

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
         var bottomSpace = (double)settings.LineSpacing;
         defaultTransform = new LineTransform(0d, 0d, 1d);
         spacedTransform = new LineTransform(0d, bottomSpace, 1d);
         emptyLineTransform = new LineTransform(0d, 0d, (100d - settings.EmptyLineScale) / 100d);
         spacedEmptyLineTransform = new LineTransform(0d, bottomSpace, (100d - settings.EmptyLineScale) / 100d);
         customTokensTransform = new LineTransform(0d, 0d, (100d - settings.CustomTokensScale) / 100d);
         spacedCustomTokensTransform = new LineTransform(0d, bottomSpace, (100d - settings.CustomTokensScale) / 100d);
      }

      public static LinePressTransformSource Create(IWpfTextView view)
      {
         return view.Properties.GetOrCreateSingletonProperty(() => new LinePressTransformSource(view));
      }

      public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
      {
         var lineText = line.Snapshot.GetText(line.Start, line.Length).Trim();
         
         var isComment = Regex.Match(lineText, @"^\/\/.*").Success;
         if (isComment) 
            return settings.ApplySpacingToComments ? spacedTransform : defaultTransform;

         var isLongOrWrappedLine = line.Length > 100 || line.End > line.Start.GetContainingLine().End
                                   || !line.IsFirstTextViewLineForSnapshotLine || !line.IsLastTextViewLineForSnapshotLine;
         if (isLongOrWrappedLine) 
            return spacedTransform;
         
         if (string.IsNullOrWhiteSpace(lineText) && settings.CompressEmptyLines)
            return spacedEmptyLineTransform;

         if (settings.CustomTokens.Contains(lineText) && settings.CompressCustomTokens)
            return spacedCustomTokensTransform;
         
         return spacedTransform;
      }

   }
}
