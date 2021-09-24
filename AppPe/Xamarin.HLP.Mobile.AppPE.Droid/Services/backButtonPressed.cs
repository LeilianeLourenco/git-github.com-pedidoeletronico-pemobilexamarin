using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.HLP.Mobile.AppPE.Droid.Services;
using Xamarin.HLP.Mobile.AppPE.Services;

[assembly: Xamarin.Forms.Dependency(typeof(backButtonPressed))]
namespace Xamarin.HLP.Mobile.AppPE.Droid.Services
{
    public class backButtonPressed : IbackButtonPressed
    {
        public static bool _canBack { get; set; } = true;
        public void SetParameter(bool canBack)
        {
            _canBack = canBack;
        }
    }
}