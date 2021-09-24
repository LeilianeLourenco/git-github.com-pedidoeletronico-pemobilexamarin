using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Generic
{
    public class DecimalPlacesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { 
            var retorno = value.ToCurrencyStringSimplesPlacesPtBr();
            return retorno; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {  
            int iMax = 0;
            var valor = (string)(value ?? "");

            int.TryParse((parameter != null ? parameter.ToString() : ""), out iMax);
            if (iMax > 0)
            {
                valor = valor.RetiraCaracterEspecial();
                if (valor.Length > iMax)
                {
                    valor = valor.Substring(0, iMax);
                    valor = valor.Insert(valor.Length - 4, ",");
                    return valor.ToDoublePtBr();
                }
            }
            valor = valor.Replace(",", "").Replace(".", "");
            valor = valor.PadLeft(5, '0');
            valor = valor.Insert(valor.Length - 4, ",");
            return valor.ToDoublePtBr();
        }
    }
}
