using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Pedido
{
    public class LoaderCarregandoProdutosConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
