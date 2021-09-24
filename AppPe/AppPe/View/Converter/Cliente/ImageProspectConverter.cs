using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Cliente
{
    public class ImageProspectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var image = Device.OnPlatform("ApplicationClienteProspeccao.png", "ApplicationClienteProspeccao.png", "Assets/ApplicationClienteProspeccao.png");
            if (value == null || value.ToString() == "") return image;
            if (ClienteRepository.ClienteEstaEfetivado(System.Convert.ToInt32(value.ToString())))
                image = Device.OnPlatform("ApplicationClienteEfetivo.png", "ApplicationClienteEfetivo.png", "Assets/ApplicationClienteEfetivo.png");
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
