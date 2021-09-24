using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Generic
{
    public class PhoneNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //return value.ToPhoneFormat();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //var valor = System.Convert.ToString((value ?? ""));

            //return valor.Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "");
            return value;
        }
    }
}
