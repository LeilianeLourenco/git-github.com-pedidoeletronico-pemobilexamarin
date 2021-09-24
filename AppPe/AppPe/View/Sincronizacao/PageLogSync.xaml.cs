using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.View.Produto;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.View.Sincronizacao
{
    public partial class PageLogSync : ContentPage
    {
        public PageLogSync(IEnumerable<AlertaSincronizacao> alertas)
        {
            InitializeComponent();
            ViewModel.LAlertaSincronizacao = new ObservableCollection<AlertaSincronizacao>(alertas);
        }


        public LogSincronizacaoViewModel ViewModel => BindingContext as LogSincronizacaoViewModel;

        public Page goToPage { get; set; }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as AlertaSincronizacao;
            if (item == null) return;
            switch (item.Table)
            {
                case TableMobile.TB_PEDIDOVENDA:
                case TableMobile.tb_produto_codigocliente:
                    {
                        goToPage = new PageListarPedidos();
                        UtilNavidate.PopAsync();
                        Device.StartTimer(UtilMethods.GetStartTime, GoToOtherPage);
                    }
                    break;
                case TableMobile.TB_PRODUTO:
                    {
                        var objProduto = ProdutoRepository.GetProduto(item.idOffLine ?? 0);
                        goToPage = new PageProduto(objProduto);
                        UtilNavidate.PopAsync();
                        Device.StartTimer(UtilMethods.GetStartTime, GoToOtherPage);
                    }
                    break;
                case TableMobile.TB_CLIENTES:
                    {
                        var cliente = ClienteRepository.GetClienteModel(item.idOffLine ?? 0);
                        goToPage = new PageCliente(cliente);
                        UtilNavidate.PopAsync();
                        Device.StartTimer(UtilMethods.GetStartTime, GoToOtherPage);
                    }
                    break;
                case "PLANOGRÁTIS":
                case "PLANO":
                    {
                        Device.OpenUri(App.AmbienteApp == App.Ambiente.Homologacao
                                   ? new Uri("http://hom-pedidoeletronico.azurewebsites.net/Account/Login")
                                   : new Uri("http://pedidoeletronico.azurewebsites.net/Account/Login"));
                    }
                    break;
            }
            ListViewAvisosSync.SelectedItem = null;
        }

        public bool bCanGotoPedido { get; set; } = true;

        private bool GoToOtherPage()
        {
            if (bCanGotoPedido)
            {
                bCanGotoPedido = false;
                if (UtilNavidate.GetTypeCurrentPage() != goToPage.GetType())
                    UtilNavidate.PushAsync(goToPage);
            }
            return bCanGotoPedido;
        }



    }
}
