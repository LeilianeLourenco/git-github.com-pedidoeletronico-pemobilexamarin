using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml.ListagemProdutoPedido
{
    public partial class DataTemplateBasic : DataTemplate
    {
        public DataTemplateBasic()
        {
            InitializeComponent();
        }

        private void TextCellItem_OnDisappearing(object sender, EventArgs e)
        {
        }

        private void BtnVisualizarVariacoes_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var idProduto = (int)button.CommandParameter;

            UtilNavidate.PushAsync(
                new PageSelecionarVariacao(
                    new ListarVariacoesPedidoViewModel(),
                    PageListarProdutosNew.currentViewModel,
                    idProduto
                )
            );
        }
    }
}
