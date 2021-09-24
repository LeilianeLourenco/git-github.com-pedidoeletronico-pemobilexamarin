using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.custom
{
    public class CustomWebView : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(CustomWebView),
            defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }



        public static readonly BindableProperty idPKProperty = BindableProperty.Create(propertyName: "idPK",
           returnType: typeof(string),
           declaringType: typeof(CustomWebView),
           defaultValue: default(string));

        public string idPK
        {
            get { return (string)GetValue(idPKProperty); }
            set { SetValue(idPKProperty, value); }
        }
    }
}
