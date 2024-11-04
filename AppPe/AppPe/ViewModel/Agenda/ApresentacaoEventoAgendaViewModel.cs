using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Agenda;
using Xamarin.HLP.Mobile.AppPE.View.Agenda;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda
{
    public class ApresentacaoEventoAgendaViewModel : NotifyCommon
    {

        public ICommand EncerrarEventoCommand { get; set; }
        public ICommand AdiarCommand { get; set; }
        public ICommand ReabrirEventoCommand { get; set; }
        public ICommand CancelarEventoCommand { get; set; }
        public ICommand VerEnderecoCommand { get; set; }
        public ICommand CheckInCommand { get; set; }
        public ICommand CheckOutCommand { get; set; }
        public string ImageIconPedido => Device.OnPlatform("ApplicationBarListarPedidos.png", "ApplicationBarListarPedidos.png", "Assets/ApplicationBarListarPedido.png");
        public string ImageIconAdiar => Device.OnPlatform("ApplicationBarAdiarAgenda.png", "ApplicationBarAdiarAgenda.png", "Assets/ApplicationBarAdiarAgenda.png");
        public ICommand EditarEventoCommand { get; set; }

        private AgendaListarModel _currentModel;
        public AgendaListarModel currentModel
        {
            get { return _currentModel; }
            set
            {
                _currentModel = value;
                NotifyPropertyChanged();
            }
        }

        private ListaHorariosAdiantamentoModel _lHorarios = new ListaHorariosAdiantamentoModel { xDisplay = "Selecione um horário", stOpcao = 0 };
        public ListaHorariosAdiantamentoModel lHorarios
        {
            get { return _lHorarios; }
            set
            {
                _lHorarios = value;
                NotifyPropertyChanged();
            }
        }

        public bool bFoiParaCadastro { get; set; } = false;

        public ApresentacaoEventoAgendaViewModel()
        {
            EncerrarEventoCommand = new Command(EncerrarEvento);
            ReabrirEventoCommand = new Command(ReabrirEvento);
            VerEnderecoCommand = new Command(VerEndereco);
            CheckInCommand = new Command(CheckIn);
            CheckOutCommand = new Command(CheckOut);
            CancelarEventoCommand = new Command(CancelarEvento);
            AdiarCommand = new Command(AdiarTarefaCommand);

            EditarEventoCommand = new Command(() =>
            {
                bFoiParaCadastro = true;
                Device.StartTimer(UtilMethods.GetStartTime, EditarAtividade);
            });

        }

        public bool EditarAtividade()
        {
            if (ExecuttingAnyCommand == false)
            {
                ExecuttingAnyCommand = true;

                var atividade = AgendaRepository.GetAtividadeModel(currentModel.idAtividadeOffline);
                UtilNavidate.PushAsync(new PageEventoNew(atividade));
            }
            return !ExecuttingAnyCommand;
        }

        public void RecarregaCurrentModel()
        {
            var resultado = AgendaRepository.GetInfinitParaEdicao(0, 1, "", App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa, false, currentModel.idAtividadeOffline, null, false, false);

            if (resultado.Any())
            {
                currentModel = resultado.FirstOrDefault();
                bool bAtrasado = false;

                if (DateTime.Now > currentModel.dtInicioEvento.GetValueOrDefault())
                    bAtrasado = true;

                if (currentModel.bEventoCancelado)
                {
                    currentModel.ColorPendencia = Color.FromHex("ff6b68");
                    currentModel.xAtraso = $"Cancelado";
                }
                else if (bAtrasado && !currentModel.bRealizado)
                {
                    currentModel.ColorPendencia = Color.FromHex("ff6b68");
                    currentModel.xAtraso = $"Atrasado";
                }
                else if (!currentModel.bRealizado)
                {
                    currentModel.ColorPendencia = Color.FromHex("2196F3");
                    currentModel.xAtraso = $"Para fazer";
                }
                else
                {
                    currentModel.ColorPendencia = Color.FromHex("32c787");
                    currentModel.xAtraso = $"Realizado";
                }

                PageListagemEventos.ViewModelStatic.canExecuteInicial = true;
            }
        }

        public bool Initialize()
        {
            if (canExecuteInicial)
                canExecuteInicial = false;

            if (lHorarios.stOpcao > 0)
            {
                RealizarAdiamento();
                lHorarios.stOpcao = 0;
            }

            return canExecuteInicial;
        }

        public async void AdiarTarefaCommand()
        {
            if (!ExecuttingAnyCommand)
            {
                ExecuttingAnyCommand = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (lHorarios == null)
                        lHorarios = new ListaHorariosAdiantamentoModel();
                    var pesquisa = new PageAdiantamentoHorarios(lHorarios)
                    {
                        Title = "Opções de adiantamento",
                    };
                    UtilNavidate.PushAsync(pesquisa);
                });
            }
        }


        public async void RealizarAdiamento()
        {
            try
            {
                var _diferencaEntreDatas = (currentModel.dtFimEvento.GetValueOrDefault() - currentModel.dtInicioEvento.GetValueOrDefault()).Hours;
                var _dtInicioEvento = DateTime.Now.ToLocalTime();
                var _dtFimEvento = DateTime.Now.ToLocalTime();


                switch (lHorarios.stOpcao)
                {
                    case 1:
                        _dtInicioEvento = _dtInicioEvento.AddHours(1);
                        _dtFimEvento = _dtInicioEvento.AddHours(_diferencaEntreDatas);
                        break;
                    case 2:
                        _dtInicioEvento = _dtInicioEvento.AddHours(2);
                        _dtFimEvento = _dtInicioEvento.AddHours(_diferencaEntreDatas);
                        break;
                    case 3:
                        _dtInicioEvento = _dtInicioEvento.AddHours(3);
                        _dtFimEvento = _dtInicioEvento.AddHours(_diferencaEntreDatas);
                        break;
                    case 4:
                        _dtInicioEvento = _dtInicioEvento.AddHours(5);
                        _dtFimEvento = _dtInicioEvento.AddHours(_diferencaEntreDatas);
                        break;
                    case 5:
                        _dtInicioEvento = _dtInicioEvento.AddDays(1);
                        _dtFimEvento = _dtInicioEvento.AddHours(_diferencaEntreDatas);
                        break;
                    case 6:
                        _dtInicioEvento = _dtInicioEvento.AddDays(2);
                        _dtFimEvento = _dtInicioEvento.AddHours(_diferencaEntreDatas);
                        break;
                    case 7:
                        _dtInicioEvento = _dtInicioEvento.AddDays(7);
                        _dtFimEvento = _dtInicioEvento.AddHours(_diferencaEntreDatas);
                        break;
                    default:
                        break;
                }

                AgendaRepository.RealizarAdiantamento(currentModel.idAtividadeOffline, _dtInicioEvento, _dtFimEvento);
                currentModel.dtInicioEvento = _dtInicioEvento;
                currentModel.dtFimEvento = _dtFimEvento;
                currentModel.xPeriodoEvento = $"{_dtInicioEvento.ToString("dd/MM/yyyy HH:mm")} até {_dtFimEvento.ToString("dd/MM/yyyy HH:mm")}";

                bool bAtrasado = false;
                if (DateTime.UtcNow > currentModel.dtInicioEvento.GetValueOrDefault())
                    bAtrasado = true;


                if (currentModel.bEventoCancelado)
                {
                    currentModel.ColorPendencia = Color.FromHex("ff6b68");
                    currentModel.xAtraso = $"Cancelado";
                }
                else if (bAtrasado && !currentModel.bRealizado)
                {
                    currentModel.ColorPendencia = Color.FromHex("ff6b68");
                    currentModel.xAtraso = $"Atrasado";
                }
                else if (!currentModel.bRealizado)
                {
                    currentModel.ColorPendencia = Color.FromHex("2196F3");
                    currentModel.xAtraso = $"Para fazer";
                }
                else
                {
                    currentModel.ColorPendencia = Color.FromHex("32c787");
                    currentModel.xAtraso = $"Realizado";
                }

                PageListagemEventos.ViewModelStatic.canExecuteInicial = true;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


        public async void EncerrarEvento()
        {
            try
            {
                AgendaRepository.RealizarEncerramento(currentModel.idAtividadeOffline);
                currentModel.bRealizado = true;
                currentModel.bEventoCancelado = false;
                currentModel.ColorPendencia = Color.FromHex("32c787");
                currentModel.xAtraso = $"Realizado";
                PageListagemEventos.ViewModelStatic.canExecuteInicial = true;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public async void VerEndereco()
        {
            try
            {
                Xamarin.Forms.Device.OpenUri(new Uri($"https://waze.com/ul?q=" + currentModel.xEnderecoCompleto + "&navigate=yes"));
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public async void CheckIn()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                AgendaListarModel agenda = new AgendaListarModel
                {
                    idAtividadeOffline = currentModel.idAtividadeOffline,
                    xLocalCheckIn = $"{location.Latitude}, {location.Longitude}"
                };

                AgendaRepository.SalvarCheckIn(agenda);

                currentModel.xLocalCheckIn = agenda.xLocalCheckIn;
                currentModel.DeuCheckIn = true;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public async void CheckOut()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                AgendaListarModel agenda = new AgendaListarModel
                {
                    idAtividadeOffline = currentModel.idAtividadeOffline,
                    xLocalCheckOut = $"{location.Latitude}, {location.Longitude}"
                };

                var result = AgendaRepository.SalvarCheckOut(agenda);

                currentModel.xLocalCheckOut = agenda.xLocalCheckOut;
                currentModel.xDuracaoCheck = result.tsDuracaoCheck?.ToString(@"hh\:mm\:ss\.ff");
                currentModel.DeuCheckOut = true;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public async void ReabrirEvento()
        {
            try
            {
                AgendaRepository.RealizarReabertura(currentModel.idAtividadeOffline);
                currentModel.bRealizado = false;
                currentModel.bEventoCancelado = false;

                bool bAtrasado = false;
                if (DateTime.UtcNow > currentModel.dtInicioEvento.GetValueOrDefault())
                    bAtrasado = true;


                if (currentModel.bEventoCancelado)
                {
                    currentModel.ColorPendencia = Color.FromHex("ff6b68");
                    currentModel.xAtraso = $"Cancelado";
                }
                else if (bAtrasado && !currentModel.bRealizado)
                {
                    currentModel.ColorPendencia = Color.FromHex("ff6b68");
                    currentModel.xAtraso = $"Atrasado";
                }
                else if (!currentModel.bRealizado)
                {
                    currentModel.ColorPendencia = Color.FromHex("2196F3");
                    currentModel.xAtraso = $"Para fazer";
                }
                else
                {
                    currentModel.ColorPendencia = Color.FromHex("32c787");
                    currentModel.xAtraso = $"Realizado";
                }


                PageListagemEventos.ViewModelStatic.canExecuteInicial = true;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


        public async void CancelarEvento()
        {
            try
            {

                AgendaRepository.CancelarEvento(currentModel.idAtividadeOffline);
                currentModel.bRealizado = false;
                currentModel.bEventoCancelado = true;
                currentModel.ColorPendencia = Color.FromHex("ff6b68");
                currentModel.xAtraso = $"Cancelado";


                PageListagemEventos.ViewModelStatic.canExecuteInicial = true;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

    }
}
