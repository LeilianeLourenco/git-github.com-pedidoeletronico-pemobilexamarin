using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Service
{
    public interface IMessageService
    {

        Task<int?> TipoListagem();
        Task<int> TipoOrdenacaoTask();
        Task<string> TipoLancamentoTask(int idEmpresa);
        Task ShowAsync(string message);

        Task<bool> ShowConfirmAsync(string message, string accept = "SIM", string cancel = "NÃO", string title = "CONFIRMAÇÃO");

        Task<EmailPedidoModel> ShowQuestionMessageEmailPedido();

        Task<string> ShowQuestionMessageSendEmailOrCall();

        Task<string> TipoDeClienteTask();
    }
}
