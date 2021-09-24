using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento.Behaviors
{
    public class DescontoItemBehaviors : Behavior<Entry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(DescontoItemBehaviors), true);
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

        private async void bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != e.OldTextValue)
            {
                if (e.NewTextValue.Split(',').Count() > 1)
                    if (e.NewTextValue.Split(',')[1].Length == 2)
                    {
                        var entry = sender as Entry;

                        if (entry != null)
                        {
                            var ViewModel = ValorUnitarioComImpostosBehaviors.GetViewModelEditar(entry);
                            if (ViewModel != null)
                            {
                                if (entry.IsFocused)
                                {
                                    if (ViewModel.vUnitarioVenda == 0)
                                    {
                                        ViewModel.vUnitarioVenda = ViewModel.vUnitarioVendaComImpostos;
                                    }


                                    if (TpValidacao == TipoValidacao.PORCENTAGEM)
                                    {
                                        PedidoVendaCalculos.CalculoDescontoPorPorcent(ViewModel.currentModel,
                                            ViewModel.pDesconto);
                                        //ViewModel.vDesconto = ViewModel.currentModel.ItensGrade.Sum(c => (c.vDesconto * c.vQtdItem));
                                        var _obj = ViewModel.currentModel.ItensGrade.Where(p => p.idProduto == ViewModel.currentModel.idProduto);
                                        ViewModel.vDesconto = _obj.FirstOrDefault().vDesconto;
                                    }
                                    else
                                    {
                                        PedidoVendaCalculos.CalculoDescontoPorValor(ViewModel.currentModel,
                                            ViewModel.vDesconto);
                                        ViewModel.pDesconto = ViewModel.currentModel.ItensGrade.FirstOrDefault().pDesconto;
                                        IsValid = PedidoVendaCalculos.DescontoValidoPorcDesc(ViewModel.currentModel);
                                        ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
                                    }

                                    // OS 35294 - Jessica Barbieri
                                    if (ViewModel.vUnitarioVenda == 0)
                                    {
                                        ViewModel.vUnitarioVendaComImpostos = ViewModel.vUnitarioVendaComImpostos - ViewModel.vDesconto;
                                        PedidoVendaCalculos.AtualizaValores(ViewModel.currentModel);
                                        AtualizaComissao(ViewModel);
                                        IDescontoValido _objDescValido;

                                        _objDescValido = new DescontoValido(pDescMaximo: ViewModel.currentModel.currentTabelaPreco.pDescontoMaximo);

                                        IsValid = _objDescValido.ValidarDesconto(pDesconto: ViewModel.pDesconto);
                                        ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;

                                        // OS 35351 - Jessica Barbieri
                                        if (IsValid == false)
                                        {

                                            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                                            {
                                                if (await App.Messages.ShowConfirmAsync("DESCONTO MÁX DE " + ViewModel.currentModel.currentTabelaPreco.pDescontoMaximo + "% ULTRAPASSADO! DESEJA CONTINUAR?",
                                                        "Não", "Sim"))
                                                {
                                                    ViewModel.vDesconto = 0;
                                                    ViewModel.pDesconto = 0;
                                                }
                                            }

                                            else
                                            {
                                                ViewModel.vDesconto = 0;
                                                ViewModel.pDesconto = 0;
                                                await App.Messages.ShowAsync("DESCONTO MÁX DE " + ViewModel.currentModel.currentTabelaPreco.pDescontoMaximo + "% ULTRAPASSADO!");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ViewModel.vUnitarioVendaComImpostos = ViewModel.vUnitarioVenda - ViewModel.vDesconto;


                                        PedidoVendaCalculos.AtualizaValores(ViewModel.currentModel);
                                        AtualizaComissao(ViewModel);
                                        IDescontoValido _objDescValido;

                                        _objDescValido = new DescontoValido(pDescMaximo: ViewModel.currentModel.currentTabelaPreco.pDescontoMaximo);

                                        IsValid = _objDescValido.ValidarDesconto(pDesconto: ViewModel.pDesconto);
                                        ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;

                                        // OS 35351 - Jessica Barbieri
                                        if (IsValid == false)
                                        {

                                            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                                            {
                                                if (await App.Messages.ShowConfirmAsync("DESCONTO MÁX DE " + ViewModel.currentModel.currentTabelaPreco.pDescontoMaximo + "% ULTRAPASSADO! DESEJA CONTINUAR?",
                                                        "Não", "Sim"))
                                                {
                                                    ViewModel.vDesconto = 0;
                                                    ViewModel.pDesconto = 0;
                                                }
                                            }

                                            else
                                            {
                                                ViewModel.vDesconto = 0;
                                                ViewModel.pDesconto = 0;
                                                await App.Messages.ShowAsync("DESCONTO MÁX DE " + ViewModel.currentModel.currentTabelaPreco.pDescontoMaximo + "% ULTRAPASSADO!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            }

        }

        public static void AtualizaComissao(EditarItemViewModel ViewModel)

        {
            if (ViewModel.currentModel.currentTabelaPreco.bEscalonada == false)
            {
                ViewModel.pComissao = ViewModel.currentModel.ItensGrade.FirstOrDefault().pComissao;
                ViewModel.vComissao = ViewModel.currentModel.ItensGrade.Sum(c => c.vComissao);
            }
            else
            {
                ViewModel.pComissao = ViewModel.currentModel.currentTabelaPreco.SelectComissaoEscalonada(
                     ViewModel.currentModel.pDesconto);

                PedidoVendaCalculos.CalculoValorComissao(ViewModel.currentModel);
                ViewModel.vComissao = ViewModel.currentModel.ItensGrade.Sum(c => c.vComissao);

            }

        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= bindable_TextChanged;
        }


    }
}
