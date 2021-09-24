using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda
{
    public class AdiantamentoHorariosViewModel : SearchCommom
    {
        
           

        private async void LoadItens()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
            });

            await Task.Run(() =>
            { 
                LItens.Add(new ListaHorariosAdiantamentoModel
                {
                    xDisplay = $"1 Hora",
                    stOpcao = 1, 
                });
                LItens.Add(new ListaHorariosAdiantamentoModel
                {
                    xDisplay = $"2 Horas",
                    stOpcao = 2,
                });
                LItens.Add(new ListaHorariosAdiantamentoModel
                {
                    xDisplay = $"3 Horas",
                    stOpcao = 3,
                });
                LItens.Add(new ListaHorariosAdiantamentoModel
                {
                    xDisplay = $"5 Horas",
                    stOpcao = 4,
                });
                LItens.Add(new ListaHorariosAdiantamentoModel
                {
                    xDisplay = $"1 dia",
                    stOpcao = 5,
                });
                LItens.Add(new ListaHorariosAdiantamentoModel
                {
                    xDisplay = $"2 dias",
                    stOpcao = 6,
                });
                LItens.Add(new ListaHorariosAdiantamentoModel
                {
                    xDisplay = $"1 semana",
                    stOpcao = 7,
                }); 
            });

            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = false;
            });
        }


        private ListaHorariosAdiantamentoModel _itemSelected;
        public ListaHorariosAdiantamentoModel ItemSelected
        {
            get { return _itemSelected; }
            set
            {
                _itemSelected = value;
                if (itemCadastro != null)
                {
                    itemCadastro.xDisplay = value.xDisplay;
                    itemCadastro.stOpcao = value.stOpcao; 
                }
                NotifyPropertyChanged();
            }
        }

        private ListaHorariosAdiantamentoModel _itemCadastro;

        public ListaHorariosAdiantamentoModel itemCadastro
        {
            get { return _itemCadastro; }
            set { _itemCadastro = value; NotifyPropertyChanged(); }
        }


        private ObservableCollection<ListaHorariosAdiantamentoModel> _lItens;

        public ObservableCollection<ListaHorariosAdiantamentoModel> LItens
        {
            get { return _lItens; }
            set
            {
                _lItens = value;
                NotifyPropertyChanged();
            }
        } 

        public AdiantamentoHorariosViewModel()
        {
            LItens = new ObservableCollection<ListaHorariosAdiantamentoModel>();
            LoadItensCommand = new Command(LoadItens);  
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
                LoadItens();
            }
            return canExecuteInicial;
        }
    }
}
