using System;
using System.Text.RegularExpressions;
using LinePress.Options;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace LinePress
{
   public class LinePressTransformSource : ILineTransformSource
   {
      private readonly LineTransform defaultTransform = new LineTransform(0d, 0d, 1d);

      private LineTransform emptyLineTransform;
      private LineTransform customTokensTransform;
      private LineTransform lineSpacingTransform;

      private readonly IWpfTextView textView;
      private readonly LinePressSettings settings = new LinePressSettings();

      private LinePressTransformSource(IWpfTextView view)
      {
         textView = view;
         SettingsStore.LoadSettings(settings);
         SettingsStore.SettingsChanged += OnSettingsChanged;
         SetTransforms();
      }

      public static LinePressTransformSource Create(IWpfTextView view)
      {
         return view.Properties.GetOrCreateSingletonProperty(() => new LinePressTransformSource(view));
      }

      public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
      {
         var lineText = line.Snapshot.GetText(line.Start, line.Length).Trim();

         if (IsComment(lineText))
            return settings.ApplySpacingToComments ? lineSpacingTransform : defaultTransform;

         if (IsLongOrWrapped(line))
            return lineSpacingTransform;

         if (settings.CompressEmptyLines && string.IsNullOrWhiteSpace(lineText))
            return emptyLineTransform;

         if (settings.CompressCustomTokens && settings.CustomTokens.Contains(lineText))
            return customTokensTransform;

         return lineSpacingTransform;
      }

      private bool IsComment(string codeLine) => Regex.Match(codeLine, @"^\/\/.*").Success;

      private bool IsLongOrWrapped(ITextViewLine line) => line.Length > 100
                                                       || !line.IsFirstTextViewLineForSnapshotLine
                                                       || !line.IsLastTextViewLineForSnapshotLine
                                                       || line.End > line.Start.GetContainingLine().End;

      private double CustomTokensScale => (100d - settings.CustomTokensScale) / 100d;

      private double EmptyLineScale => (100d - settings.EmptyLineScale) / 100d;

      private void SetTransforms()
      {
         var lineHeight = textView?.TextViewLines?.FirstVisibleLine?.TextHeight ?? 10d;
         var lineSpacing = (double)settings.LineSpacingPercent;
         var pixelSpace = settings.LineSpacingPercent != 0 ? Math.Round(lineHeight * lineSpacing / 200d) : 0d;

         lineSpacingTransform = new LineTransform(pixelSpace, pixelSpace, 1d);
         emptyLineTransform = new LineTransform(0d, 0d, EmptyLineScale);
         customTokensTransform = new LineTransform(0d, 0d, CustomTokensScale);
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
   }
}
