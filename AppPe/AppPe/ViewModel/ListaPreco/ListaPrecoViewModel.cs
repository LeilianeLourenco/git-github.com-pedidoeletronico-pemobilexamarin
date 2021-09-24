using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Model.TabelaPreco;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.ListaPreco
{
    public class ListaPrecoViewModel : ViewModelComum<DisplayListaModel>
    {

        #region Commands

        public ICommand CarregarEscalonadaCommand { get; set; }
        public ICommand FiltrarEscalonadaCommand { get; set; }

        public ICommand HabilitarSearchCommand { get; set; }

        #endregion

        #region Propriedades

        private bool _bSearch = true;

        public bool bSearch
        {
            get { return _bSearch; }
            set { _bSearch = value; NotifyPropertyChanged(); }
        }


        private StackLayout _stackNenhumRegistro;

        public StackLayout stackNenhumRegistro
        {
            get { return _stackNenhumRegistro ?? new StackLayout(); }
            set { _stackNenhumRegistro = value; }
        }
        public TabelaPrecoModel currentTabelaPreco { get; set; }

        private bool _bShowPesquisaProduto;
        public bool bShowPesquisaProduto
        {
            get { return _bShowPesquisaProduto; }
            set { _bShowPesquisaProduto = value; NotifyPropertyChanged(); }
        }

        private bool _bShowPesquisTabela;
        public bool bShowPesquisTabela
        {
            get { return _bShowPesquisTabela; }
            set { _bShowPesquisTabela = value; NotifyPropertyChanged(); }
        }

        private int _TipoPesquisa = 1;
        public int TipoPesquisa
        {
            get { return _TipoPesquisa; }
            set
            {
                _TipoPesquisa = value; NotifyPropertyChanged();
                bShowPesquisaProduto = _TipoPesquisa == 0;
                bShowPesquisTabela = _TipoPesquisa == 1;
                RegistrosResearched = new List<DisplayListaModel>();
                RegistrosAll = new List<DisplayListaModel>();
                findTabelaPreco.ClearRegistro();
                findProduto.ClearRegistro();
                isVisibleListView = false;
                Filtro = string.Empty;

                // CarregarItens()
            }
        }

        private FindGenericModel _findTabelaPreco = new FindGenericModel("TABELA DE PRECO", "");

        public FindGenericModel findTabelaPreco
        {
            get { return _findTabelaPreco; }
            set { _findTabelaPreco = value; NotifyPropertyChanged(); }
        }

        private FindGenericModel _findProduto = new FindGenericModel("PRODUTO", "");

        public FindGenericModel findProduto
        {
            get { return _findProduto; }
            set { _findProduto = value; NotifyPropertyChanged(); }
        }



        private List<BasicPickerModel> _lTabelaEscalonada;
        public List<BasicPickerModel> lTabelaEscalonada
        {
            get { return _lTabelaEscalonada; }
            set { _lTabelaEscalonada = value; NotifyPropertyChanged(); }
        }

        private BasicPickerModel _currentTabelaEscalonada;
        public BasicPickerModel currentTabelaEscalonada
        {
            get { return _currentTabelaEscalonada; }
            set { _currentTabelaEscalonada = value; NotifyPropertyChanged(); }
        }


        private string _xFiltroEscalonada;

        public string xFiltroEscalonada
        {
            get { return _xFiltroEscalonada; }
            set { _xFiltroEscalonada = value; NotifyPropertyChanged(); }
        }


        /// <summary>
        /// Todos os registros da escalonada
        /// </summary>
        public List<ModelEscalonadaDisplay> lEscalonadaAll { get; set; }

        private ObservableCollection<ModelEscalonadaDisplay> _lEscalonadaDisplays = new ObservableCollection<ModelEscalonadaDisplay>();
        public ObservableCollection<ModelEscalonadaDisplay> lEscalonadaDisplays
        {
            get { return _lEscalonadaDisplays; }
            set { _lEscalonadaDisplays = value; NotifyPropertyChanged(); }
        }

        #endregion

        public ListaPrecoViewModel()
        {

            PesquisarCommand = new Command(Filtrar);
            CarregarEscalonadaCommand = new Command(PesquisarTabelaEscalonada);
            FiltrarEscalonadaCommand = new Command(FiltrarEscalonada);

            HabilitarSearchCommand = new Command(() =>
            {
                bSearch = !bSearch;
            });
        }

        public void Filtrar()
        {
            IsBusy = true;
            Task.Yield();
            if (RegistrosAll != null)
                RegistrosResearched = RegistrosAll.Where(c => c.xDisplay.ToUpper().Contains(Filtro.ToUpper())).ToList();
            IsBusy = false;
            isVisibleListView = RegistrosResearched.Any();

        }


        public void InitOrClean(bool clean)
        {
            isVisibleListView = true;
            if (!clean)
            {
                RegistrosResearched = new List<DisplayListaModel>
                {
                    new DisplayListaModel {xDisplay = "Iniciando dados..."}
                };
            }
            else
            {
                RegistrosResearched = new List<DisplayListaModel>
                {
                    new DisplayListaModel {xDisplay = "Você já pode realizar suas pesquisas..."}
                };
            }
        }


        public bool FindProdutosByListaPreco()
        {
            try
            {
                if (IsBusy)
                {
                    RegistrosResearched = RegistrosAll = 
                        ProdutoRepository.GetAllProdutosByListaPreco(findTabelaPreco.GetId() ?? 0, false);
                    IsBusy = false;
                    isVisibleListView = RegistrosResearched.Any();
                }
                return false;
            }
            catch (Exception ex)
            {
                RegistrosResearched = new List<DisplayListaModel>();
                RegistrosAll = new List<DisplayListaModel>();
                ex.TrakException();
                IsBusy = false;
                isVisibleListView = RegistrosResearched.Any();
                return false;
            }
        }

        public bool FindListaComPrecosByProduto()
        {
            try
            {
                if (IsBusy)
                {
                    RegistrosResearched = RegistrosAll = ProdutoRepository.
                        GetAllListasPrecoByProduto((findProduto.SelectedItem.IdOnline ?? 0), 
                        findTabelaPreco.registrosToSearchAll);
                    IsBusy = false;
                    isVisibleListView = RegistrosResearched.Any();
                }
                return false;
            }
            catch (Exception ex)
            {
                RegistrosResearched = new List<DisplayListaModel>();
                RegistrosAll = new List<DisplayListaModel>();
                ex.TrakException();
                IsBusy = false;
                isVisibleListView = RegistrosResearched.Any();
                return false;
            }

        }


        private void FiltrarEscalonada()
        {
            if (lEscalonadaAll == null) return;

            if (string.IsNullOrEmpty(xFiltroEscalonada))
            {
                lEscalonadaDisplays = new ObservableCollection<ModelEscalonadaDisplay>(lEscalonadaAll);
            }
            else
            {
                var resultado = lEscalonadaAll.Where(c => c.xFiltro.Contains(xFiltroEscalonada)).ToList();
                lEscalonadaDisplays = new ObservableCollection<ModelEscalonadaDisplay>(resultado);
            }

            isVisibleListView = lEscalonadaDisplays.Any();

        }


        public void PesquisarTabelaEscalonada()
        {
            try
            {
                if (currentTabelaEscalonada != null && currentTabelaEscalonada.Id > 0)
                {
                    lEscalonadaAll = TabelaPrecoRepository.GetDadosEscalonada(currentTabelaEscalonada.Id);
                    lEscalonadaDisplays = new ObservableCollection<ModelEscalonadaDisplay>(lEscalonadaAll);
                }
            }
            catch (Exception)
            {
                lEscalonadaDisplays = new ObservableCollection<ModelEscalonadaDisplay>();
            }
        }

    }
}
