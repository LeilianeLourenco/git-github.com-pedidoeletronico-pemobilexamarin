using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Services;

namespace Xamarin.HLP.Mobile.AppPE
{
    public class GASConnect
    {
        public static void Track_App_Page(string PageNameToTrack)
        {
            DependencyService.Get<IGAService>().Track_App_Page(PageNameToTrack);
        }

        public static void Track_App_Event(string GAEventCategory, string EventToTrack)
        {
            DependencyService.Get<IGAService>().Track_App_Event(GAEventCategory, EventToTrack);
        }

        public static void Track_App_Exception(string ExceptionMessageToTrack, bool isFatalException)
        {
            DependencyService.Get<IGAService>().Track_App_Exception(ExceptionMessageToTrack, isFatalException);
        }
    }
}
