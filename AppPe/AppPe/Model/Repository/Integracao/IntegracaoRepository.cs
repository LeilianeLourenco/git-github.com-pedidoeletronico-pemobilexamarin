using System;
using Xamarin.HLP.Mobile.AppPE.Model.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Integracao
{
    public class IntegracaoRepository
    {
        IntegracaoModel test = new IntegracaoModel();

        public DateTime getDataUltimaIntegracao(int idEmpresa, string xTabela)
        {
            try
            {
                var xQuery = string.Format($"select dtUltimaSincronizacao from TB_INTEGRACAO where idEmpresa = {idEmpresa} and xTabela = '{xTabela}'");

                var resultado = App.Data.Connection.ExecuteScalar<string>(xQuery);
                return Convert.ToDateTime(resultado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int getRegistroTabelaIntegracao(string xTabela, int idEmpresa)
        {
            try
            {
                int idIntegracao = 0;

                idIntegracao = App.Data.Connection.ExecuteScalar<int>($"select idIntegracao from TB_INTEGRACAO where idEmpresa = {idEmpresa} and xTabela = '{xTabela}'");

                if (idIntegracao == 0)
                {
                    App.Data.Connection.Execute($"insert into TB_INTEGRACAO (xTabela, idEmpresa) values ('{xTabela}', {idEmpresa})");

                    idIntegracao = App.Data.Connection.ExecuteScalar<int>($"select idIntegracao from TB_INTEGRACAO where idEmpresa = {idEmpresa} and xTabela = '{xTabela}'");
                }

                return idIntegracao;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // dtCheckpoint: quando a rodada baixou só uma PARTE do que havia pendente (ex.: limite de
        // registros por rodada, ver SincronizacaoDownloadPedido), grava o checkpoint na data do
        // último registro realmente baixado em vez de "agora" — senão a próxima rodada pularia
        // direto pra frente e nunca baixaria o que ficou faltando.
        public void AtualizarDataIntegracao(string xTabela, int idEmpresa, bool falhaConexao, bool falhaErro, string xLogIntegracao = "", DateTime? dtCheckpoint = null)
        {
            bool Resultado = false;
            xLogIntegracao = xLogIntegracao.Replace("'", "");

            if (falhaConexao || falhaErro)
                Resultado = true;

            try
            {
                int idIntegracao = getRegistroTabelaIntegracao(xTabela, idEmpresa);

                string setQuery = string.Empty;

                if (Resultado)
                    setQuery = $" xLogIntegracao = '{xLogIntegracao}' ";
                else
                    setQuery = $" dtUltimaSincronizacao = '{(dtCheckpoint ?? DateTime.UtcNow).ToString("yyyy-MM-dd HH:mm:ss")}'";


                App.Data.Connection.Execute($"update TB_INTEGRACAO set {setQuery} where idIntegracao = {idIntegracao}");                  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
