using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageDetalhesPedido : ContentPage
    {
        public static DetalhesPedidoViewModel viewmodelStatic { get; set; }

        public PageDetalhesPedido(PedidoVendaListarModel _currentModel)
        {
            InitializeComponent();
            viewmodel.currentModel = _currentModel;
            viewmodelStatic = viewmodel;

        }

        public DetalhesPedidoViewModel viewmodel => BindingContext as DetalhesPedidoViewModel;

        protected override void OnAppearing()
        {
            viewmodel.ExecuttingAnyCommand = false;

            if (viewmodel.bFoiParaPedido)
            {
                viewmodel.RecarregaCurrentModel();
            }

            Device.StartTimer(UtilMethods.GetStartTime, viewmodel.Initialize);
            btnDuplicar.IsVisible = viewmodel.currentModel.stLancamento == 1;



            base.OnAppearing();
        }
    }
}
