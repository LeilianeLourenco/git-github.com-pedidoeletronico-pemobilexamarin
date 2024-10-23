using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento.Behaviors;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;
using Xamarin.HLP.Mobile.AppPE.Model;
using System.Collections.Generic;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageEditarItem : ContentPage
    {
        public PageEditarItem(EditarItemViewModel editarViewModel = null)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasBackButton(this, false);
                BindingContext = editarViewModel ?? new EditarItemViewModel();
                //DescontoItemBehaviors.AtualizaComissao(ViewModel);
                //PedidoVendaCalculos.SumTotalizadoresPageEditarItem();
                //ViewModel.ListaImagens = new List<string>
                //{
                //    "/data/user/0/com.ptbr.pedidoeletronico/files/BDB32F7D-7B15-4590-8013-1DEB8014063F.jpg"
                //};
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.canExecuteInicial = true;
            Device.StartTimer(UtilMethods.GetStartTime, InitializeDadosPage);
            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_EDITAR_PRODUTO);
        }


        private bool InitializeDadosPage()
        {
            try
            {
                if (ViewModel.canExecuteInicial)
                {
                    ViewModel.canExecuteInicial = false;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (StackLayoutItens.Children.Count == 0 && ViewModel.currentModel.ItensGrade != null)
                        {
                            foreach (var item in ViewModel.currentModel.ItensGrade)
                            {
                                StackLayoutItens.Children.Add(new GridEditItemPedido() { BindingContext = item });
                            }
                        }

                        if (StackLayoutItens.Children.Count == 0 && ViewModel.currentModel.ItensVariacao != null)
                        {
                            foreach (var item in ViewModel.currentModel.ItensVariacao)
                            {
                                StackLayoutItens.Children.Add(new VariacaoEditItemPedido() { BindingContext = item });
                            }
                        }

                        if (ViewModel.currentModel.currentTabelaPreco == null)
                        { 
                            App.Messages.ShowAsync($"Item sem Tabela de preço, contate o administrador para checar as configurações!");
                            ViewModel.IsBusy = false;
                            return;
                        }

                        ViewModel.xDescontoMaximo = $"desconto permitido {ViewModel.currentModel.currentTabelaPreco.pDescontoMaximo}%";

                        //if (ViewModel.currentModel.ImageProduto == null)
                        //    ViewModel.currentModel.ImageProduto = UtilMethods.GetLocalProdutoImageSource(ViewModel.currentModel.xFileImagePrincipal);
                        ViewModel.IsBusy = false;
                    });
                }
                return ViewModel.canExecuteInicial;
            }
            catch (Exception ex)
            {
                ViewModel.canExecuteInicial = true;
                return false;
            }
        }

        public EditarItemViewModel ViewModel => BindingContext as EditarItemViewModel;

        private async void ItemSave_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var bContinue = true;
                if (ViewModel.vSubTotal <= 0)
                {
                    if (ViewModel.currentModel.ItensGrade?.Count() > 0)
                    {
                        var _subTotalComGrade = ViewModel.currentModel.ItensGrade.Sum(p => p.vSubTotal);
                        if (_subTotalComGrade <= 0)
                        {
                            bContinue =
                            await
                                App.Messages.ShowConfirmAsync(
                                    $"Total inserido é de R$ 0,00, deseja realmente sair do lançamento?{Environment.NewLine}Todos os dados serão perdidos", "SAIR", "VOU CORRIGIR", "ALERTA");
                        }
                    }
                    else
                    {
                        bContinue =
                            await
                                App.Messages.ShowConfirmAsync(
                                    $"Total inserido é de R$ 0,00, deseja realmente sair do lançamento?{Environment.NewLine}Todos os dados serão perdidos", "SAIR", "VOU CORRIGIR", "ALERTA");
                    }
                }

                if (bContinue)
                {


                    if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                    {
                        BeforeSave();
                        UtilNavidate.PopAsync();
                    }

                    else if (!App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador &&
                             await IsValidPage())
                    {
                        BeforeSave();
                        UtilNavidate.PopAsync();
                    }
                    else
                        await App.Messages.ShowAsync(string.Format("Verifique os campos com erro."));
                }
            }
            catch (Exception ex)
            {
                await App.Messages.ShowAsync(ex.Message);
            }
        }

        private void BeforeSave()
        {
            var listViewModel = PageListarProdutosNew.currentViewModel;
            var pedidoViewModel = PagePedidoNew.CurrentViewModel;
            listViewModel?.SaveItem();
            pedidoViewModel?.currentModel?.CurrentItemModel?.SetDetalheItem();
            PedidoVendaCalculos.SumTotalizadoresPageEditarItem();
        }

        private void EntryImpostos_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != e.OldTextValue)
            {
                if (e.NewTextValue.Split(',').Count() > 1)
                    if (e.NewTextValue.Split(',')[1].Length == 2)
                    {
                        var entry = sender as Entry;
                        if (entry != null && entry.IsFocused)
                        {
                            ViewModel.vDesconto = ViewModel.pDesconto = ViewModel.currentModel.pDesconto = ViewModel.currentModel.vDesconto = 0;
                            ViewModel.vUnitarioVendaComImpostos =
                            PedidoVendaCalculos.CalculoValorUnitarioComImpostos(ViewModel.vUnitarioVendaSemImposto,
                                ViewModel.pStVenda ?? 0, ViewModel.pIpiVenda ?? 0);

                            ViewModel.vUnitarioVenda = ViewModel.currentModel.vUnitarioVenda = ViewModel.vUnitarioVendaComImpostos;
                            PedidoVendaCalculos.AtualizaValores(ViewModel.currentModel);
                        }
                    }
            }
        }

        private async Task<bool> IsValidPage(bool zerarvalores = false)
        {

            var valorUnitario = (EntryValorUnitario.Behaviors[0] as ValorUnitarioComImpostosBehaviors);
            var pdesconto = (EntryDesconto.Behaviors[0] as DescontoItemBehaviors);
            var vdesconto = (EntryValorDesconto.Behaviors[0] as DescontoItemBehaviors);          

            return await ViewModel.ValidateCamposTask(zerarvalores, valorUnitario, pdesconto, vdesconto);
        }

        protected override async void OnDisappearing()
        {
            if (!App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                await IsValidPage(true);
            base.OnDisappearing();
        }
    }
}
