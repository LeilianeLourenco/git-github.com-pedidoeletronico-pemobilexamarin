using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;
using static Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido.ListarTabelaEscalonadaViewModel;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageListarTabelaEscalonada : ContentPage
    {
        public ListarTabelaEscalonadaViewModel ViewModel => BindingContext as ListarTabelaEscalonadaViewModel;
        public PageListarTabelaEscalonada(double valorVenda, int idProduto, List<TabelaEscalonadaFaixaComissaoModel> lFaixaComissao, int idEmpresa, PedidoVendaItensModel currentItem)
        {
            try
            {
                InitializeComponent();

                ViewModel.idEmpresa = idEmpresa;
                ViewModel.idProduto = idProduto;
                ViewModel.lEscalonada = lFaixaComissao;
                ViewModel.valorVenda = valorVenda;
                ViewModel.modelItem = currentItem;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                ViewModel.canExecuteInicial = true;
                ViewModel.Initialize();
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }



        private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current = e.CurrentSelection.FirstOrDefault() as EscalonadaCollection;

            ViewModel.modelItem.vSubTotal = current.vUnitarCDesc;
            ViewModel.modelItem.vUnitarioVendaComImpostos = current.vUnitarCDesc;
            ViewModel.modelItem.vVenda = current.vUnitarCDesc; 
            ViewModel.modelItem.vUnitarioVenda = current.vUnitarCDesc;
            ViewModel.modelItem.pComissao = current.pComissaoDouble;
            ViewModel.modelItem.pDesconto = current.pDescFimFaixa;
            ViewModel.modelItem.vQtdItem = 1;
            PedidoVendaCalculos.CalculoDescontoPorPorcent(ViewModel.modelItem, ViewModel.modelItem.pDesconto);
            PedidoVendaCalculos.AtualizaValores(ViewModel.modelItem);
        }


    }
}