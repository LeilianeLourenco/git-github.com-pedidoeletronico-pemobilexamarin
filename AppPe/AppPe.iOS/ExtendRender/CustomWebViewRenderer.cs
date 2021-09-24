using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.HLP.Mobile.AppPE.Controls.custom;
using Xamarin.HLP.Mobile.AppPE.iOS.ExtendRender;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace Xamarin.HLP.Mobile.AppPE.iOS.ExtendRender
{

    public class CustomWebViewRenderer : ViewRenderer<CustomWebView, UIWebView>
    {
        protected override async void OnElementChanged(ElementChangedEventArgs<CustomWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new UIWebView());
            }
            if (e.OldElement != null)
            {
                // Cleanup
            }
            if (e.NewElement != null)
            {
                var customWebView = Element as CustomWebView;
                var caminho = await PdfClickHandler(customWebView.Uri, customWebView.idPK);
                if (Control != null)
                {
                    Control.LoadRequest(new NSUrlRequest(new NSUrl(caminho, false)));
                    Control.ScalesPageToFit = true;
                }
            }
        }


        private async Task<string> PdfClickHandler(string uri, string id)
        {
            try
            {
                var webClient = new System.Net.WebClient();
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string localFilename = $"ped_{id}.pdf";
                var pathRetorno = Path.Combine(documentsPath, localFilename);

                if (!File.Exists(pathRetorno))
                {
                    var data = await webClient.DownloadDataTaskAsync(uri);
                    File.WriteAllBytes(pathRetorno, data);
                }
               return pathRetorno;

            }
            catch (Exception ex)
            {
                return "";
            }

        }
        
    }
}
