using System;
using Foundation;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

[assembly: Dependency(typeof(Xamarin.HLP.Mobile.AppPE.iOS.Version))]

namespace Xamarin.HLP.Mobile.AppPE.iOS
{
    public class Version : IVersion
    {
        public string GetVersion()
        {
            try
            {
                VersionTracking.Track();
                return  VersionTracking.CurrentVersion; 

                //var xVersion = "";

                //if (NSBundle.MainBundle.InfoDictionary.ContainsKey(new NSString("CFBundleVersion")))
                //    xVersion = NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();
                //return xVersion;
            }
            catch (Exception)
            {
                return "";
            }
           
        }
    }
}
