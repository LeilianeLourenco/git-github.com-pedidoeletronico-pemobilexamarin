using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

[assembly: Dependency(typeof(Xamarin.HLP.Mobile.AppPE.Droid.Version))]
namespace Xamarin.HLP.Mobile.AppPE.Droid
{
    public class Version : IVersion
    {
        public string GetVersion()
        {
            VersionTracking.Track();  
            return VersionTracking.CurrentVersion;
        }
    }
}