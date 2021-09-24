using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Pedido
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return ColorStaticModel.CinzaEscuro;
            var bValor = System.Convert.ToBoolean(value.ToString());
            return bValor ? ColorStaticModel.CinzaPrincipal : ColorStaticModel.VermelhoPrincipal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ColorStaticModel.CinzaPrincipal;
        }
    }

    public class BoolToColorConverterLitaItensPedido : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return ColorStaticModel.CinzaEscuro;
            var bValor = System.Convert.ToBoolean(value.ToString());
            return !bValor ? ColorStaticModel.CinzaPrincipal : ColorStaticModel.RoxoPrincipal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ColorStaticModel.CinzaPrincipal;
        }
    }
}
