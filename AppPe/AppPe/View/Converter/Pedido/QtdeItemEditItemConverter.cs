using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Pedido
{
    public class QtdeItemEditItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var number = (double)value;
            if (number <= 0)
            {
                if (PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel == null) return "0";
                return PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.nCasasDecimais == 0
                    ? "0"
                    : "0," + "".PadRight(PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.nCasasDecimais, '0');
            }
            if (PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel == null)
                return 0;

            if (PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.nCasasDecimais <= 0)
                return number.TryToInt();
            var format = "#0." + "".PadRight(PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.nCasasDecimais, '0');
            return number.ToString(format).Replace(".", ",");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var valor = (string)value;
            valor = valor.Replace(",", "").Replace(".", "");
            if (PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel != null)
                if (PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.nCasasDecimais > 0)
                {
                    valor = valor.PadLeft(PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.nCasasDecimais + 1, '0');
                    valor = valor.Insert(valor.Length - PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.nCasasDecimais, culture.NumberFormat.CurrencyDecimalSeparator);
                }
            double number = 0;
            double.TryParse((string)valor, out number);
            return number;
        }


       
    }
}
