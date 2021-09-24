using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Cliente
{
    public class TipoPessoaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (parameter == null) return null;
                if (parameter.ToString().Equals("DOC1"))
                {
                    return value.ToString() == "1" ? "CNPJ" : "CPF";
                }
                if (parameter.ToString().Equals("DOC2"))
                {
                    return value.ToString() == "1" ? "INSCRIÇÃO ESTADUAL" : "RG";
                }
                if (parameter.ToString().Equals("Nome1"))
                {
                    return value.ToString() == "1" ? "RAZÃO SOCIAL (*)" : "NOME (*)";
                }
                if (parameter.ToString().Equals("Nome2"))
                {
                    return value.ToString() == "1" ? "NOME FANTASIA (*)" : "APELIDO (*)";
                }
                return "";
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
