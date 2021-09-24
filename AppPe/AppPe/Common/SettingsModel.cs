using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    public class SettingsModel
    {
        private static double _defultFontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
        public static double DefultFontSize
        {
            get { return _defultFontSize; }
            set { _defultFontSize = value; }
        }

    }
}
