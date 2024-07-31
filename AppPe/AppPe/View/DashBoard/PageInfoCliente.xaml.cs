using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.View.Sincronizacao
{
    public partial class PageInfoCliente : PopupPage
    {
        public static ClientesModel _dados { get; set; }

        public PageInfoCliente(ClientesModel dados)
        {
            InitializeComponent();

            lblEmail.Text = dados.xEmails;
            lblTelefone.Text = dados.xTelefones;

            _dados = dados;
        }

        private void UltimosPedidos_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.Navigation.RemovePopupPageAsync(page: this, animate: true);

                PagePedidoNew.CurrentViewModel.currentModel = new PedidoVendaModel
                {
                    idClientesOffLine = _dados.idClientesOffLine ?? 0,
                    idClientes = _dados.idClientes
                };

                var page = new PageListarPedidos(bUsaClienteEspecifico: true);
                page.setCommand(GerarPedido);
                UtilNavidate.PushAsync(page);
            }
            catch (Exception ex)
            {
            }
        }

        private async void GerarPedido()
        {
            UtilNavidate.PushAsync(new PagePedidoNew(PagePedidoNew.CurrentViewModel.currentModel, true));
        }
    }
}
