using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LinePress.Options
{
   public class Settings : INotifyPropertyChanged
   {
      private bool compressEmptyLines = false;
      private int emptyLineRate = 5;

      private bool compressCurlyBraces = false;
      private int curlyBracesRate = 5;

      public event PropertyChangedEventHandler PropertyChanged;

      public double EmptyLineScale { get; private set; } = 0.5;
      public double CurlyBraceScale { get; private set; } = 0.5;

      public bool CompressEmptyLines
      {
         get { return compressEmptyLines; }
         set
         {
            if (value == compressEmptyLines)
               return;
            compressEmptyLines = value;
            OnPropertyChanged();
         }
      }

      public int EmptyLineCompressionRate
      {
         get { return emptyLineRate; }
         set
         {
            if (value == emptyLineRate)
               return;

            emptyLineRate = value;
            EmptyLineScale = (100 - value) / 100d;
            OnPropertyChanged();
         }
      }

      public bool CompressCurlyBraces
      {
         get { return compressCurlyBraces; }
         set
         {
            if (value == compressCurlyBraces) return;
            compressCurlyBraces = value;
            OnPropertyChanged();
         }
      }

      public int CurlyBraceCompressionRate
      {
         get { return curlyBracesRate; }
         set
         {
            if (value == curlyBracesRate)
               return;

            curlyBracesRate = value;
            CurlyBraceScale = (100 - value) / 100d;
            OnPropertyChanged();
         }
      }

      public void Copy(Settings source)
      {
         CompressEmptyLines = source.CompressEmptyLines;
         CompressCurlyBraces = source.CompressCurlyBraces;
         EmptyLineCompressionRate = source.emptyLineRate;
         CurlyBraceCompressionRate = source.CurlyBraceCompressionRate;
      }

      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
   }
}
