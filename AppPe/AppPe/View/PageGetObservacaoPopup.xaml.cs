using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View
{
    public partial class PageGetObservacaoPopup : PopupPage
    {
        public PageGetObservacaoPopup(DetalhesPedidoViewModel viewModel)
        {
            try
            {
                InitializeComponent();
                this.BindingContext = viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DetalhesPedidoViewModel viewModel => BindingContext as DetalhesPedidoViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EditorMotivoCancelamento.Focus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.EfetivaAlteracaoStatus();
        }
    }
}
