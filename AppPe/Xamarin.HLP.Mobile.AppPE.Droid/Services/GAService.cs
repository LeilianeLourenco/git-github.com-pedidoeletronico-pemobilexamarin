using System;
using Android.Content;
using Android.Gms.Analytics;
using Xamarin.HLP.Mobile.AppPE.Droid.Services;
using Xamarin.HLP.Mobile.AppPE.Services;

[assembly: Xamarin.Forms.Dependency(typeof(GAService))]
namespace Xamarin.HLP.Mobile.AppPE.Droid.Services
{
    public class GAService : IGAService
    {
        //public string TrackingId = "XX-XXXXXXXX-X";
        public string TrackingId = "UA-75124498-1";

        private static GoogleAnalytics GAInstance;
        private static Tracker GATracker;

        #region Instantiation ...
        private static GAService thisRef;
        public GAService()
        {
            // no code req'd
        }

        public static GAService GetGASInstance()
        {
            if (thisRef == null)
                // it's ok, we can call this constructor
                thisRef = new GAService();
            return thisRef;
        }
        #endregion

        public void Initialize_NativeGAS(Context AppContext = null)
        {
            GAInstance = GoogleAnalytics.GetInstance(AppContext?.ApplicationContext);
            GAInstance.SetLocalDispatchPeriod(10);

            GATracker = GAInstance.NewTracker(TrackingId);
            GATracker.EnableExceptionReporting(true);
            GATracker.EnableAdvertisingIdCollection(true);
            GATracker.EnableAutoActivityTracking(true);
        }

        public void Track_App_Page(String PageNameToTrack)
        {
            try
            {
                GATracker.SetScreenName(PageNameToTrack);
                GATracker.Send(new HitBuilders.ScreenViewBuilder().Build());
            }
            catch (Exception)
            {

            }

        }

        public void Track_App_Event(String GAEventCategory, String EventToTrack)
        {
            HitBuilders.EventBuilder builder = new HitBuilders.EventBuilder();
            builder.SetCategory(GAEventCategory);
            builder.SetAction(EventToTrack);
            builder.SetLabel("AppEvent");

            GATracker.Send(builder.Build());
        }

        public void Track_App_Exception(String ExceptionMessageToTrack, Boolean isFatalException)
        {
            HitBuilders.ExceptionBuilder builder = new HitBuilders.ExceptionBuilder();
            builder.SetDescription(ExceptionMessageToTrack);
            builder.SetFatal(isFatalException);

            GATracker.Send(builder.Build());
        }
    }
}