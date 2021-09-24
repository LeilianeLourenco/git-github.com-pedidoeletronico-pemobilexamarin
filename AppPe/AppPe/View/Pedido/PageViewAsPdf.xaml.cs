using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageViewAsPdf : ContentPage
    {
        public string uri { get; set; }
        public PageViewAsPdf(string _uri, string id)
        {
            InitializeComponent();
            CustomWebViewTeste.Uri = _uri;
            CustomWebViewTeste.idPK = id;






            //webViewPdf.Source = url;
        }


        protected override async void OnAppearing()
        {
            //base.OnAppearing();
            //var browser = new WebView();
            //if (await App.IsConected() == false)
            //{
            //    var htmlSource = new HtmlWebViewSource();
            //    htmlSource.Html = @"<html><body>
            //                    <h1>Xamarin.Forms</h1>
            //                    <p>Welcome to WebView.</p>
            //                    </body>
            //                    </html>";
            //    browser.Source = htmlSource;
            //}
            //else
            //{
            //    browser.Source = "https://modelica.org/events/modelica2012/authors-guide/example-abstract.pdf"; //uri;
            //}

            //Content = browser;

        }
    }
}
