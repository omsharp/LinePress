using LinePress.Options;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System;

namespace LinePress
{
   public class LinePressTransformSource : ILineTransformSource
   {
      private static readonly LineTransform defaultTransform = new LineTransform(0.0, 0.0, 1.0);
      
      private readonly IWpfTextView view;

      private LinePressTransformSource(IWpfTextView view)
      {
         this.view = view;
         SettingsManager.SettingsSaved += OnSettingsSaved;
      }

      private void OnSettingsSaved(object sender, EventArgs e)
      {
         var firstLine = view.TextViewLines.FirstVisibleLine;
         view.DisplayTextLineContainingBufferPosition(firstLine.Start, firstLine.Top - view.ViewportTop, ViewRelativePosition.Top);
      }

      public static LinePressTransformSource Create(IWpfTextView view)
      {
         return view.Properties.GetOrCreateSingletonProperty(() => new LinePressTransformSource(view));
      }

      public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
      {
         if (!SettingsManager.CurrentSettings.CompressEmptyLines && !SettingsManager.CurrentSettings.CompressCurlyBraces)
            return defaultTransform;

         if (line.Length > 100 || line.End > line.Start.GetContainingLine().End ||
             !line.IsFirstTextViewLineForSnapshotLine || !line.IsLastTextViewLineForSnapshotLine)
         {
            // Long or wrapped lines -- even if they don't contain interesting characters -- get the default transform to avoid the cost of checking the entire line.
            return defaultTransform;
         }

         bool allWhiteSpace = true;
         bool curlyBrace = true;

         for (int i = line.Start; (i < line.End); ++i)
         {
            char c = line.Snapshot[i];

            if (c != '{' && c != '}' && !char.IsWhiteSpace(c))
            {
               return defaultTransform;
            }
            else if (!char.IsWhiteSpace(c))
            {
               allWhiteSpace = false;
               curlyBrace = (c == '{' || c == '}');
            }
         }

         if (allWhiteSpace && SettingsManager.CurrentSettings.CompressEmptyLines)
            return new LineTransform(0.0, 0.0, SettingsManager.CurrentSettings.EmptyLineScale);

         if (curlyBrace && SettingsManager.CurrentSettings.CompressCurlyBraces)
            return new LineTransform(0.0, 0.0, SettingsManager.CurrentSettings.CurlyBraceScale);

         return defaultTransform;
      }
   }
}
