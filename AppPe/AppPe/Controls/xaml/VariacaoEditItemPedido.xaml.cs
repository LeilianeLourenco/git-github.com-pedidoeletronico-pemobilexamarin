using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml.ListagemProdutoPedido;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class VariacaoEditItemPedido : StackLayout
    {
        public VariacaoEditItemPedido()
        {
            InitializeComponent();
        }

        private async void StepperValor_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            var stepper = sender as Stepper;
            if (stepper != null)
            {
                if (Math.Abs(e.NewValue - e.OldValue) > 0)
                {
                    await PedidoVendaCalculos.CalculoByStepper();
                }
            }
        }

        public PedidoVendaItensModel ViewModel => BindingContext as PedidoVendaItensModel;
    }
}
