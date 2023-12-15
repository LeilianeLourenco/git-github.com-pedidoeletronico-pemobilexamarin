using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Produto;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Produto
{
    public class ProdutoInfinitListViewModel : SearchCommom
    {
        public SearchPE controlSearchPE { get; set; }
        private ListItemModel _itemSelected;
        public Command HabiliteToSearchCommand { get; set; }
        public ICommand LoadItensCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand NovoCommand { get; set; }

        public ICommand SincronizarCommand { get; set; }


        public bool IsUsingSearch { get; set; } = false;



        private async void LoadItens()
        { 
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = true;

                    var characters = ProdutoRepository.GetToPesquisa(LItens.Count, 50, (IsUsingSearch ? xFiltro : ""), false);
                    foreach (var character in characters)
                    {
                        LItens.Add(character);
                    }
                     
                    IsBusy = false; 
                }); 
            });

            Device.BeginInvokeOnMainThread(() =>
            {
                Device.StartTimer(UtilMethods.GetStartTime, CloseIsBusy);
            });
        }



        public async void Search()
        {
            if (!IsBusy)
            {
                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsUsingSearch = true;
                        LItens = new ObservableCollection<ListItemModel>();


                        LoadItens();
                    });
                });
            }

         
        }

        public ListItemModel ItemSelected
        {
            get { return _itemSelected; }
            set
            {
                _itemSelected = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<ListItemModel> _lItens;

        public ObservableCollection<ListItemModel> LItens
        {
            get { return _lItens; }
            set
            {
                _lItens = value;
                NotifyPropertyChanged();
            }
        }


        public ProdutoInfinitListViewModel()
        {
            LItens = new ObservableCollection<ListItemModel>();
            LoadItensCommand = new Command(LoadItens);
            SearchCommand = new Command(Search);

            SincronizarCommand = new Command(() =>
            {
                var pageSync = new PageSyncNew("Total");
                pageSync.ViewModel.AcaoAfterSyncCommand = new Command(Search);
                UtilNavidate.Sincronizar(pageSync);
            });


            NovoCommand = new Command(async () =>
            {
                if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                {
                    UtilNavidate.PushAsync(new PageProduto(new ProdutoModel()));
                }
                else
                {
                    await App.Messages.ShowAsync("Somente administrador do sistema pode adicionar produto");
                }
            });
            HabiliteToSearchCommand = new Command(() =>
            {
                bFind = !bFind;
                if (bFind)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        controlSearchPE.GetEntry().Focus();
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        controlSearchPE.GetEntry().Unfocus();
                    });
                }
            });
        }



        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    IsBusy = true;
                //});
                LItens = new ObservableCollection<ListItemModel>();
                LoadItens();
            }
            return canExecuteInicial;
        }


        public async Task Remover(int idProdutoOffLine)
        {
            var objCliente = ProdutoRepository.GetProduto(idProdutoOffLine);

            if (await ProdutoRepository.Delete(objCliente))
                Search();
        }


    }
}
