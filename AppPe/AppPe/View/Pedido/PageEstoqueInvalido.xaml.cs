using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageEstoqueInvalido : ContentPage
    {
        public PageEstoqueInvalido(PedidoNewViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }

        private async void StepperValor_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            var stepper = sender as Stepper;
            if (stepper == null) return;
            var item = stepper.BindingContext as PedidoVendaItensModel;
            if (item != null)
                PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel = item;
            await PedidoVendaCalculos.CalculoByStepper();
        }

        private void MenuItem_OnClicked(object sender, EventArgs e)
        {
            UtilNavidate.PopAsync();
        }
    }
}
