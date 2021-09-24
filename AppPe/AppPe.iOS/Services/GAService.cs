using System;
using Foundation;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.iOS.Services;
using Xamarin.HLP.Mobile.AppPE.Services;

[assembly: Dependency(typeof(GAService))]
namespace Xamarin.HLP.Mobile.AppPE.iOS.Services
{
    public class GAService : IGAService
    {
        public string TrackingId = "UA-75124498-1";

        public static Google.Analytics.ITracker Tracker;
        private const string AllowTrackingKey = "AllowTracking";

        #region Instantition...
        private static GAService thisRef;
        public GAService()
        {
            // no code req'd
        }

        public static GAService GetGASInstance()
        {
            return thisRef ?? (thisRef = new GAService());
        }

        #endregion

        public void Initialize_NativeGAS()
        {
            var optionsDict = NSDictionary.FromObjectAndKey(new NSString("YES"), new NSString(AllowTrackingKey));

            NSUserDefaults.StandardUserDefaults.RegisterDefaults(optionsDict);

            Google.Analytics.Gai.SharedInstance.OptOut = !NSUserDefaults.StandardUserDefaults.BoolForKey(AllowTrackingKey);

            Google.Analytics.Gai.SharedInstance.DispatchInterval = 10;
            Google.Analytics.Gai.SharedInstance.TrackUncaughtExceptions = true;

            Tracker = Google.Analytics.Gai.SharedInstance.GetTracker("Pedido Eletrônico", TrackingId);
        }

        public void Track_App_Page(string PageNameToTrack)
        {
            Tracker.Set(Google.Analytics.GaiConstants.ScreenName, PageNameToTrack);
            Tracker.Send(Google.Analytics.DictionaryBuilder.CreateScreenView().Build());
        }

        public void Track_App_Event(string GAEventCategory, string EventToTrack)
        {
            Tracker.Send(Google.Analytics.DictionaryBuilder.CreateEvent(GAEventCategory, EventToTrack, "AppEvent", null).Build());
            Google.Analytics.Gai.SharedInstance.Dispatch(); // Manually dispatch the event immediately
        }

        public void Track_App_Exception(string ExceptionMessageToTrack, Boolean isFatalException)
        {
            Tracker.Send(Google.Analytics.DictionaryBuilder.CreateException(ExceptionMessageToTrack, isFatalException).Build());
        }
    }
}
