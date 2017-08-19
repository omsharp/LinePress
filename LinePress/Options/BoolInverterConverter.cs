using System;
using System.Windows.Data;


namespace LinePress.Options {
    public class BoolInverterConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            if (value is bool) return !(bool) value;
            return value;
        }


        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            if (value is bool) return !(bool) value;
            return value;
        }

        #endregion
    }
}