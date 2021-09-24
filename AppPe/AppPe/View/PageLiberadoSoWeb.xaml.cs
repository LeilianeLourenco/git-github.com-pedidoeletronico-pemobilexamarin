using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View
{
    public partial class PageLiberadoSoWeb : ContentPage
    {
        public PageLiberadoSoWeb()
        {
            InitializeComponent();

            GridRedirect.Command = new Command(async () =>
           {

               if (App.AmbienteApp == App.Ambiente.Homologacao)
               {
                   await Share.RequestAsync(new ShareTextRequest
                   { 
                       Uri = "http://hom-pedidoeletronico.azurewebsites.net/Account/Login"
                   });

                   //await
                   //    Plugin.Share.CrossShare.Current.OpenBrowser(
                   //        "http://hom-pedidoeletronico.azurewebsites.net/Account/Login");
               }
               else
               {
                   await Share.RequestAsync(new ShareTextRequest
                   {
                       Uri = "https://pedidoeletronico.com/Account/Login"
                   });
                   //await
                   //    Plugin.Share.CrossShare.Current.OpenBrowser(
                   //        "http://pedidoeletronico.com/Account/Login");
               }

           });

            GridRedirectBlog.Command = new Command(async () =>
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Uri = "https://pedidoeletronico.wordpress.com/"
                });

                //await
                //    Plugin.Share.CrossShare.Current.OpenBrowser(
                //        "https://pedidoeletronico.wordpress.com/");
            });
        }
    }
}
