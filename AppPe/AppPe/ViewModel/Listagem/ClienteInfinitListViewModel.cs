

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Listagem
{

    public class ClienteInfinitListViewModel : SearchCommom
    {
        public SearchPE controlSearchPE { get; set; }
        private ListItemModel _itemSelected;
        public ICommand HabiliteToSearchCommand { get; set; }
        public ICommand LoadItensCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand NovoCommand { get; set; }
        public ICommand SincronizarCommand { get; set; }
        public bool IsUsingSearch { get; set; } = false;




        private async void LoadItens()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
            });

            await Task.Run(() =>
            {
                var characters = ClienteRepository.Get(LItens.Count, 50, (IsUsingSearch ? xFiltro : ""));
                foreach (var character in characters)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        LItens.Add(character);
                    });
                }
            });

            Device.BeginInvokeOnMainThread(() =>
            {
                Device.StartTimer(UtilMethods.GetStartTime, CloseIsBusy);
            });
        }



        public async void Search()
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




        public ClienteInfinitListViewModel()
        {
            LItens = new ObservableCollection<ListItemModel>();
            LoadItensCommand = new Command(LoadItens);
            SearchCommand = new Command(Search);
            NovoCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageCliente(new ClientesModel()));
            });
            SincronizarCommand = new Command(() =>
            {
                var pageSync = new PageSyncNew();
                pageSync.ViewModel.AcaoAfterSyncCommand = new Command(Search);
                UtilNavidate.Sincronizar(pageSync);
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
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = true;
                });
                LItens = new ObservableCollection<ListItemModel>();
                LoadItens();
            }
            return canExecuteInicial;
        }


        public async Task Remover(int idClienteOffLine)
        {
            var objCliente = ClienteRepository.GetClienteModel(idClienteOffLine);

            if (await ClienteRepository.Delete(objCliente))
            {
                Search();
            }
        }
    }
}
