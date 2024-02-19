using System;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.Model.Empresa;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;
using Xamarin.HLP.Mobile.AppPE.Model.Financeiro;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    public class TableMobile
    {
        public const string TB_STATUS_PROIBIDO = "TB_STATUS_PROIBIDO";
        public const string TB_SINCRONIZACAOESTOQUE = "TB_SINCRONIZACAOESTOQUE";
        public const string TB_ATIVIDADES = "TB_ATIVIDADES"; //agenda
        public const string TB_TIPOATIVIDADESCRM = "TB_TIPOATIVIDADESCRM"; //agenda
        public const string TB_EXTENSAO = "TB_EXTENSAO";
        public const string TB_FORCEATUALIZACAO = "TB_FORCEATUALIZACAO";
        public const string TB_STATUS = "TB_STATUS";
        public const string TB_CONDICAOPAGAMENTO = "TB_CONDICAOPAGAMENTO";
        public const string TB_RECEBIMENTOTITULOS = "TB_RECEBIMENTOTITULOS";
        public const string TB_RECEBIMENTOTITULOS_MOVIMENTACOES = "TB_RECEBIMENTOTITULOS_MOVIMENTACOES";
        public const string TB_CONFIGURACOES_GERAIS = "TB_CONFIGURACOES_GERAIS";
        public const string TB_CONFIGURACOES_ESPECIFICAS = "TB_CONFIGURACOES_ESPECIFICAS";
        public const string TB_JORNADA_TRABALHO = "TB_JORNADA_TRABALHO";
        public const string TB_JORNADA_TRABALHO_HORARIOS = "TB_JORNADA_TRABALHO_HORARIOS";
        public const string TB_FORMA_PAGAMENTO = "TB_FORMA_PAGAMENTO";

        public const string TB_CATEGORIA = "TB_CATEGORIA";
        public const string TB_UNIDADEMEDIDA = "TB_UNIDADEMEDIDA";
        public const string TB_LOGSYNC = "TB_LOGSYNC";
        public const string TB_LOGEXCLUSAO = "TB_LOGEXCLUSAO";
        public const string TB_RAMOATIVIDADE = "TB_RAMOATIVIDADE";
        public const string TB_IMAGEM = "TB_IMAGEM";
        public const string TB_TRANSPORTADORAS = "TB_TRANSPORTADORAS";
        public const string TB_PEDIDOVENDAITENS = "TB_PEDIDOVENDAITENS";
        public const string TB_REPRESENTADA_ASPNETUSERS = "TB_REPRESENTADA_ASPNETUSERS";
        public const string AspNetUsersRoles = "AspNetUsersRoles";
        public const string TB_PEDIDOVENDA = "TB_PEDIDOVENDA";
        public const string TB_PRODUTO = "TB_PRODUTO";
        public const string TB_MOVIMENTOESTOQUE = "TB_MOVIMENTOESTOQUE";
        public const string TB_LOCAL_ESTOQUE = "TB_LOCAL_ESTOQUE";
        public const string TB_LOCAL_ESTOQUE_CLIENTES = "TB_LOCAL_ESTOQUE_CLIENTES";
        public const string TB_LOCAL_ESTOQUE_REPRESENTANTES = "TB_LOCAL_ESTOQUE_REPRESENTANTES";
        public const string TB_LOCAL_ESTOQUE_UF = "TB_LOCAL_ESTOQUE_UF";
        public const string TB_LOCAL_ESTOQUE_RAMOSATIVIDADES = "TB_LOCAL_ESTOQUE_RAMOSATIVIDADES";
        public const string TB_EMPRESA = "TB_EMPRESA";
        public const string TB_EMPRESA_ASPNETUSERS_METAS = "TB_EMPRESA_ASPNETUSERS_METAS";
        public const string TB_EMPRESA_ASPNETUSERS = "TB_EMPRESA_ASPNETUSERS";
        public const string TB_PERMISSOES_REPRESENTANTES = "TB_PERMISSOES_REPRESENTANTES";       
        public const string TB_EQUIPE_REPRESENTANTES = "TB_EQUIPE_REPRESENTANTES";
        public const string TB_REPRESENTADA = "TB_REPRESENTADA";
        public const string TB_ENDERECO = "TB_ENDERECO";
        public const string TB_CONTATOS = "TB_CONTATOS";
        public const string TB_CLIENTES = "TB_CLIENTES";
        public const string tb_produto_codigocliente = "tb_produto_codigocliente";
        public const string TB_TABELAESCALONADA = "TB_TABELAESCALONADA";
        public const string TB_TABELAESCALONADA_FAIXACOMISSAO = "TB_TABELAESCALONADA_FAIXACOMISSAO";
        public const string TB_TABELAESCALONADA_REPRESENTANTE = "TB_TABELAESCALONADA_REPRESENTANTE";
        public const string TB_GRADETAMANHO = "TB_GRADETAMANHO";
        public const string TB_GRADECOR = "TB_GRADECOR";
        public const string TB_PRODUTO_GRADES = "TB_PRODUTO_GRADES";
        public const string TB_PRODUTO_GRADES_COMPOSICAO = "TB_PRODUTO_GRADES_COMPOSICAO";
        public const string TB_TABELAPRECO = "TB_TABELAPRECO";
        public const string TB_TABELAPRECOITEM = "TB_TABELAPRECOITEM";
        public const string TB_TABELA_PRECO_CLIENTES = "TB_TABELA_PRECO_CLIENTES";
        public const string TB_TABELA_PRECO_REPRESENTANTES = "TB_TABELA_PRECO_REPRESENTANTES";
        public const string TB_TABELAPRECO_REPRESENTACOES = "TB_TABELAPRECO_REPRESENTACOES";
        public const string TB_INTEGRACAO = "TB_INTEGRACAO";
        public const string AspNetUsers = "AspNetUsers";

        public const string CurrentUserLogin = "CurrentUserLogin";
        public const string AspNetUsersClaims = "AspNetUsersClaims";
        public const string AspNetUsersLogins = "AspNetUsersLogins";
        public const string AspNetRoles = "AspNetRoles";

        public const string tb_tabelapreco_uf_cliente = "tb_tabelapreco_uf_cliente";
        public const string tb_tabelapreco_ramoatividade_cliente = "tb_tabelapreco_ramoatividade_cliente";
        public const string tb_clienteramosatividade = "tb_clientesramosatividade";

        public static string GetTableNameByModel<T>() where T : class
        {
            return GetInfoModel<T>(tipoRetornoInfoClass: UtilHttp.TipoRetornoInfoClass.TableName);
        }
        public static string GetPrimaryKeyNameByModel<T>() where T : class
        {
            return GetInfoModel<T>(tipoRetornoInfoClass: UtilHttp.TipoRetornoInfoClass.PrimaryKey);
        }
        public static string GetApiRegistroByModel<T>() where T : class
        {
            return GetInfoModel<T>(tipoRetornoInfoClass: UtilHttp.TipoRetornoInfoClass.ApiRegistro);
        }


        public static string GetInfoModel<T>(UtilHttp.TipoRetornoInfoClass tipoRetornoInfoClass = UtilHttp.TipoRetornoInfoClass.TableName) where T : class
        {
            #region dados
            var classe = typeof(T);
            if (classe == typeof(PedidoVendaModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idPedidoVenda";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIpedidoVenda";
                    default:
                        return TB_PEDIDOVENDA;
                }
            if (classe == typeof(CondicaoPagamentoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idCondicaoPagamento";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIprazo";
                    default:
                        return TB_CONDICAOPAGAMENTO;
                }
            if (classe == typeof(tb_produto_codigocliente))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idCodigo";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIprodutoCodigoCliente";
                    default:
                        return tb_produto_codigocliente;
                }
            if(classe == typeof(IntegracaoModel))
                switch(tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idIntegracao";
                    default:
                        return TB_INTEGRACAO;
                }
            if (classe == typeof(ExtensaoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idEmpresa";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiExtensoes";
                    default:
                        return TB_EXTENSAO;
                }
            if (classe == typeof(RecebimentoTitulosModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idRecebimentoTitulo";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiRecebimentoTitulos";
                    default:
                        return TB_RECEBIMENTOTITULOS;
                }
            if (classe == typeof(RecebimentoTitulosMovimentacaoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idRecebimentoTituloMovimentacao";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiRecebimentoTitulosMovimentacao";
                    default:
                        return TB_RECEBIMENTOTITULOS_MOVIMENTACOES;
                }
            if (classe == typeof(StatusModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idStatus";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiStatusVenda";
                    default:
                        return TB_STATUS;
                }
            if (classe == typeof(AtividadeAgendaModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idAtividade";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiAtividadeAgendaMobile";
                    default:
                        return TB_ATIVIDADES;
                }
            if (classe == typeof(TipoAtividadeAgendaModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTipoAtividade";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiAtividadeAgendaMobile";
                    default:
                        return TB_TIPOATIVIDADESCRM;
                }
            if (classe == typeof(CategoriaProdutoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idCategoria";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APICategoriaProduto";
                    default:
                        return TB_CATEGORIA;
                }
            if (classe == typeof(ClientesModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idClientes";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIcliente";
                    default:
                        return TB_CLIENTES;
                }
            if (classe == typeof(ContatoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idContatos";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIcontato";
                    default:
                        return TB_CONTATOS;
                }
            if (classe == typeof(PermissoesRepresentantesModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idPermissaoUsuario";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIpermissoesrepresentantes";
                    default:
                        return TB_PERMISSOES_REPRESENTANTES;
                }
            if (classe == typeof(EquipeRepresentantesModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idEquipeRepresentante";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIequiperepresentantes";
                    default:
                        return TB_EQUIPE_REPRESENTANTES;
                }
            if (classe == typeof(EmpresaAspnetUsersModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idEmpresa_aspnetUsers";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIempresaAspnetUsers";
                    default:
                        return TB_EMPRESA_ASPNETUSERS;
                }

            if (classe == typeof(EmpresaAspnetUsers_Meta))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idEmpresaAspnetUsers_Meta";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIempresaAspnetUsers";
                    default:
                        return TB_EMPRESA_ASPNETUSERS_METAS;
                }

            if (classe == typeof(AspNetUsersModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIAspnetUsers";
                    default:
                        return AspNetUsers;
                }
            if (classe == typeof(EmpresaModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idEmpresa";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIEmpresa";
                    default:
                        return TB_EMPRESA;
                }
            if (classe == typeof(ConfiguracaoGeralModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idConfiguracaoGeral";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIConfiguracoesGerais";
                    default:
                        return TB_CONFIGURACOES_GERAIS;
                }
            if (classe == typeof(ConfiguracaoEspecificaModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idConfiguracaoEspecifica";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiConfiguracoesEspecificas";
                    default:
                        return TB_CONFIGURACOES_ESPECIFICAS;
                }
            if (classe == typeof(JornadaModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idJornada";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiJornadaTrabalho";
                    default:
                        return TB_JORNADA_TRABALHO;
                }
            if (classe == typeof(JornadaHorariosModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idJornadaDia";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiJornadaTrabalhoHorarios";
                    default:
                        return TB_JORNADA_TRABALHO_HORARIOS;
                }
            if (classe == typeof(FormaPagamentoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idFormaPagamento";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiFormasPagamento";
                    default:
                        return TB_FORMA_PAGAMENTO;
                }

            if (classe == typeof(EstoqueModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idMovimentoEstoqueMobile";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiEstoque";
                    default:
                        return TB_MOVIMENTOESTOQUE;
                }
            if (classe == typeof(LocalEstoqueModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idLocalEstoque";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiEstoqueMobile";
                    default:
                        return TB_LOCAL_ESTOQUE;
                }
            if (classe == typeof(LocalEstoqueClientesModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idLocalEstoqueCliente";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiEstoqueMobile";
                    default:
                        return TB_LOCAL_ESTOQUE_CLIENTES;
                }
            if (classe == typeof(LocalEstoqueRepresentantesModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idLocalEstoqueRepresentante";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiEstoqueMobile";
                    default:
                        return TB_LOCAL_ESTOQUE_REPRESENTANTES;
                }
            if (classe == typeof(LocalEstoqueUfModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idLocalEstoqueUf";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiEstoqueMobile";
                    default:
                        return TB_LOCAL_ESTOQUE_UF;
                }
            if (classe == typeof(LocalEstoqueClienteRamoAtividadesDataModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idLocalEstoqueRamoAtividade";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiEstoqueMobile";
                    default:
                        return TB_LOCAL_ESTOQUE_RAMOSATIVIDADES;
                }
            if (classe == typeof(EnderecoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idEndereco";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIendereco";
                    default:
                        return TB_ENDERECO;
                }
            if (classe == typeof(EstoqueInsuficienteModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idEstoqueInsuficiente";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "";
                    default:
                        return "";
                }
            if (classe == typeof(SincronizacaoInicialEstoque))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idSincEstIni";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "";
                    default:
                        return "";
                }
            if (classe == typeof(GradeCorModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idGradeCor";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIgradeCor";
                    default:
                        return TB_GRADECOR;
                }
            if (classe == typeof(GradeVariacaoProdutoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idGradeProduto";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiVariacaoGradesMobile";
                    default:
                        return TB_PRODUTO_GRADES;
                }
            if (classe == typeof(GradeVariacaoProdutoComposicaoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idGradeProdutoComposicao";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiVariacaoGradesComposicaoMobile";
                    default:
                        return TB_PRODUTO_GRADES_COMPOSICAO;
                }
            if (classe == typeof(GradeTamanhoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idGradeTamanho";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIgradeTamanho";
                    default:
                        return TB_GRADETAMANHO;
                }
            if (classe == typeof(ImagemModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idImagem";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIimagem";
                    default:
                        return TB_IMAGEM;
                }
            if (classe == typeof(LogExclusaoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idLogExclusao";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "";
                    default:
                        return TB_LOGEXCLUSAO;
                }
            if (classe == typeof(AspNetUsersModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "Id";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "";
                    default:
                        return AspNetUsers;
                }
            if (classe == typeof(ProdutoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idProduto";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIproduto";
                    default:
                        return TB_PRODUTO;
                }
            if (classe == typeof(RamoAtividadeModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idRamoAtividade";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIRamoAtividade";
                    default:
                        return TB_RAMOATIVIDADE;
                }
            if (classe == typeof(RepresentadaAspnetUsersModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idRepresentada_aspnetUsers";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIrepresentadaAspnetUsers";
                    default:
                        return TB_REPRESENTADA_ASPNETUSERS;
                }
            if (classe == typeof(RepresentadaModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idRepresentada";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIrepresentada";
                    default:
                        return TB_REPRESENTADA;
                }
            if (classe == typeof(TabelaPrecoItemModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTabelaPrecoItem";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APItabelaPrecoItem";
                    default:
                        return TB_TABELAPRECOITEM;
                }
            if (classe == typeof(TabelaPrecoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTabelaPreco";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APItabelaPreco";
                    default:
                        return TB_TABELAPRECO;
                }
            if (classe == typeof(TabelaPrecoRepresentacoesModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTabelaPrecoRepresentacoes";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APItabelaPrecoRepresentacoes";
                    default:
                        return TB_TABELAPRECO_REPRESENTACOES;
                }
            if (classe == typeof(TabelaPrecoClientesModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTabelaPrecoClientes";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APItabelaPrecoClientes";
                    default:
                        return TB_TABELA_PRECO_CLIENTES;
                }
            if (classe == typeof(TabelaPrecoRepresentantesModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTabelaPrecoRepresentantes";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APItabelaPrecoRepresentantes";
                    default:
                        return TB_TABELA_PRECO_REPRESENTANTES;
                }
            if (classe == typeof(TransportadorasModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTransportadora";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APItransportadora";
                    default:
                        return TB_TRANSPORTADORAS;
                }
            if (classe == typeof(UnidadeMedidaModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idUnidadeMedida";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "APIunidademedida";
                    default:
                        return TB_UNIDADEMEDIDA;
                }

            if (classe == typeof(TabelaEscalonadaModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTabelaPrecoEscalonada";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiTabelaEscalonada";
                    default:
                        return TB_TABELAESCALONADA;
                }
            if (classe == typeof(TabelaEscalonadaRepresentanteModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTabelaEscalonadaRepresentante";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiTabelaEscalonadaRepresentante";
                    default:
                        return TB_TABELAESCALONADA_REPRESENTANTE;
                }
            if (classe == typeof(TabelaEscalonadaFaixaComissaoModel))
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTabelaEscalonadaFaixaComissao";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiTabelaEscalonadaComissao";
                    default:
                        return TB_TABELAESCALONADA_FAIXACOMISSAO;
                }
            if(classe == typeof(TabelaPrecoClienteUfModel))
            {
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        {
                            return "idTabelaPrecoUfCliente";
                        }
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        {
                            return "APItabelaPrecoUf";
                        }
                    default:
                        {
                            return tb_tabelapreco_uf_cliente;
                        }
                }
            }
            if(classe == typeof(TabelaPrecoClienteRamoModel))
            {
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idTabelaPrecoRamoAtividadeCliente";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        {
                            return "APITabelaprecoClienteRamo";
                        }
                    default:
                        {
                            return tb_tabelapreco_ramoatividade_cliente;
                        }
                }
            }

            if(classe == typeof(ClienteRamosAtividade))
            {
                switch (tipoRetornoInfoClass)
                {
                    case UtilHttp.TipoRetornoInfoClass.PrimaryKey:
                        return "idClienteRamoAtividade";
                    case UtilHttp.TipoRetornoInfoClass.ApiRegistro:
                        return "ApiClienteRamosAtividade";
                    default:
                        return tb_clienteramosatividade;
                }
            }

            throw new Exception("Tipo de retorno info class não implementado.");

            #endregion

        }




    }

}
