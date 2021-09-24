using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Cliente
{
    public class ListFieldCompactConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var items = new ObservableCollection<BasicPickerModel>();
                if (value == null) return items;
                foreach (var item in value.ToString().Split(',').Where(item => item != ""))
                {
                    items.Add(new BasicPickerModel
                    {
                        Display = item,
                        Detail = item.IsValidEmailAddress() ? "enviar email" : "clique para ligar",
                        Image = item.IsValidEmailAddress() ? "ApplicationMail" : "ApplicationPhone"
                    });
                }
                return items;

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
