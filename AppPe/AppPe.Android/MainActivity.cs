using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;
using FFImageLoading.Forms.Platform;
using ImageCircle.Forms.Plugin.Droid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TEditor.Droid;
using Xamarin.HLP.Mobile.AppPE.Droid.Services;
using AndroidResource = Android.Resource;
using AndroidView = Android.Views.View;
using Color = Android.Graphics.Color;

namespace Xamarin.HLP.Mobile.AppPE.Droid
{
    [Activity(Label = "pedidoeletronico.com", Icon = "@drawable/iconPE", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const int RequestBluetoothPermissionCode = 1001;
        private const int RequestLocationPermissionCode = 1002;
        private const int RequestNotificationPermissionCode = 1003;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

            // Inicializações de plugins
            ImageCircleRenderer.Init();
            CachedImageRenderer.Init(false);
            CachedImageRenderer.InitImageViewHandler();
            Rg.Plugins.Popup.Popup.Init(this);
            GAService.GetGASInstance().Initialize_NativeGAS(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            TEditorDroid.Initialize();
            DisplayCrashReport();
            CheckAndRequestPermissions();

            var content = FindViewById<AndroidView>(AndroidResource.Id.Content);
            ViewCompat.SetOnApplyWindowInsetsListener(content, new Services.InsetsListener());

            Window.SetStatusBarColor(Color.ParseColor("#1565C0"));

            LoadApplication(new App());
        }

        private void CheckAndRequestPermissions()
        {
            var permissionsToRequest = new List<string>();

            if (CheckSelfPermission(Manifest.Permission.BluetoothConnect) != Permission.Granted)
                permissionsToRequest.Add(Manifest.Permission.BluetoothConnect);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                if (CheckSelfPermission(Manifest.Permission.PostNotifications) != Permission.Granted)
                    permissionsToRequest.Add(Manifest.Permission.PostNotifications);
            }

            if (permissionsToRequest.Any())
                RequestPermissions(permissionsToRequest.ToArray(), 1000);
        }

        private void CheckAndRequestBluetoothPermission()
        {
            if (CheckSelfPermission(Manifest.Permission.BluetoothConnect) != Permission.Granted)
                RequestPermissions(new string[] { Manifest.Permission.BluetoothConnect }, RequestBluetoothPermissionCode);
        }

        private void CheckAndRequestNotificationPermission()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                if (CheckSelfPermission(Manifest.Permission.PostNotifications) != Permission.Granted)
                    RequestPermissions(new string[] { Manifest.Permission.PostNotifications }, RequestNotificationPermissionCode);
            }
        }

        private void CheckAndRequestLocationPermission()
        {
            if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
            {
                RequestPermissions(new string[] { Manifest.Permission.AccessFineLocation }, RequestLocationPermissionCode);
            }
        }

        public override void OnBackPressed()
        {
            if (!backButtonPressed._canBack)
            {
                return;
            }
            else
                base.OnBackPressed();
        }

        #region handling error

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = System.IO.Path.Combine(libraryPath, errorFileName);
                var errorMessage = $"Time: {DateTime.Now}\r\nError: Unhandled Exception\r\n{exception.ToString()}";
                System.IO.File.WriteAllText(errorFilePath, errorMessage);
                // Log to Android Device Logging.
                //Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        /// If there is an unhandled exception, the exception information is displayed 
        /// on screen the next time the app is started (only in debug configuration)
        /// </summary>
        [Conditional("DEBUG")]
        private void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var errorFilePath = System.IO.Path.Combine(libraryPath, errorFilename);

            if (!System.IO.File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = System.IO.File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(this)
                .SetPositiveButton("Clear", (sender, args) =>
                {
                    System.IO.File.Delete(errorFilePath);
                })
                .SetNegativeButton("Close", (sender, args) =>
                {
                    // User pressed Close.
                })
                .SetMessage(errorText)
                .SetTitle("Crash Report")
                .Show();
        }

        #endregion
    }
}
