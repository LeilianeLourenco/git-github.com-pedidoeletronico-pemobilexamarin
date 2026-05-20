using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.HLP.Mobile.AppPE.Model.Financeiro;

namespace Xamarin.HLP.Mobile.AppPE.View.Popup
{
    public partial class PopupPixPedido : PopupPage
    {
        private readonly string _cCopiaCola;
        private readonly string _cUrlPix;

        public PopupPixPedido(RecebimentoTitulosModel pix)
        {
            InitializeComponent();

            _cCopiaCola = pix?.cCopiaCola;
            _cUrlPix = pix?.cUrlPix;

            if (!string.IsNullOrEmpty(_cUrlPix))
            {
                lblLink.Text = _cUrlPix;
                layoutLink.IsVisible = true;
            }

            if (!string.IsNullOrEmpty(_cCopiaCola))
            {
                lblCopiaCola.Text = _cCopiaCola;
                layoutCopiaCola.IsVisible = true;
            }

            CloseWhenBackgroundIsClicked = true;
        }

        private async void OnCopiarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_cCopiaCola)) return;
            await Clipboard.SetTextAsync(_cCopiaCola);
            await App.Messages.ShowAsync("Código PIX copiado");
        }

        private async void OnLinkTapped(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_cUrlPix)) return;
            try
            {
                await Browser.OpenAsync(_cUrlPix, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                ex.TrakException("PopupPixPedido.OnLinkTapped");
                await App.Messages.ShowAsync("Não foi possível abrir o link.");
            }
        }

        private async void OnFecharClicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
