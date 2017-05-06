using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

namespace LinePress.Options
{
   public class LinePressSettings : ISettings, INotifyPropertyChanged
   {
      private bool compressEmptyLines = true;
      private bool compressCurlyBraces = true;

      private int emptyLineRate = 50;
      private int curlyBracesRate = 15;


      public double EmptyLineScale { get; private set; } = 0.50;
      public double CurlyBraceScale { get; private set; } = 0.75;

      #region ISettings Members

      public string Name => "LinePress";
      
      #endregion


      [Setting]
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

      [Setting]
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

      [Setting]
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

      [Setting]
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


      #region INotifyPropertyChanged Members

      public event PropertyChangedEventHandler PropertyChanged;

      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      #endregion
   }
}
