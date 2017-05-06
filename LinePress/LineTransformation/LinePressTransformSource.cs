using LinePress.Options;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System;

namespace LinePress
{
   public class LinePressTransformSource : ILineTransformSource
   {
      private readonly LineTransform defaultTransform = new LineTransform(0.0, 0.0, 1.0);

      private LineTransform emptyLineTransform;
      private LineTransform curlyBraceTransform;

      private readonly IWpfTextView textView;
      private readonly LinePressSettings settings = new LinePressSettings();

      private LinePressTransformSource(IWpfTextView view)
      {
         textView = view;
         SettingsStore.LoadSettings(settings);
         SettingsStore.Saved += OnSettingsSaved;
         SetTransforms();
      }

      private void OnSettingsSaved()
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
         emptyLineTransform = new LineTransform(0.0, 0.0, settings.EmptyLineScale);
         curlyBraceTransform = new LineTransform(0.0, 0.0, settings.CurlyBraceScale);
      }

      public static LinePressTransformSource Create(IWpfTextView view)
      {
         return view.Properties.GetOrCreateSingletonProperty(() => new LinePressTransformSource(view));
      }

      public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
      {
         if (!settings.CompressEmptyLines && !settings.CompressCurlyBraces)
            return defaultTransform;

         if (line.Length > 100 || line.End > line.Start.GetContainingLine().End ||
             !line.IsFirstTextViewLineForSnapshotLine || !line.IsLastTextViewLineForSnapshotLine)
         {
            // Long or wrapped lines -- even if they don't contain interesting characters -- get the default transform to avoid the cost of checking the entire line.
            return defaultTransform;
         }

         bool allWhiteSpace = true;
         bool curlyBraceOnly = false;

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
               curlyBraceOnly = true;
            }
         }

         if (allWhiteSpace && settings.CompressEmptyLines)
            return emptyLineTransform;

         if (curlyBraceOnly && settings.CompressCurlyBraces)
            return curlyBraceTransform;

         return defaultTransform;
      }
   }
}
