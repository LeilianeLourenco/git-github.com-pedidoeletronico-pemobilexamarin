using FFImageLoading.Forms.Platform;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using TEditor.iOS;
using UIKit;
using Xamarin.HLP.Mobile.AppPE.iOS.Services;

namespace Xamarin.HLP.Mobile.AppPE.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ImageCircleRenderer.Init();
            CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();
            Rg.Plugins.Popup.Popup.Init();
            GAService.GetGASInstance().Initialize_NativeGAS();
            global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            global::Xamarin.Forms.Forms.Init();
            TEditoriOS.Initialize();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
