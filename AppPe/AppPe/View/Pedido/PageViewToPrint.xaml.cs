using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageViewToPrint : PopupPage
    {
        public PageViewToPrint(int idPedidoVendaOffLine)
        {
            InitializeComponent();
            
            
            ViewModel.idPedidoVendaOffLine = idPedidoVendaOffLine;
        }


        public PedidoToPrintViewModel ViewModel => BindingContext as PedidoToPrintViewModel;


        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
        }
    }
}
