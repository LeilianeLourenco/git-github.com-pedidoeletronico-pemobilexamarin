using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Pedido
{
    public class SvgImageStatusEmailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var param = (parameter ?? "CLIENTE").ToString();

            var objPedidoVenda = (value as PedidoVendaModel);
            if (objPedidoVenda != null)
            {
                if (param.ToUpper().Equals("CLIENTE"))
                {
                    if (!objPedidoVenda.stEnviadoCliente)
                    {
                        return "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationInativo.svg";
                    }

                    return objPedidoVenda.idPedidoVenda != null 
                        ? "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationAtivoPedido.svg" 
                        : "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationAtivoCinzaPedido.svg";
                }
                if (param.ToUpper().Equals("REPRESENTACAO"))
                {
                    if (!objPedidoVenda.stEnviadoRepresentacao)
                    {
                        return "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationInativo.svg";
                    }

                    return objPedidoVenda.idPedidoVenda != null 
                        ? "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationAtivoPedido.svg" 
                        : "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationAtivoCinzaPedido.svg";
                }
            }

            var bvalor = (bool)(value ?? false);

            return bvalor ? "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationAtivoPedido.svg" : "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplitacionInativo.svg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
