using AndroidX.Core.View;
using AppView = Android.Views.View;

namespace Xamarin.HLP.Mobile.AppPE.Droid.Services
{
    public class InsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
    {
        [System.Obsolete]
        public WindowInsetsCompat OnApplyWindowInsets(AppView v, WindowInsetsCompat insets)
        {
            var top = insets.SystemWindowInsetTop;
            var bottom = insets.SystemWindowInsetBottom;
            var left = insets.SystemWindowInsetLeft;
            var right = insets.SystemWindowInsetRight;

            v.SetPadding(left, top, right, bottom);
            return insets;
        }
    }
}
