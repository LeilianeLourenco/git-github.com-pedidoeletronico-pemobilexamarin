using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Agenda;
using Xamarin.HLP.Mobile.AppPE.View.Service;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda;

namespace Xamarin.HLP.Mobile.AppPE.View.Agenda
{ 
    public partial class PageListagemEventos : ContentPage
    { 
        public static ListarEventosViewModel ViewModelStatic { get; set; }


        public PageListagemEventos()
        {
            InitializeComponent();
            Inicial(); 
        }


        public PageListagemEventos(bool bUsaClienteEspecifico)
        {
            InitializeComponent(); 
            Inicial();
            viewModel.bUsaClienteEspecifico = bUsaClienteEspecifico;
        }

        private void Inicial()
        {
            PageListagemEventos.ViewModelStatic = viewModel;
            viewModel.controlSearchPE = SearchBarPesquisa; 
        }

        public ListarEventosViewModel viewModel => BindingContext as ListarEventosViewModel;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Config.bNeedRefresh)
            {
                viewModel.Config.bNeedRefresh = false;
                viewModel.canExecuteInicial = true;
            }


            viewModel.ExecuttingAnyCommand = false; 
            //if (viewModel.canExecuteInicial)
            //{
            //    if (Device.RuntimePlatform == Device.iOS)
            //    {
            //        viewModel.TratamentoErroToiOS();
            //        await Task.Yield();
            //    }
            //}


            Device.StartTimer(UtilMethods.GetStartTime, viewModel.Initialize);
        }

        public void setCommand(Action acao)
        {
            ToolbarItemNovoEvento.Command = new Command(acao);
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
                    viewModel.currentModel = ListViewDados.SelectedItem as AgendaListarModel;
                    UtilNavidate.PushAsync(new PageApresentacaoEvento(viewModel.currentModel));
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

                var agenda = toolbar.BindingContext as AgendaListarModel;

                if (agenda == null) return;
                if (toolbar.Text.ToUpper().Equals("EXCLUIR"))
                {
                    if (!await App.Messages.ShowConfirmAsync(MessageService.QuestionDelete)) return;
                    if (AgendaRepository.ExcluirEvento(agenda.idAtividadeOffline))
                    {
                        viewModel.atividades.Remove(agenda);
                    }
                    else
                        await App.Messages.ShowAsync("Não foi possível excluir esse lançamento");
                }
            }
        }

    }
}