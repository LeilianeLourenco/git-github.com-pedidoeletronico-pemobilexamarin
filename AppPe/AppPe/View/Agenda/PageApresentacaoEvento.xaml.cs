using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Agenda;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda;


namespace Xamarin.HLP.Mobile.AppPE.View.Agenda
{
    public partial class PageApresentacaoEvento : ContentPage
    {
        public static ApresentacaoEventoAgendaViewModel viewmodelStatic { get; set; }

        public PageApresentacaoEvento(AgendaListarModel _currentModel)
        {
            InitializeComponent();
            viewmodel.currentModel = _currentModel;
            viewmodelStatic = viewmodel;
        }


        public ApresentacaoEventoAgendaViewModel viewmodel => BindingContext as ApresentacaoEventoAgendaViewModel;


        protected override void OnAppearing()
        {
            viewmodel.ExecuttingAnyCommand = false;

            if (viewmodel.bFoiParaCadastro)
            {
                viewmodel.RecarregaCurrentModel();
            }

            Device.StartTimer(UtilMethods.GetStartTime, viewmodel.Initialize); 
            base.OnAppearing();

            SvgImagePinLocation.Command = new Command(execute: () =>
            {
                Xamarin.Forms.Device.OpenUri(new Uri($"https://waze.com/ul?q=" + viewmodel.currentModel.xEnderecoCompleto + "&navigate=yes")); 
            });
             
        }


        private async void AtualizarEvento(object sender, EventArgs e)
        {
            var _switch = sender as SwitchCell;
            if (_switch != null && viewmodel.currentModel.bRealizado != _switch.On)
            { 
                if (_switch.On)
                {
                    viewmodel.EncerrarEvento();
                    bCancelamentoAction.On = false;
                }
                else
                {
                    viewmodel.ReabrirEvento();
                    bCancelamentoAction.On = false;
                }
            }
        }

        private async void CancelarEvento(object sender, EventArgs e)
        {
            var _switch = sender as SwitchCell;
            if (_switch != null && viewmodel.currentModel.bEventoCancelado != _switch.On)
            {
                if (_switch.On)
                {
                    if (await App.Messages.ShowConfirmAsync(
                              "Deseja realmente CANCELAR este evento?", "SIM", "NÃO",
                              "AVISO"))
                    { 
                        viewmodel.CancelarEvento();
                        bAtividadeRealizadaAction.On = false;
                    }
                    else
                    {
                        _switch.On = false;
                    }
                }
                else
                {
                    viewmodel.ReabrirEvento();
                }
            }
        }



    }
}