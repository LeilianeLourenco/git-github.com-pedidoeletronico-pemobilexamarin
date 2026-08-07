using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.iOS.Services;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao;

[assembly: Dependency(typeof(BackgroundSyncService_iOS))]
namespace Xamarin.HLP.Mobile.AppPE.iOS.Services
{
    // Equivalente iOS do BackgroundSyncService_Android. O Android roda a sincronização
    // dentro de um Foreground Service (SincronizacaoService); iOS não tem esse conceito,
    // então aqui só dispara InitSyncComplete() direto — funciona enquanto o app está em
    // primeiro plano, o que cobre o fluxo normal (usuário fica na tela de Sincronização
    // esperando o popup fechar). Antes desta classe não existir, DependencyService.Get
    // retornava null e a sincronização nunca era chamada no iOS.
    public class BackgroundSyncService_iOS : IBackgroundSyncService
    {
        public void StartSync()
        {
            Task.Run(async () =>
            {
                var vm = new SincronizacaoNewViewModel();

                vm.currentModel.OnMensagemChanged += (message) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send<object, string>(vm, "SyncAttMensagem", message);
                    });
                };

                vm.currentModel.OnCountChanged += (count) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send<object, int>(vm, "SyncAttCount", count);
                    });
                };

                await vm.InitSyncComplete();

                Device.BeginInvokeOnMainThread(() =>
                {
                    MessagingCenter.Send<object>(vm, "SyncFinalizada");
                });
            });
        }
    }
}
