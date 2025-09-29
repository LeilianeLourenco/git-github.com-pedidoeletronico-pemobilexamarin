using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Sincronizacao;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.Droid.Services
{
    [Service(ForegroundServiceType = ForegroundService.TypeDataSync)]
    public class SincronizacaoService : Service
    {
        private SincronizacaoNewViewModel vm;

        public const int ServiceRunningNotifId = 1000;

        public override IBinder OnBind(Intent intent) => null;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    "sync_channel",
                    "Sincronizando...",
                    NotificationImportance.High
                )
                {
                    Description = "Sincronizando..."
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }

            var notification = new NotificationCompat.Builder(this, "sync_channel")
              .SetContentTitle("Sincronizando...")
              .SetContentText("Preparando para iniciar...")
              .SetSmallIcon(Resource.Drawable.nuvem)
              .SetOngoing(true)
              .SetPriority((int)NotificationPriority.High)
              .SetCategory(Notification.CategoryService)
              .Build();

            StartForeground(ServiceRunningNotifId, notification);

            Task.Run(async () =>
            {
                vm = new SincronizacaoNewViewModel();
                vm.OnStatusChanged += (message, count) =>
                {
                    var builder = new NotificationCompat.Builder(this, "sync_channel")
                        .SetContentTitle("Sincronizando...")
                        .SetSmallIcon(Resource.Drawable.nuvem)
                        .SetOngoing(true);

                    builder.SetContentText(message);
                    NotificationManagerCompat.From(this).Notify(ServiceRunningNotifId, builder.Build());

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send<SincronizacaoNewModel>(new SincronizacaoNewModel
                        {
                            Display = message,
                            iCount = count
                        }, "SyncAtt");
                    });
                };

                await vm.InitSyncComplete();

                Device.BeginInvokeOnMainThread(() =>
                {
                    MessagingCenter.Send<object>(this, "SyncFinalizada");
                });

                StopForeground(true);
                StopSelf();
            });

            return StartCommandResult.Sticky;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            base.OnTaskRemoved(rootIntent);

            // Para a sincronização quando o app for fechado pelos recentes
            StopForeground(true);
            StopSelf();

            SincronizacaoNewViewModel.ocorreuErro = true;
        }
    }
}
