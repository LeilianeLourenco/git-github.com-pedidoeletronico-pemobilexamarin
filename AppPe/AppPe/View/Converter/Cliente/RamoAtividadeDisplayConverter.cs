using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Cliente
{
    public class RamoAtividadeDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var display = "";
            if (value != null)
            {
                display = RamoAtividadeRepository.GetBasicPickerModel(System.Convert.ToInt32(value)).Display;
            }
            return display;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
