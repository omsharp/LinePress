using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace LinePress.Options
{
   public class LinePressSettings : ISettings, INotifyPropertyChanged
   {
      #region Fields

      private bool compressEmptyLines = true;
      private bool compressCustomTokens = true;

      // lineSpacing = topSpace + bottomSpace
      private double lineSpacing = 0; 
      private double topSpace = 0;
      private double bottomSpace = 0;
      private int emptyLineScale = 50;
      private int customTokensScale = 25;
      
      private ObservableCollection<string> customTokens
         = new ObservableCollection<string> { "{", "}" };

      #endregion

      #region Events

      public event Action TokenAdded;

      #endregion

      #region Constructors

      public LinePressSettings()
      {
         InsertTokenCommand = new RelayCommand<string>(CanInsertToken, t =>
         {
            CustomTokens.Add(t);
            TokenAdded?.Invoke();
         });

         DeleteTokenCommand = new RelayCommand<string>(CanDeleteToken, t => CustomTokens.Remove(t));
      }

      #endregion

      #region Settings Properties

      public double LineSpacing
      {
         get { return lineSpacing; }
         set { SetField(ref lineSpacing, value); }
      }

      [Setting]
      public double TopSpace
      {
          get { return lineSpacing / 2; }
          set { SetField(ref topSpace, value); }
      }

      [Setting]
      public double BottomSpace
      {
         get { return lineSpacing / 2; }
         set { SetField(ref bottomSpace, value); }
      }

      [Setting]
      public bool CompressEmptyLines
      {
         get { return compressEmptyLines; }
         set { SetField(ref compressEmptyLines, value); }
      }

      [Setting]
      public int EmptyLineScale
      {
         get { return emptyLineScale; }
         set { SetField(ref emptyLineScale, value); }
      }

      [Setting]
      public bool CompressCustomTokens
      {
         get { return compressCustomTokens; }
         set { SetField(ref compressCustomTokens, value); }
      }

      [Setting]
      public int CustomTokensScale
      {
         get { return customTokensScale; }
         set { SetField(ref customTokensScale, value); }
      }

      [Setting]
      public string CustomTokensString
      {
         get { return ConvertTokensListToString(); }
         set { BuildTokensListFromString(value); }
      }

      #endregion

      #region Non-Settings Properties

      public ObservableCollection<string> CustomTokens => customTokens;

      #endregion

      #region Commands

      public RelayCommand<string> InsertTokenCommand { get; private set; }
      public RelayCommand<string> DeleteTokenCommand { get; private set; }

      private bool CanInsertToken(string token) =>
         !string.IsNullOrWhiteSpace(token) && !CustomTokens.Contains(token);

      private bool CanDeleteToken(string token) =>
         !string.IsNullOrWhiteSpace(token) && CustomTokens.Contains(token);

      #endregion

      #region Helpers

      private void BuildTokensListFromString(string str)
      {
         customTokens.Clear();

         foreach (var token in str.Split(null))
            customTokens.Add(token);
      }

      private string ConvertTokensListToString()
      {
         var stringBuilder = new StringBuilder(customTokens[0]);

         for (var i = 1; i < customTokens.Count; i++)
         {
            stringBuilder.Append(' ');
            stringBuilder.Append(customTokens[i]);
         }
         
         return stringBuilder.ToString();
      }

      #endregion

      #region ISettings Members

      public string Key => "LinePress";

      #endregion

      #region INotifyPropertyChanged Members

      public event PropertyChangedEventHandler PropertyChanged;

      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
      {
         if (EqualityComparer<T>.Default.Equals(field, value))
            return;

         field = value;

         OnPropertyChanged(propertyName);
      }
      #endregion
   }
}
