using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.DashBoard
{
    public class DashBoardViewModel : NotifyCommon
    {

        #region Properties

        private bool _isAdm;

        public bool isAdm
        {
            get { return _isAdm; }
            set
            {
                _isAdm = value;
                NotifyPropertyChanged();
            }
        }

        private double _vendasHoje;

        public double VendasHoje
        {
            get { return _vendasHoje; }
            set
            {
                _vendasHoje = value;
                NotifyPropertyChanged();
            }
        }

        private double _VendasOntem;

        public double VendasOntem
        {
            get { return _VendasOntem; }
            set
            {
                _VendasOntem = value;
                NotifyPropertyChanged();
            }
        }

        private int _orcamentosAbertos;

        public int OrcamentosAbertos
        {
            get { return _orcamentosAbertos; }
            set
            {
                _orcamentosAbertos = value;
                NotifyPropertyChanged();
            }
        }

        private int _Clientesprospect;

        public int Clientesprospect
        {
            get { return _Clientesprospect; }
            set
            {
                _Clientesprospect = value;
                NotifyPropertyChanged();
            }
        }
        private bool _bShowTodos = false;
        public bool bShowTodos
        {
            get { return _bShowTodos; }
            set
            {
                _bShowTodos = value;
                NotifyPropertyChanged();
                labelTotal = value ? "Totais da empresa" : "Seus totais";
                RefreshDashBoardDados();
            }
        }


        private string _labelTotal = "Seus totais";

        public string labelTotal
        {
            get { return _labelTotal; }
            set
            {
                _labelTotal = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        public async void RefreshDashBoardDados()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    isAdm = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador;

                    if(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.stDataRelatorios.GetValueOrDefault() == 0)
                        VendasHoje = PedidoRepository.GetFaturamento(DateTime.Today, bShowTodos); 
                    else
                        VendasHoje = PedidoRepository.GetFaturamentoPorDataFaturamento(DateTime.Today, bShowTodos);



                    var date = DateTime.Today.AddDays(-1);
                    if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.stDataRelatorios.GetValueOrDefault() == 0)
                        VendasOntem = PedidoRepository.GetFaturamento(date, bShowTodos);
                    else
                        VendasOntem = PedidoRepository.GetFaturamentoPorDataFaturamento(date, bShowTodos);
                   



                    OrcamentosAbertos = PedidoRepository.GetOrcamentosAbertos(bShowTodos);
                    Clientesprospect = ClienteRepository.GetClientesProspect(bShowTodos);
                });
            });
        }


       

    }
}
