using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Generic
{
    public class CpfMaskConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (PageCliente.StaticViewModel?.currentModel?.stJuridico == 0)
            {
                var retorno = value.ToCpfFormat();
                return retorno;
            }
            else
            {
                return value;
            }
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }


    public class CNPJMaskConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (PageCliente.StaticViewModel?.currentModel?.stJuridico == 1)
            {
                var retorno = value.ToCNPJFormat();

                return retorno;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
