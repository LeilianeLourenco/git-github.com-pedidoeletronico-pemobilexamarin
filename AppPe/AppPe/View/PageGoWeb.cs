using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View
{
    public class PageGoWeb : ContentPage
    {
        public PageGoWeb(string URL, string xTitle)
        {
            var browser = new WebView();

            browser.Source = "http://xamarin.com";

            Content = browser;
        }
    }
}
