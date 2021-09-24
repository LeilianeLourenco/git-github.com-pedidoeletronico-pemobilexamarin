using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Generic
{
    public class InvertBoolConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return !System.Convert.ToBoolean(value.ToString());
            }
            catch (Exception ex)
            {
                return true;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
