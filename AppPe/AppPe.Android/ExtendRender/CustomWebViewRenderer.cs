using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.HLP.Mobile.AppPE.Controls.custom;
using Xamarin.HLP.Mobile.AppPE.Droid.ExtendRender;
using Environment = System.Environment;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace Xamarin.HLP.Mobile.AppPE.Droid.ExtendRender
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        protected override async void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var customWebView = Element as CustomWebView;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                var xCaminho = await PdfClickHandler(customWebView.Uri, customWebView.idPK);
                //Control.LoadUrl(xCaminho);
                //Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///android_asset/Content/{0}", WebUtility.UrlEncode(customWebView.Uri))));
                Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", 
                                string.Format("file:///{0}", xCaminho)));
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