using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Model.TabelaPreco;
using Xamarin.HLP.Mobile.AppPE.ViewModel.ListaPreco;

namespace Xamarin.HLP.Mobile.AppPE.View.ListaPreco
{
    public partial class PageListaPreco : TabbedPage
    {
        public PageListaPreco()
        {
            try
            {
                InitializeComponent();
                GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_LISTA_PRECO);
                this.Children.Remove(PageTabelaEscalonada);
                viewmodel.InitOrClean(false);
            }
            catch (Exception ex) 
            {
                ex.TrakException();
            }
        }


        public ListaPrecoViewModel viewmodel => BindingContext as ListaPrecoViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.StartTimer(UtilMethods.GetStartTime, initialize);
        }

        public bool initialize()
        {

            if (viewmodel.canExecuteInicial)
            {
                try
                {
                    viewmodel.isVisibleListView = true;
                    viewmodel.IsBusy = false;
                    viewmodel.canExecuteInicial = false;
                    Task.Yield();
                    var ltabelas = TabelaPrecoRepository.GetAllTables();
                    var lTabelasPicker = new List<BasicPickerModel>();
                    foreach (var item in ltabelas)
                    {
                        var tab = new BasicPickerModel
                        {
                            Id = item.idTabelaPreco,
                            Display = item.xTabelaPreco,
                            Detail = $"Indice de {item.pIndice.ToCurrencyStringSimplesPtBr()} % ",
                            ColorDisplay = (!item.bCampanha ? ColorStaticModel.VerdePedido : ColorStaticModel.Orcamento),
                            bTrazerImagem = false
                        };
                        lTabelasPicker.Add(tab);
                    }

                    viewmodel.lTabelaEscalonada = TabelaPrecoRepository.GetTabelaEscalonadaToDisplay();
                    if (viewmodel.lTabelaEscalonada.Any())
                    {
                        this.Children.Add(PageTabelaEscalonada);
                    }
                    viewmodel.currentTabelaEscalonada = viewmodel.lTabelaEscalonada.FirstOrDefault();
                    viewmodel.findTabelaPreco = new FindGenericModel(
                                                               lTabelasPicker,
                                                               null,
                                                               "TABELA DE PREÇO", "ApplicationPrazo");

                    viewmodel.findTabelaPreco.ActionToSelectedChanged = () =>
                    {
                        if (viewmodel.findTabelaPreco.GetId() != null && viewmodel.findTabelaPreco.GetId() != 0)
                        {
                            viewmodel.RegistrosResearched = new List<DisplayListaModel>();
                            viewmodel.IsBusy = true;
                            Device.StartTimer(UtilMethods.GetStartTime, viewmodel.FindProdutosByListaPreco);
                        }
                    };

                    CarregarProdutos();
                }
                catch (Exception ex)
                {
                    ex.TrakException();
                }
            }

            viewmodel.findProduto.ModalisOpenning = false;
            viewmodel.findTabelaPreco.ModalisOpenning = false;
            return viewmodel.canExecuteInicial;
        }



        private async void CarregarProdutos()
        {
            try
            {
                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        viewmodel.findProduto = new FindGenericModel(ProdutoRepository.GetAll(), null, "PRODUTO", "ApplicationPrazo");
                        ViewCellProduto.BindingContext = viewmodel.findProduto;
                        ViewCellTabelaPreco.BindingContext = viewmodel.findTabelaPreco;
                        viewmodel.findProduto.ActionToSelectedChanged = () =>
                    {
                        if (viewmodel.findProduto.GetId() != null && viewmodel.findProduto.GetId() != 0)
                        {
                            viewmodel.RegistrosResearched = new List<DisplayListaModel>();
                            viewmodel.IsBusy = true;
                            Device.StartTimer(UtilMethods.GetStartTime, viewmodel.FindListaComPrecosByProduto);
                        }
                    };
                        viewmodel.InitOrClean(true);
                        viewmodel.findProduto.ModalisOpenning = false;
                    });
                });
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }
    }
}
