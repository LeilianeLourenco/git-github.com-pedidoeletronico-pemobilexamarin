using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Testes
{
    public partial class PageTeste1 : ContentPage
    {
        public PageTeste1()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            //var idTeste = App.Data.Connection.Table<PedidoVendaModel>().FirstOrDefault().idPedidoVendaOffLine;

            //UtilNavidate.ShowPopupNew(new PageViewToPrint(idTeste ?? 0));
        }
    }
}
