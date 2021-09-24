using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Cliente
{
    public class DisplayToNullValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null && (string.IsNullOrEmpty(value.ToString()) == false))
                {
                    return value.ToString().ToUpper();
                }
                else
                {
                    return parameter == null ? "clique para selecionar" : parameter.ToString().ToUpper();
                }
            }
            catch (Exception)
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
