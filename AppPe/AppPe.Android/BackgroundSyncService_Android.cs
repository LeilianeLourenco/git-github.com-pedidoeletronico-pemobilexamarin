using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.HLP.Mobile.AppPE.Droid.Services;
using Application = Android.App.Application;

[assembly: Dependency(typeof(BackgroundSyncService_Android))]

namespace Xamarin.HLP.Mobile.AppPE.Droid.Services
{
    public class BackgroundSyncService_Android : IBackgroundSyncService
    {
        public void StartSync()
        {
            var intent = new Intent(Application.Context, typeof(SincronizacaoService));
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                Application.Context.StartForegroundService(intent);
            else
                Application.Context.StartService(intent);
        }
    }
}
