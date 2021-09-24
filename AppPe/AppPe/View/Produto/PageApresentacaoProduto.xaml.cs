using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Produto;

namespace Xamarin.HLP.Mobile.AppPE.View.Produto
{
    public partial class PageApresentacaoProduto : ContentPage
    {
        public PageApresentacaoProduto(int idProdutoOffLine)
        {
            InitializeComponent();
            ViewModel.idProdutoOffLine = idProdutoOffLine;
        }

        public ProdutoAprosentacaoViewModel ViewModel => BindingContext as ProdutoAprosentacaoViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.pageProduto != null)
            {
                if (StaticModel.StaticProdutoModel.needRefresh)
                    ViewModel.canExecuteInicial = true;
            }
            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);

            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_APRESENTACAO_PRODUTO);
        }

    }
}
