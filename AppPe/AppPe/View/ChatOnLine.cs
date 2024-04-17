using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Droid
{
    public class ChatOnLine : ContentPage
    {

        public ChatOnLine()
        {          
            AbrirLinkNoNavegador("https://pedidoeletronico.com/home/chatonline");
        }

        public async void AbrirLinkNoNavegador(string url)
        {
            try
            {
                await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {             
                Console.WriteLine($"Erro ao abrir o navegador: {ex.Message}");
            }
        }

    }
}