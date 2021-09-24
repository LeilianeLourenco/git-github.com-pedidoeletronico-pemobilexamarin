using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento.Behaviors
{
    public class ValorUnitarioComImpostosBehaviors : Behavior<Entry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(ValorUnitarioComImpostosBehaviors), true);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
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
                            var _currentItem = ViewModel.currentModel;

                            if(ViewModel.currentModel.currentTabelaPreco == null)
                            {
                                return;
                            }

                            if (ViewModel == null) return;


                            if (entry.IsFocused) // se estivir focado, eu zero os valores de desconto
                            {
                                var _vTabela = _currentItem.currentTabelaPreco != null ? ViewModel.vUnitarioVenda
                               : ViewModel.vUnitarioVendaComImpostos;

                                var _vDesconto = _vTabela - ViewModel.vUnitarioVendaComImpostos;

                                if (_vDesconto > 0 && _vTabela > 0)
                                {
                                    ViewModel.vDesconto = _vDesconto;
                                    double _pDescAux = ((_vDesconto * 100) / _vTabela);

                                    _pDescAux = Math.Round(value: _pDescAux, digits: 2, mode: MidpointRounding.ToEven);

                                    ViewModel.pDesconto = _pDescAux;
                                }
                                else
                                {
                                    ViewModel.vDesconto = 0;
                                    ViewModel.pDesconto = 0;
                                }

                                IDescontoValido _objDescValido;

                                _objDescValido = new DescontoValido(pDescMaximo: ViewModel.currentModel.currentTabelaPreco.pDescontoMaximo);

                                IsValid = _objDescValido.ValidarDesconto(pDesconto: ViewModel.pDesconto); 
                                ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;

                                if (!IsValid)
                                {

                                    if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                                    {
                                        if (await App.Messages.ShowConfirmAsync("DESCONTO MÁX DE " + ViewModel.currentModel.currentTabelaPreco.pDescontoMaximo + "% ULTRAPASSADO! DESEJA CONTINUAR?",
                                                "Não", "Sim"))
                                        {
                                            ViewModel.vUnitarioVendaComImpostos += ViewModel.vDesconto;
                                            ViewModel.vDesconto = 0;
                                            ViewModel.pDesconto = 0;
                                        }
                                    }

                                    else
                                    {
                                        ViewModel.vUnitarioVendaComImpostos += ViewModel.vDesconto;
                                        ViewModel.vDesconto = 0;
                                        ViewModel.pDesconto = 0;
                                        await App.Messages.ShowAsync("DESCONTO MÁX DE " + ViewModel.currentModel.currentTabelaPreco.pDescontoMaximo + "% ULTRAPASSADO!");
                                    }
                                }


                                if ((_currentItem.ItensGrade?.Count ?? 0) > 0)
                                {
                                    foreach (var item in _currentItem.ItensGrade)
                                    {
                                        item.vDesconto = ViewModel.vDesconto;
                                    }
                                }

                                PedidoVendaCalculos.CalculoValorSubTotal(ViewModel.currentModel);
                                PedidoVendaCalculos.CalculoValorComissao(ViewModel.currentModel);
                                PedidoVendaCalculos.CalculoValorSubTotalSemImpostos(ViewModel.currentModel);
                                ViewModel.currentModel.SetDetalheItem();
                            }
                        }
                    }
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= bindable_TextChanged;
        }




        public static EditarItemViewModel GetViewModelEditar(Entry entry)
        {
            EditarItemViewModel ViewModel = null;
            if (entry.BindingContext != null)
            {
                if (entry.BindingContext.GetType() == typeof(EditarItemViewModel))
                {
                    ViewModel = entry.BindingContext as EditarItemViewModel;
                }
                else
                {
                    ViewModel = (entry.BindingContext as ListarProdutosNewViewModel)?.editarItemViewModel;
                }
            }

            return ViewModel;
        }


    }
}
