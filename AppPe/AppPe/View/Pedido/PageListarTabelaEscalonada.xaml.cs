using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{ 
    public partial class PageListarTabelaEscalonada : ContentPage
    {
        public ListarTabelaEscalonadaViewModel ViewModel => BindingContext as ListarTabelaEscalonadaViewModel;
        public PageListarTabelaEscalonada(double valorVenda, int idProduto, List<TabelaEscalonadaFaixaComissaoModel> lFaixaComissao, int idEmpresa)
        {
            try
            {
                InitializeComponent();

                ViewModel.idEmpresa = idEmpresa;
                ViewModel.idProduto = idProduto;
                ViewModel.lEscalonada = lFaixaComissao;
                ViewModel.valorVenda = valorVenda;
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
    }
}