using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.View.Sincronizacao
{
    public partial class PageInfoCliente : PopupPage
    {
        public static ClientesModel _dados { get; set; }
        public string _imgCliente => Device.OnPlatform("ApplicationBarListarClientes.png", "ApplicationBarListarClientes.png", "Assets/ApplicationBarListarClientes.png");

        public PageInfoCliente(ClientesModel dados)
        {
            InitializeComponent();

            lblEmail.Text = dados.xEmails.Replace(",", Environment.NewLine);
            lblTelefone.Text = dados.xTelefones.Replace(",", Environment.NewLine);
            imgCliente.Source = _imgCliente;

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
                page.setCommand(UltimosPedidos);
                UtilNavidate.PushAsync(page);
            }
            catch (Exception ex)
            {
            }
        }

        private void NavegarCliente(object sender, EventArgs e)
        {
            App.Navigation.RemovePopupPageAsync(page: this, animate: true);
            UtilNavidate.PushAsync(new PageApresentacaoClienteNew(_dados.idClientesOffLine ?? 0));
        }

        private void UltimosPedidos()
        {
            UtilNavidate.PushAsync(new PagePedidoNew(PagePedidoNew.CurrentViewModel.currentModel, true));
        }       

        private void NovoPedido_Clicked(object sender, EventArgs e)
        {
            App.Navigation.RemovePopupPageAsync(page: this, animate: true);
            UtilNavidate.PushAsync(new PagePedidoNew(new PedidoVendaModel()));
        }
    }
}
