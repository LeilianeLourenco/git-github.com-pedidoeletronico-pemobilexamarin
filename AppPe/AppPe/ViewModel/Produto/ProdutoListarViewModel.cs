using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Produto
{
    public class ProdutoListarViewModel : ViewModelComum<FindGenericModel>
    {

        public ProdutoListarViewModel()
        {
            Title = "PRODUTOS";
            PesquisarCommand = new Command(Listar);
        }

        public void OnSelectedItem()
        {
            //if (currentModel.SelectedItem == null) return;
            //StaticModel.StaticProdutoModel =
            //    ProdutoRepository.GetProduto(currentModel.SelectedItem.Id);
            //UtilNavidate.PushAsync(new PageApresentacaoProduto());

        }

        public async void Remover(int idProdutoOffLine)
        {
            var objRegistro = ProdutoRepository.GetProduto(idProdutoOffLine);

            if (await ProdutoRepository.Delete(objRegistro))
                currentModel.Listar();
        }


        public void Listar()
        {
            IsBusy = true;
            Task.Yield();
            currentModel.Listar(currentModel.Filtro);
            IsBusy = false;
            isVisibleListView = currentModel.RegistrosAgrupados.Any();
        }


        public bool InicializarDados()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                if (StaticModel.StaticFindProdutoModel == null ||
                    StaticModel.StaticFindProdutoModel.RegistrosToSearch.Count == 0)
                {
                    StaticModel.StaticFindProdutoModel = new FindGenericModel
                        (
                        ProdutoRepository.GetAll(false),
                        null,
                        "produtos",
                        acrionToSelectedChanged: OnSelectedItem,
                        image: ""
                        );

                }
                currentModel = StaticModel.StaticFindProdutoModel;
                currentModel.SelectedItem = null;
                Listar();
                IsBusy = false;
            }
            return canExecuteInicial;
        }
    }
}
