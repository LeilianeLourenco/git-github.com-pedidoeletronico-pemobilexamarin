using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class ListarTabelaEscalonadaViewModel : SearchCommom
    {
        public class EscalonadaCollection : NotifyCommon
        {
            public string pFimFaixa { get; set; }
            public string pComissao { get; set; }
            public string vMaxDesc { get; set; }
            public string Sequencia { get; set; }
        }

        public List<double> vDesc = new List<double>();
        public int idEmpresa { get; set; }
        public double valorVenda { get; set; }
        public List<TabelaEscalonadaFaixaComissaoModel> lEscalonada { get; set; }
        public int idProduto { get; set; }  
        private ObservableCollection<EscalonadaCollection> _escalonadas;
        public ObservableCollection<EscalonadaCollection> Escalonadas
        {
            get { return _escalonadas; }
            set
            {
                _escalonadas = value;
                NotifyPropertyChanged();
            }
        }

        public ListarTabelaEscalonadaViewModel()
        {
            
        }

        public bool Initialize()
        {
            if (canExecuteInicial && !IsBusy)
            {
                canExecuteInicial = false;
                Escalonadas = new ObservableCollection<EscalonadaCollection>();
                LoadItens(idEmpresa);
            }

            return canExecuteInicial;
        }
        public async void LoadItens(int idEmpresa)
        {
            try
            {
                if (!IsBusy)
                {
                    if(lEscalonada == null)
                    {
                        Escalonadas.Add(new EscalonadaCollection
                        {
                            pFimFaixa = "",
                            pComissao = "",
                            vMaxDesc = ""
                         });
                    }
                    else
                    {
                        foreach(var val in lEscalonada)
                        {
                            vDesc.Add((valorVenda / 100) * Convert.ToDouble(val.pFimFaixa));
                        }

                        await Task.Run(() =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsBusy = true;

                                int indexEscalonada = 0;
                                foreach (var item in lEscalonada)
                                {
                                    indexEscalonada++;
                                    Escalonadas.Add(new EscalonadaCollection
                                    { 
                                        pFimFaixa = item.pFimFaixa.ToString().ToCurrencyStringSimplesPtBr(),
                                        pComissao = item.pComissao.ToString().ToCurrencyStringSimplesPlacesPtBr(),
                                        vMaxDesc = (valorVenda - (valorVenda * (item.pFimFaixa / 100))).ToCurrencyStringPtBr(),
                                        Sequencia = $"{indexEscalonada}" 
                                    }); 
                                } 

                                IsBusy = false;
                            });
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException("desculpe por isso =/", true);

                IsBusy = false;
            }
        }
    }
}
