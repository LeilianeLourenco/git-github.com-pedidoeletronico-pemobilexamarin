using System;

namespace Xamarin.HLP.Mobile.AppPE.Services
{
    public interface IGAService
    {
        void Track_App_Page(String PageNameToTrack);

        void Track_App_Event(String GAEventCategory, String EventToTrack);

        void Track_App_Exception(String ExceptionMessageToTrack, Boolean isFatalException);
    }

    public struct GAEventCategory
    {
        public static String Category1 { get { return "Category1"; } set { } }
        public static String Category2 { get { return "Category2"; } set { } }
        public static String Category3 { get { return "Category3"; } set { } }
    };

}
