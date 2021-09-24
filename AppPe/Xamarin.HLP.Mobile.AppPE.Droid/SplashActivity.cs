using Android.App;
using Android.Content;
using Android.Support.V7.App;


namespace Xamarin.HLP.Mobile.AppPE.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();

            StartActivity(intent: new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}