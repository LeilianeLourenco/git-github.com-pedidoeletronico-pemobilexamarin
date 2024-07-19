using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Converter.Generic;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.View.Pesquisas;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pesquisa;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class PedidoNewViewModel : SearchCommom
    {
        public ICommand DateOrcamentoVisibilityCommand { get; set; }
        public ICommand RepresentacaoPdfVisibilityCommand { get; set; }
        public ICommand VendedorVisibilityCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand GoToProdutosCommand { get; set; }
        public ICommand GoToFinanceiroCommand { get; set; }
        public ICommand GoToStatusCommand { get; set; }
        public ICommand GoToClientesCommand { get; set; }
        public ICommand GoToRepresentantesCommand { get; set; }
        public ICommand GoToTransportadoraCommand { get; set; }
        public ICommand GoToEnderecoCommand { get; set; }
        public ICommand GoToRedespachoCommand { get; set; }
        public ICommand GoToConficaoPgtoCommand { get; set; }
        public ICommand GoToFormaPgtoCommand { get; set; }

        public ICommand GoToComplementosCommand { get; set; }
        public ICommand GoToDescontoCommand { get; set; }


        public Command ChangeTimePedidoCommand { get; set; }

        public Command ChangeTimePrevisaoCommand { get; set; }
        public Command ChangeTimeOrcamentoCommand { get; set; }
        public Command GoToRepresentacoesCommand { get; set; }
        public Command ChangeTipoLancamentoCommand { get; set; }

        public ICommand CancelPedidoCommand { get; set; }

        public ICommand GoToEstoqueCommand { get; set; }



        #region Propriedades
     
        private PedidoVendaModel _currentModel;

        public PedidoVendaModel currentModel
        {
            get { return _currentModel; }
            set
            {
                _currentModel = value;
                NotifyPropertyChanged();
            }
        }

        private Color _ColorTipoLancamento = ColorStaticModel.Pedido;

        public Color ColorTipoLancamento
        {
            get { return _ColorTipoLancamento; }
            set
            {
                _ColorTipoLancamento = value;
                NotifyPropertyChanged();
            }
        }

        private string _xDisplayComplementos;

        public string xDisplayComplementos
        {
            get { return _xDisplayComplementos; }
            set { _xDisplayComplementos = value; NotifyPropertyChanged(); }
        }

        private string _xDisplayDesconto;

        public string xDisplayDesconto
        {
            get { return _xDisplayDesconto; }
            set { _xDisplayDesconto = value; NotifyPropertyChanged(); }
        }


        private bool _isShowRepresentantes;

        public bool isShowRepresentantes
        {
            get { return _isShowRepresentantes; }
            set
            {
                _isShowRepresentantes = value;
                NotifyPropertyChanged();
            }
        }

        private bool _ShowBotaoContrato;

        public bool ShowBotaoContrato
        {
            get { return _ShowBotaoContrato; }
            set
            {
                _ShowBotaoContrato = value;
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

        private ListItemModel _ItemEndereco = new ListItemModel { Detail = "clique aqui para pesquisar" };

        public ListItemModel ItemEndereco
        {
            get { return _ItemEndereco; }
            set
            {
                _ItemEndereco = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _ItemCondicaoPgto = new ListItemModel { Display = "clique aqui para pesquisar" };

        public ListItemModel ItemCondicaoPgto
        {
            get { return _ItemCondicaoPgto; }
            set
            {
                _ItemCondicaoPgto = value;
                NotifyPropertyChanged();
            }
        }


        private ListItemModel _ItemFormaPgto = new ListItemModel { Display = "clique aqui para pesquisar" };

        public ListItemModel ItemFormaPgto
        {
            get { return _ItemFormaPgto; }
            set
            {
                _ItemFormaPgto = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _ItemTransportadora = new ListItemModel { Display = "clique aqui para pesquisar" };

        public ListItemModel ItemTransportadora
        {
            get { return _ItemTransportadora; }
            set
            {
                _ItemTransportadora = value;
                NotifyPropertyChanged();
            }
        }


        private ListItemModel _ItemRedespacho = new ListItemModel { Display = "clique aqui para pesquisar" };

        public ListItemModel ItemRedespacho
        {
            get { return _ItemRedespacho; }
            set
            {
                _ItemRedespacho = value;
                NotifyPropertyChanged();
            }
        }


        private ListItemModel _ItemRepresentante = new ListItemModel { Display = "clique aqui para pesquisar" };

        public ListItemModel ItemRepresentante
        {
            get { return _ItemRepresentante; }
            set
            {
                _ItemRepresentante = value;
                NotifyPropertyChanged();
            }
        }


        private List<ListItemModel> _lRepresentacoes = new List<ListItemModel>();

        public List<ListItemModel> lRepresentacoes
        {
            get { return _lRepresentacoes; }
            set
            {
                _lRepresentacoes = value;
                NotifyPropertyChanged();
            }
        }
        private ListItemModel _representada;
        public ListItemModel representada
        {
            get { return _representada; }
            set { _representada = value; NotifyPropertyChanged(); }
        }

        private bool _bCancelado = false;

        public bool bCancelado
        {
            get { return _bCancelado; }
            set
            {
                _bCancelado = value;
                NotifyPropertyChanged();
            }
        }



        private int _idUltimaCondicaoPgto = 0;
        public int idUltimaCondicaoPgto
        {
            get { return _idUltimaCondicaoPgto; }
            set
            {
                _idUltimaCondicaoPgto = value;
                NotifyPropertyChanged();
            }
        }

        private int _idUltimoCliente = 0;
        public int idUltimoCliente
        {
            get { return _idUltimoCliente; }
            set
            {
                _idUltimoCliente = value;
                NotifyPropertyChanged();
            }
        }




        #endregion



        #region Nao tratadas

        private int _countItens = 0;

        public int CountItens
        {
            get { return _countItens; }
            set
            {
                _countItens = value;
                NotifyPropertyChanged();
            }
        }


        private double _vSubTotal;

        public double vSubTotal
        {
            get { return _vSubTotal; }
            set
            {
                _vSubTotal = value;
                NotifyPropertyChanged();
            }
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


        private double _vFinanceiroEmAberto;

        public double vFinanceiroEmAberto
        {
            get { return _vFinanceiroEmAberto; }
            set
            {
                _vFinanceiroEmAberto = value;
                NotifyPropertyChanged();
            }
        }

        private double _vFinanceiroVencido;

        public double vFinanceiroVencido
        {
            get { return _vFinanceiroVencido; }
            set
            {
                _vFinanceiroVencido = value;
                NotifyPropertyChanged();
            }
        }


        private bool _bPedidoByCliente;

        public bool bPedidoByCliente
        {
            get { return _bPedidoByCliente; }
            set
            {
                _bPedidoByCliente = value;
                NotifyPropertyChanged();
            }
        }


        private bool _bUsaMinimoVendas;

        public bool bUsaMinimoVendas
        {
            get { return _bUsaMinimoVendas; }
            set { _bUsaMinimoVendas = value; NotifyPropertyChanged(); }
        }




        private IEnumerable<Group<string, PedidoVendaItensModel>> _registrosEstoqueAgrupados;

        public IEnumerable<Group<string, PedidoVendaItensModel>> RegistrosEstoqueAgrupados
        {
            get { return _registrosEstoqueAgrupados; }
            set
            {
                _registrosEstoqueAgrupados = value;
                NotifyPropertyChanged();
            }
        }
   
        public double? vDescCondicao { get; set; } //alterado

        public int? idTabelaPrecoCondicao { get; set; } //alterado



        #endregion



        public PedidoNewViewModel()
        {
            currentModel = new PedidoVendaModel();
            CancelPedidoCommand = new Command(CancelPress);
            SaveCommand = new Command(Save, CanSave);

            ChangeTimePedidoCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!ExecuttingAnyCommand)
                    {
                        ExecuttingAnyCommand = true;
                        var page = new PageSelectDate(currentModel, SelectDateViewModel.tipolancamento.PEDIDO);
                        UtilNavidate.PushModalAsync(page);
                    }
                });
            });

            ChangeTimePrevisaoCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!ExecuttingAnyCommand)
                    {
                        ExecuttingAnyCommand = true;
                        var page = new PageSelectDate(currentModel, SelectDateViewModel.tipolancamento.PREVISAO_ENTREGA);
                        UtilNavidate.PushModalAsync(page);
                    }
                });
            });

            ChangeTimeOrcamentoCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var page = new PageSelectDate(currentModel, SelectDateViewModel.tipolancamento.ORCAMENTO);
                        UtilNavidate.PushModalAsync(page);
                    });
                }
            });


            GoToRepresentacoesCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var page = new PageBasicList(representada, lRepresentacoes, "Representação PDF");
                        UtilNavidate.PushModalAsync(page);
                    });
                }
            });


            ChangeTipoLancamentoCommand = new Command(async () =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    var resultado = await App.Messages.TipoLancamentoTask();

                    if (resultado == "Cancelar")
                    {

                        ExecuttingAnyCommand = false;
                        return;
                    }
                    if (resultado.Equals("PEDIDO"))
                    {
                        currentModel.stLancamento = 1;
                    }
                    else
                    {
                        currentModel.stLancamento = 0;
                        if (currentModel.idPedidoVenda == null)
                        {
                            var nDias = EmpresaRepository.GetnDiasValidadeOrcamento();
                            currentModel.dtValidadeOrcamento = DateTime.Now.AddDays(Convert.ToDouble(nDias));
                        }
                    }
                    ChangeLancamento();
                    DateOrcamentoVisibilityCommand.Execute(null);
                    ExecuttingAnyCommand = false;
                }


            });


            GoToStatusCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ItemSatus == null)
                            ItemSatus = new ListItemModel();
                        var pesquisa = new PagePesquisaPadrao(ItemSatus,
                            PesquisaPadraoViewModel.Tabela.STATUS_PEDIDO)
                        {
                            Title = "Status do lançamento",
                        };
                        UtilNavidate.PushAsync(pesquisa);
                    });
                }

            });

            GoToClientesCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    if (currentModel.lItens.Count == 0)
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
                    else
                    {
                        App.Messages.ShowAsync("Impossível alterar o cliente do pedido após ter sido lançado itens.");
                    }
                }

            });

            GoToComplementosCommand = new Command(async () =>
                {
                    if (ItemCliente.Id == 0)
                    {
                        await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                        return;
                    }

                    ExecuttingAnyCommand = true;
                    var page = new PageComplementosPedido(this);
                    UtilNavidate.ShowPopupNew(page);
                });

            GoToDescontoCommand = new Command(async () =>
            {
                if (ItemCliente.Id == 0)
                {
                    await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                    return;
                }

                ExecuttingAnyCommand = true;
                var page = new PageDescontoPedido(this);
                UtilNavidate.ShowPopupNew(page);
            });

            GoToConficaoPgtoCommand = new Command(async () =>
            {
                if (ItemCliente.Id == 0)
                {
                    await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                    return;
                }

                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ItemCondicaoPgto == null)
                            ItemCondicaoPgto = new ListItemModel();
                        var pesquisa = new PagePesquisaPadrao(ItemCondicaoPgto,
                            PesquisaPadraoViewModel.Tabela.TB_CONDICAO_PAGAMENTO, currentModel.idClientesOffLine)
                        {
                            Title = "Condição de Pagamento",
                        };
                        UtilNavidate.PushAsync(pesquisa);
                    });

                    AtualizaTotalizadoresPedido(); //alterado
                }

            });

            GoToFormaPgtoCommand = new Command(async () =>
            {
                if (ItemCliente.Id == 0)
                {
                    await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                    return;
                }

                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ItemFormaPgto == null)
                            ItemFormaPgto = new ListItemModel();
                        var pesquisa = new PagePesquisaPadrao(ItemFormaPgto,
                            PesquisaPadraoViewModel.Tabela.TB_FORMA_PAGAMENTO, null, ItemCondicaoPgto.Id)
                        {
                            Title = "Forma de Pagamento",
                        };
                        UtilNavidate.PushAsync(pesquisa);
                    });
                }

            });

            GoToEnderecoCommand = new Command(async () =>
            {
                if (ItemCliente.Id == 0)
                {
                    await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                    return;
                }
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ItemEndereco == null)
                            ItemEndereco = new ListItemModel();

                        var pesquisa = new PagePesquisaPadrao(ItemEndereco,
                           PesquisaPadraoViewModel.Tabela.TB_ENDERECO, currentModel.idClientesOffLine)
                        {
                            Title = "Endereços",
                        };

                        UtilNavidate.PushAsync(pesquisa);
                    });
                }
            });
        
            GoToTransportadoraCommand = new Command(async () =>
            {
                if (ItemCliente.Id == 0)
                {
                    await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                    return;
                }
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ItemTransportadora == null)
                            ItemTransportadora = new ListItemModel();

                        var pesquisa = new PagePesquisaPadrao(ItemTransportadora,
                            PesquisaPadraoViewModel.Tabela.TB_TRANSPORTADORA)
                        {
                            Title = "Transportadora",
                        };
                        UtilNavidate.PushAsync(pesquisa);
                    });
                }


            });

            GoToRedespachoCommand = new Command(async () =>
            {
                if (ItemCliente.Id == 0)
                {
                    await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                    return;
                }
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ItemRedespacho == null)
                            ItemRedespacho = new ListItemModel();

                        var pesquisa = new PagePesquisaPadrao(ItemRedespacho,
                            PesquisaPadraoViewModel.Tabela.TB_TRANSPORTADORA)
                        {
                            Title = "Redespacho",
                        };
                        UtilNavidate.PushAsync(pesquisa);
                    });
                }


            });

            GoToRepresentantesCommand = new Command(async () =>
            {
                if (ItemCliente.Id == 0)
                {
                    await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                    return;
                }

                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ItemRepresentante == null)
                            ItemRepresentante = new ListItemModel();
                        var pesquisa = new PagePesquisaPadrao(ItemRepresentante,
                            PesquisaPadraoViewModel.Tabela.TB_REPRESENTANTE)
                        {
                            Title = "Vendedor",
                        };
                        UtilNavidate.PushAsync(pesquisa);
                    });
                }

            });

            GoToProdutosCommand = new Command(NavigateToProdutos);

            GoToFinanceiroCommand = new Command(NavigateToFinanceiro);

            GoToEstoqueCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (RegistrosEstoqueAgrupados.Any())
                            UtilNavidate.PushAsync(new PageEstoqueInvalido(this));
                    });
                }

            });
        }

        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = true;
                    ExecuttingAnyCommand = canExecuteInicial = false;

                    ChangeLancamento();

                    //Novo método de checar configurações dentro do pedido;
                    ValidaConfiguracoesGerais();

                    if (bPedidoByCliente)
                    {
                        ItemCliente = ClienteRepository.GetRegistro(currentModel.idClientesOffLine);
                        SetConiguracoesDoCliente();
                        //AtualizaTotalizadoresPedido();
                    }
                    else if (currentModel.idPedidoVendaOffLine != null && currentModel.idPedidoVendaOffLine > 0)
                    {
                        idUltimaCondicaoPgto = currentModel.idCondicaoPagamento.GetValueOrDefault();
                        idUltimoCliente = currentModel.idClientesOffLine;
                        SetConfiguracoes(currentModel.idRepresentantePedido ?? 0, currentModel.idTransportadora ?? 0,
                            currentModel.idCondicaoPagamento ?? 0, currentModel.idClientesOffLine, currentModel.idRedespacho.GetValueOrDefault(), currentModel.xFormaPagamento);
                        VerificaStatusCancelado();
                        //AtualizaTotalizadoresPedido();
                        RotinaValidacaoEstoque();
                    }
                    else
                    {
                        if (currentModel.dtValidadeOrcamento == null)
                        {
                            currentModel.dtValidadeOrcamento = DateTime.Now.SqlMinDateTime();
                        }

                        var _exibeAnotacoesPedido = EmpresaRepository.MostraAnotacaoPedidoDaEmpresa(idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
                        if (_exibeAnotacoesPedido)
                            currentModel.xInfAdicional = EmpresaRepository.GetAnotacaoEmpresaParaPedido(idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
                    }

                    AtualizaTotalizadoresPedido();

                    //tratando o horário do pedido caso for um novo, porque se não ficar subtraindo quando edita
                    if (currentModel.dEmissao.Kind != DateTimeKind.Local && currentModel.idPedidoVendaOffLine.GetValueOrDefault() == 0)
                    {
                        //currentModel.dEmissao = currentModel.dEmissao.ToLocalTime();
                    }
                    if (currentModel.dtPrevisto != null)
                        if ((currentModel.dtPrevisto ?? DateTime.Now).Kind != DateTimeKind.Local && currentModel.idPedidoVendaOffLine.GetValueOrDefault() == 0)
                        {
                            currentModel.dtPrevisto = (currentModel.dtPrevisto ?? DateTime.Now).ToLocalTime();
                        }

                    if (currentModel.dtValidadeOrcamento != null)
                        if ((currentModel.dtValidadeOrcamento ?? DateTime.Now).Kind != DateTimeKind.Local && currentModel.idPedidoVendaOffLine.GetValueOrDefault() == 0)
                        {
                            currentModel.dtValidadeOrcamento =
                                (currentModel.dtValidadeOrcamento ?? DateTime.Now).ToLocalTime();
                        }
                    Device.StartTimer(UtilMethods.GetStartTime, SetFalseIsBusy);
                });
            }

            if (currentModel.CurrentItemModel != null)
            {
                AtualizaTotalizadoresPedido();
                currentModel.CurrentItemModel.editting = false;
                currentModel.CurrentItemModel.SetDetalheItem();
            }
            return canExecuteInicial;
        }


        public bool SetFalseIsBusy()
        {
            ExecuttingAnyCommand = IsBusy = false;
            return IsBusy;
        }


        public void ChangeLancamento()
        {
            ColorTipoLancamento = (currentModel.stLancamento == 0)
                ? ColorStaticModel.Orcamento
                : ColorStaticModel.Pedido;
            ItemSatus = StatusRepository.GetDefault(currentModel.stLancamento);

        }

        public void GetRepresentacao()
        {
            try
            {
                lRepresentacoes =
                     new List<ListItemModel>(RepresentadaRepository.GetListItemModel());

                representada = lRepresentacoes.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

        }
        private async void NavigateToFinanceiro()
        {
            if (currentModel.idClientesOffLine == 0)
            {
                await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                return;
            }

            if (!ExecuttingAnyCommand)
            {
                ExecuttingAnyCommand = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    UtilNavidate.PushAsync(new PageFinanceiroCliente(currentModel.idClientesOffLine));
                });
            }
        }

        private async void NavigateToProdutos()
        {
            if (currentModel.idClientesOffLine == 0)
            {
                await App.Messages.ShowAsync("Antes disso, selecione um cliente");
                return;
            }

            if (!ExecuttingAnyCommand)
            {
                ExecuttingAnyCommand = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    UtilNavidate.PushAsync(new PageListarProdutosNew(this));
                });
            }
        }

        private PageListarProdutosNew _itenspedidos;

        public PageListarProdutosNew itenspedidos
        {
            get
            {
                _itenspedidos = _itenspedidos ?? new PageListarProdutosNew(this);
                return _itenspedidos;
            }
            set { _itenspedidos = value; }
        }


        public async void SetConiguracoesDoCliente()
        {
            await Task.Run(() =>
            {
                var idRepresentante = ClienteRepository.GetIdRepresentanteDoCliente(ItemCliente.Id);


                //checando se NÃO é administrador, caso não seja, vou forçar que o idEmpresa_aspnetuser do representante seja preenchido em vez do vinculado no cliente
                if (idRepresentante == 0 ||
                (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAcessoTodosClientes == 1 &&
                idRepresentante != App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers &&
                !App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador))
                    idRepresentante = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0;

                var idTransportadora = ClienteRepository.GetIdTransportadoraCliente(currentModel.idClientesOffLine);

                var idRedespacho = ClienteRepository.GetIdRedespachoCliente(currentModel.idClientesOffLine);

                if (idUltimoCliente == 0 || currentModel.idClientesOffLine != idUltimoCliente || idUltimaCondicaoPgto == 0)
                    idUltimaCondicaoPgto = ClienteRepository.GetIdCondicaoAsync(currentModel.idClientesOffLine, App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa) ?? 0;

                idUltimoCliente = currentModel.idClientesOffLine;


                var anotacao = ClienteRepository.GetAnotacaoCliente(ItemCliente.Id);

                //validação de configuração geral de minimo de compra 
                if (currentModel.bUsaMinimoVendas && currentModel.stCadastroMinimoVenda == 3)
                {
                    var _limiteAux = ClienteRepository.GetValorMinimoVenda(currentModel.idClientesOffLine);
                    var _msgAux = string.Empty;

                    if (currentModel.stCalculoMinimoVenda == 1)
                    {
                        _msgAux = $"Este cliente deve pedir no mínimo {Extensions.ToCurrencyStringSimplesPtBr(_limiteAux)} itens.";
                    }
                    else
                    {
                        _msgAux = $"Este cliente deve pedir no mínimo {Extensions.ToCurrencyStringPtBr(_limiteAux)}";
                    }

                    currentModel.bUsaMinimoVendas = true;
                    currentModel.xMinimoVendas = _msgAux;
                    currentModel.vLimiteMinimoVendaCliente = _limiteAux;
                }

                if (!string.IsNullOrEmpty(anotacao))
                {
                    if (!currentModel.xInfAdicional.ToUpper().Contains(anotacao.ToUpper()))
                    {
                        currentModel.xInfAdicional += string.IsNullOrEmpty(currentModel.xInfAdicional) ? anotacao : Environment.NewLine + anotacao;
                    }
                }


                if (ItemCliente != null && ItemCliente.Id > 0)
                {
                    KeyValuePair<double, double> _retornoFin = FinanceiroRepository.BuscaAbertosEVencidos(ItemCliente.Id);

                    vFinanceiroEmAberto = _retornoFin.Key;
                    vFinanceiroVencido = _retornoFin.Value;
                }

                SetConfiguracoes(idRepresentante, idTransportadora, idUltimaCondicaoPgto, null, idRedespacho, currentModel.xFormaPagamento);

            });
        }

        public void SetConfiguracoes(int idRepresentante, int idTransportadora, int idCondicaoPgto,
            int? idClienteOffLine = null, int? idRedespacho = null, string xFormaPagamento = null)
        {
            currentModel.idRepresentantePedido = idRepresentante;
            if (idClienteOffLine != null && idClienteOffLine > 0)
            {
                ItemCliente = ClienteRepository.GetRegistro(idClienteOffLine ?? 0);
            }

            if (idRepresentante == 0)
                idRepresentante = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0;

            if (ItemRepresentante.Id != idRepresentante)
                ItemRepresentante = EmpresaAspnetUsersRepository.GetRegistro(idRepresentante);


            if (idTransportadora > 0 && ItemTransportadora != null && ItemTransportadora.Id == 0)
                ItemTransportadora = TransportadoraRepository.GetItem(idTransportadora);

            if (idRedespacho.GetValueOrDefault() > 0 && ItemRedespacho.Id == 0)
                ItemRedespacho = TransportadoraRepository.GetItem(idRedespacho.GetValueOrDefault());

            if (idCondicaoPgto > 0 && ItemCondicaoPgto != null && (ItemCondicaoPgto.Id == 0 || idCondicaoPgto != ItemCondicaoPgto.Id))
            {
                bool bAplicaMelhoriaRosaMaria = ConfiguracaoGeralRepositorio.GetMelhoriaEspecificaCondicoesPagamento(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
                if (!bAplicaMelhoriaRosaMaria || currentModel.idPedidoVendaOffLine.GetValueOrDefault() > 0)
                {
                    ItemCondicaoPgto = CondicaoPagamentoRepository.GetItem(idCondicaoPgto, currentModel.idClientesOffLine);
                    //OS 35323 - Jessica Barbieri
                    if (ItemCondicaoPgto.Id == 0)
                    {
                        App.Messages.ShowAsync("Não foi encontrado nenhuma condição de pagamento para o cliente, verifique com o administrador se as condições possuem parâmetros como tabela de preço");
                    }
                    else
                    {
                        var condicaoPag = CondicaoPagamentoRepository.GetItem(ItemCondicaoPgto.Id, currentModel.idClientesOffLine);
                        vDescCondicao = condicaoPag.vDescCondicao;
                        idTabelaPrecoCondicao = condicaoPag.idTabelaPreco;
                    }
                }
            }

            if (string.IsNullOrEmpty(xFormaPagamento) && ItemFormaPgto != null && (string.IsNullOrEmpty(ItemFormaPgto.Display) || xFormaPagamento != ItemFormaPgto.Display))
            {
                ItemFormaPgto = CondicaoPagamentoRepository.BuscaFormasPagamentoPorCondicao(string.Empty, idCondicaoPgto).OrderBy(t => t.Display).FirstOrDefault();
                //OS 35323 - Jessica Barbieri
                xFormaPagamento = ItemFormaPgto.Display;
            }
            else ItemFormaPgto = CondicaoPagamentoRepository.BuscaFormasPagamentoPorCondicao(string.Empty, idCondicaoPgto).Where(t => t.Display == xFormaPagamento).FirstOrDefault();
        }

        public void VerificaStatusCancelado()
        {
            bCancelado = ItemSatus.Id == 1;
        }

        public void AtualizaTotalizadoresPedido()
        {
            try
            {
                if (currentModel.lItens == null) return;


                var condicaoPag = CondicaoPagamentoRepository.GetItem(ItemCondicaoPgto.Id);
                idUltimaCondicaoPgto = ItemCondicaoPgto.Id;
                if (condicaoPag.vDescCondicao != vDescCondicao || idTabelaPrecoCondicao.GetValueOrDefault() != condicaoPag.idTabelaPreco.GetValueOrDefault())
                {
                    if (vDescCondicao == null)
                        vDescCondicao = 0;

                    idTabelaPrecoCondicao = condicaoPag.idTabelaPreco.GetValueOrDefault();
                    var stValorTabelaPreco = TabelaPrecoRepository.GetTabelaPrecoParaPedidoVenda(idTabelaPrecoCondicao.GetValueOrDefault());

                    foreach (var itens in currentModel.lItens)
                    {
                        if (idTabelaPrecoCondicao.GetValueOrDefault() > 0)
                        {
                            if (itens.idTabelaPreco != idTabelaPrecoCondicao)
                            {
                                itens.idTabelaPreco = idTabelaPrecoCondicao.GetValueOrDefault();
                            }
                            else
                            {
                                itens.idTabelaPreco = idTabelaPrecoCondicao.GetValueOrDefault();
                            }

                            itens.bTabelasCarregadas = false;
                        }
                        else if (condicaoPag.idTabelaPreco.GetValueOrDefault() > 0)
                        {
                            if (itens.idTabelaPreco != condicaoPag.idTabelaPreco.GetValueOrDefault())
                            {
                                itens.idTabelaPreco = condicaoPag.idTabelaPreco.GetValueOrDefault();
                            }
                            else
                            {
                                itens.idTabelaPreco = condicaoPag.idTabelaPreco.GetValueOrDefault();
                            }

                            itens.bTabelasCarregadas = false;
                        }

                        //conversado com o paulo dia 21/03/2019
                        //a regra das condições vão se aplicar no valor origem do produto
                        // então não vai ter os calculos de desconto sobre desconto ou acrescimo
                        // simplismente recalcula de acordo com o valor original
                        double _valorOrigem = itens.vUnitarioVendaSemImposto;

                        if (stValorTabelaPreco != null && stValorTabelaPreco == 2)
                        {
                            var _tblItem = TabelaPrecoRepository.GetTabelaPrecoItemParaPedidoVenda(itens.idTabelaPreco, itens.idProduto);
                            // se não trouxer a tblItem é porque o item não está vinculado a tabela de preço manual, então eu só atribuo o vUnitarioVenda atual para
                            // evitar de dar erro no item
                            if (_tblItem != null)
                            {
                                itens.pIpiVenda = (double)_tblItem.pIpiVenda;
                                itens.pStVenda = (double)_tblItem.pStVenda;
                                _valorOrigem = _tblItem.vVenda.GetValueOrDefault();
                            }
                            else
                            {
                                _valorOrigem = itens.vUnitarioVenda;
                            }
                        }
                        else
                        {
                            var _stvalorTabelaPrecoProduto = TabelaPrecoRepository.GetTabelaPrecoParaPedidoVenda(itens.idTabelaPreco);
                            if (_stvalorTabelaPrecoProduto != 2)
                            {
                                _valorOrigem = TabelaPrecoRepository.GetValorProdutoTabelaPreco(itens.idTabelaPreco, itens.idProduto);
                                itens.pIpiVenda = ProdutoRepository.GetPorcIpi(itens.idProdutoOffLine);
                                itens.pStVenda = ProdutoRepository.GetPorcSt(itens.idProdutoOffLine);
                            }
                            else
                            {
                                var _tblItem = TabelaPrecoRepository.GetTabelaPrecoItemParaPedidoVenda(itens.idTabelaPreco, itens.idProduto);
                                if (_tblItem != null)
                                {
                                    itens.pIpiVenda = (double)_tblItem.pIpiVenda;
                                    itens.pStVenda = (double)_tblItem.pStVenda;
                                    _valorOrigem = _tblItem.vVenda.GetValueOrDefault();
                                }
                                else
                                {
                                    _valorOrigem = itens.vUnitarioVenda;
                                }
                            }
                        }


                        itens.vUnitarioVenda = _valorOrigem = (_valorOrigem + ((itens.pIpiVenda.GetValueOrDefault() / 100) * _valorOrigem) + ((itens.pStVenda.GetValueOrDefault() / 100) * _valorOrigem)).ArredondarValorDecimal(nCasasDecimais: 2);
                        //adiciono o vdesc antigo para somar no pDesconto e calcular qual o valor que deveria ficar;
                        if (vDescCondicao < 0)
                            itens.pDesconto += vDescCondicao.GetValueOrDefault();

                        //somando o desconto no já existente.
                        itens.pDesconto -= condicaoPag.vDescCondicao.GetValueOrDefault();

                        itens.vDesconto = (_valorOrigem * (itens.pDesconto / 100)).ArredondarValorDecimal(nCasasDecimais: 2);
                        itens.vVenda = itens.vUnitarioVendaComImpostos = _valorOrigem - itens.vDesconto;

                        //ocorre quando existe acréscimo na condição
                        if (itens.pDesconto < 0 || itens.vDesconto < 0)
                        {
                            itens.pDesconto = 0;
                            itens.vDesconto = 0;
                        }

                        double qtd = 0;
                        if (itens.ItensGrade != null && itens.ItensGrade.Any())
                        {
                            qtd = itens.ItensGrade.Sum(c => c.vQtdItem);
                            foreach (var item in itens.ItensGrade)
                            {
                                item.vUnitarioVenda = itens.vUnitarioVenda;
                                item.vVenda = item.vUnitarioVendaComImpostos = itens.vVenda;
                                item.vSubTotal = item.vVenda * item.vQtdItem;
                                item.vDesconto = itens.vDesconto;
                                item.pDesconto = itens.pDesconto;
                            }
                        }
                        else
                        {

                            qtd = itens.vQtdItem;
                        }

                        itens.vSubTotal = itens.vVenda * qtd;

                        //checando comissão do item
                        var _dIpi = itens.pIpiVenda != null && itens.stDescontaIpiComissao == false ? itens.pIpiVenda / 100 : 0;
                        var _dSt = itens.pStVenda != null && itens.stDescontaStComissao == false ? itens.pStVenda / 100 : 0;

                        var _vBaseComissao = itens.vVenda / (_dIpi + _dSt + 1);
                        var _pComissao = itens.pComissao / 100;
                        itens.vComissao = ((_vBaseComissao.GetValueOrDefault() * _pComissao) * itens.vQtdItem).ArredondarValorDecimal(nCasasDecimais: 2);

                        // se existir grades, atribuo a comissão recalculada para influenciar na somatória e nas informações do banco
                        if (itens.ItensGrade != null && itens.ItensGrade.Any())
                        {
                            foreach (var item in itens.ItensGrade)
                            {
                                if (item.vQtdItem > 0)
                                {
                                    item.vComissao = ((_vBaseComissao.GetValueOrDefault() * _pComissao) * item.vQtdItem).ArredondarValorDecimal(nCasasDecimais: 2);
                                }
                            }
                        }


                        //metodo criado para atualizar os valores de cada produto na aba ITENS
                        if (itens.ItensGrade != null && itens.ItensGrade.Any())
                        {
                            var qtde = itens.ItensGrade.Sum(c => c.vQtdItem);
                            itens.xDetalheItem =
                                qtde <= 0
                                    ? null
                                    : $"{itens.xQtde} - {itens.xValorSubTotal}";
                        }

                        else
                        {
                            itens.xDetalheItem =
                                itens.vQtdItem <= 0
                                    ? null
                                    : $"{itens.xQtde} - {itens.xValorSubTotal}";
                        }

                        if (itens.pDesconto < 0 || itens.vDesconto < 0)
                        {
                            itens.pDesconto = 0;
                            itens.vDesconto = 0;
                        }
                    }

                    //atribui a variavel para armazenar na viewmodel, atualizo tanto o vDescontoCond qto ItemCondicaoPgto.vDescontoCondicao.                    
                    vDescCondicao = condicaoPag.vDescCondicao;
                    ItemCondicaoPgto.vDescCondicao = condicaoPag.vDescCondicao;
                    idTabelaPrecoCondicao = condicaoPag.idTabelaPreco;
                }

                vSubTotal = currentModel.lItens.Sum(c => c.ItensGrade?.Sum(o => o.vSubTotal) ?? c.vSubTotal);

                var dComplementos = currentModel.vFretePed + currentModel.vSeguroPed + currentModel.vOutrasPed;
                vSubTotal = vSubTotal + dComplementos;
                xDisplayComplementos = dComplementos.ToCurrencyStringPtBr();

                vDescontoTotal = currentModel.lItens.Sum(c => c.ItensGrade?.Sum(o => o.vDesconto * o.vQtdItem) ?? (c.vDesconto * c.vQtdItem));
                xDisplayDesconto = vDescontoTotal.ToCurrencyStringPtBr();

                vTotalComissao = currentModel.lItens.Sum(c => c.ItensGrade?.Sum(o => o.vComissao) ?? c.vComissao);                
                CountItens = currentModel.lItens.Count;
            }
            catch (Exception ex)
            {
                ex.TrakException("PedidoViewModel.AtualizaTotalizadoresPedido");
            }
        }



        public void BuscaFormasPagamentoTelaVendas()
        {
            try
            {
                ItemFormaPgto = CondicaoPagamentoRepository.BuscaFormasPagamentoPorCondicao(string.Empty, ItemCondicaoPgto.Id).OrderBy(t => t.Display).FirstOrDefault();
                currentModel.xFormaPagamento = ItemFormaPgto.Display;
            }
            catch (Exception ex)
            {
                ex.TrakException("PedidoViewModel.BuscaFormasPagamentos");
            }
        }




        public void RateiaDescontoTotalPedido()
        {
            if (currentModel.lItens == null) return;

            IsBusy = true;
            var _itens = PedidoRepository.AplicarRateio(currentModel.lItens, (decimal)vDescontoTotal);

            foreach (var item in currentModel.lItens)
            {




            }



            IsBusy = false;
        }


        /// <summary>
        /// Classe criada para a validação das novas configurações gerais da tb_configuracoes_gerais.
        /// </summary>
        public void ValidaConfiguracoesGerais()
        {
            try
            {
                var _configuracoesGerais = ConfiguracaoGeralRepositorio.GetConfiguracaoEmpresa();
                if (_configuracoesGerais.bUtilizaLimiteMinimoVendas)
                {
                    string _msgAux = string.Empty;
                    double _vLimiteAux = 0;

                    if (_configuracoesGerais.stCadastroLimiteVendasEmpresa != 2 && _configuracoesGerais.stCadastroLimiteVendasEmpresa != 3)
                    {
                        switch (_configuracoesGerais.stCadastroLimiteVendasEmpresa)
                        {
                            case 1:
                                _vLimiteAux = _configuracoesGerais.dValorLimiteMinimo;
                                if (_configuracoesGerais.stCalculoLimiteVendasEmpresa == 1)
                                {
                                    _msgAux = $"Este pedido deve possuir no mínimo {Extensions.ToCurrencyStringSimplesPtBr(_vLimiteAux)} itens";
                                }
                                else
                                {
                                    _msgAux = $"Este pedido deve possuir no mínimo {Extensions.ToCurrencyStringPtBr(_vLimiteAux)}";
                                }
                                break;
                            case 4:
                                _msgAux = $"Atenção, o mínimo de compras está ativo para algumas tabelas de preços!";
                                break;
                            case 5:
                                _vLimiteAux = _configuracoesGerais.dValorLimiteMinimo;
                                _msgAux = $"Cada título financeiro deste pedido deve possuir no mínimo {Extensions.ToCurrencyStringPtBr(_vLimiteAux)}";
                                break;
                        }
                    }


                    currentModel.xMinimoVendas = _msgAux;
                    currentModel.bUsaMinimoVendas = true;
                    currentModel.vLimiteMinimoVenda = _vLimiteAux;
                    currentModel.stCadastroMinimoVenda = _configuracoesGerais.stCadastroLimiteVendasEmpresa;
                    currentModel.stCalculoMinimoVenda = _configuracoesGerais.stCalculoLimiteVendasEmpresa;
                    currentModel.bForcarMinimoVendas = _configuracoesGerais.bForcarMinimoVendas;
                }



                currentModel.xInformacaoContrato = _configuracoesGerais.xInformacaoContrato;
                if (!string.IsNullOrEmpty(currentModel.xInformacaoContrato))
                    ShowBotaoContrato = true;


                currentModel.bBloquearVisualizacaoEstoqueVendedor = _configuracoesGerais.bBloquearVisualizacaoEstoqueVendedor;
                currentModel.bMostraFaixaEscalonada = _configuracoesGerais.bMostraFaixaTabelaEscalonada;

                currentModel.bAplicaMelhoriaEscolherRepresentacaoPdf = ConfiguracaoGeralRepositorio.GetMelhoriaEspecificaRepresentacaoPdf(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa).GetValueOrDefault();
                if (currentModel.bAplicaMelhoriaEscolherRepresentacaoPdf == true)
                    GetRepresentacao();

                RepresentacaoPdfVisibilityCommand.Execute(null);
            }
            catch (Exception ex)
            {
                ex.TrakException("PedidoViewModel.ValidaConfiguracoesGerais");
            }
        }

        public void RotinaValidacaoEstoque()
        {
            if (currentModel.EstoqueInvalido == false) return;
            var dadosEstoque = EstoqueRepository.GetAll(currentModel.idPedidoVendaOffLine ?? 0);
            var lItensEstoque = new List<PedidoVendaItensModel>();
            foreach (var item in currentModel.lItens)
            {
                foreach (var itemGrade in item.ItensGrade)
                {
                    EstoqueInsuficienteModel itemSemEstoque = null;
                    if (item.HasGrade)
                    {
                        itemSemEstoque = dadosEstoque.FirstOrDefault(c => c.idProduto == itemGrade.idProduto &&
                                                                          (c.idGradeCor ?? 0) ==
                                                                          (itemGrade.idGradeCor ?? 0) &&
                                                                          (c.idGradeTamanho ?? 0) ==
                                                                          (itemGrade.idGradeTamanho ?? 0));
                    }
                    else
                        itemSemEstoque = dadosEstoque.FirstOrDefault(c => c.idProduto == itemGrade.idProduto);


                    if (itemSemEstoque == null) continue;
                    itemGrade.dEstoque = itemSemEstoque.dEstoqueAtual;
                    itemGrade.xDescricaoToEstoque = item.xDescricao + " - " + itemGrade.xDescricao;
                    lItensEstoque.Add(itemGrade);
                }
            }

            if (lItensEstoque.Count > 0)
            {
                RegistrosEstoqueAgrupados =
                    new ObservableCollection<Group<string, PedidoVendaItensModel>>(from item in lItensEstoque
                                                                                   orderby item.idProdutoOffLine descending
                                                                                   group item by item.xDisplayEstoque
                        into grupos
                                                                                   select new Group<string, PedidoVendaItensModel>(grupos.Key, grupos));
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

                if (ItemCliente.Id == 0)
                {
                    await App.Messages.ShowAsync("Cliente é um campo obrigatório para o lançamento.");
                    return;
                }
                if (ItemCondicaoPgto.Id == 0)
                {
                    await App.Messages.ShowAsync("Condição de pagamento é um campo obrigatório para o lançamento");
                    return;
                }

                if (currentModel.lItens.Count(c => c.vQtdItem > 0) == 0 && vSubTotal <= 0)
                {
                    await App.Messages.ShowAsync("Ao menos um item é necessário para finalizarmos.");
                    return;
                }
                if (ItemSatus.Id == 1 && (currentModel.xMotivoCancelamento ?? "").Equals(""))
                {
                    await App.Messages.ShowAsync("Informe o motivo de cancelamento.");
                    return;
                }

                if (currentModel.stLancamento == 0 && ItemSatus.Id == 2 && currentModel.stPedidoVenda == 0)
                {
                    if (
                        await
                            App.Messages.ShowConfirmAsync(
                                "O SISTEMA IRÁ GERAR UM PEDIDO BASEADO NESSE ORÇAMENTO, CONFIRMA?", "SIM", "NÃO",
                                "AVISO") == false)
                    {
                        await App.Messages.ShowAsync("OK, MUDE O STATUS DO ORÇAMENTO.");
                        return;
                    }
                    if (await FinanceiroRepository.ValidaLimiteCredito(ItemCliente.Id, currentModel.idPedidoVendaOffLine,
                         vSubTotal, ItemCondicaoPgto.Id) == false)
                    {
                        return;
                    }

                    //if (FinanceiroRepository.PedidoFechadoIncorretamente(ItemCliente.Id, currentModel.idPedidoVendaOffLine,
                    //     vSubTotal, ItemCondicaoPgto.Id))
                    //{
                    //    currentModel.bPedidoFechadoIncorretamente = true;
                    //}
                }

                if (currentModel.bUsaMinimoVendas)
                {
                    var _retornoValidacao = await ConfiguracaoGeralRepositorio.ValidaConfiguracoesGerais(modelPedido: currentModel, ItemCondicaoPagamento: ItemCondicaoPgto);
                    currentModel.bValidadoMinimoVendas = _retornoValidacao.bValidado;
                    if (!_retornoValidacao.bValidado && App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                    {
                        if (!await
                            App.Messages.ShowConfirmAsync($"{_retornoValidacao.xMensagemValidacao}, deseja continuar?",
                                "Sim", "Não"))
                        {
                            return;
                        }
                        else
                        {
                            currentModel.bValidadoMinimoVendas = true;
                        }
                    }
                    else if (!_retornoValidacao.bValidado && !App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador && currentModel.bForcarMinimoVendas)
                    {
                        await App.Messages.ShowAsync($"{_retornoValidacao.xMensagemValidacao}");
                        return;
                    }
                    else if (!_retornoValidacao.bValidado && !App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador && !currentModel.bForcarMinimoVendas)
                    {
                        if (!await
                            App.Messages.ShowConfirmAsync($"{_retornoValidacao.xMensagemValidacao}, ao gerar este pedido ele ficará pendente para aprovação, deseja continuar?", "Sim", "Não"))
                        {
                            return;
                        }
                        else
                        {
                            currentModel.bAguardandoAprovacao = true;
                            currentModel.idStatus = ConfiguracaoGeralRepositorio.ObterIdStatusAberto();
                        }
                    }
                }

                if (isShowRepresentantes)
                {
                    if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers != ItemRepresentante.Id)
                    {
                        if (!await
                            App.Messages.ShowConfirmAsync(
                                "VENDEDOR SELECIONADO NO LANÇAMENTO NÃO É O REPRESENTATE LOGADO, DESEJA CONTINUAR?",
                                "OK, ESTOU CIENTE",
                                "NÃO, IREI CORRIGIR!"))
                        {
                            return;
                        }
                    }

                }
                if (currentModel.stLancamento == 1)
                {
                    if (await
                        FinanceiroRepository.ValidaLimiteCredito(ItemCliente.Id, currentModel.idPedidoVendaOffLine, vSubTotal,
                            ItemCondicaoPgto.Id) == false)
                    {
                        return;
                    }
                }

                EmailPedidoModel email = new EmailPedidoModel();
                //if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                //{
                email = await App.Messages.ShowQuestionMessageEmailPedido();
                //}
                //else
                //{
                //    var paraEmail  = RepresentadaRepository.get
                //}

                if (email != null)
                {

                    var bgerarOrcamento = currentModel.stLancamento == 0 && ItemSatus.Id == 2 && currentModel.stPedidoVenda == 0;

                    if (currentModel.bValidadoMinimoVendas)
                    {
                        currentModel.stPedidoVenda = (byte)ItemSatus.Id;
                        if (!string.IsNullOrEmpty(ItemSatus?.XId))
                            currentModel.idStatus = Convert.ToInt32(ItemSatus.XId);

                    }
                    if (ItemCondicaoPgto?.Id > 0)
                        currentModel.idCondicaoPagamento = ItemCondicaoPgto.Id;
                    if (ItemTransportadora?.Id > 0)
                        currentModel.idTransportadora = ItemTransportadora.Id;
                    if (ItemCliente?.Id > 0)
                    {
                        currentModel.idClientesOffLine = ItemCliente.Id;
                    }
                    if (ItemRedespacho?.Id > 0)
                        currentModel.idRedespacho = ItemRedespacho.Id;


                    currentModel.xFormaPagamento = ItemFormaPgto.Display ?? "";

                    currentModel.idRepresentantePedido = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador ? ItemRepresentante.Id : App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers;

                    currentModel.stEnviadoCliente = email.bEnviaCliente;
                    currentModel.stEnviadoRepresentacao = email.bEnviaRepresentacoes;

                    if (currentModel.bAplicaMelhoriaEscolherRepresentacaoPdf == true)
                        currentModel.idRepresentadaPdf = representada.Id;
                    
                    currentModel.dEmissao = currentModel.dEmissao;
                    currentModel.xEndereco = ItemEndereco.Detail;

                    PedidoRepository.SavePedidoVenda(currentModel);

                    //quando vem pelo gerar pedido do cliente, o pagelistarpedidos não foi invocado
                    if (PageListarPedidos.ViewModelStatic == null)
                    {
                        UtilNavidate.PushAsync(new PageListarPedidos());
                    }
                    else
                    {
                        PageListarPedidos.ViewModelStatic.canExecuteInicial = true;
                        UtilNavidate.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("PedidoViewModel.Save", ex.Message, true);
            }
        }
        private async void CancelPress()
        {
            if (currentModel.idPedidoVenda == null)
            {
                if (await UtilMessages.QuestionToBackAsync())
                {

                    //PedidoVendaModel objPedido = currentModel;
                    //Cancelar(currentModel);

                    // ProdutoRepository.AtualizarEstoqueProduto(idEmpresa: objPedido.,
                    //idProduto: item.idProduto, idLocalEstoque: item.idLocalEstoque, vQtdItem: item.vQtdItem);

                    UtilNavidate.PopAsync();
                }
            }
            else
            {
                UtilNavidate.PopAsync();
            }
        }

        public static void Cancelar(PedidoVendaModel objPedido)
        {
            try
            {

                if (objPedido.idPedidoDisplay != null && objPedido.idPedidoDisplay <= 0)
                    objPedido.idPedidoDisplay = null;

                double vTotalPedido = 0;
                double vDescontoTotal = 0;
                foreach (var item in objPedido.lItens)
                {
                    if (item.ItensGrade != null && item.ItensGrade.Any())
                    {
                        var _qtdadeTotal = item.ItensGrade.Where(itemgrade => itemgrade.vQtdItem > 0).Sum(itemgrade => itemgrade.vQtdItem);
                        var _itemAux = item.ItensGrade.Where(itemgrade => itemgrade.vQtdItem > 0).FirstOrDefault();
                        double _descontoUnitario = 0;

                        if (_itemAux != null)
                            _descontoUnitario = _itemAux.vDesconto;

                        vDescontoTotal += _qtdadeTotal * _descontoUnitario;
                        vTotalPedido += item.ItensGrade.Where(itemgrade => itemgrade.vQtdItem > 0).Sum(itemgrade => itemgrade.vSubTotal);
                    }
                    else
                    {
                        vDescontoTotal += (item.vDesconto * item.vQtdItem);
                        vTotalPedido += item.vSubTotal;
                    }
                }

                objPedido.stValidaEnvioParaRepresentada = objPedido.stEnviadoRepresentacao;

                objPedido.vTotalProduto = vTotalPedido;

                if (objPedido.idPedidoVenda == null || objPedido.idPedidoVenda == 0)
                    vTotalPedido = vTotalPedido + objPedido.vFretePed + objPedido.vSeguroPed + objPedido.vOutrasPed;

                objPedido.VTotal = vTotalPedido;
                objPedido.vDescontoPed = vDescontoTotal;
                objPedido.idEmpresa = (int)App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                objPedido.idAspnetUsers = App.CurrentAspnetUserModel.Id;

                if (objPedido.idPedidoVenda == null)
                    objPedido.dtUltimaAlteracao = DateTime.UtcNow.ToDateTimeSync();

                //if (objPedido.stLancamento == 0)
                //    objPedido.dtValidadeOrcamento = null;

                if (objPedido.stLancamento == 1)
                    objPedido.dtValidadeOrcamento = null;

                if (objPedido.idPedidoVendaOffLine == null)
                {
                    if (objPedido.idAspnetUsers == null)
                        objPedido.idAspnetUsers = App.CurrentAspnetUserModel.Id;
                    App.Data.Connection.Insert(objPedido);
                }
                else
                    App.Data.Connection.Update(objPedido);

                foreach (var itemRemovido in objPedido.ItensRemovidos)
                {
                    App.Data.Connection.Delete(itemRemovido);
                }


                foreach (var item in objPedido.lItens)
                {

                    var anotacao = ProdutoRepository.GetAnotacaoProduto(item.idProdutoOffLine);

                    if (!string.IsNullOrEmpty(anotacao))
                    {
                        if (!item.xInfAdicionais.ToUpper().Contains(anotacao.ToUpper()))
                        {
                            item.xInfAdicionais += string.IsNullOrEmpty(item.xInfAdicionais)
                                ? anotacao
                                : Environment.NewLine + anotacao;
                        }
                    }

                    if (item.HasGrade || item.ItensGrade != null)
                    {
                        if (item.idItemAgrupamento == null)
                            item.idItemAgrupamento = objPedido.GetNextValidAgrupamento();

                        foreach (var itemGrade in item.ItensGrade)
                        {
                            itemGrade.xInfAdicionais = item.xInfAdicionais;
                            itemGrade.idItemAgrupamento = item.idItemAgrupamento;
                            atualizarEstoque(objPedido, itemGrade);
                        }
                    }
                    else
                    {
                        if (item.idItemAgrupamento == null)
                            item.idItemAgrupamento = objPedido.GetNextValidAgrupamento();
                        atualizarEstoque(objPedido, item);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
            }

        }

        private static void atualizarEstoque(PedidoVendaModel objPedido, PedidoVendaItensModel item)
        {
            try
            {
                item.idEmpresa = objPedido.idEmpresa;

                if (item.vQtdEstoque != null && item.vQtdItem > 0)
                {
                    EstoqueModel _retornoEstoqueProdutoMobile = new EstoqueModel();

                    if (item.idGradeCor != null || item.idGradeTamanho != null)
                    {
                        _retornoEstoqueProdutoMobile = ProdutoRepository.ObterRegistroEstoqueComGradeProduto(item.idEmpresa, item.idProduto ?? 0, item.idGradeCor, item.idGradeTamanho);
                    }
                    else
                    {
                        _retornoEstoqueProdutoMobile = ProdutoRepository.ObterRegistroEstoqueProduto(item.idEmpresa, item.idProduto ?? 0);
                    }

                    if (_retornoEstoqueProdutoMobile == null)
                    {
                        _retornoEstoqueProdutoMobile = new EstoqueModel
                        {
                            idProduto = item.idProduto ?? 0,
                            idEmpresa = item.idEmpresa,
                            idGradeCor = item.idGradeCor,
                            idGradeTamanho = item.idGradeTamanho,
                            vEstoque = 0
                        };
                    }

                    _retornoEstoqueProdutoMobile.vEstoque += item.vQtdItem;
                    App.Data.Connection.Update(_retornoEstoqueProdutoMobile);
                }


            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
            }
        }

    }
}
