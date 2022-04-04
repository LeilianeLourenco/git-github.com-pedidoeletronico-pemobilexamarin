using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.View.Pesquisas;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pesquisa;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class DetalhesPedidoViewModel : NotifyCommon
    {
        #region Declaração de interfaces        

        #endregion

        #region Commands
        public ICommand ClosePopupCommand { get; set; }
        public ICommand ShowItensPedidoCommand { get; set; }
        public ICommand EditarPedidoCommand { get; set; }
        public ICommand VisualizarCommand { get; set; }
        public ICommand CompartilharCommand { get; set; }
        public ICommand DownloadCommand { get; set; }
        public ICommand SendEmailCommand { get; set; }
        public ICommand DuplicarPedidoCommand { get; set; }
        public ICommand MudarStatusCommand { get; set; }
        #endregion

        private PedidoVendaListarModel _currentModel;

        public PedidoVendaListarModel currentModel
        {
            get { return _currentModel; }
            set
            {
                _currentModel = value;
                NotifyPropertyChanged();
            }
        }

        private string _xMotivoCancelamento;

        public string xMotivoCancelamento
        {
            get { return _xMotivoCancelamento; }
            set { _xMotivoCancelamento = value; NotifyPropertyChanged(); }
        }

        private double _vTotalComissao;

        public double vTotalComissao
        {
            get { return _vTotalComissao; }
            set
            {
                _vTotalComissao = value;
                NotifyPropertyChanged();
            }
        }

        private double _vDescontoTotal;

        public double vDescontoTotal
        {
            get { return _vDescontoTotal; }
            set
            {
                _vDescontoTotal = value;
                NotifyPropertyChanged();
            }
        }

        public PedidoVendaModel objPedidoGeradoOuDuplicado { get; set; }

        public bool bFoiParaPedido { get; set; } = false;

        private bool _bSincronizado;

        public bool bSincronizado
        {
            get { return _bSincronizado; }
            set
            {
                _bSincronizado = value;
                NotifyPropertyChanged();
            }
        }

        private bool? _bUtilizaMelhoriaEscolherRepresentadaPdf;

        public bool? bUtilizaMelhoriaEscolherRepresentadaPdf
        {
            get { return _bUtilizaMelhoriaEscolherRepresentadaPdf; }
            set
            {
                _bUtilizaMelhoriaEscolherRepresentadaPdf = value;
                NotifyPropertyChanged();
            }
        }


        private ListItemModel _ItemSatus = new ListItemModel { Display = "Selecione um status" };

        public ListItemModel ItemSatus
        {
            get { return _ItemSatus; }
            set
            {
                _ItemSatus = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _representada;
        public ListItemModel representada
        {
            get { return _representada; }
            set { _representada = value; NotifyPropertyChanged(); }
        }

        public DetalhesPedidoViewModel()
        {
            ClosePopupCommand = new Command(() => { UtilNavidate.PopPopupNew(); });
            VisualizarCommand = new Command(() =>
            {
                UtilNavidate.ShowPopupNew(new PageViewToPrint(currentModel.idPedidoVendaOffLine));
            });
         
            ShowItensPedidoCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    UtilNavidate.ShowPopupNew(new PageShowItensPedido());
                }
            });

            DownloadCommand = new Command(VisualizarPDF);

            CompartilharCommand = new Command(CompartilharWhatsApp);

            EditarPedidoCommand = new Command(() =>
            {
                if (currentModel.idPedidoVenda == null)
                {
                    bFoiParaPedido = true;
                    Device.StartTimer(UtilMethods.GetStartTime, EditarPedido);
                }
                else
                    App.Messages.ShowAsync("Pedido já sincronizado, impossível ser editado");
            });

            SendEmailCommand = new Command(SendEmail);

            DuplicarPedidoCommand = new Command(DuplicarPedido);


            MudarStatusCommand = new Command(MudarStatus);


        }


        public async void MudarStatus()
        {
            if (currentModel.stPedidoVenda == 1)
            {
                await App.Messages.ShowAsync("Registro se encontra cancelado, impossível alteração de status.");
                return;
            }
            if (!ExecuttingAnyCommand)
            {
                ExecuttingAnyCommand = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (ItemSatus == null)
                        ItemSatus = new ListItemModel();
                    var pesquisa = new PagePesquisaPadrao(ItemSatus,
                        PesquisaPadraoViewModel.Tabela.STATUS_PEDIDO_APRESENTACAO)
                    {
                        Title = "Status do lançamento",
                    };
                    UtilNavidate.PushAsync(pesquisa);
                });
            }
        }

        public bool EditarPedido()
        {
            if (ExecuttingAnyCommand == false)
            {
                ExecuttingAnyCommand = true;

                var pedido = PedidoRepository.GetPedidoVendaModel(currentModel.idPedidoVendaOffLine);
                UtilNavidate.PushAsync(new PagePedidoNew(pedido));
            }
            return !ExecuttingAnyCommand;
        }

        public void VisualizarPDF()
        {
            try
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;


                    if (bUtilizaMelhoriaEscolherRepresentadaPdf.GetValueOrDefault())
                    { 
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (representada == null)
                                representada = new ListItemModel();
                            var pesquisa = new PagePesquisaPadrao(representada,
                                PesquisaPadraoViewModel.Tabela.TB_REPRESENTADA)
                            {
                                Title = "Representação para o PDF",
                            };
                            UtilNavidate.PushAsync(pesquisa);
                        });
                    }
                    else
                    {

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var url = UtilMethods.EncodeRoute("PedidoVenda", "Report",
                                $"idPedidoVenda={currentModel.idPedidoVenda}");
                            //inicio os 34140

                            // Implementado o ajuste de encrypt da o.s 34045

                            //Havia sido feita correção em Web, com uma implementação do framework, porém a dll utilizada no web,

                            //não está disponível no xamarin, portanto feito ajuste manual abaixo.

                            //url = url.Replace('+', '*');

                            url = url.Replace(oldValue: "+", newValue: "%2B"); 

                            //Fim os 34140
                            //Device.OpenUri(new Uri(url));

                            Launcher.OpenAsync(new Uri(url));
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


        public void VisualizarPDFPorRepresentada()
        {
            try
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var url = UtilMethods.EncodeRoute("PedidoVenda", "Report",
                            $"idPedidoVenda={currentModel.idPedidoVenda}");
                        //inicio os 34140

                        // Implementado o ajuste de encrypt da o.s 34045

                        //Havia sido feita correção em Web, com uma implementação do framework, porém a dll utilizada no web,

                        //não está disponível no xamarin, portanto feito ajuste manual abaixo.

                        //url = url.Replace('+', '*');

                        url = url.Replace(oldValue: "+", newValue: "%2B");
                        
                        //Fim os 34140
                        //Device.OpenUri(new Uri(url));

                        Launcher.OpenAsync(new Uri(url));
                    });

                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public async void CompartilharWhatsApp()
        {
            try
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var url = UtilMethods.EncodeRoute("PedidoVenda", "Report",
                            $"idPedidoVenda={currentModel.idPedidoVenda}");

                        //inicio os 34045

                        //url = url.Replace('+', '*');
                        
                        //Havia sido feita correção em Web, com uma implementação do framework, porém a dll utilizada no web,
                        
                        //não está disponível no xamarin, portanto feito ajuste manual abaixo.

                        url = url.Replace(oldValue: "+", newValue: "%2B");

                        //fim os 34045

                        var saudacao = "";
                        if (DateTime.Now.Hour < 12)
                            saudacao = "Bom dia {0},";
                        else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
                            saudacao = "Boa tarde {0},";
                        else
                            saudacao = "Boa noite {0},";
                        saudacao = string.Format(saudacao, currentModel.DisplayCliente);

                        var text =
                            $@"{saudacao} segue abaixo a url de visualização do seu {(currentModel.stLancamento == 0
                                ? "orçamento"
                                : "pedido")} {currentModel.idPedidoDisplay}, emitido {currentModel.XdEmissao}.{Environment
                                .NewLine}";


                        if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.WinPhone)
                        {
                            await Share.RequestAsync(new ShareTextRequest
                            {
                                Text = text,
                                Title = "pedidoeletronico.com",
                                Uri = url
                            });


                            //await Plugin.Share.CrossShare.Current.Share(url, text, "Pedido Eletrônico");
                        }
                        else
                        {
                            text += url;
                            await Share.RequestAsync(new ShareTextRequest
                            {
                                Text = text,
                                Title = "pedidoeletronico.com"
                            });

                            //await Plugin.Share.CrossShare.Current.Share(text, "Pedido Eletrônico");
                        }

                        ExecuttingAnyCommand = false;
                    });
                }





            }
            catch (Exception ex)
            {
                await App.Messages.ShowAsync("App WhatsApp não foi encontrado");
            }


        }

        public async void SendEmail()
        {
            if (await App.IsConected() == false)
            {
                await App.Messages.ShowAsync("Sem conexão com internet.");
                return;
            }

            var email = await App.Messages.ShowQuestionMessageEmailPedido();
            if (email != null)
            {
                if (email.bEnviaCliente || email.bEnviaRepresentacoes)
                {
                    var emailtosend = new EmailPedidoModel
                    {
                        idPedidoVendaOffLine = currentModel.idPedidoVendaOffLine,
                        idPedidoVenda = currentModel.idPedidoVenda,
                        idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                        bEnviaCliente = email.bEnviaCliente,
                        bEnviaRepresentacoes = email.bEnviaRepresentacoes,
                        idAspnetUsers = App.CurrentAspnetUserModel.Id
                    };
                    UtilHttp.SendEmailPedido(email);
                    await App.Messages.ShowAsync("Solicitação enviada com sucesso.");
                }
            }
        }



        public async void DuplicarPedido()
        {
            try
            {

                //if (currentModel.stPedidoVenda == 1)
                //{
                //    await App.Messages.ShowAsync("Lançamento CANCELADO, impossível realizar essa ação.");
                //    return;
                //}

                if (currentModel.stLancamento == 1)
                {
                    objPedidoGeradoOuDuplicado = await PedidoRepository.DuplicarPedido(currentModel);
                    if (objPedidoGeradoOuDuplicado != null)
                    {
                        PageListarPedidos.ViewModelStatic.canExecuteInicial = true;
                        var resultado = await
                            App.Messages.ShowConfirmAsync(
                                $@"Registro duplicado com sucesso. {Environment.NewLine}Deseja abrir o lançamento ?",
                                "ABRIR PEDIDO");
                        if (resultado)
                        {
                            UtilNavidate.PopAsync();
                            Device.StartTimer(UtilMethods.GetStartTime, GoToPedidoVenda);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> GerarPedidoByOrcamento()
        {
            objPedidoGeradoOuDuplicado = await PedidoRepository.GerarPedidoByOrcamentoNew(currentModel);
            if (objPedidoGeradoOuDuplicado != null)
            {
                PageListarPedidos.ViewModelStatic.canExecuteInicial = true;
                var resultado = await
                    App.Messages.ShowConfirmAsync(
                        $@"Pedido gerado com sucesso! {Environment.NewLine}Deseja abrir o lançamento ?", "ABRIR PEDIDO");
                if (resultado)
                {
                    UtilNavidate.PopAsync();
                    Device.StartTimer(UtilMethods.GetStartTime, GoToPedidoVenda);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool bCanGotoPedido { get; set; } = true;

        private bool GoToPedidoVenda()
        {
            if (bCanGotoPedido)
            {
                bCanGotoPedido = false;
                UtilNavidate.PushAsync(new PagePedidoNew(objPedidoGeradoOuDuplicado));
            }
            return bCanGotoPedido;
        }



        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                bSincronizado = currentModel.idPedidoVenda != null;
                ItemSatus = new ListItemModel
                {
                    XId = currentModel.idStatus.ToString(),
                    Display = currentModel.xNomeStatus
                };

                representada = new ListItemModel
                {
                    XId = currentModel.idRepresentadaPdf.ToString(),
                    Display = "Representada"
                };
                

                vDescontoTotal = PedidoRepository.SumDescontoItens(currentModel.idPedidoVendaOffLine);
                vTotalComissao = PedidoRepository.SumFieldItem(currentModel.idPedidoVendaOffLine, "vComissao");
                bUtilizaMelhoriaEscolherRepresentadaPdf = ConfiguracaoGeralRepositorio.GetMelhoriaEspecificaRepresentacaoPdf(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
            }

            if (ItemSatus.XId != currentModel.idStatus.ToString())
            {
                AnaliseDeMudancaDeStatus();
            }


            if (bUtilizaMelhoriaEscolherRepresentadaPdf.GetValueOrDefault() && representada.XId != currentModel.idRepresentadaPdf.GetValueOrDefault().ToString())
            {
                VisualizarPDFPorRepresentada();
            }

            return canExecuteInicial;
        }

        public StatusModel _newstatus { get; set; }
        public async void AnaliseDeMudancaDeStatus()
        {
            var status = Convert.ToInt32(ItemSatus.XId);
            if (status > 0)
            {
                if (status != currentModel.idStatus)
                {
                    _newstatus = StatusRepository.GetRegistro(status);

                    if (_newstatus.stVenda == 1) // mudou para cancelado 
                    {
                        if (currentModel.stLancamento == 1) // é um pedido
                        {
                            // verificar se tem duplicata baixada
                            if (currentModel.idPedidoVenda != null && currentModel.idPedidoVenda > 0)
                            {
                                var resultado =
                                    FinanceiroRepository.VerificaPedidoComFinanceiroLancado(
                                        currentModel.idPedidoVenda ?? 0);

                                if (resultado)
                                {
                                    await
                                        App.Messages.ShowAsync(
                                            "Existe movimentação financeira para esse pedido, impossível cancelar.");
                                    VoltarStatusAtual();
                                    return;
                                }
                            }


                        }
                        else
                        {
                            // é um orçamento

                            if ((currentModel.idPedidoVenda ?? 0) > 0)
                            {
                                var bTemPedidos = PedidoRepository.ExistemPedidosByOrcamento(currentModel.idPedidoVenda ?? 0);

                                if (bTemPedidos)
                                {
                                    await
                                        App.Messages.ShowAsync(
                                            "Existem pedidos vinculados a esse orçamento, impossível cancelar.");
                                    VoltarStatusAtual();
                                    return;
                                }
                            }
                        }
                    }

                    if (_newstatus.stVenda == 0) // mudou para aberto
                    {
                        await App.Messages.ShowAsync("Orçamento não pode voltar para aberto.");
                        VoltarStatusAtual();
                        return;
                    }

                    if (_newstatus.stVenda == 2) // mudou para vendido
                    {
                        if (currentModel.stLancamento == 0) // é um orçamento.
                        {
                            var retorno = await GerarPedidoByOrcamento();
                            if (retorno)
                            {
                                EfetivaAlteracaoStatus();
                                return;
                            }
                            else
                            {
                                VoltarStatusAtual();
                                return;
                            }

                        }
                    }

                    if (
                            await
                                App.Messages.ShowConfirmAsync(
                                    $"Confirma a mudança do status '{currentModel.xNomeStatus}' para {_newstatus.xNome} ?") ==
                            false)
                    {
                        VoltarStatusAtual();
                        return;
                    }
                    bCanGotoPedido = true;
                    if (_newstatus.stVenda == 1)
                    {
                        Device.StartTimer(UtilMethods.GetStartTime, GotoMessageCancel);
                    }
                    else
                        EfetivaAlteracaoStatus();
                }
            }
        }


        public bool bCanGoMessageCancel { get; set; } = true;

        private bool GotoMessageCancel()
        {
            if (bCanGotoPedido)
            {
                bCanGotoPedido = false;
                var pageMensagem = new PageGetObservacaoPopup(this);
                UtilNavidate.ShowPopupNew(pageMensagem);
            }
            return bCanGotoPedido;
        }


        public void VoltarStatusAtual()
        {
            ItemSatus = new ListItemModel
            {
                Id = currentModel.idStatus,
                Display = currentModel.xNomeStatus
            };
        }

        public void RecarregaCurrentModel()
        {
            bCanGotoPedido = false;
            var resultado = PedidoRepository.GetInfinit(0, 1, "", "", null, currentModel.idPedidoVendaOffLine);

            if (resultado.Any())
            {
                currentModel = resultado.FirstOrDefault();
                PageListarPedidos.ViewModelStatic.canExecuteInicial = true;
            }
        }


        public void EfetivaAlteracaoStatus()
        {
            try
            {
                if (_newstatus.stVenda == 1 && string.IsNullOrEmpty(xMotivoCancelamento))
                {
                    VoltarStatusAtual();
                    return;
                }

                PedidoRepository.UpdateStatus(currentModel.idPedidoVendaOffLine, _newstatus.idStatus, currentModel.idStatus, _newstatus.stVenda, xMotivoCancelamento ?? "");
                currentModel.idStatus = _newstatus.idStatus;
                currentModel.stPedidoVenda = (byte)_newstatus.stVenda;
                currentModel.xCor = _newstatus.xCor;
                currentModel.xSigla = _newstatus.xSigla;
                currentModel.xNomeStatus = _newstatus.xNome;
                currentModel.PublicNotifyPropertyChanged("ColorSincronizacao");
                currentModel.PublicNotifyPropertyChanged("ColorSigla");

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


    }
}
