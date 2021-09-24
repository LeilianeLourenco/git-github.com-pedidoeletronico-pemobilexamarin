using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda;

namespace Xamarin.HLP.Mobile.AppPE.View.Agenda
{
    public partial class PageConfigurarPesquisaListagemEventos : ContentPage
    {
        public PageConfigurarPesquisaListagemEventos(ConfiguracaoPesquisaListagemEventos config)
        {
            InitializeComponent();
            ViewModel.currentModel = new ConfiguracaoPesquisaListagemEventos
            {
                bTrazerRealizados = config.bTrazerRealizados,
                bTrazerCancelados = config.bTrazerCancelados,
                bTrazerTodosRepresentantes = config.bTrazerTodosRepresentantes,
                bOrdernarCrescente= config.bOrdernarCrescente
            };

            ViewModel.configDaListagem = config;
        }
         
        public ConfiguracaoPesquisaListagemEventosViewModel ViewModel => BindingContext as ConfiguracaoPesquisaListagemEventosViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
        }
    }
}