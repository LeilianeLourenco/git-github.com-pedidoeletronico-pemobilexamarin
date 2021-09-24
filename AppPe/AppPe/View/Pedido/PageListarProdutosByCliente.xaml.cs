using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageListarProdutosByCliente : ContentPage
    {
        public PageListarProdutosByCliente( int idClientesOffLine, int idClientes, bool bUltimosProdutosAdquiridos)
        {
            InitializeComponent();

            ViewModel.idClientesOffLine = idClientesOffLine;
            ViewModel.idClientes = idClientes;
            ViewModel.bUltimosProdutosAdquiridos = bUltimosProdutosAdquiridos;
        }



        public PageListarProdutosByClienteViewModel ViewModel => BindingContext as PageListarProdutosByClienteViewModel;


        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
        }
    }
}
