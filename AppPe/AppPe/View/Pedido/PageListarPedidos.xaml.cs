using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Service;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageListarPedidos : ContentPage
    {

        public static ListarPedidoViewModelNew ViewModelStatic { get; set; }

        public PageListarPedidos()
        {
            InitializeComponent();
            Inicial();
        }

        public PageListarPedidos(bool bUsaClienteEspecifico)
        {
            InitializeComponent();
            Inicial();
            viewModel.bUsaClienteEspecifico = bUsaClienteEspecifico;
        }


        public void setCommand(Action acao)
        {
            //if (!IsBusy)
            //{
            //    ToolbarItemNovoPedido.Command = new Command(acao); 
            //}

            ToolbarItemNovoPedido.Command = new Command(acao);
        }

        private void Inicial()
        {
            PageListarPedidos.ViewModelStatic = viewModel;
            viewModel.controlSearchPE = SearchBarPesquisa;

            if (!App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                ToolbarItems.Remove(RepresentantesToolbarItem);
        }


        public ListarPedidoViewModelNew viewModel => BindingContext as ListarPedidoViewModelNew;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            viewModel.ExecuttingAnyCommand = false;

            //if (viewModel.canExecuteInicial)
            //{
            //    if (Device.RuntimePlatform == Device.iOS)
            //    {
            //        viewModel.TratamentoErroToiOS();
            //        await Task.Yield();
            //    }
            //}

            Device.BeginInvokeOnMainThread(() =>
            { 
                Device.StartTimer(UtilMethods.GetStartTime, viewModel.Initialize);
            });
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null)
                    return;
                if (viewModel.ExecuttingAnyCommand == false)
                {
                    viewModel.ExecuttingAnyCommand = true;
                    viewModel.currentModel = ListViewDados.SelectedItem as PedidoVendaListarModel;
                    UtilNavidate.PushAsync(new PageDetalhesPedido(viewModel.currentModel));
                }
                ListViewDados.SelectedItem = null;

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            var toolbar = sender as ToolbarItem;
            if (toolbar != null)
            {

                var pedido = toolbar.BindingContext as PedidoVendaListarModel;

                if (pedido == null) return;
                if (toolbar.Text.ToUpper().Equals("EXCLUIR"))
                {
                    if (pedido.idPedidoVenda == null)
                    {
                        if (!await App.Messages.ShowConfirmAsync(MessageService.QuestionDelete)) return;
                        if (PedidoRepository.Delete(pedido.idPedidoVendaOffLine))
                        {
                            viewModel.pedidos.Remove(pedido);
                        }
                        else
                            await App.Messages.ShowAsync("Não foi possível excluir esse lançamento");
                    }
                    else
                    {
                        await App.Messages.ShowAsync("Impossível excluir um lançamento já sincronizado.");
                    }
                }
                else if (toolbar.Text.ToUpper().Equals("DETALHES"))
                {
                    var objPedido = PedidoRepository.GetPedidoVendaModel(pedido.idPedidoVendaOffLine);
                    objPedido.EstoqueInvalido = pedido.EstoqueInvalido;
                    viewModel.currentModel = pedido;
                    UtilNavidate.PushAsync(new PageDetalhesPedido(viewModel.currentModel));
                }
            }
        }
    }
}
