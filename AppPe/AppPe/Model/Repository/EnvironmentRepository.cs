using System;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{

    /// <summary>
    /// Classe criada para regras do ambiente mobile
    /// </summary>
    public static class EnvironmentRepository
    {

        /// <summary>
        /// Exclui todas as tabelas do aplicativo para fazer a sincronização inicial
        /// </summary>
        /// <returns></returns>
        public static bool ExcluirTodosRegistros()
        {
            try
            {
                var xQuery =
                     $@"DELETE FROM TB_RECEBIMENTOTITULOS_MOVIMENTACOES WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_RECEBIMENTOTITULOS WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);


                xQuery =
                      $@"DELETE FROM TB_MOVIMENTOESTOQUE WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";


                App.Data.Connection.Execute(xQuery);

                xQuery =
                     $@"DELETE FROM TB_PEDIDOVENDAITENS WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);


                xQuery =
                      $@"DELETE FROM TB_PEDIDOVENDA WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                       $@"DELETE FROM TB_ENDERECO WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);


                xQuery =
                      $@"DELETE FROM TB_CONTATOS WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);


                xQuery =
                      $@"DELETE FROM TB_CLIENTES WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_PRODUTO WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);



                xQuery =
                    $@"DELETE FROM TB_STATUS WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);


                xQuery =
                     $@"DELETE FROM TB_CONDICAOPAGAMENTO WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);


                xQuery =
                      $@"DELETE FROM TB_CONFIGURACOES_GERAIS WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_CATEGORIA WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_UNIDADEMEDIDA WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_LOGEXCLUSAO WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_RAMOATIVIDADE WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_IMAGEM WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_TRANSPORTADORAS WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_REPRESENTADA_ASPNETUSERS";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_EMPRESA WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_REPRESENTADA WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM tb_produto_codigocliente WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_TABELAESCALONADA WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_TABELAESCALONADA_FAIXACOMISSAO WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_TABELAESCALONADA_REPRESENTANTE WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_GRADETAMANHO WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_GRADECOR WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_TABELAPRECO WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_TABELAPRECOITEM WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_TABELA_PRECO_CLIENTES WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_TABELA_PRECO_REPRESENTANTES WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);


                xQuery =
                      $@"DELETE FROM TB_TABELAPRECO_REPRESENTACOES WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM tb_tabelapreco_uf_cliente WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM tb_tabelapreco_ramoatividade_cliente WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM tb_clientesramosatividade WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                      $@"DELETE FROM TB_TABELAPRECOITEM WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);


                xQuery =
                      $@"DELETE FROM TB_TABELAPRECOITEM WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);


                xQuery =
                      $"DELETE FROM TB_SINCRONIZACAOESTOQUE WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);


                xQuery =
                    $"DELETE FROM TB_EXTENSAO_EMPRESA WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                    $@"DELETE FROM TB_FORCEATUALIZACAO";

                App.Data.Connection.Execute(xQuery);

                xQuery =
                    $@"DELETE FROM TB_PERMISSOES_REPRESENTANTES WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);

                return true;
            }
            catch (Exception ex)
            {
                ex.TrakException("RemoveEstoquePedido");
            }

            //se chegar aqui é porque algo falhou
            return false;
        }

        public static void ExcluirRegistrosNecessarios(string nomeApi, object idEmpresa)
        {
            if (nomeApi == "APIequiperepresentantes")
            {
                string xQuery = $@"DELETE FROM TB_EQUIPE_REPRESENTANTES WHERE idEmpresa = {idEmpresa}";

                App.Data.Connection.Execute(xQuery);
            }
            if (nomeApi == "APItabelaPrecoRepresentantes")
            {
                string xQuery = $@"DELETE FROM TB_TABELA_PRECO_REPRESENTANTES WHERE idEmpresa = {idEmpresa}";

                App.Data.Connection.Execute(xQuery);
            }
            if (nomeApi == "APItabelaPrecoUf")
            {
                string xQuery = $@"DELETE FROM tb_tabelapreco_uf_cliente WHERE idEmpresa = {idEmpresa}";

                App.Data.Connection.Execute(xQuery);
            }
            if (nomeApi == "APITabelaprecoClienteRamo")
            {
                string xQuery = $@"DELETE FROM tb_tabelapreco_ramoatividade_cliente WHERE idEmpresa = {idEmpresa}";

                App.Data.Connection.Execute(xQuery);
            }
        }
    }
}
