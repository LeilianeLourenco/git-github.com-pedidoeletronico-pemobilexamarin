using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageConfigurarPesquisaProduto : ContentPage
    {
        public PageConfigurarPesquisaProduto(ConfiguracaoPesquisaProdutoModel config)
        {
            InitializeComponent();
            ViewModel.currentModel = new ConfiguracaoPesquisaProdutoModel
            {
                Ordenacao = config.Ordenacao,
                bUltimasCompras = config.bUltimasCompras,
                paramRepresentacao = config.paramRepresentacao,
                paramCategoria = config.paramCategoria
            };

            ViewModel.configDoPedido = config;

        }

        public ConfigurarPesquisaProdutoViewModel ViewModel => BindingContext as ConfigurarPesquisaProdutoViewModel;


        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
        }


        private void TextCellCategoria_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ViewModel.canExecuteInicial == false && ViewModel.IsBusy == false)
                if (ViewModel?.lCategoria != null)
                {
                    ViewModel.GetRepresentacao();
                }
        }

        private void TextCellRepresentacoes_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ViewModel.canExecuteInicial == false && ViewModel.IsBusy == false)
                if (ViewModel?.lRepresentacoes != null)
                {
                    ViewModel.GetCategorias();
                }
        }
    }
}
