using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Pedido
{
   public class DisplayToCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var valor = System.Convert.ToInt32((value ?? 0));


            var parametro = System.Convert.ToString((parameter ?? "{0}"));

            return string.Format(parametro, valor);


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
