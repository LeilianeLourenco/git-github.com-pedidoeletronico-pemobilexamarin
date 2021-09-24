using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Generic
{
    public class FormatDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return value.ToCurrencyStringPtBr();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
