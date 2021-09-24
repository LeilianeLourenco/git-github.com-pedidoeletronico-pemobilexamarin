using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;

namespace Xamarin.HLP.Mobile.AppPE.Droid
{

    //You can specify additional application information in this attribute
    //[Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public static string xVersion { get; set; }

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        { 
        }

        public override void OnCreate()
        {
            base.OnCreate();

            
            var context = this.ApplicationContext;
            xVersion = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;


            RegisterActivityLifecycleCallbacks(this);
            //A great place to initialize Xamarin.Insights and Dependency Services!
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}