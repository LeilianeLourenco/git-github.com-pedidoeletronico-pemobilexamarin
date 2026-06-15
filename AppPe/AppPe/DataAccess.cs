using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.Model.Cidades;
using Xamarin.HLP.Mobile.AppPE.Model.Empresa;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;
using Xamarin.HLP.Mobile.AppPE.Model.Financeiro;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais;
using Xamarin.HLP.Mobile.AppPE.Model.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE
{
    public class DataAccess : IDisposable
    {
        public SQLiteConnection Connection;
        private IConfig config;

        public DataAccess()
        {
            try
            {
                App.xErrorDataBase = "";
                config = DependencyService.Get<IConfig>();
                Connection = new SQLiteConnection(Path.Combine(config.DirectoryDB, "DB_PEDIDOELETRONICO.db3"), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, true);

                // WAL + synchronous NORMAL: o fsync deixa de acontecer a cada insert (só em checkpoint),
                // acelerando muito a sincronização em massa (ex.: 40 mil pedidos na carga inicial).
                try
                {
                    Connection.ExecuteScalar<string>("PRAGMA journal_mode=WAL;");
                    Connection.Execute("PRAGMA synchronous=NORMAL;");
                }
                catch (Exception exPragma) { App.xErrorDataBase += exPragma.Message + " - PRAGMA WAL"; }
            }
            catch (Exception ex)
            {
                App.xErrorDataBase += ex.Message;
            }
        }

        public void PrimeiraAnalise()
        {
            try
            {
                try
                {
                    this.Connection.CreateTable<CurrentUserLoginModel>();
                }
                catch (Exception ex)
                {
                    App.xErrorDataBase += ex.Message + " - " + TableMobile.AspNetUsers;
                }
                try
                {
                    this.Connection.CreateTable<ExtensaoEmpresaModel>();
                }
                catch (Exception ex)
                {
                    App.xErrorDataBase += ex.Message + " - " + TableMobile.AspNetUsers;
                }
                try { this.Connection.CreateTable<AspNetUsersModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.AspNetUsers; }
                try { this.Connection.CreateTable<tb_produto_codigocliente>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + "tb_produto_codigocliente"; }
                try { this.Connection.CreateTable<EmpresaAspnetUsersModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_EMPRESA_ASPNETUSERS; }
                try { this.Connection.CreateTable<EmpresaAspnetUsers_Meta>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_EMPRESA_ASPNETUSERS_METAS; }
                try { this.Connection.CreateTable<EmpresaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_EMPRESA; }
                try { this.Connection.CreateTable<OmieConfiguracaoGeralModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_OMIE_CONFIGURACOES_GERAIS; }
                try { this.Connection.CreateTable<ConfiguracaoGeralModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CONFIGURACOES_GERAIS; }
                try { this.Connection.CreateTable<TabelaEscalonadaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELAESCALONADA; }
                try { this.Connection.CreateTable<TabelaEscalonadaFaixaComissaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELAESCALONADA_FAIXACOMISSAO; }
                try { this.Connection.CreateTable<TabelaEscalonadaRepresentanteModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELAESCALONADA_REPRESENTANTE; }
                try { this.Connection.CreateTable<StatusModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_STATUS; }
                try { this.Connection.CreateTable<RecebimentoTitulosPostModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_RECEBIMENTOTITULOS_POST; }
                try { this.Connection.CreateTable<RecebimentoTitulosModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_RECEBIMENTOTITULOS; }
                try { this.Connection.CreateTable<RecebimentoTitulosMovimentacaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_RECEBIMENTOTITULOS_MOVIMENTACOES; }
                // PERFORMANCE: índice p/ acelerar as somas de títulos por pedido na listagem (PedidoRepository.GetInfinit).
                // Sem ele, cada subquery de soma varre a tabela inteira, 1x por pedido -> listagem travava ~48s.
                try { this.Connection.CreateIndex("IDX_RECTITULOS_PEDIDO_EMPRESA", TableMobile.TB_RECEBIMENTOTITULOS, new string[] { "idPedidoVenda", "idEmpresa" }); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - IDX_RECTITULOS_PEDIDO_EMPRESA"; }
                try { this.Connection.CreateTable<ForceAtualizacaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_FORCEATUALIZACAO; }
                try { this.Connection.CreateTable<LogExclusaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_LOGEXCLUSAO; }
                try { this.Connection.CreateTable<ConfiguracaoEspecificaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CONFIGURACOES_ESPECIFICAS; }
                try { this.Connection.CreateTable<IntegracaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_INTEGRACAO; }

                try { this.Connection.CreateTable<RepresentadaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REPRESENTADA; }
                try { this.Connection.CreateTable<RepresentadaAspnetUsersModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REPRESENTADA_ASPNETUSERS; }
                try { this.Connection.CreateTable<EnderecoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_ENDERECO; }
                try { this.Connection.CreateTable<ContatoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CONTATOS; }
                try { this.Connection.CreateTable<ClientesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CLIENTES; }
                try { this.Connection.CreateTable<ClientesCondicoesPagamentoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CLIENTES_CONDICOESPAGAMENTO; }
                try { this.Connection.CreateTable<CategoriaProdutoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CATEGORIA; }
                try { this.Connection.CreateTable<CondicaoPagamentoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CONDICAOPAGAMENTO; }
                try { this.Connection.CreateTable<RamoAtividadeModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_RAMOATIVIDADE; }
                try { this.Connection.CreateTable<ImagemModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_IMAGEM; }
                try { this.Connection.CreateTable<GradeCorModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_GRADECOR; }
                try { this.Connection.CreateTable<GradesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_GRADES; }
                try { this.Connection.CreateTable<GradesComposicaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_GRADES_COMPOSICAO; }
                try { this.Connection.CreateTable<GradeVariacaoProdutoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_PRODUTO_GRADES; }
                try { this.Connection.CreateTable<GradeVariacaoProdutoComposicaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_PRODUTO_GRADES_COMPOSICAO; }
                try { this.Connection.CreateTable<GradeTamanhoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_GRADETAMANHO; }
                try { this.Connection.CreateTable<UnidadeMedidaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_UNIDADEMEDIDA; }
                try { this.Connection.CreateTable<ProdutoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_PRODUTO; }
                try { this.Connection.CreateTable<EstoqueModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_MOVIMENTOESTOQUE; }
                try { this.Connection.CreateTable<LocalEstoqueModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_LOCAL_ESTOQUE; }
                try { this.Connection.CreateTable<LocalEstoqueClientesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_LOCAL_ESTOQUE_CLIENTES; }
                try { this.Connection.CreateTable<LocalEstoqueRepresentantesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_LOCAL_ESTOQUE_REPRESENTANTES; }
                try { this.Connection.CreateTable<LocalEstoqueUfModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_LOCAL_ESTOQUE_UF; }
                try { this.Connection.CreateTable<LocalEstoqueClienteRamoAtividadesDataModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_LOCAL_ESTOQUE_RAMOSATIVIDADES; }
                try { this.Connection.CreateTable<EstoqueInsuficienteModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + "TB_ESTOQUE_INSUFICIENTE"; }
                try { this.Connection.CreateTable<TransportadorasModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TRANSPORTADORAS; }
                try { this.Connection.CreateTable<TabelaPrecoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELAPRECO; }
                try { this.Connection.CreateTable<TabelaPrecoItemModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELAPRECOITEM; }
                try { this.Connection.CreateTable<TabelaPrecoClientesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELA_PRECO_CLIENTES; }
                try { this.Connection.CreateTable<TabelaPrecoRepresentacoesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELAPRECO_REPRESENTACOES; }
                try { this.Connection.CreateTable<TabelaPrecoRepresentantesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELA_PRECO_REPRESENTANTES; }
                try { this.Connection.CreateTable<PedidoVendaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_PEDIDOVENDA; }
                try { this.Connection.CreateTable<PedidoVendaItensModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_PEDIDOVENDAITENS; }
                try { this.Connection.CreateTable<SincronizacaoInicialEstoque>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_SINCRONIZACAOESTOQUE; }
                try { this.Connection.CreateTable<EmailPedidoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + "EmailPedidoModel"; }
                try { this.Connection.CreateTable<AtividadeAgendaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_ATIVIDADES; }
                try { this.Connection.CreateTable<AnexosModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_ANEXOS; }
                try { this.Connection.CreateTable<TipoAtividadeAgendaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TIPOATIVIDADESCRM; }
                try { this.Connection.CreateTable<JornadaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_JORNADA_TRABALHO; }
                try { this.Connection.CreateTable<JornadaHorariosModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_JORNADA_TRABALHO_HORARIOS; }
                try { this.Connection.CreateTable<FormaPagamentoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_FORMA_PAGAMENTO; }
                try { this.Connection.CreateTable<StatusRepresentanteProibido>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_STATUS_PROIBIDO; }
                try { this.Connection.CreateTable<PermissoesRepresentantesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_PERMISSOES_REPRESENTANTES; }
                try { this.Connection.CreateTable<EquipeRepresentantesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_EQUIPE_REPRESENTANTES; }

                try { this.Connection.CreateTable<CidadesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CIDADES; }

                try { this.Connection.CreateTable<RegrasComerciaisModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS; }
                try { this.Connection.CreateTable<RcFaixasModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_FAIXAS; }
                try { this.Connection.CreateTable<RcCriteriosModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS; }
                try { this.Connection.CreateTable<RccProdutosModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_PRODUTOS; }
                try { this.Connection.CreateTable<RcFaixasCriterioVinculoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_FAIXAS_CRITERIO_VINCULO; }


                try { this.Connection.CreateTable<RccClientesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_CLIENTES; }
                try { this.Connection.CreateTable<RccCategoriaProdutoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_CATEGORIAPRODUTO; }
                try { this.Connection.CreateTable<RccCondicaoPagamentoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_CONDICAOPAGAMENTO; }
                try { this.Connection.CreateTable<RccRamoAtividadesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_RAMOATIVIDADES; }
                try { this.Connection.CreateTable<RccUfModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_UF; }
                try { this.Connection.CreateTable<RccRepresentadasModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_REPRESENTADAS; }
                try { this.Connection.CreateTable<RccTabelaPrecoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_TABELAPRECO; }

                try
                {
                    this.Connection.CreateTable<ClienteRamosAtividade>();
                }
                catch (Exception ex)
                {
                    App.xErrorDataBase += ex.Message + " - " + TableMobile.tb_clienteramosatividade;
                }

                try
                {
                    this.Connection.CreateTable<TabelaPrecoClienteUfModel>();
                }
                catch (Exception ex)
                {
                    App.xErrorDataBase += ex.Message + " - " + "Tabela Preco Cliente UF";
                }
                try
                {
                    this.Connection.CreateTable<TabelaPrecoClienteRamoModel>();
                }
                catch (Exception ex)
                {
                    App.xErrorDataBase += ex.Message + " - " + "Tabela Preco Cliente Ramo";
                }

                // Índices para acelerar a listagem de pedidos com volume alto (evita full scan/sort na tela "Pedidos")
                try { this.Connection.Execute("CREATE INDEX IF NOT EXISTS ix_pv_listagem ON tb_pedidovenda (idEmpresa, idRepresentantePedido, idClientesOffLine)"); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - ix_pv_listagem"; }
                try { this.Connection.Execute("CREATE INDEX IF NOT EXISTS ix_pv_idpedidodisplay ON tb_pedidovenda (idPedidoDisplay)"); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - ix_pv_idpedidodisplay"; }
                try { this.Connection.Execute("CREATE INDEX IF NOT EXISTS ix_estoque_insuf_pv ON TB_ESTOQUE_INSUFICIENTE (idPedidoVendaOffLine)"); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - ix_estoque_insuf_pv"; }
            }
            catch (Exception ex)
            {
                App.xErrorDataBase += ex.Message + " - " + TableMobile.CurrentUserLogin;
            }
        }

        public async Task SegundaAnalise()
        {
            await Task.Run(() =>
            {
                try
                {

                    try { this.Connection.CreateTable<RecebimentoTitulosModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_RECEBIMENTOTITULOS; }
                    try { this.Connection.CreateTable<RecebimentoTitulosMovimentacaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_RECEBIMENTOTITULOS_MOVIMENTACOES; }
                }
                catch (Exception ex)
                {
                    App.xErrorDataBase += ex.Message + " - " + TableMobile.CurrentUserLogin;
                }
            });
        }




        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
