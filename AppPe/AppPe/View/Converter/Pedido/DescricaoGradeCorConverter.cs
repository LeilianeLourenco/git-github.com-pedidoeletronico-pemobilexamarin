using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Pedido
{
    public class DescricaoGradeCorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var item = value as PedidoVendaItensModel;
                if (item != null && (item.HasGrade || item.idGradeCor != null || item.idGradeTamanho != null))
                    return item.xDescricao;
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
