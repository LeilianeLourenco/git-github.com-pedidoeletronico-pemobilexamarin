using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Annotations;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.Empresa;
using Xamarin.HLP.Mobile.AppPE.View.ListaPreco;
using Xamarin.HLP.Mobile.AppPE.View.ListPopup;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.View.Produto;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    public class ViewModelComum<T> : INotifyPropertyChanged where T : class
    {
        public bool ExecuttingAnyCommand { get; set; } = false;
        public bool ModalisOpenning { get; set; } = false;
        public ICommand NavigateToCommand { get; set; }
        public ICommand NavidateToBackCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand PesquisarCommand { get; set; }



        private bool _canExecuteInicial = true;

        public bool canExecuteInicial
        {
            get { return _canExecuteInicial; }
            set { _canExecuteInicial = value; NotifyPropertyChanged(); }
        }

        private bool _isVisibleListView;
        public bool isVisibleListView
        {
            get { return _isVisibleListView; }
            set
            {
                _isVisibleListView = value; NotifyPropertyChanged();
            }
        }


        public ViewModelComum()
        {
            NavigateToCommand = new Command(NavigateTo);
            NavidateToBackCommand = new Command(BackPage);
            CancelCommand = new Command(() => { UtilMessages.QuestionToBack(); });
        }

        private T _currentModel;
        public T currentModel
        {
            get { return _currentModel; }
            set
            {
                _currentModel = value;
                NotifyPropertyChanged();
            }
        }


        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged(); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; NotifyPropertyChanged(); }
        }



        public bool bAdmin
        {
            get
            {
                try
                {
                    return App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        private List<T> _registrosResearched;
        /// <summary>
        /// Registros pesquisados
        /// </summary>
        public List<T> RegistrosResearched
        {
            get { return _registrosResearched; }
            set { _registrosResearched = value; NotifyPropertyChanged(); }
        }

        private List<T> _registrosAll;
        /// <summary>
        /// Todos os registros disponíveis
        /// </summary>
        public List<T> RegistrosAll
        {
            get { return _registrosAll; }
            set { _registrosAll = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<T> _registrosToSearch;
        [Obsolete]
        public ObservableCollection<T> RegistrosToSearch
        {
            get { return _registrosToSearch; }
            set { _registrosToSearch = value; NotifyPropertyChanged(); }
        }


        private ObservableCollection<Group<string, T>> _registrosAgrupados;
        public ObservableCollection<Group<string, T>> RegistrosAgrupados
        {
            get { return _registrosAgrupados; }
            set { _registrosAgrupados = value; NotifyPropertyChanged(); }
        }



        public string ImageIconBack => Device.OnPlatform("ApplicationBarBack.png", "", "Assets/ApplicationBarBack.png");
        public string ImageIconAdd => Device.OnPlatform("ApplicationBarAdd.png", "ApplicationBarAdd.png", "Assets/ApplicationBarAdd.png");
        public string ImageIconSave => Device.OnPlatform("ApplicationBarSave.png", "ApplicationBarSave.png", "Assets/ApplicationBarSave.png");
        public string ImageIconSearch => Device.OnPlatform("ApplicationBarSearch.png", "ApplicationBarSearch.png", "Assets/ApplicationBarSearch.png");
        public string ImageListaHorizontal => Device.OnPlatform("ApplicationLista.png", "ApplicationLista.png", "Assets/ApplicationLista.png");
        public string ImageListaVertical => Device.OnPlatform("ApplicationLista_2.png", "ApplicationLista_2.png", "Assets/ApplicationLista_2.png");
        public string ImageIconCancel => Device.OnPlatform("ApplicationBarCancel.png", "ApplicationBarCancel.png", "Assets/ApplicationBarCancel.png");
        public string ImageIconDelete => Device.OnPlatform("ApplicationBarDelete.png", "ApplicationBarDelete.png", "Assets/ApplicationBarDelete.png");
        public string ImageIconDeleteWin => Device.OnPlatform("", "", "Assets/ApplicationBarDelete.png");
        public string ImageIconSync => Device.OnPlatform("ApplicationBarSync.png", "ApplicationBarSync.png", "Assets/ApplicationBarSync.png");
        public string ImageIconListarClientes => Device.OnPlatform("ApplicationBarListarClientes.png", "ApplicationBarListarClientes.png", "Assets/ApplicationBarListarClientes.png");
        public string ImageIconListarPreco => Device.OnPlatform("ApplicationBarListarTabelaPreco.png", "ApplicationBarListarTabelaPreco.png", "Assets/ApplicationBarListarTabelaPreco.png");
        public string ImageIconPedido => Device.OnPlatform("ApplicationBarListarPedidos.png", "ApplicationBarListarPedidos.png", "Assets/ApplicationBarListarPedido.png");
        public string ImageIconListarProdutos => Device.OnPlatform("ApplicationBarListarProdutos.png", "ApplicationBarListarProdutos.png", "Assets/ApplicationBarListarProdutos.png");
        public string ImageIconListarAgenda => Device.OnPlatform("ApplicationBarListarAgenda.png", "ApplicationBarListarAgenda.png", "Assets/ApplicationBarListarAgenda.png");

        public string ImageIconEdit => Device.OnPlatform("", "ApplicationBarEdit.png", "Assets/ApplicationBarEdit.png");
        public string ImageIconEditWin => Device.OnPlatform("", "", "Assets/ApplicationBarEdit.png");
        public string ImageIconGerarPedido => Device.OnPlatform("ApplicationBarPedido.png", "ApplicationBarPedido.png", "Assets/ApplicationBarPedido.png");
        public string ImageIconAddContato => Device.OnPlatform("ApplicationBarAddContato.png", "ApplicationBarAddContato.png", "Assets/ApplicationBarAddContato.png");
        public string ImageIconAddEndereco => Device.OnPlatform("ApplicationBarAddEndereco.png", "ApplicationBarAddEndereco.png", "Assets/ApplicationBarAddEndereco.png");

        public async void NavigateTo(object param)
        {
            if (param == null) return;
            if (param.ToString() == "PageEmpresa")
            {
                if (await App.IsConected() || App.CurrentAspnetUserModel.lEpresaAspnetUsersModel.Count == 1)
                    UtilNavidate.PushAsync(new PageEmpresa());
                else
                {
                    await App.Messages.ShowAsync("É necessário conexão com internet para trocar de empresa.");
                }
            }
            else if (param.ToString() == "PageListaPedido")
            {
                UtilNavidate.PushAsync(new PageListarPedidos());
            }
            else if (param.ToString() == "PageLcliente")
            {
                UtilNavidate.PushAsync(new PageInfinitListClientes());
            }
            else if (param.ToString() == "PageListaPreco")
            {
                UtilNavidate.PushAsync(new PageListaPreco());
            }


            else if (param.ToString() == "PageLproduto")
            {
                UtilNavidate.PushAsync(new PageInfinitListProdutos());
            }
            else if (param.ToString() == "PageContato")
            {
                //UtilNavidate.PushAsync(new PageContato());
            }
            else if (param.ToString() == "PageEndereco+")
            {
                //StaticModel.StaticClientesModel.objEndereco = new EnderecoModel();
                //UtilNavidate.PushAsync(new PageEndereco());
            }
            else if (param.ToString() == "PagePedido+")
            {
                UtilNavidate.PushAsync(new PagePedidoNew(new PedidoVendaModel()));
            }
        }

        public PageFindGeneric PageToFindGeneric { get; set; }

        public void ShowPageToFind(object model)
        {
            var objFindGenericModel = model as FindGenericModel;
            if (objFindGenericModel == null) return;
            objFindGenericModel.Listar();
            PageToFindGeneric = new PageFindGeneric(bindcontext: objFindGenericModel);
            UtilNavidate.PushAsync(PageToFindGeneric);
        }

        public void BackPage()
        {
            UtilNavidate.PopAsync();
        }

        private string _filtro = "";
        public string Filtro
        {
            get { return _filtro; }
            set
            {
                _filtro = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
