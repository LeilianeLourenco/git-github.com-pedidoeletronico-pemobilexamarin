using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.View.Service
{
    public class MessageService : ViewModel.Service.IMessageService
    {

        public static string QuestionDelete = "Deseja realmente excluir o lançamento?";

        public static string QuestionSair = "Deseja realmente excluir o lançamento?";

        // DisplayAlert só pode rodar na thread principal — no Android isso derruba o app
        // inteiro (Java.Lang.RuntimeException: "Can't create handler inside thread ... that
        // has not called Looper.prepare()") quando chamado de dentro de uma Task em segundo
        // plano sem passar por MainThread. Como ShowAsync/ShowConfirmAsync são chamados de
        // muitos lugares (inclusive de dentro de Task.Run), garante aqui, na origem, que
        // sempre executa na thread principal, não importa de onde foi chamado.
        public async Task ShowAsync(string message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
                await Application.Current.MainPage.DisplayAlert("AVISO", message, "OK"));
        }

        public async Task<bool> ShowConfirmAsync(string message, string accept = "SIM", string cancel = "NÃO", string title = "CONFIRMAÇÃO")
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
                await Application.Current.MainPage.DisplayAlert(
                    title: title,
                    message: message,
                    accept: accept,
                    cancel: cancel));
        }


        /// <summary>
        /// Tipo da pagina de produtos no pedido
        /// 0-SIMPLES
        /// 1-SIMPLES + IMG
        /// 2-COMPLETA
        /// </summary>
        public async Task<int?> TipoListagem()
        {
            try
            {
                var action =
    await
        Application.Current.MainPage.DisplayActionSheet("TIPO DE LISTA",
            "voltar", null, "Completa", "Simples", "Simples + imagem");


                if (action == null)
                    return null;

                if (action.Equals("Completa"))
                {
                    return 2;
                }
                else if (action.Equals("Simples"))
                {
                    return 0;
                }
                else if (action.Equals("Simples + imagem"))
                {
                    return 1;
                }

            }
            catch (System.Exception ex)
            {
                ex.TrakException();
            }
            return null;
        }

        public async Task<EmailPedidoModel> ShowQuestionMessageEmailPedido()
        {
            var action =
                await
                    Application.Current.MainPage.DisplayActionSheet("ENVIAR EMAIL PARA :",
                        "Não enviar", null, "Representação", "Cliente", "Ambos");

            var objReturn = new EmailPedidoModel();

            if (action == null)
                return null;
            if (action.ToLower().Equals("representação"))
            {
                objReturn.bEnviaRepresentacoes = true;

            }
            else if (action.ToLower().Equals("cliente"))
            {
                objReturn.bEnviaCliente = true;
            }
            else if (action.ToLower().Equals("ambos"))
            {
                objReturn.bEnviaRepresentacoes = true;
                objReturn.bEnviaCliente = true;
            }

            return objReturn;
        }



        public async Task<string> TipoLancamentoTask(int idEmpresa)
        {
            bool permiteOrcamento = ExtensaoEmpresaRepository.GetbGeraOrcamento(idEmpresa);

            var action =
                await
                    Application.Current.MainPage.DisplayActionSheet("TIPO DE LANÇAMENTO",
                        "Cancelar", null, "PEDIDO", permiteOrcamento ? "ORÇAMENTO" : null);


            if (action == null)
                return "Cancelar";

            return action;
        }


        public async Task<int> TipoOrdenacaoTask()
        {
            var action =
                await
                    Application.Current.MainPage.DisplayActionSheet("DESEJA ORDENAR POR ?",
                        "Voltar", null, "DESCRIÇÃO", "CÓDIGO ALTERNATIVO");


            if (action == null || action.Equals("Voltar"))
                return -1;

            return action == "DESCRIÇÃO" ? 0 : 1;
        }



        public async Task<string> ShowQuestionMessageSendEmailOrCall()
        {
            var action =
                await
                    Application.Current.MainPage.DisplayActionSheet("O QUE DESEJA FAZER ?",
                        "voltar", null, "Enviar email", "Ligar");

            if (action == null || action.ToUpper().Equals("voltar"))
                return "";
            return action.ToUpper();
        }

        public async Task<string> TipoDeClienteTask()
        {

            var action =
                await
                    Application.Current.MainPage.DisplayAlert("Pergunta", "CLIENTE É PESSOA ...", "FÍSICA", "JURÍDICA");

            return action ? "F" : "J";
        }

        public MessageService() { }






    }
}
