using Microsoft.VisualStudio.Text.Differencing;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace LinePress
{
   [Export(typeof(ILineTransformSourceProvider))]
   [ContentType("text")]
   [TextViewRole(PredefinedTextViewRoles.Document)]
   [TextViewRole(PredefinedTextViewRoles.EmbeddedPeekTextView)]
   [TextViewRole(PredefinedTextViewRoles.PreviewTextView)]
   [TextViewRole(PredefinedTextViewRoles.Interactive)]
   [TextViewRole("PRINTABLE")]
   internal class LinePressTransformSourceProvider : ILineTransformSourceProvider
   {
      public ILineTransformSource Create(IWpfTextView textView)
      {
         if (textView.Roles.Contains(DifferenceViewerRoles.LeftViewTextViewRole)
             || textView.Roles.Contains(DifferenceViewerRoles.RightViewTextViewRole)
             || textView.Roles.Contains("VSMERGEDEFAULT" /* MergeViewerRoles.VSMergeDefaultRole from TFS */))
         {
            // Ignore diff views
            return null;
         }

         return LinePressTransformSource.Create(textView);
      }
   }
}
