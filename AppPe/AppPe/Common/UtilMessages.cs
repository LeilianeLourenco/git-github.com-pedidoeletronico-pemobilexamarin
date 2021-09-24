using System.Threading.Tasks;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    public class UtilMessages
    {
        public static async Task<bool> Exclusao()
        {
            return await App.Messages.ShowConfirmAsync("CONFIRMA A EXCLUSÃO DO REGISTRO ?");
        }

        public static void CamposFaltandoParaSalvar()
        {
            App.Messages.ShowAsync("INFORME DADOS PARA TODOS OS CAMPOS COM O (*).");
        }

        public static async void QuestionToBack()
        {
            var result = await App.Messages.ShowConfirmAsync("Deseja realmente sair do lançamento ?");
            if (result) UtilNavidate.PopAsync();
        } 

        public async static Task<bool> QuestionToBackAsync()
        {
            return await App.Messages.ShowConfirmAsync("Deseja realmente sair do lançamento ?");

        }

        public static void InternetNecessaria()
        {
            App.Messages.ShowAsync("Necessário estar com internet!");

        }

    }
}
