using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageShowItensPedido : PopupPage
    {
        public PageShowItensPedido()
        {
            InitializeComponent();
            
        }

        public ShowItensPedidoViewModel viewmodel => BindingContext as ShowItensPedidoViewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewmodel.Initialize();
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            PageDetalhesPedido.viewmodelStatic.ExecuttingAnyCommand = false;
        }
    }

 
}
