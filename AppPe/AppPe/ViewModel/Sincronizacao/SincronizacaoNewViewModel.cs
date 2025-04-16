using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hlp.PedidoEletronico.Domain.Business.Bo;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.Model.Empresa;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;
using Xamarin.HLP.Mobile.AppPE.Model.Financeiro;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.PagSeguro;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Agenda;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Integracao;
using Xamarin.HLP.Mobile.AppPE.Model.Sincronizacao;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.View.MainPage;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.View.Produto;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao
{
    public class SincronizacaoNewViewModel : SearchCommom
    {
        public ICommand AcaoAfterSyncCommand { get; set; } = null;

        private SincronizacaoNewModel _currentModel;

        public SincronizacaoNewModel currentModel
        {
            get { return _currentModel; }
            set
            {
                _currentModel = value;
                NotifyPropertyChanged();
            }
        }


        public PlanosBo currentPlano { get; set; }

        public static bool bFalhaConexao { get; set; }

        public bool bFalhaTotalDeConexao { get; set; }

        private static bool _ocorreuErro;

        public static bool ocorreuErro
        {
            get { return _ocorreuErro; }
            set { _ocorreuErro = value; }
        }

        public bool bForcarSyncInit { get; set; }

        private DateTime _lastDateSync = DateTime.Today.AddYears(-50);

        public DateTime lastDateSync
        {
            get { return _lastDateSync; }
            set
            {
                _lastDateSync = value;
                NotifyPropertyChanged();
            }
        }

        private static string _xMensagemErro = "";

        public static string xMensagemErro
        {
            get { return _xMensagemErro; }
            set { _xMensagemErro = value; }
        }

        private DateTime _lastDateServerSync = DateTime.Today.AddYears(-50);

        public DateTime lastDateServerSync
        {
            get { return _lastDateServerSync; }
            set
            {
                _lastDateServerSync = value;
                NotifyPropertyChanged();
            }
        }



        private DateTime _lastDateServerSyncCliente = DateTime.Today.AddYears(-50);

        public DateTime lastDateServerSyncCliente
        {
            get { return _lastDateServerSync; }
            set
            {
                _lastDateServerSync = value;
                NotifyPropertyChanged();
            }
        }


        public SincronizacaoNewViewModel()
        {
            currentModel = new SincronizacaoNewModel();
            SituacaoInicial();
            if (Device.RuntimePlatform == Device.Android)
                CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        private async void Current_ConnectivityChanged(object sender,
            Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (await App.IsConected() == false)
            {
                bFalhaConexao = true;
                AnaliseFinalSincronizacao();
                bFalhaTotalDeConexao = true;
            }
        }


        #region METHODS

        public async void SyncAssnaturaPedido()
        {
            if (!IsBusy)
            {
                if (await App.IsConected())
                {
                    IsBusy = true;
                    ocorreuErro = bFalhaConexao = false;
                    currentModel.Display = "iniciando...";
                    currentModel.LAlertaSincronizacao = new List<AlertaSincronizacao>();
                    currentModel.Display = "UPLOAD PEDIDOS";
                    var lPedidos = PedidoRepository.GetAllPedidosToSync();
                    currentModel.iCount = lPedidos.Count;
                    foreach (var pedido in lPedidos)
                    {
                        currentModel.iCount--;
                        if (pedido == null) continue;
                        var objPedidoSync = await UtilHttp.PostRegistroToCloud(pedido, "AssPedido");
                        if (objPedidoSync.resulStruct.stRetorno == RetornoSalvar.Excecao)
                        {
                            AnaliseFinalSincronizacao("Erro ao sincronizar");
                            return;
                        }
                    }
                    AnaliseFinalSincronizacao();
                }
            }
        }

        public async void InitSyncComplete()
        {
            try
            {
                if (!IsBusy)
                    if (await App.IsConected())
                    {
                        IsBusy = true;
                        ocorreuErro = bFalhaConexao = false;
                        currentModel.Display = "iniciando...";
                        currentModel.LAlertaSincronizacao = new List<AlertaSincronizacao>();

                        currentModel.Display = "verificando seu plano...";
                        var acesso = await
                            UtilHttp.AcessoPermitido(
                                App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.idAspnetUser);

                        if (acesso == null)
                        {
                            AnaliseFinalSincronizacao("Houve uma falha de conexão na validação do seu plano atual, verifique sua conexão e tente novamente!");
                            return;
                        }

                        //atribuição do plano atual do usuário
                        switch (acesso.idProdutoPlanoAtual)
                        {
                            case 0:
                                App.planoAtual = Planos.nenhum;
                                break;
                            case 1:
                                App.planoAtual = Planos.plstarter;
                                break;
                            case 2:
                                App.planoAtual = Planos.plsbus;
                                break;
                            case 3:
                                App.planoAtual = Planos.plbus;
                                break;
                            case 4:
                                App.planoAtual = Planos.plprem;
                                break;
                            case 5:
                                App.planoAtual = Planos.plfree;
                                break;
                            case 6:
                                App.planoAtual = Planos.pldeg;
                                break;
                            default:
                                App.planoAtual = Planos.plfree;
                                break;
                        }

                        currentPlano = new PlanosBo((Planos)acesso.idProdutoPlanoAtual);

                        if (acesso.stAcessoPermitido == TipoAcessoPermitido.ok || App.ForcarAtualizacao)
                        {
                            if (acesso.idProdutoPlanoAtual == 5 && App.ForcarAtualizacao == false)
                            {
                                currentModel.LAlertaSincronizacao.Add(new AlertaSincronizacao
                                {
                                    Table = "PLANOGRÁTIS",
                                    Display = "Sincronização não disponível.",
                                    Detail = "Plano grátis, faça upgrade"
                                });
                                AnaliseFinalSincronizacao();
                            }
                            else
                            {
                                //Inicio o.s 34140 - Linha adicionada temporariamente pois o produção não tinha a url da api que o permitesincronizacao usa.
                                //KeyValuePair<bool, string> _bPermiteSincronizacao = new KeyValuePair<bool, string>(true, "Representante Ativo");
                                //Fim o.s 34140

                                var _bPermiteSincronizacao = await UtilHttp.PermiteSincronizacao(idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                    idRepresentante: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0);

                                if (_bPermiteSincronizacao.Key == true)
                                {   //Fim ajuste;
                                    if (bForcarSyncInit)
                                        lastDateServerSync = DateTime.Today.AddYears(-50);

                                    if (lastDateServerSync.Year > 2000)
                                    {
                                        // EXCLUSÕES UPLOAD
                                        await InitSyncExclusaoUpload();
                                        // EXCLUSÕES DOWNLOAD
                                        await InitExclusoesDownload(lastDateServerSync);
                                        // UPLOAD CADASTROS
                                        await UploadAll();
                                    }
                                    if (bForcarSyncInit)
                                    {
                                        // EXCLUSÕES DOWNLOAD
                                        await InitExclusoesDownload(lastDateServerSync);
                                        //var _retornoExclusoes = EnvironmentRepository.ExcluirTodosRegistros();
                                    }
                                    // DOWNLOAD TABELAS
                                    await InitSincronizacaoDownload();
                                }
                                else
                                {
                                    currentModel.LAlertaSincronizacao.Add(new AlertaSincronizacao
                                    {
                                        Table = "REPRESENTANTEINATIVO",
                                        Display = "Vendedor inativo.",
                                        Detail = "Vendedor inativo para a empresa corrente. Entre em contato com seu administrador!"
                                    });
                                    AnaliseFinalSincronizacao();
                                }
                            }
                        }
                        else
                        {
                            currentModel.LAlertaSincronizacao.Add(new AlertaSincronizacao
                            {
                                Table = "PLANO",
                                Display =
                                    $"Sincronização não disponível - {acesso.stAcessoPermitido} - {acesso.idProdutoPlanoAtual}",
                                Detail = "Algo deu errado ao buscar seu plano."
                            });
                            AnaliseFinalSincronizacao();
                        }
                    }
                    else
                        AnaliseFinalSincronizacao("A internet parece estar indisponível !");
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                ocorreuErro = true;
                AnaliseFinalSincronizacao(ex.Message);
            }
            catch (Exception ex)
            {
                ocorreuErro = true;
                AnaliseFinalSincronizacao(ex.Message);
            }
        }

        #region DOWNLOAD        

        private async Task InitSincronizacaoDownload()
        {
            try
            {
                var dtServer =
                    await UtilHttp.GetDateServer();
                if (dtServer != null)
                {
                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.ultimaSyncServerDateTime =
                        ((DateTime)dtServer).ToDateTimeSync();
                }

                await SincronizacaoDownload<EmpresaModel>();



                if (App.planoAtual != Planos.plfree
                    && App.planoAtual != Planos.nenhum
                    && App.planoAtual != Planos.plstarter)
                    await SincronizacaoDownload<EstoqueModel>();

                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<ExtensaoEmpresaModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<OmieConfiguracaoGeralModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownloadPaginado<ConfiguracaoGeralModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownloadPaginado<ConfiguracaoEspecificaModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<EmpresaAspnetUsersModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<StatusModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<RepresentadaModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<RepresentadaAspnetUsersModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<PermissoesRepresentantesModel>();
                //if (!ocorreuErro && !bFalhaConexao)
                //    await SincronizacaoDownload<EquipeRepresentantesModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<RamoAtividadeModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<CategoriaProdutoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<CondicaoPagamentoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<ClientesCondicoesPagamentoModel>();
                //if (!ocorreuErro && !bFalhaConexao)
                //    await SincronizacaoDownloadPaginado<FormaPagamentoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<GradeCorModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<GradeTamanhoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<GradesModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TabelaPrecoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TabelaPrecoItemModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TabelaPrecoRepresentantesModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TabelaPrecoRepresentacoesModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TabelaPrecoClientesModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TabelaEscalonadaModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TabelaEscalonadaFaixaComissaoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TabelaEscalonadaRepresentanteModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<RecebimentoTitulosModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<RecebimentoTitulosMovimentacaoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<UnidadeMedidaModel>();

                //comentada pois a api de imagem traz apenas as imagens de produto
                //agora o mesmo é utilizado no processo de baixar produto onde traz as imagens vinculadas. issue #432
                //await SincronizacaoDownload<ImagemModel>();

                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<ProdutoModel>();


                //if (!ocorreuErro && !bFalhaConexao)
                //    await SincronizacaoDownloadPaginado<GradeVariacaoProdutoModel>();
                //if (!ocorreuErro && !bFalhaConexao)
                //    await SincronizacaoDownloadPaginado<GradeVariacaoProdutoComposicaoModel>();

                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<tb_produto_codigocliente>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<ClientesModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<ContatoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<EnderecoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TransportadorasModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<CategoriaProdutoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TabelaPrecoClienteUfModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<TabelaPrecoClienteRamoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownload<ClienteRamosAtividade>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownloadPedido();
                if (!ocorreuErro && !bFalhaConexao)
                    await AnaliseDeRepresentantes();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownloadTipoAtividadesAgenda();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownloadAgenda();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownloadLocalEstoque();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownloadPaginado<JornadaModel>();

                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownloadGrades<GradesComposicaoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownloadGrades<GradeVariacaoProdutoModel>();
                if (!ocorreuErro && !bFalhaConexao)
                    await SincronizacaoDownloadGrades<GradeVariacaoProdutoComposicaoModel>();

                if (bFalhaConexao)
                    AnaliseFinalSincronizacao("Ocorreu um erro de conexão com a internet durante a sincronização, tente novamente.");
                else if (ocorreuErro)
                    AnaliseFinalSincronizacao("Houve uma queda na conexão da internet durante a sincronização, tente novamente.");
                else
                    AnaliseFinalSincronizacao();
            }
            catch (Exception ex)
            {
                AnaliseFinalSincronizacao(ex.Message);
            }
        }


        private async Task SincronizacaoDownloadPaginado<T>(bool bForceInicial = false) where T : class
        {
            IntegracaoRepository integ = new IntegracaoRepository();
            int idEmp = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
            var xTableName = TableMobile.GetTableNameByModel<T>();
            int _paginaSinc = 1;

            if (xTableName == "TB_RECEBIMENTOTITULOS_MOVIMENTACOES")
            {
                currentModel.Display = $"lote movimentacoes";
            }
            else
            {
                currentModel.Display = $"lote " + xTableName;
            }

            try
            {
                while (true)
                {
                    if (!ocorreuErro || !bFalhaConexao)
                    {
                        var lsync = new List<T>();
                        if (xTableName == TableMobile.GetTableNameByModel<EstoqueModel>())
                        {
                            var dateserver = lastDateServerSync;
                            lsync = await
                                UtilHttp.GetListRegistroPaginadoSync<T>(
                                        Page: _paginaSinc,
                                        param1: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                        param2: dateserver,
                                        param3: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers);

                            if (lsync?.Count() > 0)
                            {
                                foreach (var p in lsync)
                                {
                                    var registro = p as EstoqueModel;
                                    EstoqueRepository.RemoveAllEstoqueSincronizacao(registro.idProduto);
                                }
                            }

                            if (lsync?.Count() > 0 && !bFalhaConexao)
                            {
                                await SavePrivate(lsync, xTableName);
                                //await SavePrivatePaginado(lsync, xTableName, _paginaSinc); 
                            }

                            break;
                        }
                        else if (xTableName == TableMobile.GetTableNameByModel<ConfiguracaoEspecificaModel>()
                            || xTableName == TableMobile.GetTableNameByModel<ConfiguracaoGeralModel>()
                            || xTableName == TableMobile.GetTableNameByModel<FormaPagamentoModel>())
                        {
                            lsync = await
                                UtilHttp.GetListRegistroPaginadoSync<T>(
                                        Page: _paginaSinc,
                                        param1: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                        param2: lastDateServerSync,
                                        param3: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers);


                            if (lsync?.Count() > 0 && !bFalhaConexao)
                            {
                                await SavePrivatePaginado(lsync, xTableName, _paginaSinc);

                                _paginaSinc++;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else if (xTableName == TableMobile.GetTableNameByModel<PedidoVendaModel>())
                        {
                            var xPrimaryKeyName = TableMobile.GetPrimaryKeyNameByModel<PedidoVendaModel>();
                            var lRegistros = await UtilHttp.GetPedidosVendas<PedidoVendaModel>(
                                              idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                              page: _paginaSinc,
                                              dtUltimaAlteracao: lastDateServerSync,
                                              idAspNetUsers: App.CurrentAspnetUserModel.Id);

                            if (lRegistros?.Count() > 0 && !bFalhaConexao)
                            {
                                foreach (var registro in lRegistros)
                                {
                                    if (registro != null)
                                        await SaveSincronizacao(registro, xPrimaryKeyName, xTableName);
                                }

                                _paginaSinc++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            var dateserver = lastDateServerSync;
                            if (bForceInicial)
                                dateserver = DateTime.Today.AddYears(-50);


                            lsync = await
                                UtilHttp.GetListRegistroPaginadoSync<T>(
                                        Page: _paginaSinc,
                                        param1: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                        param2: dateserver,
                                        param3: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers);

                            if (lsync?.Count() > 0 && !bFalhaConexao)
                            {
                                await SavePrivatePaginado(lsync, xTableName, _paginaSinc);

                                _paginaSinc++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                integ.AtualizarDataIntegracao(xTableName, idEmp, bFalhaConexao, ocorreuErro, xMensagemErro);
            }
            catch (Exception ex)
            {
                if (xTableName.ToUpper().Contains("TB_RECEBIMENTO"))
                {
                    await FinanceiroRepository.RemoverTodosRecebimentos<T>();
                    await SincronizacaoDownloadPaginado<T>(true);
                }
                else
                {
                    throw new Exception($"{xTableName} - {ex.Message}");
                }
            }



            ocorreuErro = currentModel.LAlertaSincronizacao.Count(c => c.bErro) > 0;
        }


        private async Task SincronizacaoDownloadPedido()
        {
            IntegracaoRepository integ = new IntegracaoRepository();
            var listaID = new List<PedidosToSyncModel>();
            var xapi = TableMobile.GetApiRegistroByModel<PedidoVendaModel>();
            var xTableName = TableMobile.GetTableNameByModel<PedidoVendaModel>();
            currentModel.Display = xTableName;

            int idEmp = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
            var _ultimaDataSinc = integ.getDataUltimaIntegracao(idEmp, xTableName);

            if (_ultimaDataSinc == null || _ultimaDataSinc.Year < 2000 || bForcarSyncInit)
                _ultimaDataSinc = lastDateServerSync;
            else
                _ultimaDataSinc = _ultimaDataSinc.AddMinutes(-10);


            listaID = await
                UtilHttp.GetRegistroIDSync(
                    param1: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                    param2: _ultimaDataSinc,
                    ApiController: $"{xapi}/Get",
                    param3: App.CurrentAspnetUserModel.Id);

            int _pagina = 1;
            if (listaID.Count > 0 && !bFalhaConexao)
            {
                currentModel.iCount = listaID.Count();
                while (true)
                {
                    var xPrimaryKeyName = TableMobile.GetPrimaryKeyNameByModel<PedidoVendaModel>();
                    var lRegistros =
                              await
                                  UtilHttp.GetPedidosVendas<PedidoVendaModel>(
                                      idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                      page: _pagina,
                                      dtUltimaAlteracao: _ultimaDataSinc,
                                      idAspNetUsers: App.CurrentAspnetUserModel.Id);

                    if (lRegistros?.Count() > 0 && !bFalhaConexao)
                    {
                        foreach (var registro in lRegistros)
                        {
                            if (registro != null)
                                await SaveSincronizacao(registro, xPrimaryKeyName, xTableName);

                            currentModel.iCount--;
                        }

                        _pagina++;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            integ.AtualizarDataIntegracao(xTableName, idEmp, bFalhaConexao, ocorreuErro, xMensagemErro);
            ocorreuErro = currentModel.LAlertaSincronizacao.Count(c => c.bErro) > 0;
        }


        private async Task SincronizacaoDownloadLocalEstoque()
        {
            int listaID = 0;
            var xTableName = TableMobile.GetTableNameByModel<LocalEstoqueModel>();
            currentModel.Display = xTableName;

            IntegracaoRepository integ = new IntegracaoRepository();
            int idEmp = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
            var _ultimaDataSinc = integ.getDataUltimaIntegracao(idEmp, xTableName);

            if (_ultimaDataSinc == null || _ultimaDataSinc.Year < 2000 || bForcarSyncInit)
                _ultimaDataSinc = lastDateServerSync;
            else
                _ultimaDataSinc = _ultimaDataSinc.AddMinutes(-10);

            listaID = await
                UtilHttp.GetQuantidadeTotalLocaisEstoque(
                    idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                    dtUltimaAlteracao: _ultimaDataSinc,
                    stTipoBuscar: 0);

            int _pagina = 1;
            if (listaID > 0 && !bFalhaConexao)
            {
                currentModel.iCount = listaID;
                while (true)
                {
                    var xPrimaryKeyName = TableMobile.GetPrimaryKeyNameByModel<LocalEstoqueModel>();
                    var lRegistros =
                              await
                                  UtilHttp.GetLocaisEstoque<LocalEstoqueModel>(
                                      idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                      page: _pagina,
                                      dtUltimaAlteracao: _ultimaDataSinc,
                                      stTipoBuscar: 0);

                    if (lRegistros?.Count() > 0 && !bFalhaConexao)
                    {
                        foreach (var registro in lRegistros)
                        {
                            if (registro != null)
                                await SaveSincronizacao(registro, xPrimaryKeyName, xTableName);

                            currentModel.iCount--;
                        }

                        _pagina++;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            integ.AtualizarDataIntegracao(xTableName, idEmp, bFalhaConexao, ocorreuErro, xMensagemErro);
            ocorreuErro = currentModel.LAlertaSincronizacao.Count(c => c.bErro) > 0;
        }


        private async Task SincronizacaoDownloadAgenda()
        {
            var xapi = TableMobile.GetApiRegistroByModel<AtividadeAgendaModel>();
            var xTableName = TableMobile.GetTableNameByModel<AtividadeAgendaModel>();

            IntegracaoRepository integ = new IntegracaoRepository();
            int idEmp = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
            var _ultimaDataSinc = integ.getDataUltimaIntegracao(idEmp, xTableName);

            if (_ultimaDataSinc == null || _ultimaDataSinc.Year < 2000 || bForcarSyncInit)
                _ultimaDataSinc = lastDateServerSync;
            else
                _ultimaDataSinc = _ultimaDataSinc.AddMinutes(-10);

            currentModel.Display = xTableName;
            currentModel.iCount = await
                                  UtilHttp.GetCountAtividades(
                                      idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                      dtUltimaAlteracao: _ultimaDataSinc,
                                      idAspNetUsers: App.CurrentAspnetUserModel.Id);


            int _pagina = 1;
            if (currentModel.iCount > 0 && !bFalhaConexao)
            {
                while (true)
                {
                    var xPrimaryKeyName = TableMobile.GetPrimaryKeyNameByModel<AtividadeAgendaModel>();
                    var lRegistros =
                              await
                                  UtilHttp.GetAtividadesAgenda<AtividadeAgendaModel>(
                                      idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                      page: _pagina,
                                      dtUltimaAlteracao: _ultimaDataSinc,
                                      idAspNetUsers: App.CurrentAspnetUserModel.Id);

                    if (lRegistros?.Count() > 0 && !bFalhaConexao)
                    {
                        foreach (var registro in lRegistros)
                        {
                            if (registro != null)
                                await SaveSincronizacao(registro, xPrimaryKeyName, xTableName);

                            currentModel.iCount--;
                        }

                        _pagina++;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            integ.AtualizarDataIntegracao(xTableName, idEmp, bFalhaConexao, ocorreuErro, xMensagemErro);
            ocorreuErro = currentModel.LAlertaSincronizacao.Count(c => c.bErro) > 0;
        }


        private async Task SincronizacaoDownloadTipoAtividadesAgenda()
        {
            var xapi = TableMobile.GetApiRegistroByModel<TipoAtividadeAgendaModel>();
            var xTableName = TableMobile.GetTableNameByModel<TipoAtividadeAgendaModel>();
            currentModel.Display = xTableName;

            IntegracaoRepository integ = new IntegracaoRepository();
            int idEmp = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
            var _ultimaDataSinc = integ.getDataUltimaIntegracao(idEmp, xTableName);

            if (_ultimaDataSinc == null || _ultimaDataSinc.Year < 2000 || bForcarSyncInit)
                _ultimaDataSinc = lastDateServerSync;
            else
                _ultimaDataSinc = _ultimaDataSinc.AddMinutes(-10);


            currentModel.iCount = await
                                  UtilHttp.GetCountTipoAtividades(
                                      idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                      dtUltimaAlteracao: _ultimaDataSinc);


            int _pagina = 1;
            if (currentModel.iCount > 0 && !bFalhaConexao)
            {
                while (true)
                {
                    var xPrimaryKeyName = TableMobile.GetPrimaryKeyNameByModel<TipoAtividadeAgendaModel>();
                    var lRegistros =
                              await
                                  UtilHttp.GetTipoAtividadesAgenda<TipoAtividadeAgendaModel>(
                                      idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                      page: _pagina,
                                      dtUltimaAlteracao: _ultimaDataSinc);

                    if (lRegistros?.Count() > 0 && !bFalhaConexao)
                    {
                        foreach (var registro in lRegistros)
                        {
                            if (registro != null)
                                await SaveSincronizacao(registro, xPrimaryKeyName, xTableName);

                            currentModel.iCount--;
                        }

                        _pagina++;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            integ.AtualizarDataIntegracao(xTableName, idEmp, bFalhaConexao, ocorreuErro, xMensagemErro);
            ocorreuErro = currentModel.LAlertaSincronizacao.Count(c => c.bErro) > 0;
        }


        private async Task<bool> AnaliseDeRepresentantes()
        {
            try
            {
                if (lRepresentantesToAnalise.Count() == 0)
                    lRepresentantesToAnalise = App.CurrentAspnetUserModel.lEpresaAspnetUsersModel;

                currentModel.Display = "Analise de usuarios...";
                foreach (var representante in lRepresentantesToAnalise)
                {
                    var user = await UtilHttp.GetRegistroSync<AspNetUsersModel>(representante.idEmpresa_aspnetUsers);
                    if (user != null)
                    {
                        var xQuery = $@"SELECT COUNT(*) FROM {TableMobile.AspNetUsers} WHERE {"Id"} = '{user.Id}' ";
                        try
                        {
                            var icount = App.Data.Connection.ExecuteScalar<int>(xQuery);
                            if (icount > 0)
                                App.Data.Connection.Update(user);
                            else
                                App.Data.Connection.Insert(user);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    var empresaLocal = EmpresaAspnetUsersRepository.GetEmpresaAspnetUsers(representante.idEmpresa_aspnetUsers ?? 0);

                    if (empresaLocal != null)
                    {
                        //verifico se o usuario era comum e virou adm
                        if (empresaLocal.stAdministrador == false && representante.stAdministrador)
                            empresaLocal.UltimaSyncDateTime = DateTime.MinValue;
                        else
                            empresaLocal.UltimaSyncDateTime = empresaLocal.UltimaSyncDateTime;

                        empresaLocal.stAtivo = representante.stAtivo;
                        empresaLocal.stAdministrador = representante.stAdministrador;
                        empresaLocal.stAcessoTodosClientes = representante.stAcessoTodosClientes;
                        empresaLocal.xApelido = representante.xApelido;
                        empresaLocal.xEmail = representante.xEmail;
                        empresaLocal.xNome = representante.xNome;
                        empresaLocal.xMeuID = representante.xMeuID;
                        empresaLocal.vMetaCorrente = representante.vMetaCorrente;
                        empresaLocal.imUsuario = representante.imUsuario;
                        empresaLocal.idJornada = representante.idJornada;

                        App.Data.Connection.Update(empresaLocal);

                        if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa == empresaLocal.idEmpresa &&
                            App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xEmail.ToUpper().Equals(empresaLocal.xEmail.ToUpper()))
                        {
                            //App.CurrentAspnetUserModel.lEpresaAspnetUsersModel = lRepresentantesToAnalise;           
                            App.EnvironmentPE.vMetaCorrente = empresaLocal.vMetaCorrente;
                            //atualizando a jornada do usuário local logado
                            App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idJornada = empresaLocal.idJornada;
                            App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAcessoTodosClientes = empresaLocal.stAcessoTodosClientes;

                            LoginRepository.UpdateUser();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return false;
            }
            return true;
        }

        private List<EmpresaAspnetUsersModel> lRepresentantesToAnalise { get; set; }


        private async Task SincronizacaoDownload<T>(bool bForceInicial = false) where T : class
        {
            IntegracaoRepository integ = new IntegracaoRepository();

            var xTableName = TableMobile.GetTableNameByModel<T>();

            int idEmp = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

            // issue 2705 ajustando a data de sincronização IGOR BIANCHINI SPAGNOL
            var _ultimaDataSinc = integ.getDataUltimaIntegracao(idEmp, xTableName);

            if (_ultimaDataSinc == null || _ultimaDataSinc.Year < 2000 || bForcarSyncInit)
                _ultimaDataSinc = DateTime.Today.AddYears(-50);
            else
                _ultimaDataSinc = _ultimaDataSinc.AddMinutes(-5);

            try
            {

                if (!ocorreuErro && !bFalhaConexao)
                {
                    currentModel.Display = xTableName;

                    var lsync = new List<T>();
                    if (xTableName == TableMobile.GetTableNameByModel<RepresentadaAspnetUsersModel>())
                    {
                        //primeiro busca tudo
                        lsync = await UtilHttp.GetListRegistroSync<T>(param1: idEmp,
                                                          param2: _ultimaDataSinc,
                                                          param3:
                                                          xTableName == TableMobile.TB_PEDIDOVENDA ? App.CurrentAspnetUserModel.Id : null);

                        List<RepresentadaAspnetUsersModel> _listRep = lsync as List<RepresentadaAspnetUsersModel>;
                        var _representantesLinkados = EmpresaAspnetUsersRepository.GetListaRepsLinkados(idEmp);
                        //aqui estou buscando as representadas que os vendedores sincronizados pra essa conta possuem acesso.
                        foreach (var empresaAspnetUsersModel in _representantesLinkados)
                        {
                            //RepresentadaRepository.DeleteAllByRepresentante(idRepresentanteAspNetUsers);

                            var _listRepresentadasParaSalvar = _listRep.Where(r => r.idEmpresa_aspnetUsers == empresaAspnetUsersModel).ToList();
                            await SavePrivate(_listRepresentadasParaSalvar, xTableName);
                        }
                    }
                    else if (xTableName == TableMobile.GetTableNameByModel<EstoqueModel>())
                    {
                        lsync = await
                            UtilHttp.GetListRegistroSync<T>(
                                    param1: idEmp,
                                    param2: _ultimaDataSinc,
                                    param3:
                                    xTableName == TableMobile.TB_PEDIDOVENDA ? App.CurrentAspnetUserModel.Id : null);

                        if (lsync?.Count() > 0)
                        {
                            foreach (var p in lsync)
                            {
                                var registro = p as EstoqueModel;
                                EstoqueRepository.RemoveAllEstoqueSincronizacao(registro.idProduto);
                            }
                        }

                        await SavePrivate(lsync, xTableName);
                    }
                    else if (xTableName == TableMobile.GetTableNameByModel<EmpresaAspnetUsersModel>())
                    {
                        lRepresentantesToAnalise = new List<EmpresaAspnetUsersModel>();
                        var lRepresentantes = App.CurrentAspnetUserModel.lEpresaAspnetUsersModel;
                        var lEmpresas = lRepresentantes.Select(c => c.idEmpresa).Distinct().ToList();
                        foreach (var idEmpresa in lEmpresas)
                        {
                            lsync = await
                                UtilHttp.GetListRegistroSync<T>(
                                        param1: idEmpresa,
                                        param2: _ultimaDataSinc,
                                        param3:
                                        xTableName == TableMobile.TB_PEDIDOVENDA
                                            ? App.CurrentAspnetUserModel.Id
                                            : null);

                            lRepresentantesToAnalise.AddRange(lsync as List<EmpresaAspnetUsersModel>);

                            await SavePrivate(lsync, xTableName);
                        }
                    }
                    else
                    {
                        lsync = await UtilHttp.GetListRegistroSync<T>(
                                    param1: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                    param2: _ultimaDataSinc,
                                    param3:
                                    xTableName == TableMobile.TB_PEDIDOVENDA ? App.CurrentAspnetUserModel.Id : null);
                        await SavePrivate(lsync, xTableName);
                    }

                    integ.AtualizarDataIntegracao(xTableName, idEmp, bFalhaConexao, ocorreuErro, xMensagemErro);
                }
            }
            catch (Exception ex)
            {
                if (xTableName.ToUpper().Contains("TB_RECEBIMENTO"))
                {
                    await FinanceiroRepository.RemoverTodosRecebimentos<T>();
                    await SincronizacaoDownload<T>(true);
                }
                else
                {
                    throw new Exception($"{xTableName} - {ex.Message}");
                }
            }
        }

        private async Task SincronizacaoDownloadGrades<T>(bool bForceInicial = false) where T : class
        {
            IntegracaoRepository integ = new IntegracaoRepository();

            var xTableName = TableMobile.GetTableNameByModel<T>();

            int idEmp = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

            var _ultimaDataSinc = integ.getDataUltimaIntegracao(idEmp, xTableName);

            if (_ultimaDataSinc == null || _ultimaDataSinc.Year < 2000 || bForcarSyncInit)
                _ultimaDataSinc = DateTime.Today.AddYears(-50);
            else
                _ultimaDataSinc = _ultimaDataSinc.AddMinutes(-5);

            try
            {
                if (!ocorreuErro && !bFalhaConexao)
                {
                    currentModel.Display = xTableName;

                    var lsync = new List<T>();

                    lsync = await UtilHttp.GetListRegistroGradeSync<T>(
                                param1: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                param2: _ultimaDataSinc,
                                param3:
                                xTableName == TableMobile.TB_PEDIDOVENDA ? App.CurrentAspnetUserModel.Id : null);
                    await SavePrivate(lsync, xTableName);

                    integ.AtualizarDataIntegracao(xTableName, idEmp, bFalhaConexao, ocorreuErro, xMensagemErro);
                }
            }
            catch (Exception ex)
            {
                if (xTableName.ToUpper().Contains("TB_RECEBIMENTO"))
                {
                    await FinanceiroRepository.RemoverTodosRecebimentos<T>();
                    await SincronizacaoDownload<T>(true);
                }
                else
                {
                    throw new Exception($"{xTableName} - {ex.Message}");
                }
            }
        }

        #endregion

        #region UPLOAD

        public async Task UploadAll()
        {
            if (lastDateSync.Year > 2000)
            {
                await PostUpload(ClienteRepository.GetClientesModelsToSync());

                await PostUpload(ContatoRepository.GetAllContatoModelsToSync());

                await PostUpload(EnderecoRepository.GetAllEnderecoModelsToSync());

                await PostUploadAgenda(AgendaRepository.GetAtividadeAgendaParaUploadModel());

                await PostUpload(ProdutoRepository.GetAllToSync());

                await PostUploadPedido();
            }
        }

        private async Task PostUploadPedido()
        {
            try
            {
                var bMudouStatusPedido = false;
                var dtServerToChangeStatus = await UtilHttp.GetDateServer();

                await Task.Run(async () =>
                {
                    var lidPedidos = PedidoRepository.GetPedidosToSync(lastDateSync.ToDateTimeSync());

                    currentModel.Display = "UPLOAD PEDIDOS/ORÇAMENTOS";
                    currentModel.iCount = lidPedidos.Count;

                    foreach (var idPedidoOffLine in lidPedidos.Where(idPedidoOffLine => idPedidoOffLine.ToString().IsNumber()))
                    {
                        try
                        {
                            currentModel.iCount--;
                            var pedido = PedidoRepository.GetPedidoVendaModelToSync(Convert.ToInt32(idPedidoOffLine));

                            if (pedido == null) continue;

                            if (pedido.idClientes == 0)
                                pedido.idClientes = ClienteRepository.GetIdClienteNuvem(pedido.idClientesOffLine);
                            //pedido.dEmissao = pedido.dEmissao;
                            pedido.dtUltimaAlteracao = lastDateSync.ToDateTimeSync();
                            pedido.bControlaEstoque = currentPlano.bControlaEstoqueGrade;
                            pedido.bPedidoComAlteracao = false;

                            pedido.dEmissao = pedido.dEmissao.AddHours(-3);

                            var objPedidoSync = UtilHttp.PostRegistroToCloud(pedido).Result;

                            if (objPedidoSync != null)
                            {
                                if (objPedidoSync?.resulStruct.stRetorno == RetornoSalvar.Sucesso)
                                {
                                    if ((objPedidoSync.objModel.stEnviadoCliente ||
                                            objPedidoSync.objModel.stEnviadoRepresentacao))
                                    {
                                        SentEmail(pedido.idPedidoVendaOffLine ?? 0,
                                            objPedidoSync.objModel.idPedidoVenda ?? 0, objPedidoSync.objModel.idEmpresa,
                                            pedido.stEnviadoCliente, pedido.stEnviadoRepresentacao, pedido.idRepresentadaPdf);
                                    }
                                    pedido.idPedidoVenda = objPedidoSync.objModel.idPedidoVenda;
                                    pedido.idPedidoDisplay = objPedidoSync.objModel.idPedidoDisplay;
                                    pedido.idCondicaoPagamento = objPedidoSync.objModel.idCondicaoPagamento;
                                    pedido.xErroIntegracao = objPedidoSync.resulStruct.erroIntegracao;

                                    pedido.dtUltimaAlteracao =
                                        (objPedidoSync.objModel.dtUltimaAlteracao ?? DateTime.Now).ToDateTimeSync();
                                    App.Data.Connection.Update(pedido);

                                    EstoqueRepository.RemoveEstoquePedido(idPedidoOffLine);

                                    if (pedido.stLancamento == 0)
                                        PedidoRepository.UpdateAfterUpload(idPedidoOffLine, pedido.idPedidoVenda ?? 0);
                                }
                                else if (objPedidoSync.resulStruct.stRetorno == RetornoSalvar.EstoqueInsuficiente)
                                {
                                    var dadosEstoque =
                                        JsonConvert.DeserializeObject<List<EstoqueInsuficienteModel>>(
                                            objPedidoSync.resulStruct.retorno.ToString());

                                    var _estoqueValidado = EstoqueRepository.SaveEstoqueInsuficiente(dadosEstoque,
                                        pedido.idPedidoVendaOffLine ?? 0);

                                    currentModel.LAlertaSincronizacao.Add(new AlertaSincronizacao
                                    {
                                        idOffLine = pedido.idPedidoVendaOffLine,
                                        Table = TableMobile.TB_PEDIDOVENDA,
                                        Display = "Problemas com estoque",
                                        Detail = $"Cliente: {ClienteRepository.GetDisplayByIdOffLine(pedido.idClientesOffLine)}",
                                        DetailEstoque = _estoqueValidado
                                    });
                                }
                                else if (objPedidoSync.resulStruct.stRetorno == RetornoSalvar.Excecao)
                                {
                                    pedido.xErroPedido = objPedidoSync?.resulStruct.retorno?.ToString() ?? "";
                                    App.Data.Connection.Update(pedido);

                                    currentModel.LAlertaSincronizacao.Add(new AlertaSincronizacao
                                    {
                                        idOffLine = pedido.idPedidoVendaOffLine,
                                        Table = TableMobile.TB_PEDIDOVENDA,
                                        Display = "Erro ao subir Pedido",
                                        Detail = $"{objPedidoSync?.resulStruct.retorno}"
                                    });
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    var pedidosComStatusAlterados = PedidoRepository.GetPedidosAlteradosStatus();

                    foreach (var pedidoComStatusAlterado in pedidosComStatusAlterados)
                    {
                        bMudouStatusPedido = true;
                        // quando for necessário mudar apenas os status dos pedidos
                        var objRetorno = UtilHttp.PostRegistroToCloud(pedidoComStatusAlterado, "Post2").Result;

                        if (objRetorno.resulStruct.stRetorno != RetornoSalvar.Sucesso)
                            if ((pedidoComStatusAlterado.idStatusOld ?? 0) > 0)
                            {
                                var statusOld = StatusRepository.GetRegistro(pedidoComStatusAlterado.idStatusOld ?? 0);
                                var statusNew = StatusRepository.GetRegistro(pedidoComStatusAlterado.idStatus ?? 0);

                                PedidoRepository.VoltarParaStatusAnterior(
                                    pedidoComStatusAlterado.idPedidoVendaOffLine ?? 0,
                                    statusOld);

                                currentModel.LAlertaSincronizacao.Add(new AlertaSincronizacao
                                {
                                    Table = TableMobile.tb_produto_codigocliente,
                                    Display =
                                        $"Pedido: {pedidoComStatusAlterado.idPedidoDisplay}, não foi possível alterar o status '{statusOld.xNome}' para '{statusNew.xNome}'",
                                    Detail = objRetorno.resulStruct.retorno.ToString(),
                                    bErro = false
                                });
                            }

                        PedidoRepository.UpdateStatusParaNaoAlterado(pedidoComStatusAlterado.idPedidoVenda ?? 0);
                    }

                });

                if (bMudouStatusPedido)
                    await InitExclusoesDownload(dtServerToChangeStatus ?? DateTime.Now.AddMinutes(-2).ToUniversalTime());
            }

            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("SincronizacaoViewModel.PostUploadPedido", ex.Message,
                    true);
            }
        }

        private async Task PostUpload<T>(IEnumerable<T> lista) where T : class
        {
            try
            {
                var dtUltimaAlteracaoLocal = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime;
                var registros = lista as IList<T> ?? lista.ToList();
                var xTable = TableMobile.GetInfoModel<T>();
                var xPKonline = TableMobile.GetPrimaryKeyNameByModel<T>();
                currentModel.Display = "UPLOAD";
                currentModel.iCount = registros.Count;

                var xBlingApiKey = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.xBlingApiKey;
                var xOmieAppKey = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.xOmieAppKey;
                var bIntegracao = (string.IsNullOrEmpty(xBlingApiKey) == false) ||
                                  (string.IsNullOrEmpty(xOmieAppKey) == false);


                foreach (var registro in registros.Where(c => Convert.ToInt32(c.GetPropValue(xPKonline) ?? 0) != 0))
                {
                    var dtUltimaAlteracaoNuvem =
                        await UtilHttp.GetValueSync<DateTime>(controller: "APIverificaUltimaAlteracao",
                            param1: xTable,
                            param2: registro.GetPropValue(xPKonline),
                            param3: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);

                    // verifico qual é a alteração mais recente.
                    // se for a offline, subo as informações para a nuvem.
                    // se for a da nuvem, eu não faço nada pois o próximo processo é o processo de Download.
                    if (dtUltimaAlteracaoNuvem < dtUltimaAlteracaoLocal ||
                        dtUltimaAlteracaoNuvem == dtUltimaAlteracaoLocal)
                    {
                        if (registro.GetType() == typeof(ClientesModel))
                        {
                            var clientesModel = registro as ClientesModel;
                            if (clientesModel != null)
                            {
                                clientesModel.dtCadastro = clientesModel.dtCadastro.ToDateTimeSync();
                            }
                        }
                        else if (registro.GetType() == typeof(ProdutoModel))
                        {
                            var produtoModel = registro as ProdutoModel;
                            if (produtoModel != null)
                                produtoModel.dtCadastro = produtoModel.dtCadastro.ToDateTimeSync();
                        }
                        await UtilHttp.PostRegistroToCloud(registro); // mantem registro da Local   
                    }
                }


                foreach (var newregistro in registros.Where(c => Convert.ToInt32(c.GetPropValue(xPKonline) ?? 0) == 0))
                {
                    currentModel.iCount--;


                    if (xTable == TableMobile.GetTableNameByModel<ClientesModel>() && bIntegracao)
                    {
                        var item = (newregistro as ClientesModel);
                        if (item != null)
                            item.lEndereco =
                                new ObservableCollection<EnderecoModel>(
                                    EnderecoRepository.GetAll(item.idClientesOffLine ?? 0));
                    }

                    var registroSync = await UtilHttp.PostRegistroToCloud(newregistro);
                    if (registroSync == null) continue;

                    if (xTable == TableMobile.GetTableNameByModel<ContatoModel>())
                    {
                        if (registroSync.resulStruct.stRetorno != RetornoSalvar.Sucesso) continue;
                        var registroModel = registroSync.objModel as ContatoModel;
                        if (registroModel == null || registroModel.idContatos == null) continue;
                        var model = newregistro as ContatoModel;
                        if (model == null) continue;
                        registroModel.idClientesOffLine = model.idClientesOffLine;
                        registroModel.idContatoOffLine = model.idContatoOffLine;
                        App.Data.Connection.Update(registroModel);
                    }
                    else if (xTable == TableMobile.GetTableNameByModel<EnderecoModel>())
                    {
                        if (registroSync.resulStruct.stRetorno != RetornoSalvar.Sucesso) continue;
                        var registroModel = registroSync.objModel as EnderecoModel;
                        if (registroModel == null || registroModel.idEndereco == null) continue;
                        var model = newregistro as EnderecoModel;
                        if (model == null) continue;
                        registroModel.idClientesOffLine = model.idClientesOffLine;
                        registroModel.idEnderecoOffLine = model.idEnderecoOffLine;
                        App.Data.Connection.Update(registroModel);
                    }
                    else if (xTable == TableMobile.GetTableNameByModel<ClientesModel>())
                    {
                        if (registroSync.resulStruct.stRetorno == RetornoSalvar.Sucesso)
                        {
                            var registroModel = registroSync.objModel as ClientesModel;
                            if (registroModel == null || registroModel.idClientes == null) continue;
                            var model = newregistro as ClientesModel;
                            if (model == null) continue;
                            registroModel.idClientesOffLine = model.idClientesOffLine;
                            ClienteRepository.UpdateAfterUpload(registroModel);
                        }
                        else
                        {
                            var registroOff = newregistro as ClientesModel;
                            if (registroOff == null) return;
                            currentModel.LAlertaSincronizacao.Add(new AlertaSincronizacao
                            {
                                idOffLine = registroOff.idClientesOffLine,
                                Table = TableMobile.TB_CLIENTES,
                                Display = (registroSync.resulStruct.retorno ?? "").ToString(),
                                Detail = registroOff.xRazaoSocial
                            });
                        }


                    }
                    else if (xTable == TableMobile.GetTableNameByModel<ProdutoModel>())
                    {
                        if (registroSync.resulStruct.stRetorno == RetornoSalvar.Sucesso)
                        {
                            var registroModel = registroSync.objModel as ProdutoModel;
                            if (registroModel == null || registroModel.idProduto == null) continue;
                            var model = newregistro as ProdutoModel;
                            if (model == null) continue;

                            registroModel.idProdutoOffLine = model.idProdutoOffLine;
                            ProdutoRepository.UpdateAfterUpload(registroModel);
                        }
                        else
                        {
                            var produtoOff = newregistro as ProdutoModel;
                            if (produtoOff == null) return;
                            currentModel.LAlertaSincronizacao.Add(new AlertaSincronizacao
                            {
                                idOffLine = produtoOff.idProdutoOffLine,
                                Table = TableMobile.TB_PRODUTO,
                                Display = (registroSync.resulStruct.retorno ?? "").ToString(),
                                Detail = ProdutoRepository.GetNomeByIdOffLine(produtoOff.idProdutoOffLine ?? 0)
                            });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ocorreuErro = true;
                GoogleInsightsReportingConstants.TrakException("SincronizacaoViewModel.PostUpload", ex.Message, true);
            }

        }

        private async Task PostUploadAgenda<T>(IEnumerable<T> lista) where T : class
        {
            try
            {
                var dtUltimaAlteracaoLocal = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime;
                var registros = lista as IList<AtividadeAgendaModel>;
                var xTable = TableMobile.GetInfoModel<T>();
                var xPKonline = TableMobile.GetPrimaryKeyNameByModel<T>();
                currentModel.Display = "UPLOAD";
                currentModel.iCount = registros.Count;


                foreach (var registro in registros.Where(c => Convert.ToInt32(c.GetPropValue(xPKonline) ?? 0) != 0))
                {
                    var dtUltimaAlteracaoNuvem =
                        await UtilHttp.GetValueSync<DateTime>(controller: "APIverificaUltimaAlteracao",
                            param1: xTable,
                            param2: registro.GetPropValue(xPKonline),
                            param3: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);

                    // verifico qual é a alteração mais recente.
                    // se for a offline, subo as informações para a nuvem.
                    // se for a da nuvem, eu não faço nada pois o próximo processo é o processo de Download.
                    if (dtUltimaAlteracaoNuvem < dtUltimaAlteracaoLocal ||
                        dtUltimaAlteracaoNuvem == dtUltimaAlteracaoLocal)
                    {
                        //if (registro.dtInicioEvento != null)
                        //    if ((registro.dtInicioEvento ?? DateTime.Now).Kind != DateTimeKind.Local)
                        //        registro.dtInicioEvento = (registro.dtInicioEvento ?? DateTime.Now).ToLocalTime();


                        //if (registro.dtFimEvento != null)
                        //    if ((registro.dtFimEvento ?? DateTime.Now).Kind != DateTimeKind.Local)
                        //        registro.dtFimEvento = (registro.dtFimEvento ?? DateTime.Now).ToLocalTime();

                        await UtilHttp.PostAgendaToCloud(registro); // mantem registro da Local   
                    }
                }


                foreach (var newregistro in registros.Where(c => Convert.ToInt32(c.GetPropValue(xPKonline) ?? 0) == 0))
                {
                    currentModel.iCount--;
                    //if (newregistro.dtInicioEvento != null)
                    //    if ((newregistro.dtInicioEvento ?? DateTime.Now).Kind != DateTimeKind.Local)
                    //        newregistro.dtInicioEvento = (newregistro.dtInicioEvento ?? DateTime.Now).ToLocalTime();


                    //if (newregistro.dtFimEvento != null)
                    //    if ((newregistro.dtFimEvento ?? DateTime.Now).Kind != DateTimeKind.Local)
                    //        newregistro.dtFimEvento = (newregistro.dtFimEvento ?? DateTime.Now).ToLocalTime();

                    var item = (newregistro as AtividadeAgendaModel);
                    if (item.idCliente.GetValueOrDefault() == 0 && item.idClienteOffline.GetValueOrDefault() > 0)
                    {
                        var xQuery =
           $@"SELECT idClientes from tb_clientes where idClientesOffLine = {item.idClienteOffline} and idEmpresa = {App
               .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                        item.idCliente = App.Data.Connection.ExecuteScalar<int>(xQuery);
                    }


                    var registroSync = await UtilHttp.PostAgendaToCloud(newregistro);
                    if (registroSync == null) continue;

                    var registroModel = registroSync as AtividadeAgendaModel;
                    if (registroModel == null || registroModel.idAtividade == null) continue;
                    var model = newregistro as AtividadeAgendaModel;
                    if (model == null) continue;
                    registroModel.idClienteOffline = model.idClienteOffline;
                    registroModel.idAtividadeOffline = model.idAtividadeOffline;
                    App.Data.Connection.Update(registroModel);
                }
            }
            catch (Exception ex)
            {
                ocorreuErro = true;
                GoogleInsightsReportingConstants.TrakException("SincronizacaoViewModel.PostUploadAgenda", ex.Message, true);
            }

        }


        #endregion

        #region EXCLUSÃO

        private async void AnaliseExclusao<T>(IReadOnlyCollection<LogExclusaoModel> logs) where T : class, new()
        {
            if (logs == null) return;

            currentModel.Display = logs.FirstOrDefault().xTable;
            try
            {
                var xPrimaryKey = TableMobile.GetPrimaryKeyNameByModel<T>();
                foreach (var log in logs)
                {
                    var icount =
                        App.Data.Connection.ExecuteScalar<int>(
                            $"select count(*) from {log.xTable} where {xPrimaryKey} = {log.idPK}");
                    if (icount <= 0) continue;

                    var resultado =
                        App.Data.Connection.Query<T>(
                            $"select * from {log.xTable} where {xPrimaryKey} = {log.idPK.ToString()}");
                    var objToRemove = resultado.FirstOrDefault();

                    if (TableMobile.GetTableNameByModel<T>() == TableMobile.TB_PEDIDOVENDA)
                    {
                        var pedidoVendaModel = objToRemove as PedidoVendaModel;
                        if (pedidoVendaModel != null)
                        {
                            //Atualiza o estoque no mobile dos produtos de um pedido excluido no web - OS 35094 - Jessica Barbieri

                            List<EstoqueModel> _listagemEstoque = new List<EstoqueModel>();
                            var _pedidoVendaModelAux = PedidoRepository.GetPedidoVendaModel(pedidoVendaModel.idPedidoVendaOffLine ?? 0);

                            foreach (var p in _pedidoVendaModelAux.lItens)
                            {
                                EstoqueRepository.RemoveAllEstoqueSincronizacao(p.idProduto ?? 0);

                                var dateserver = lastDateServerSync;

                                var registro =
                                await
                                UtilHttp.GetRegistroSyncEstoque<EstoqueModel>(
                                    idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                                    idPK: p.idProduto,
                                    data: dateserver);

                                _listagemEstoque.AddRange(registro as List<EstoqueModel>);
                                PedidoRepository.Delete(pedidoVendaModel.idPedidoVendaOffLine ?? 0);

                                if (_listagemEstoque?.Count() > 0)
                                    await SavePrivate(_listagemEstoque, TableMobile.GetTableNameByModel<EstoqueModel>());
                            }
                        }
                    }
                    else
                        App.Data.Connection.Delete(objectToDelete: objToRemove);
                }
            }
            catch (Exception ex)
            {
                ocorreuErro = true;
                throw new Exception($"{currentModel.Display} - {ex.Message}");
            }
        }

        /// <summary>
        /// UPLOAD DAS EXCLUSÕES LOCAIS PARA A NUVEM
        /// </summary>
        /// <returns></returns>
        private async Task InitSyncExclusaoUpload()
        {
            try
            {

                var dados = App.Data.Connection.Query<LogExclusaoModel>(
                    $"SELECT * FROM {TableMobile.TB_LOGEXCLUSAO} WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}");

                if (!dados.Any()) return;
                currentModel.Display = "UPLOAD EXCLUSÕES";
                currentModel.iCount = dados.Count;
                foreach (var log in dados)
                {
                    currentModel.iCount--;
                    var bRemoved = false;
                    switch (log.xTable)
                    {
                        case TableMobile.TB_ENDERECO:
                            {
                                bRemoved = await UtilHttp.DeleteAsync<EnderecoModel>(log.idPK);
                            }
                            break;
                        case TableMobile.TB_CONTATOS:
                            {
                                bRemoved = await UtilHttp.DeleteAsync<ContatoModel>(log.idPK);
                            }
                            break;
                    }
                    if (bRemoved)
                        App.Data.Connection.Delete(log);
                }
            }
            catch (Exception ex)
            {
                ocorreuErro = true;
                ex.TrakException();
            }
        }

        /// <summary>
        /// download das exclusões da nuvem para o mobile.
        /// </summary>
        /// <param name="dtUltimaSincronizacaoServidor">data da última sincronização</param>
        /// <returns></returns>
        private async Task InitExclusoesDownload(DateTime dtUltimaSincronizacaoServidor)
        {
            try
            {
                var lnaotratado = new List<string>();
                currentModel.Display = "ANALISE DE EXCLUSÃO";
                //var date = currentModel.lastDateServerSync.AddHours(-2);
                //var lregistros = await UtilHttp.GetRegistroToRemoveSync<LogExclusaoModel>(date);
                var lregistros = await UtilHttp.GetRegistroToRemoveSync<LogExclusaoModel>(dtUltimaSincronizacaoServidor);
                if (lregistros.Any())
                {
                    currentModel.iCount = lregistros.Count;
                    foreach (var registro in lregistros.Select(c => c.xTable).Distinct())
                    {
                        currentModel.iCount--;
                        var group = lregistros.Where(c => c.xTable == registro).ToList();

                        switch (registro.ToUpper())
                        {
                            case "TB_PRODUTO_CODIGOCLIENTE":
                                AnaliseExclusao<tb_produto_codigocliente>(logs: group);
                                break;

                            case TableMobile.TB_PRODUTO:
                                AnaliseExclusao<ProdutoModel>(logs: group);
                                break;
                            case TableMobile.TB_CONDICAOPAGAMENTO:
                                AnaliseExclusao<CondicaoPagamentoModel>(logs: group);
                                break;
                            case TableMobile.TB_CATEGORIA:
                                AnaliseExclusao<CategoriaProdutoModel>(logs: group);
                                break;
                            case TableMobile.TB_RAMOATIVIDADE:
                                AnaliseExclusao<RamoAtividadeModel>(logs: group);
                                break;
                            case TableMobile.TB_IMAGEM:
                                AnaliseExclusao<ImagemModel>(logs: group);
                                break;
                            case TableMobile.TB_TRANSPORTADORAS:
                                AnaliseExclusao<TransportadorasModel>(logs: group);
                                break;
                            case TableMobile.TB_ENDERECO:
                                AnaliseExclusao<EnderecoModel>(logs: group);
                                break;
                            case TableMobile.TB_CONTATOS:
                                AnaliseExclusao<ContatoModel>(logs: group);
                                break;
                            case TableMobile.TB_CLIENTES:
                                AnaliseExclusao<ClientesModel>(logs: group);
                                break;
                            case TableMobile.TB_CLIENTES_CONDICOESPAGAMENTO:
                                AnaliseExclusao<ClientesCondicoesPagamentoModel>(logs: group);
                                break;
                            case TableMobile.TB_GRADETAMANHO:
                                AnaliseExclusao<GradeTamanhoModel>(logs: group);
                                break;
                            case TableMobile.TB_GRADECOR:
                                AnaliseExclusao<GradeCorModel>(logs: group);
                                break;
                            case TableMobile.TB_TABELAPRECO:
                                AnaliseExclusao<TabelaPrecoModel>(logs: group);
                                break;
                            case TableMobile.TB_TABELAPRECOITEM:
                                AnaliseExclusao<TabelaPrecoItemModel>(logs: group);
                                break;
                            case TableMobile.TB_TABELA_PRECO_CLIENTES:
                                AnaliseExclusao<TabelaPrecoClientesModel>(logs: group);
                                break;
                            case TableMobile.TB_TABELA_PRECO_REPRESENTANTES:
                                AnaliseExclusao<TabelaPrecoRepresentantesModel>(logs: group);
                                break;
                            case TableMobile.TB_TABELAPRECO_REPRESENTACOES:
                                AnaliseExclusao<TabelaPrecoRepresentacoesModel>(logs: group);
                                break;
                            case TableMobile.TB_RECEBIMENTOTITULOS:
                                AnaliseExclusao<RecebimentoTitulosModel>(logs: group);
                                break;
                            case TableMobile.TB_RECEBIMENTOTITULOS_MOVIMENTACOES:
                                AnaliseExclusao<RecebimentoTitulosMovimentacaoModel>(logs: group);
                                break;
                            case TableMobile.TB_UNIDADEMEDIDA:
                                AnaliseExclusao<UnidadeMedidaModel>(logs: group);
                                break;
                            case TableMobile.TB_ATIVIDADES:
                                AnaliseExclusao<AtividadeAgendaModel>(logs: group);
                                break;
                            case TableMobile.TB_TIPOATIVIDADESCRM:
                                AnaliseExclusao<TipoAtividadeAgendaModel>(logs: group);
                                break;
                            case TableMobile.TB_PEDIDOVENDA:
                                AnaliseExclusao<PedidoVendaModel>(logs: group);
                                break;
                            case TableMobile.TB_STATUS:
                                AnaliseExclusao<StatusModel>(logs: group);
                                break;
                            case TableMobile.TB_TABELAESCALONADA:
                                AnaliseExclusao<TabelaEscalonadaModel>(logs: group);
                                break;
                            case TableMobile.TB_TABELAESCALONADA_FAIXACOMISSAO:
                                AnaliseExclusao<TabelaEscalonadaFaixaComissaoModel>(logs: group);
                                break;
                            case TableMobile.TB_TABELAESCALONADA_REPRESENTANTE:
                                AnaliseExclusao<TabelaEscalonadaRepresentanteModel>(logs: group);
                                break;
                            case TableMobile.TB_REPRESENTADA:
                                AnaliseExclusao<RepresentadaModel>(logs: group);
                                break;
                            case TableMobile.TB_EMPRESA_ASPNETUSERS_METAS:
                            case TableMobile.TB_EMPRESA_ASPNETUSERS:
                            case TableMobile.TB_PEDIDOVENDAITENS:
                            case TableMobile.TB_REPRESENTADA_ASPNETUSERS:
                                break;
                            case TableMobile.TB_PERMISSOES_REPRESENTANTES:
                                break;
                            default:
                                lnaotratado.Add(registro);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ocorreuErro = true;
                ex.TrakException();
            }

        }

        #endregion

        #endregion

        #region CanExecute

        public bool CanExecuteSincronizacao(object param)
        {
            var can = !IsBusy;

            return can;
        }

        #endregion

        #region Metodos privados

        private static void SentEmail(int idPedidoVendaOffLine, int idPedidoVenda, int idEmpresa, bool stEnviadoCliente,
            bool stEnviadoRepresentacao, int? idRepresentadaPdf)
        {
            var email = new EmailPedidoModel
            {
                idPedidoVendaOffLine = idPedidoVendaOffLine,
                idPedidoVenda = idPedidoVenda,
                idEmpresa = idEmpresa,
                bEnviaCliente = stEnviadoCliente,
                bEnviaRepresentacoes = stEnviadoRepresentacao,
                idRepresentadaPdf = idRepresentadaPdf,
                idAspnetUsers = App.CurrentAspnetUserModel.Id
            };
            UtilHttp.SendEmailPedido(email);
        }

        private async Task SaveSincronizacao<T>(T registro, string xPrimaryKeyName, string xTableName) where T : class
        {
            await Task.Run(() =>
            {
                var icount = 0;
                var idPk = registro.GetPropValue(xPrimaryKeyName);
                if (idPk != null)
                {
                    var xQuery = $@"SELECT COUNT(*) FROM {xTableName} WHERE {xPrimaryKeyName} = '{idPk}' ";
                    try
                    {
                        icount = App.Data.Connection.ExecuteScalar<int>(xQuery);
                    }
                    catch (Exception ex)
                    {
                        ocorreuErro = true;
                        ex.TrakException();
                    }
                }

                if (icount == 0) // registro ainda não sincronizado
                {
                    #region Model Específico

                    var insertgeneric = true;

                    if (registro.GetType() == typeof(ContatoModel) ||
                        registro.GetType() == typeof(EnderecoModel))
                    {
                        var idClienteNuvem = registro.GetPropValue("idClientes");
                        if (idClienteNuvem != null)
                        {
                            var idOffLine = App.Data.Connection.ExecuteScalar<int>(
                                $@"select idClientesOffLine from {TableMobile.TB_CLIENTES} where idClientes = {idClienteNuvem}");
                            if (idOffLine != 0)
                                registro.SetPropValue("idClientesOffLine", idOffLine);
                        }
                    }

                    if (registro.GetType() == typeof(TabelaPrecoModel))
                    {
                        var item = registro as TabelaPrecoModel;
                        if (item?.dInicial != null && item.dFinal != null)
                        {
                            item.dInicial = (item.dInicial ?? DateTime.Now).ToLocalTime();
                            item.dFinal = (item.dFinal ?? DateTime.Now).ToLocalTime();
                        }
                    }

                    if (registro.GetType() == typeof(AtividadeAgendaModel))
                    {
                        var item = registro as AtividadeAgendaModel;

                        if (item.idCliente == null)
                            item.idCliente = 0;

                        var xQuery =
                      $@"SELECT idClientesOffLine from tb_clientes where idClientes = {item.idCliente} and idEmpresa = {App
                          .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";


                        var idClienteOffline = App.Data.Connection.ExecuteScalar<int?>(xQuery);
                        item.idClienteOffline = idClienteOffline;
                    }

                    if (registro.GetType() == typeof(PedidoVendaModel))
                    {
                        insertgeneric = false;
                        SavePedidoSync(registro);
                    }

                    if (registro.GetType() == typeof(LocalEstoqueModel))
                    {
                        insertgeneric = false;
                        var estoque = registro as LocalEstoqueModel;

                        if (estoque.idLocalEstoque > 0)
                        {
                            App.Data.Connection.Insert(registro);
                            if (estoque.lClientesAtrelados?.Count() > 0)
                                App.Data.Connection.InsertAll(estoque.lClientesAtrelados);
                            if (estoque.lRepresentantesAtrelados?.Count() > 0)
                                App.Data.Connection.InsertAll(estoque.lRepresentantesAtrelados);
                            if (estoque.lUfAtrelados?.Count() > 0)
                                App.Data.Connection.InsertAll(estoque.lUfAtrelados);
                            if (estoque.lRamoAtividades?.Count() > 0)
                                App.Data.Connection.InsertAll(estoque.lRamoAtividades);
                        }
                    }

                    if (registro.GetType() == typeof(JornadaModel))
                    {
                        var jornada = registro as JornadaModel;
                        //removendo os horários pra inserir novamente
                        PedidoRepository.RemoveHorariosJornadaNova(jornada.idJornada);

                        foreach (var item in jornada.lHorarios)
                        {
                            App.Data.Connection.Insert(item);
                        }

                    }

                    #endregion

                    //se for registro genérico ele insere
                    if (insertgeneric)
                        App.Data.Connection.Insert(registro);

                    if (registro.GetType() == typeof(StatusModel))
                    {
                        var status = registro as StatusModel;
                        StatusRepository.SalvarStatusProibidos(status.idStatus, status.lRepresentantesProibidos);
                    }
                }
                else
                //if (!App.ForcarAtualizacao)
                {
                    #region Model Específio

                    if (registro.GetType() == typeof(ClientesModel))
                    {
                        var xQuery = $@"SELECT idClientes, idClientesOffLine from {xTableName} where idClientes = {idPk} and idEmpresa = {App
                                .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                        var registroLocal = App.Data.Connection.Query<ClientesModel>(xQuery).FirstOrDefault();
                        if (registroLocal != null)
                        {
                            var registroNuvem = registro as ClientesModel;
                            if (registroNuvem != null)
                            {
                                registroNuvem.dtCadastro = registroNuvem.dtCadastro.ToDateTimeSync();
                                registroNuvem.idClientesOffLine = registroLocal.idClientesOffLine;
                            }
                        }
                    }
                    else if (registro.GetType() == typeof(ContatoModel))
                    {
                        var xQuery = $@"SELECT idContatos, idContatoOffLine, idClientesOffLine from {TableMobile.TB_CONTATOS} where idContatos = {idPk} and idEmpresa = {App
                                .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                        var registroLocal = App.Data.Connection.Query<ContatoModel>(xQuery).FirstOrDefault();
                        if (registroLocal != null)
                        {
                            var registroNuvem = registro as ContatoModel;
                            if (registroNuvem != null)
                            {
                                registroNuvem.idClientesOffLine = registroLocal.idClientesOffLine;
                                registroNuvem.idContatoOffLine = registroLocal.idContatoOffLine;
                            }
                        }
                    }
                    else if (registro.GetType() == typeof(AtividadeAgendaModel))
                    {
                        var item = registro as AtividadeAgendaModel;

                        if (item.idCliente == null)
                            item.idCliente = 0;

                        var xQuery = $@"SELECT idClientesOffLine from tb_clientes where idClientes = {item.idCliente} and idEmpresa = {App
                            .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";


                        var idClienteOffline = App.Data.Connection.ExecuteScalar<int?>(xQuery);
                        item.idClienteOffline = idClienteOffline;

                        xQuery = $@"SELECT idAtividadeOffline from {TableMobile.TB_ATIVIDADES} where idAtividade = {item.idAtividade} and idEmpresa = {App
                            .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                        var idAtividadeOffline = App.Data.Connection.ExecuteScalar<int>(xQuery);
                        item.idAtividadeOffline = idAtividadeOffline;
                    }
                    else if (registro.GetType() == typeof(EnderecoModel))
                    {
                        var xQuery =
                            $@"SELECT idEndereco, idEnderecoOffLine, idClientesOffLine from {TableMobile.TB_ENDERECO} where idEndereco = {idPk} and idEmpresa = {App
                                .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                        var registroLocal = App.Data.Connection.Query<EnderecoModel>(xQuery).FirstOrDefault();
                        if (registroLocal != null)
                        {
                            var registroNuvem = registro as EnderecoModel;
                            if (registroNuvem != null)
                            {
                                registroNuvem.idClientesOffLine = registroLocal.idClientesOffLine;
                                registroNuvem.idEnderecoOffLine = registroLocal.idEnderecoOffLine;
                            }
                        }
                    }
                    else if (registro.GetType() == typeof(PedidoVendaModel))
                    {
                        var objPedido = registro as PedidoVendaModel;
                        if (objPedido != null)
                        {
                            var idPedidoVendaOffLine = PedidoRepository.GetIdOffLine(objPedido.idPedidoVenda);
                            if (PedidoRepository.Delete(idPedidoVendaOffLine))
                                SavePedidoSync(registro);
                        }
                    }
                    else if (registro.GetType() == typeof(ProdutoModel))
                    {
                        var objRegistro = registro as ProdutoModel;
                        if (objRegistro != null)
                        {
                            objRegistro.dtCadastro = objRegistro.dtCadastro.ToDateTimeSync();
                            objRegistro.idProdutoOffLine =
                                ProdutoRepository.GetIdOffLineByIdOnline(objRegistro.idProduto ?? 0);

                            //if (!string.IsNullOrEmpty(objRegistro.xFileImagePrincipal))
                            //    UtilHttp.SaveImagem(objRegistro.xFileImagePrincipal);
                        }
                    }
                    else if (registro.GetType() == typeof(TabelaPrecoModel))
                    {
                        var objRegistro = registro as TabelaPrecoModel;
                        if (objRegistro != null)
                        {
                            if (objRegistro.dInicial.HasValue)
                            {
                                objRegistro.dInicial = objRegistro.dInicial.Value.ToLocalTime().ToDateTimeSync();
                            }
                            if (objRegistro.dFinal.HasValue)
                            {
                                objRegistro.dFinal.Value.ToLocalTime().ToDateTimeSync();
                            }
                        }
                    }
                    else if (registro.GetType() == typeof(LocalEstoqueModel))
                    {
                        var estoque = registro as LocalEstoqueModel;

                        if (estoque.idLocalEstoque > 0)
                        {
                            var xQuery = $@"SELECT xNomeLocal, idLocalEstoqueOffline, idLocalEstoque from {TableMobile.TB_LOCAL_ESTOQUE} where idLocalEstoque = {estoque.idLocalEstoque} and idEmpresa = {App
                              .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                            var estoqueInserido = App.Data.Connection.Query<LocalEstoqueModel>(xQuery).FirstOrDefault();

                            if (estoqueInserido != null)
                            {

                                estoque.idLocalEstoqueOffline = estoqueInserido.idLocalEstoqueOffline;

                                //App.Data.Connection.Update(estoqueInserido);

                                //processo de clientes
                                string where = string.Empty;
                                if (estoque.lClientesAtrelados?.Count() > 0)
                                {
                                    string _lClientesAtrelados = string.Join(",", estoque.lClientesAtrelados.Select(t => t.idClientes).ToList());
                                    where = $" and idClientes not in ({_lClientesAtrelados})";
                                }

                                App.Data.Connection.Execute($@"DELETE FROM TB_LOCAL_ESTOQUE_CLIENTES where idLocalEstoque = {estoque.idLocalEstoque}  {where}");

                                //inserindo os que não estão
                                if (estoque.lClientesAtrelados?.Count() > 0)
                                {
                                    List<int> idsClientesLocaisRelacionados = App.Data.Connection.Table<LocalEstoqueClientesModel>().Where(c => c.idLocalEstoque == estoque.idLocalEstoque).Select(t => t.idClientes).ToList();
                                    App.Data.Connection.InsertAll(estoque.lClientesAtrelados.Where(t => !idsClientesLocaisRelacionados.Contains(t.idClientes)));
                                }


                                //processo de representantes
                                where = string.Empty;
                                if (estoque.lRepresentantesAtrelados?.Count() > 0)
                                {
                                    string _lRepresentantesAtrelados = string.Join(",", estoque.lRepresentantesAtrelados.Select(t => t.idEmpresa_aspnetUsers).ToList());
                                    where = $" and idEmpresa_aspnetUsers not in ({_lRepresentantesAtrelados})";
                                }

                                App.Data.Connection.Execute($@"DELETE FROM TB_LOCAL_ESTOQUE_REPRESENTANTES where idLocalEstoque = {estoque.idLocalEstoque}  {where}");

                                if (estoque.lRepresentantesAtrelados?.Count() > 0)
                                {
                                    List<int> idsRepresentantesLocaisRelacionados = App.Data.Connection.Table<LocalEstoqueRepresentantesModel>().Where(c => c.idLocalEstoque == estoque.idLocalEstoque).Select(t => t.idEmpresa_aspnetUsers).ToList();
                                    App.Data.Connection.InsertAll(estoque.lRepresentantesAtrelados.Where(t => !idsRepresentantesLocaisRelacionados.Contains(t.idEmpresa_aspnetUsers)));
                                }


                                //processo de uf
                                where = string.Empty;
                                if (estoque.lUfAtrelados?.Count() > 0)
                                {
                                    string _lUfAtrelados = string.Empty;

                                    if (estoque.lUfAtrelados?.Count() == 1)
                                    {
                                        _lUfAtrelados = $"'{estoque.lUfAtrelados.Select(t => t.xUf).FirstOrDefault()}'";
                                    }
                                    else
                                    {
                                        int i = 0;
                                        foreach (var item in estoque.lUfAtrelados)
                                        {
                                            i++;
                                            _lUfAtrelados += $"'{item.xUf}'";
                                            if (i < estoque.lUfAtrelados?.Count())
                                            {
                                                _lUfAtrelados += $",";
                                            }
                                        }
                                    }


                                    where = $" and xUf not in ({_lUfAtrelados})";
                                }

                                App.Data.Connection.Execute($@"DELETE FROM TB_LOCAL_ESTOQUE_UF where idLocalEstoque = {estoque.idLocalEstoque}  {where}");

                                if (estoque.lUfAtrelados?.Count() > 0)
                                {
                                    List<string> ufsAtreladas = App.Data.Connection.Table<LocalEstoqueUfModel>().Where(c => c.idLocalEstoque == estoque.idLocalEstoque).Select(t => t.xUf).ToList();
                                    App.Data.Connection.InsertAll(estoque.lUfAtrelados.Where(t => !ufsAtreladas.Contains(t.xUf)));
                                }

                                //processo de ramos
                                where = string.Empty;
                                if (estoque.lRamoAtividades?.Count() > 0)
                                {
                                    string _lRamoAtividades = string.Join(",", estoque.lRamoAtividades.Select(t => t.idRamoAtividade).ToList());
                                    where = $" and idRamoAtividade not in ({_lRamoAtividades})";
                                }

                                App.Data.Connection.Execute($@"DELETE FROM TB_LOCAL_ESTOQUE_RAMOSATIVIDADES where idLocalEstoque = {estoque.idLocalEstoque}  {where}");

                                if (estoque.lRamoAtividades?.Count() > 0)
                                {
                                    List<int> idsRamosLocaisRelacionados = App.Data.Connection.Table<LocalEstoqueClienteRamoAtividadesDataModel>().Where(c => c.idLocalEstoque == estoque.idLocalEstoque).Select(t => t.idRamoAtividade).ToList();
                                    App.Data.Connection.InsertAll(estoque.lRamoAtividades.Where(t => !idsRamosLocaisRelacionados.Contains(t.idRamoAtividade)));
                                }
                            }
                        }
                    }
                    else if (registro.GetType() == typeof(JornadaModel))
                    {
                        var jornada = registro as JornadaModel;
                        //removendo os horários pra inserir novamente
                        PedidoRepository.RemoveHorariosJornadaNova(jornada.idJornada);

                        foreach (var item in jornada.lHorarios)
                        {
                            App.Data.Connection.Insert(item);
                        }

                    }
                    else if (registro.GetType() == typeof(StatusModel))
                    {
                        var status = registro as StatusModel;

                        StatusRepository.RemoverProbidos(status.idStatus);

                        StatusRepository.SalvarStatusProibidos(status.idStatus, status.lRepresentantesProibidos);
                    }
                    #endregion

                    App.Data.Connection.Update(registro);
                }



                if (registro.GetType() == typeof(EmpresaModel))
                {
                    var empresa = registro as EmpresaModel;
                    if (empresa != null && !string.IsNullOrEmpty(empresa.imLogoMarca))
                        UtilHttp.SaveImagem(empresa.imLogoMarca);
                }
            });
        }

        private void SavePedidoSync<T>(T registro) where T : class
        {
            var objPedido = registro as PedidoVendaModel;
            if (objPedido?.lItens != null && objPedido.lItens.Count > 0)
            {
                //objPedido.dEmissao = objPedido.dEmissao.ToDateTimeSync();
                objPedido.dtUltimaAlteracao = (objPedido.dtUltimaAlteracao ?? DateTime.Now).ToDateTimeSync();
                objPedido.dtValidadeOrcamento = (objPedido.dtValidadeOrcamento ?? DateTime.Now).ToDateTimeSync();
                objPedido.idClientesOffLine = ClienteRepository.GetIdClienteOffLine(objPedido.idClientes);
                objPedido.idPedidoVendaOffLine = null;
                PedidoRepository.SavePedidoVenda(objPedido);
            }
        }

        public void SituacaoInicial()
        {
            if (!IsBusy)
            {
                currentModel.Display = ". . .";
                currentModel.iCount = 0;
                if (App.CurrentAspnetUserModel != null && App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel != null)
                {
                    lastDateSync = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime;
                    lastDateServerSync =
                        App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.ultimaSyncServerDateTime ??
                        lastDateSync;
                }
            }
        }

        private async void AnaliseFinalSincronizacao(string exMessage = "")
        {
            if (!bFalhaTotalDeConexao)
            {
                IsBusy = false;
                await Task.Delay(100);
                // set null, porque preciso que a próxima vez que entre no listar de clientes, produto faça a consulta novamente
                if (bFalhaConexao == false)
                {
                    if (exMessage != "" && currentModel.LAlertaSincronizacao.Count(c => c.bErro) == 0)
                    {
                        await App.Messages.ShowAsync(exMessage);
                        FecharPopup();

                        if (exMessage.Contains("encontra-se inativo nessa empresa"))
                            UtilNavidate.EfetivarLogoff();
                    }
                    else if (currentModel.LAlertaSincronizacao.Count(c => c.bErro) > 0)
                    {
                        ocorreuErro = true;
                        await App.Messages.ShowAsync("Algumas inconsistências na sincronização foram encontradas.");
                        FecharPopup(true);
                    }
                    else
                    {
                        StaticModel.StaticFindClienteModel = null;
                        StaticModel.StaticFindProdutoModel = null;
                        StaticModel.lTabelasPrecoCampanhas = null;

                        bForcarSyncInit = false;
                        lastDateSync =
                            App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime = DateTime.UtcNow;
                        EmpresaAspnetUsersRepository.AtualizaEmpresaAspnetUsersModel(
                            App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel);
                        PageHomeNew.ViewModelStatic.AtualizaImagemApp();
                        LoginRepository.RefreshTipoUsuario();
                        AcaoAfterSyncCommand?.Execute(null);
                        EstoqueRepository.RemoveAllEstoquePedido();
                        if (currentModel.LAlertaSincronizacao.Count(c => c.bErro == false) > 0)
                        {
                            FecharPopup(true);
                        }
                        else
                        {
                            FecharPopup();
                        }

                        var currentUser = EmpresaAspnetUsersRepository.GetUsuario();
                        if (currentUser.stAtivo == false)
                        {
                            await
                                App.Messages.ShowAsync(
                                    "Usuário encontra-se inativo na empresa corrente, será necessário o login novamente");
                            UtilNavidate.EfetivarLogoff();
                        }
                    }
                }
                else
                {
                    await App.Messages.ShowAsync(bFalhaConexao
                        ? $"O dispositivo ficou sem internet no decorrer da sincronização, alguns dados podem não ter sido sincronizados. {Environment.NewLine}Sincronize novamente."
                        : exMessage);
                    FecharPopup();

                }
            }
            else
            {
                FecharPopup();
            }
        }

        #endregion

        private async void FecharPopup(bool viewError = false)
        {
            try
            {
                CrossConnectivity.Current.ConnectivityChanged -= Current_ConnectivityChanged;
                await App.Navigation.PopPopupAsync();
                var main = Application.Current.MainPage as RootPage;
                if (main != null)
                {
                    main.Detail.Opacity = 1;

                    var navigationPage = main.Detail as NavigationPage;
                    if (navigationPage != null)

                        if ((navigationPage.CurrentPage.GetType() != typeof(PageListarPedidos)) && (navigationPage.CurrentPage.GetType() != typeof(PageInfinitListClientes)) && (navigationPage.CurrentPage.GetType() != typeof(PageInfinitListProdutos)))
                        {
                            App.ParamBackButtonPressed?.SetParameter(true);
                        }
                }
                if (viewError)
                {
                    UtilNavidate.PushAsync(new PageLogSync(currentModel.LAlertaSincronizacao));
                }
            }
            catch (Exception ex)
            {
                ex.TrakException("", false);
            }
        }


        private async Task SavePrivate<T>(List<T> lsync, string xTableName) where T : class
        {
            if (!bFalhaConexao)
            {
                var xPrimaryKeyName = TableMobile.GetPrimaryKeyNameByModel<T>();
                if (lsync.Count > 0)
                {
                    //Metodos executados apenas uma unica vez para limpar seus respectivos registros
                    if (lsync[0].GetType() == typeof(RecebimentoTitulosModel))
                        (lsync as List<RecebimentoTitulosModel>).GroupBy(x => x.idPedidoVenda).ForEach(l => FinanceiroRepository.RemoverTodosRecebimentosPedido(l.FirstOrDefault().idPedidoVenda));
                    if (lsync[0].GetType() == typeof(RepresentadaAspnetUsersModel))
                        (lsync as List<RepresentadaAspnetUsersModel>).GroupBy(x => x.idEmpresa_aspnetUsers).ForEach(l => RepresentadaRepository.RemoverTodosRepresentantes(l.FirstOrDefault().idEmpresa_aspnetUsers));

                    currentModel.iCount = lsync.Count;
                    foreach (var registro in lsync)
                    {
                        if (registro.GetType() == typeof(ExtensaoEmpresaModel))
                        {
                            var extensaoModel = registro as ExtensaoEmpresaModel;
                            if (extensaoModel != null)
                                extensaoModel.idEmpresa =
                                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                        }

                        if (registro.GetType() == typeof(EmpresaAspnetUsersModel))
                        {
                            var usuario = registro as EmpresaAspnetUsersModel;
                            if (usuario != null)
                            {
                                if (usuario.idEmpresa_aspnetUsers == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers)
                                {
                                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.imUsuario = usuario.imUsuario;
                                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.bGravaLocRepresentante = usuario.bGravaLocRepresentante;
                                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.vMetaCorrente = usuario.vMetaCorrente;
                                    PageHomeNew.ViewModelStatic.AtualizaImagemApp();
                                }
                            }
                        }

                        if (registro.GetType() == typeof(ProdutoModel))
                        {
                            var prod = registro as ProdutoModel;
                            if (prod != null && !string.IsNullOrEmpty(prod.xFileImagePrincipal))
                            {
                                //if (prod.lImagens?.Count() > 0)
                                //{
                                //    foreach (var img in prod.lImagens)
                                //    {
                                //        var xNameImage = img.xFilePath.PathToNameImage();
                                //        var buffer = Convert.FromBase64String(img.base64Image);
                                //        App.Picture.SavePictureToDisk(xNameImage, buffer);

                                //        await SaveSincronizacao(img, "idImagem", "TB_IMAGEM");
                                //    }
                                //}
                                var _lImagens = await UtilHttp.GetRegistroSyncImagem(prod.idEmpresa, prod.idProduto, lastDateServerSync);
                                if (_lImagens?.Count() > 0)
                                {
                                    foreach (var img in _lImagens)
                                    {
                                        var xNameImage = img.xFilePath.PathToNameImage();
                                        var buffer = Convert.FromBase64String(img.base64Image);
                                        App.Picture.SavePictureToDisk(xNameImage, buffer);
                                        await SaveSincronizacao(img, "idImagem", "TB_IMAGEM");
                                    }
                                }
                            }
                        }

                        await SaveSincronizacao(registro, xPrimaryKeyName, xTableName);
                        currentModel.iCount--;
                    }
                }
            }
        }






        private async Task SavePrivatePaginado<T>(List<T> lsync, string xTableName, int page) where T : class
        {
            if (!bFalhaConexao)
            {
                var xPrimaryKeyName = TableMobile.GetPrimaryKeyNameByModel<T>();
                if (lsync.Count > 0)
                {
                    currentModel.iCount = page;
                    foreach (var registro in lsync)
                    {
                        if (registro.GetType() == typeof(ExtensaoEmpresaModel))
                        {
                            var extensaoModel = registro as ExtensaoEmpresaModel;
                            if (extensaoModel != null)
                                extensaoModel.idEmpresa =
                                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                        }
                        if (registro.GetType() == typeof(EmpresaAspnetUsersModel))
                        {
                            var usuario = registro as EmpresaAspnetUsersModel;
                            if (usuario != null)
                            {
                                if (usuario.idEmpresa_aspnetUsers ==
                                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers)
                                {
                                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.imUsuario =
                                        usuario.imUsuario;
                                    PageHomeNew.ViewModelStatic.AtualizaImagemApp();

                                }
                            }
                        }
                        if (registro.GetType() == typeof(ProdutoModel))
                        {
                            var prod = registro as ProdutoModel;
                            if (prod != null && !string.IsNullOrEmpty(prod.xFileImagePrincipal))
                            {
                                //if (prod.lImagens?.Count() > 0)
                                //{
                                //    foreach (var img in prod.lImagens)
                                //    {
                                //        var xNameImage = img.xFilePath.PathToNameImage();
                                //        var buffer = Convert.FromBase64String(img.base64Image);
                                //        App.Picture.SavePictureToDisk(xNameImage, buffer);

                                //        await SaveSincronizacao(img, "idImagem", "TB_IMAGEM");
                                //    }
                                //}
                                var _lImagens = await UtilHttp.GetRegistroSyncImagem(
                                  prod.idEmpresa,
                                  prod.idProduto,
                                  lastDateServerSync);
                                if (_lImagens?.Count() > 0)
                                {
                                    foreach (var img in _lImagens)
                                    {
                                        var xNameImage = img.xFilePath.PathToNameImage();
                                        var buffer = Convert.FromBase64String(img.base64Image);
                                        App.Picture.SavePictureToDisk(xNameImage, buffer);
                                        await SaveSincronizacao(img, "idImagem", "TB_IMAGEM");
                                    }
                                }
                            }
                        }


                        await SaveSincronizacao(registro, xPrimaryKeyName, xTableName);
                    }
                }
            }
        }

    }
}

