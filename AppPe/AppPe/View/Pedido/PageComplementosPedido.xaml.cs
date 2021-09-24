using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageComplementosPedido : PopupPage
    {
        public PageComplementosPedido(PedidoNewViewModel _viewModel)
        {
            InitializeComponent();
            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.POPUP_COMPLEMENTO_PEDIDO);
            viewmodel = _viewModel;
            this.BindingContext = _viewModel;

            EntryvFretePed.Completed += (sender, e) => { EntryvSeguroPed.Focus(); };

            EntryvSeguroPed.Completed += (sender, e) => { EntryvOutrasPed.Focus(); };

            EntryvOutrasPed.Completed += (sender, e) => { btnVoltar.Focus(); };

        }

        public PedidoNewViewModel viewmodel { get; set; }
        private void EntryComplementos_OnTextChanged(object sender, TextChangedEventArgs e)
        {

            if (e.NewTextValue != e.OldTextValue)
            {
                if (e.NewTextValue.Split(',').Count() > 1)
                    if (e.NewTextValue.Split(',')[1].Length == 2)
                    {
                        var entry = sender as Entry;
                        if (entry != null && entry.IsFocused)
                        {
                            viewmodel.AtualizaTotalizadoresPedido();
                        }
                    }
            }
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            
            UtilNavidate.PopPopupNew();
        }

        protected override void OnDisappearing()
        {
            viewmodel.ExecuttingAnyCommand = false;
            base.OnDisappearing();
        }
    }
}
