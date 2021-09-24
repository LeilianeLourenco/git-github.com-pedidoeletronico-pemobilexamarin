using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Pedido
{
    public class CountRegistrosDiariosConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var pedidoVendaListarModels = value as List<PedidoVendaListarModel>;
            var icount = pedidoVendaListarModels == null ? 0 : pedidoVendaListarModels.Count;
            return icount == 0 ? "NENHUM LANÇAMENTO FEITO HOJE" : string.Format("TOTAL DE LANÇAMENTO HOJE: {0}", icount);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
