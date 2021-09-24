using System.Linq;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento.Behaviors
{
    public class ComissaoItemBehaviors : Behavior<Entry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(ComissaoItemBehaviors), true);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        public TipoValidacao TpValidacao { get; set; }

        public enum TipoValidacao
        {
            VALOR, PORCENTAGEM
        }
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += bindable_TextChanged;
        }

        private void bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != e.OldTextValue)
            {
                if (e.NewTextValue.Split(',').Count() > 1)
                    if (e.NewTextValue.Split(',')[1].Length == 4)
                    {
                        var entry = sender as Entry;
                        if (entry != null)
                        {
                            var ViewModel = ValorUnitarioComImpostosBehaviors.GetViewModelEditar(entry); 
                            if (ViewModel != null)
                            {

                                IsValid = PedidoVendaCalculos.ComissaoValida(ViewModel.currentModel);
                                ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;

                                if (entry.IsFocused)
                                {
                                    if (TpValidacao == TipoValidacao.PORCENTAGEM)
                                    {
                                        PedidoVendaCalculos.CalculoValorComissao(ViewModel.currentModel);
                                        ViewModel.vComissao = ViewModel.currentModel.ItensGrade.Sum(c => c.vComissao);
                                    }
                                    else
                                    {
                                        PedidoVendaCalculos.CalculoPorcComissao(ViewModel.currentModel, ViewModel.vComissao);
                                        ViewModel.pComissao = ViewModel.currentModel.ItensGrade.FirstOrDefault().pComissao;
                                    }
                                }
                            }
                        }
                    }
            }
        }
        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= bindable_TextChanged;
        }
    }
}
