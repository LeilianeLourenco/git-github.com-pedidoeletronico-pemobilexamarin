using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Pedido
{
    public class ValorItemSemDescontoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null) return "";

                var item = value as PedidoVendaItensModel;

                var retorno = item != null && item.vUnitarioVenda > item.vVenda ? (item.vUnitarioVenda.ToCurrencyStringPtBr() + " / ") : "";

                return retorno;
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
