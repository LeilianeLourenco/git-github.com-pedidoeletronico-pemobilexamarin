using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class PedidoListarViewModel : ViewModelComum<PedidoVendaListarModel>
    {
        public ICommand CommandPesquisar { get; set; }

        private List<BasicPickerModel> _lParamFindPedido = new List<BasicPickerModel>();
        public List<BasicPickerModel> lParamFindPedido
        {
            get { return _lParamFindPedido; }
            set { _lParamFindPedido = value; NotifyPropertyChanged(); }
        }

        private bool _bVisibleParamFindPedido = false;
        public bool bVisibleParamFindPedido
        {
            get { return _bVisibleParamFindPedido; }
            set { _bVisibleParamFindPedido = value; NotifyPropertyChanged(); }
        }


        private byte _paramFindPedido;
        public byte ParamFindPedido
        {
            get { return _paramFindPedido; }
            set { _paramFindPedido = value; NotifyPropertyChanged(); }
        }



        public PedidoListarViewModel()
        {
            //RegistrosToSearch = StaticModel.StaticFindPedidoModel;

            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
            {
                bVisibleParamFindPedido = true;

                lParamFindPedido.Add(new BasicPickerModel {Id = 0, Display = "Meus pedidos"});
                lParamFindPedido.Add(new BasicPickerModel {Id = 1, Display = "Todos pedidos"});
            }

            CommandPesquisar = new Command(Listar);
            RegistrosToSearch = new ObservableCollection<PedidoVendaListarModel>();
            Listar();
        }

        private ObservableCollection<PedidoVendaListarModel> _ListaHojeFiltrado;

        public ObservableCollection<PedidoVendaListarModel> ListaHojeFiltrado
        {
            get { return _ListaHojeFiltrado; }
            set { _ListaHojeFiltrado = value; NotifyPropertyChanged(); }
        }


        private List<PedidoVendaListarModel> _listaHoje = new List<PedidoVendaListarModel>();
        public List<PedidoVendaListarModel> ListaHoje
        {
            get { return _listaHoje; }
            set { _listaHoje = value; NotifyPropertyChanged(); }
        }

        public async void Listar()
        {
            await Task.Run(() =>
            {
                IEnumerable<PedidoVendaListarModel> registrosFiltrados = this.RegistrosToSearch;
                var lHoje = ListaHoje;

                if (!string.IsNullOrEmpty(Filtro))
                {
                    registrosFiltrados = this.RegistrosToSearch.Where(l =>
                        l.DisplayCliente.ToString().Contains(Filtro.ToUpper()) ||
                        l.VTotal.ToString().Contains(Filtro.ToUpper()) ||
                        l.TipoLancamento.ToString().Contains(Filtro.ToUpper()) ||
                        l.idPedidoDisplay.ToString().Contains(Filtro));

                    lHoje = this.ListaHoje.Where(l =>
                        l.DisplayCliente.ToString().Contains(Filtro.ToUpper()) ||
                        l.VTotal.ToString().Contains(Filtro.ToUpper()) ||
                        l.TipoLancamento.ToString().Contains(Filtro.ToUpper()) ||
                        l.idPedidoDisplay.ToString().Contains(Filtro)).ToList();
                }

                RegistrosAgrupados =
                        new ObservableCollection<Group<string, PedidoVendaListarModel>>(
                            from registro in registrosFiltrados
                            orderby registro.idPedidoDisplay descending
                            group registro by registro.XdEmissao
                                into grupos
                                select new Group<string, PedidoVendaListarModel>(grupos.Key, grupos));

                ListaHojeFiltrado = new ObservableCollection<PedidoVendaListarModel>(lHoje);

            });
        }
    }
}
