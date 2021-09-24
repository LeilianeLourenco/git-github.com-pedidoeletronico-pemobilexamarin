using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Pedido
{
    public class ShortGroupKeyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var longdate = (value ?? "").ToString();
            if (longdate != "")
            {
                var date = System.Convert.ToDateTime(longdate);
                return System.Convert.ToInt32(date.ToString("dd")) + "/" + System.Convert.ToInt32(date.ToString("MM"));
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
