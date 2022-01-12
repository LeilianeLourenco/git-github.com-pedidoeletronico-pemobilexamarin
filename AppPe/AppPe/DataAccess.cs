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
using Xamarin.HLP.Mobile.AppPE.Model.Empresa;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;
using Xamarin.HLP.Mobile.AppPE.Model.Financeiro;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
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
                    this.Connection.CreateTable<ExtensaoModel>();
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
                try { this.Connection.CreateTable<ConfiguracaoGeralModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CONFIGURACOES_GERAIS; }
                try { this.Connection.CreateTable<TabelaEscalonadaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELAESCALONADA; }
                try { this.Connection.CreateTable<TabelaEscalonadaFaixaComissaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELAESCALONADA_FAIXACOMISSAO; }
                try { this.Connection.CreateTable<TabelaEscalonadaRepresentanteModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TABELAESCALONADA_REPRESENTANTE; }
                try { this.Connection.CreateTable<StatusModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_STATUS; }
                try { this.Connection.CreateTable<RecebimentoTitulosModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_RECEBIMENTOTITULOS; }
                try { this.Connection.CreateTable<RecebimentoTitulosMovimentacaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_RECEBIMENTOTITULOS_MOVIMENTACOES; }
                try { this.Connection.CreateTable<ForceAtualizacaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_FORCEATUALIZACAO; }
                try { this.Connection.CreateTable<LogExclusaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_LOGEXCLUSAO; }
                try { this.Connection.CreateTable<ConfiguracaoEspecificaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CONFIGURACOES_ESPECIFICAS; }
                try { this.Connection.CreateTable<IntegracaoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_INTEGRACAO; }

                try { this.Connection.CreateTable<RepresentadaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REPRESENTADA; }
                try { this.Connection.CreateTable<RepresentadaAspnetUsersModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_REPRESENTADA_ASPNETUSERS; }
                try { this.Connection.CreateTable<EnderecoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_ENDERECO; }
                try { this.Connection.CreateTable<ContatoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CONTATOS; }
                try { this.Connection.CreateTable<ClientesModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CLIENTES; }
                try { this.Connection.CreateTable<CategoriaProdutoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CATEGORIA; }
                try { this.Connection.CreateTable<CondicaoPagamentoModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_CONDICAOPAGAMENTO; }
                try { this.Connection.CreateTable<RamoAtividadeModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_RAMOATIVIDADE; }
                try { this.Connection.CreateTable<ImagemModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_IMAGEM; }
                try { this.Connection.CreateTable<GradeCorModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_GRADECOR; }
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
                try { this.Connection.CreateTable<TipoAtividadeAgendaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TIPOATIVIDADESCRM; }
                try { this.Connection.CreateTable<JornadaModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TIPOATIVIDADESCRM; }
                try { this.Connection.CreateTable<JornadaHorariosModel>(); } catch (Exception ex) { App.xErrorDataBase += ex.Message + " - " + TableMobile.TB_TIPOATIVIDADESCRM; }
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
