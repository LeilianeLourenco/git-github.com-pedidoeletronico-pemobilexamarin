using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Produto;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Produto
{
    public class ProdutoAprosentacaoViewModel : ViewModelComum<ProdutoModel>
    {
        public ICommand AtualizarCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        private bool _isAdm;

        public bool isAdm
        {
            get { return _isAdm; }
            set { _isAdm = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// Utilizada no carrossel de imagens da PageEditarItem
        /// </summary>
        private List<ImageSource> _listaImagens;
        public List<ImageSource> ListaImagens
        {
            get { return _listaImagens; }
            set { _listaImagens = value; NotifyPropertyChanged(); }
        }

        public ProdutoAprosentacaoViewModel()
        {
            currentModel = new ProdutoModel();
            isAdm = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador;
            AtualizarCommand = new Command(async () =>
            {

                if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                {
                    pageProduto._ViewModel.currentModel = currentModel;
                    UtilNavidate.PushAsync(pageProduto);
                }
                else
                    await App.Messages.ShowAsync("Somente administrador do sistema pode adicionar/editar produto");

            });
            DeleteCommand = new Command(Delete);
        }

        public int idProdutoOffLine { get; set; }
        public async void Delete()
        {
            var removido = await ProdutoRepository.Delete(currentModel);
            if (!removido) return;
            UtilNavidate.PopAsync();
        }

        public string xDot => "-".PadLeft(200, '-').Replace("-", "- ");

        private string _xRepresentada;
        public string xRepresentada
        {
            get { return _xRepresentada; }
            set { _xRepresentada = value; NotifyPropertyChanged(); }
        }

        private string _xUN;

        public string xUN
        {
            get { return _xUN; }
            set { _xUN = value; NotifyPropertyChanged(); }
        }

        private string _xCategoria;

        public string xCategoria
        {
            get { return _xCategoria; }
            set { _xCategoria = value; NotifyPropertyChanged(); }
        }

        public PageProduto pageProduto { get; set; }

        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                currentModel =
                        ProdutoRepository.GetProduto(idProdutoOffLine);

                if (currentModel.ListaImagens?.Count > 0)
                {
                    ListaImagens = currentModel.ListaImagens;
                }

                if (currentModel.idRepresentada != 0)
                    xRepresentada = RepresentadaRepository.GetNomeRepresentada(currentModel.idRepresentada);
                if (currentModel.idUnidadeMedida != 0)
                    xUN = UnidadeMedidaRepository.GetNomeUN(currentModel.idUnidadeMedida);
                if (currentModel.idCategoria != 0)
                    xCategoria = CategoriaRepository.GetCategoriaPickerModel(currentModel.idCategoria).Display;


                currentModel.DeleteCommand = new Command(Delete);

                pageProduto = new PageProduto(currentModel);
            }
            return canExecuteInicial;
        }


    }
}
