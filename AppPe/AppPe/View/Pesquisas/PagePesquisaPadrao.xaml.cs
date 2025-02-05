using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pesquisa;

namespace Xamarin.HLP.Mobile.AppPE.View.Pesquisas
{
    public partial class PagePesquisaPadrao : ContentPage
    {
        public PagePesquisaPadrao(ListItemModel item, PesquisaPadraoViewModel.Tabela Table,
            int? idClienteOffLine = null, int? idCondicaoPagamento = null, object Filtro = null)
        {
            InitializeComponent();
            ViewModel.controlSearchPE = SearchBarPesquisa;
            ViewModel.itemCadastro = item;
            ViewModel.tabela = Table;
            ViewModel.idClienteOffline = idClienteOffLine;
            ViewModel.idCondicaoPagamento = idCondicaoPagamento;
            ViewModel.objFiltro = Filtro;
        }

        public PesquisaPadraoViewModel ViewModel => BindingContext as PesquisaPadraoViewModel;


        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listItemModel = e.SelectedItem as ListItemModel;
            if (listItemModel != null)
            {
                if (ViewModel.tabela == PesquisaPadraoViewModel.Tabela.TB_REPRESENTANTE_MAIS_TODOS)                
                    PageListarPedidos.ViewModelStatic.canExecuteInicial = true;
                
                else if (ViewModel.tabela == PesquisaPadraoViewModel.Tabela.TB_CONDICAO_PAGAMENTO)               
                    PagePedidoNew.CurrentViewModel.BuscaFormasPagamentoTelaVendas();
                
                else if (ViewModel.tabela == PesquisaPadraoViewModel.Tabela.TB_ENDERECO)
                    PagePedidoNew.CurrentViewModel.currentModel.xEndereco = PagePedidoNew.CurrentViewModel.ItemEndereco.Detail;

                UtilNavidate.PopAsync();

                //os 35400
                PagePedidoNew.CurrentViewModel.AtualizaTotalizadoresPedido();
            }
        }
    }
}
