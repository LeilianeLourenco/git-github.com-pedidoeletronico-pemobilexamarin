using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Pedido
{
    public class IdDisplayPedidoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var pedido = value as PedidoVendaListarModel;
                if (pedido == null) return null;
                var xReturn = "#";
                if (parameter == null)
                {
                    if (pedido.idPedidoDisplay != null)
                    {
                        xReturn = "#" + pedido.idPedidoDisplay.ToString().PadLeft(6, '0');
                    }
                }
                else if (parameter.ToString().Equals("COMPLETE"))
                {
                    //if (pedido.idPedidoDisplay != null)
                    //{
                    //    return string.Format("{0} #{1}  -  {2}",
                    //                  pedido.TipoLancamentoShort,
                    //                  pedido.idPedidoDisplay.ToString().PadLeft(6, '0'),
                    //                  pedido.VTotal.ToCurrencyStringPtBr());
                    //}
                    //else 
                    //{
                    //    return string.Format("{0} # {1}  -  {2}",
                    //                 pedido.TipoLancamentoShort,
                    //                 "",
                    //                 pedido.VTotal.ToCurrencyStringPtBr());
                        
                    //}
                }
                return xReturn;

            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("idDisplayPedidoConverter", ex.Message, true);
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
