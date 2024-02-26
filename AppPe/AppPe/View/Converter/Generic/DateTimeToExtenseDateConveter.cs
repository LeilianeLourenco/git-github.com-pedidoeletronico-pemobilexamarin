using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Generic
{
    public class DateTimeToExtenseDateConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                DateTime d;
                DateTime.TryParse(value.ToString(), out d);

                if (d.Year < 2000)
                    return "-";
                return d.ToLocalTime().ToString("g");
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeToExtenseFullDateConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                DateTime dataHoraEUA = DateTime.UtcNow;
                TimeZoneInfo fusoHorarioBrasil = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
                DateTime dataHoraBrasil = TimeZoneInfo.ConvertTimeFromUtc(dataHoraEUA, fusoHorarioBrasil);

                if (dataHoraBrasil.Year < 2000)
                    return "-";
                return dataHoraBrasil.ToString("dd/MM/yyyy     HH:mm");
            }
            else
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
