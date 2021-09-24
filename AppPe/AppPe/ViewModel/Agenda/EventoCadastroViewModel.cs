using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Agenda;
using Xamarin.HLP.Mobile.AppPE.View.Agenda;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.View.Pesquisas;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pesquisa;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda
{
    public class EventoCadastroViewModel : SearchCommom
    {

        public ICommand GoToClientesCommand { get; set; }
        public ICommand GoToAtividadesCommand { get; set; }
        public ICommand CancelPedidoCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ChangeTimeInicioCommand { get; set; }
        public ICommand ChangeTimeFimCommand { get; set; }

        private AtividadeAgendaModel _currentModel;
        public AtividadeAgendaModel currentModel
        {
            get { return _currentModel; }
            set
            {
                _currentModel = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _ItemCliente = new ListItemModel { Display = "clique aqui para pesquisar" };
        public ListItemModel ItemCliente
        {
            get { return _ItemCliente; }
            set
            {
                _ItemCliente = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _ItemAtividade = new ListItemModel { Display = "clique aqui para pesquisar" };
        public ListItemModel ItemAtividade
        {
            get { return _ItemAtividade; }
            set
            {
                _ItemAtividade = value;
                NotifyPropertyChanged();
            }
        }

        public EventoCadastroViewModel()
        {
            IsBusy = true;
            currentModel = new AtividadeAgendaModel();

            CancelPedidoCommand = new Command(CancelPress);
            SaveCommand = new Command(Save, CanSave);

            GoToClientesCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ItemCliente == null)
                            ItemCliente = new ListItemModel();
                        var pesquisa = new PagePesquisaPadrao(ItemCliente,
                            PesquisaPadraoViewModel.Tabela.TB_CLIENTE)
                        {
                            Title = "Cliente",
                        };
                        UtilNavidate.PushAsync(pesquisa);
                    });
                }

            });

            GoToAtividadesCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ItemAtividade == null)
                            ItemAtividade = new ListItemModel();
                        var pesquisa = new PagePesquisaPadrao(ItemAtividade,
                            PesquisaPadraoViewModel.Tabela.TB_TIPOATIVIDADESCRM)
                        {
                            Title = "Atividades",
                        };
                        UtilNavidate.PushAsync(pesquisa);
                    });
                }

            });

            ChangeTimeInicioCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!ExecuttingAnyCommand)
                    {
                        ExecuttingAnyCommand = true;
                        var page = new PageSelectDate(currentModel, SelectDateViewModel.tipolancamento.INICIO_EVENTO);
                        UtilNavidate.PushModalAsync(page);
                    }
                });
            });

            ChangeTimeFimCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!ExecuttingAnyCommand)
                    {
                        ExecuttingAnyCommand = true;
                        var page = new PageSelectDate(currentModel, SelectDateViewModel.tipolancamento.FIM_EVENTO);
                        UtilNavidate.PushModalAsync(page);
                    }
                });
            });

            IsBusy = false;
        }

        private async void CancelPress()
        {
            if (currentModel.idAtividadeOffline == 0)
            {
                if (await UtilMessages.QuestionToBackAsync())
                {
                    UtilNavidate.PopAsync();
                }
            }
            else
            {
                UtilNavidate.PopAsync();
            }
        }
        public async void Save()
        {
            if (CanSave())
            {
                IsBusy = true;
                await ValidateToSave();
                IsBusy = false;
            }
        }

        public bool CanSave()
        {
            return IsBusy == false;
        }


        public async Task ValidateToSave()
        {
            try
            {
                //if (ItemCliente.Id == 0)
                //{
                //    await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                //    return;
                //}
                if (ItemAtividade.Id == 0)
                {
                    await App.Messages.ShowAsync("Antes disso, selecione uma atividade");
                    return;
                }

                AgendaRepository.SaveAtividade(currentModel);
                PageListagemEventos.ViewModelStatic.canExecuteInicial = true;
                UtilNavidate.PopAsync();
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("EventocadastroViewModel.Save", ex.Message, true);
            }
        }

        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                IsBusy = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (currentModel.idAtividadeOffline > 0)
                    {
                        ItemCliente = ClienteRepository.GetRegistro(currentModel.idClienteOffline.GetValueOrDefault());
                        ItemAtividade = AgendaRepository.GetRegistroParaListagem(currentModel.idTipoAtividade);
                    }

                    if (currentModel.dtInicioEvento != null)
                        if ((currentModel.dtInicioEvento ?? DateTime.Now).Kind != DateTimeKind.Local) 
                            currentModel.dtInicioEvento = (currentModel.dtInicioEvento ?? DateTime.Now).ToLocalTime();


                    if (currentModel.dtFimEvento != null)
                        if ((currentModel.dtFimEvento ?? DateTime.Now).Kind != DateTimeKind.Local)
                            currentModel.dtFimEvento = (currentModel.dtFimEvento ?? DateTime.Now).ToLocalTime();

                });
                IsBusy = false;
            }

            return canExecuteInicial;
        }

        public async void SetEnderecoCliente()
        {
            await Task.Run(() =>
            {
                currentModel.xEnderecoCompleto = AgendaRepository.GetEnderecoClienteEscolhido(
                            App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                            currentModel.idClienteOffline.GetValueOrDefault());
            });
        }
    }
}
