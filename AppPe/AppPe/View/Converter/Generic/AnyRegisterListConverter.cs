using System;
using System.Collections;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Generic
{
    public class AnyRegisterListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // parameter = N - normal / L - Lista
            var param = (parameter ?? "N").ToString();
            if (value == null && param == "N")
            {
                return true;
            }
            if (value == null && param == "L")
            {
                return false;
            }
            var list = value as IList;
            if (list != null && list.Count == 0 && param == "N")
                return true;

            return list != null && list.Count > 0 && param == "L";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
