using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Financeiro;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class PageFinanceiroClienteViewModel : SearchCommom
    {
        public class FinanceiroCollection : NotifyCommon
        {
            private string _SequenciaTitulo;
            public string SequenciaTitulo
            {
                get { return _SequenciaTitulo; }
                set { _SequenciaTitulo = value; NotifyPropertyChanged(); }
            }

            private string _ValorTitulo;
            public string ValorTitulo
            {
                get { return _ValorTitulo; }
                set { _ValorTitulo = value; NotifyPropertyChanged(); }
            }

            private string _xDtEmissao;
            public string xDtEmissao
            {
                get { return _xDtEmissao; }
                set { _xDtEmissao = value; NotifyPropertyChanged(); }
            }

            private string _xDtVencimento;
            public string xDtVencimento
            {
                get { return _xDtVencimento; }
                set { _xDtVencimento = value; NotifyPropertyChanged(); }
            }
        }
         
        public int nUltimaPaginaBuscada { get; set; }
        public bool bPararBusca { get; set; }

        private int _idClienteOffline; 
        public int idClienteOffline
        {
            get { return _idClienteOffline; }
            set
            {
                _idClienteOffline = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<FinanceiroCollection> _financeiros;

        public ObservableCollection<FinanceiroCollection> Financeiros
        {
            get { return _financeiros; }
            set
            {
                _financeiros = value;
                NotifyPropertyChanged();
            }
        }

        public bool Initialize()
        {
            if (canExecuteInicial && !IsBusy)
            {
                canExecuteInicial = false;
                Financeiros = new ObservableCollection<FinanceiroCollection>();
                LoadItens(nUltimaPaginaBuscada); 
            }

            return canExecuteInicial;
        }


        public async void LoadItens(int page)
        {
            try
            {
                if (IsBusy)
                    return;

                if (page == 0)
                    page = nUltimaPaginaBuscada = 1;
                else
                    nUltimaPaginaBuscada = page;

                    await Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            IsBusy = true;

                            List<RecebimentoTitulosModel> lRetorno = FinanceiroRepository.BuscarTitulosEmAberto(page, idClienteOffline);

                            if (lRetorno?.Count == 0)
                                bPararBusca = true;

                            foreach (var item in lRetorno)
                            {
                                Financeiros.Add(new FinanceiroCollection
                                {
                                    SequenciaTitulo = $"Duplicata {item.xNumeroTitulo}",
                                    ValorTitulo = (item.vTitulo - item.vRecebido).ToCurrencyStringPtBr(),
                                    xDtEmissao = $"Emissão: {item.dtEmissao.ToString("dd/MM/yyyy")}",
                                    xDtVencimento = $"Vencimento: {item.dtVencimento.ToString("dd/MM/yyyy")}"
                                });
                            }

                            IsBusy = false;
                        });
                    });  
            }
            catch (Exception ex)
            {
                ex.TrakException("desculpe por isso =/", true);
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    IsBusy = false;
                //});

                IsBusy = false;
            }
        }
    }
}
