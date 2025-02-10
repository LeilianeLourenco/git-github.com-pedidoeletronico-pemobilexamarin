using Hlp.PedidoEletronico.Domain.Business.Calculos;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Controls.custom;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml.ListagemProdutoPedido;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class VariacaoEditItemPedido : StackLayout
    {
        private StackLayout _stackLayoutItens;
        private ObservableCollection<PedidoVendaItensModel> _item;
        private EditarItemViewModel _page;

        public VariacaoEditItemPedido(StackLayout stackLayoutItens,
            ObservableCollection<PedidoVendaItensModel> item, EditarItemViewModel page)
        {
            InitializeComponent();
            _stackLayoutItens = stackLayoutItens;
            _item = item;
            _page = page;
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

        private void btnFiltroVariacao(object sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                var _buttonClicked = sender as Button;
                if (_buttonClicked.BackgroundColor != Color.FromHex("#555555"))
                {

                    foreach (var buttons in ScrollButtonsVariacoes.Children)
                    {
                        var _btn = buttons as Button;

                        if (_buttonClicked.Text == _btn.Text)
                        {
                            _btn.TextColor = Color.FromHex("#fff");
                            _btn.BackgroundColor = Color.FromHex("#555555");
                        }
                        else
                        {
                            _btn.TextColor = Color.FromHex("#000");
                            _btn.BackgroundColor = Color.LightGray;
                        }
                    }

                    List<List<int>> selectedParameters = new List<List<int>>();
                    var parameter = clickedButton.CommandParameter;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var child in _stackLayoutItens.Children)
                        {
                            if (child is VariacaoEditItemPedido variacaoEdit)
                            {
                                var innerStack = variacaoEdit.FindByName<StackLayout>("ScrollButtonsVariacoes");

                                if (innerStack != null)
                                {
                                    foreach (var item in innerStack.Children)
                                    {
                                        if (item is Button button)
                                        {
                                            if (item?.BackgroundColor == Color.FromHex("#555555"))
                                            {
                                                var buttonParameter = button.CommandParameter;

                                                if (buttonParameter is List<int> buttonIdProdutoLista)
                                                {
                                                    if (parameter is List<int> clickedIdProdutoList)
                                                    {
                                                        var contem = clickedIdProdutoList.Any(x => buttonIdProdutoLista.Contains(x));
                                                        if (contem)
                                                            selectedParameters.Add(buttonIdProdutoLista);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (parameter is List<int> clickedIdProdutoLista)
                        {
                            for (int i = 0; i < _stackLayoutItens.Children.Count; i++)
                            {
                                var child = _stackLayoutItens.Children[i];

                                if (child is VariacaoEditItemPedido variacaoEdit)
                                {
                                    var innerStack = variacaoEdit.FindByName<StackLayout>("ScrollButtonsVariacoes");

                                    if (innerStack != null)
                                    {
                                        foreach (var item in innerStack.Children)
                                        {
                                            if (item is Button button)
                                            {
                                                var buttonParameter = button.CommandParameter;

                                                if (buttonParameter is List<int> buttonIdProdutoLista)
                                                {
                                                    //if (item?.BackgroundColor == Color.FromHex("#555555"))
                                                    //{
                                                    //    var contem = clickedIdProdutoLista.Any(x => buttonIdProdutoLista.Contains(x));
                                                    //    if (contem)
                                                    //        selectedParameters.Add(buttonIdProdutoLista);
                                                    //}

                                                    bool hasCommonId = false;

                                                    if (selectedParameters.Count > 0)
                                                    {
                                                        bool allContainId = selectedParameters.All(innerList =>
                                                                    buttonIdProdutoLista.Any(id => innerList.Contains(id)));

                                                        hasCommonId = buttonIdProdutoLista.Any(id => clickedIdProdutoLista.Contains(id)
                                                            && allContainId);
                                                    }
                                                    else
                                                        hasCommonId = buttonIdProdutoLista.Any(id => clickedIdProdutoLista.Contains(id));

                                                    if (!hasCommonId)
                                                    {
                                                        button.TextColor = Color.FromHex("#000");
                                                        button.BackgroundColor = Color.FromHex("#fff");
                                                        button.BorderColor = Color.LightGray;
                                                    }

                                                    button.IsEnabled = hasCommonId;

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var idProduto = selectedParameters
                            .Skip(1)
                            .Aggregate(new HashSet<int>(selectedParameters.First()),
                                (intersection, nextList) =>
                                {
                                    intersection.IntersectWith(nextList);
                                    return intersection;
                                })
                                .FirstOrDefault();

                        string xQuery = $"select vVenda, stVendaSemEstoque, xNome from tb_produto where idProduto = {idProduto}";
                        var produto = App.Data.Connection.Query<ProdutoModel>(xQuery).FirstOrDefault();

                        if (produto != null)
                        {
                            var item = _item.FirstOrDefault();

                            item.idProduto = idProduto;
                            item.stVendaSemEstoque = produto.stVendaSemEstoque;
                            item.vUnitarioVendaComImpostos = produto.vVenda;
                            item.vVenda = produto.vVenda;
                            item.xDescricao = produto.xNome;

                            if (item.vQtdEstoque == 0)
                                item.vQtdItem = 0;

                            _page.vUnitarioVendaSemImposto = produto.vVenda;
                            _page.vUnitarioVendaComImpostos = produto.vVenda;
                            _page.vUnitarioVenda = produto.vVenda;

                            var current = _page.CurrentLocalEstoque;
                            _page.CurrentLocalEstoque = null;
                            _page.CurrentLocalEstoque = current;
                        }
                    });
                }
            }
        }
    }
}
