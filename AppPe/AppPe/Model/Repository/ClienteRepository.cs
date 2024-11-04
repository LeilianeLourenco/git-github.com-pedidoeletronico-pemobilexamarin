using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using static Xamarin.HLP.Mobile.AppPE.Model.Cadastros.ClientesModel;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class ClienteRepository
    {

        /// <summary>
        /// GET todos os registros de cliente ( BasicPickerModel )
        /// </summary>
        /// <param name="bAtivo">True - traz somente clientes ativos, False - todos os clientes</param>
        /// <returns></returns>
        public static List<BasicPickerModel> GetAll()
        {
            var xQuery = "";
            const string xFields =
                "idClientesOffLine Id, xRazaoSocial Display, xFantasia Detail , stAtivo ,  idClientes IdOnline, bProblemaSincronizacao ";
            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAcessoTodosClientes == 1)
                xQuery =
                    $"select {xFields} from {TableMobile.TB_CLIENTES} where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            else
                xQuery =
                    $"select {xFields} from {TableMobile.TB_CLIENTES} where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and (idEmpresa_aspnetUsers = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers} or idEmpresa_aspnetUsers  is null )";
            var result = App.Data.Connection.Query<BasicPickerModel>(xQuery);
            return result;
        }


        public static ClientesModel FirstCliente()
        {
            return
                App.Data.Connection.Table<ClientesModel>()
                    .FirstOrDefault(c => c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
        }

        public static int FirstIdCliente()
        {
            return
                App.Data.Connection.Table<ClientesModel>()
                    .FirstOrDefault(c => c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa)
                    .idClientesOffLine ?? 0;
        }

        public static ClientesModel Save(ClientesModel objClienteModel)
        {
            try
            {
                objClienteModel.idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;


                if (string.IsNullOrEmpty(objClienteModel.cAlternativo))
                {
                    var _countClientes = App.Data.Connection
                        .Table<ClientesModel>()
                        .Count(c => c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);

                    objClienteModel.cAlternativo = _countClientes.ToString() + "A";
                }

                if (objClienteModel.idRedespacho == 0)
                    objClienteModel.idRedespacho = null;

                //só se aplica quando for novo
                int _idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                bool? bAplicaMelhoriaTl = ConfiguracaoGeralRepositorio.GetMelhoriaEspecificaTl(_idEmpresa);
                if (bAplicaMelhoriaTl.GetValueOrDefault() && objClienteModel.idClientes.GetValueOrDefault() == 0)
                {
                    var _estadoCliente = objClienteModel.lEndereco.Select(t => t.xEstado).FirstOrDefault();
                    if (!string.IsNullOrEmpty(_estadoCliente))
                    {
                        var idTransportadora = App.Data.Connection.Table<EnderecoModel>()
                            .FirstOrDefault(c => c.idEmpresa == _idEmpresa && c.idTransportadora > 0 && c.xEstado == _estadoCliente)
                            .idTransportadora ?? 0;

                        if (idTransportadora > 0)
                        {
                            objClienteModel.idTransportadora = idTransportadora;
                        }
                    }
                }

                if (objClienteModel.idClientesOffLine == null)
                {
                    if (objClienteModel.idAspnetUsers == null)
                        objClienteModel.idAspNetUserInclusao =
                            objClienteModel.idAspnetUsers = App.CurrentAspnetUserModel.Id;


                    objClienteModel.idEmpresa_aspnetUsers =
                        App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers;
                    objClienteModel.dtCadastro = DateTime.Now.ToUniversalTime();
                    objClienteModel.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                    App.Data.Connection.Insert(objClienteModel);
                }
                else
                {
                    objClienteModel.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                    App.Data.Connection.Update(objClienteModel);
                }

                if (objClienteModel.lContato.Any(c => c.idContatoOffLine == null))
                {
                    foreach (var contato in objClienteModel.lContato.Where(c => c.idContatoOffLine == null))
                    {
                        contato.idClientesOffLine = objClienteModel.idClientesOffLine;
                        contato.idClientes = objClienteModel.idClientes;
                        ContatoRepository.Save(contato);
                    }
                }
                if (objClienteModel.lEndereco.Any(c => c.idEnderecoOffLine == null))
                {
                    var datetime = DateTime.Now.ToUniversalTime();
                    foreach (var endereco in objClienteModel.lEndereco.Where(c => c.idEnderecoOffLine == null))
                    {
                        endereco.idClientesOffLine = objClienteModel.idClientesOffLine;
                        endereco.idClientes = objClienteModel.idClientes;
                        datetime = datetime.AddSeconds(1);
                        endereco.dtCadastro = datetime;
                        EnderecoRepository.Save(endereco);
                    }
                }
            }
            catch (Exception ex) // catch all other errors
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
                //Insights.Report(ex, Insights.Severity.Error);
            }
            return objClienteModel;
        }

        public static bool ClienteJaExiste(string xNome, int? idClientes = null)
        {
            var count = 0;
            if (idClientes != null)
                count =
                    App.Data.Connection
                        .Table<ClientesModel>()
                        .Count(c => c.xRazaoSocial == xNome && c.idClientesOffLine != idClientes && c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
            else
                count =
                    App.Data.Connection
                        .Table<ClientesModel>()
                        .Count(c => c.xRazaoSocial == xNome && c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);

            return count > 0;
        }

        public static bool CpfCnpjClienteJaExiste(string valor, int? idClientes = null)
        {
            var count = 0;

            var xQuery = "";
            if (idClientes != null)
                xQuery = $"SELECT COUNT(*) FROM {TableMobile.TB_CLIENTES} WHERE xCpfCnpj = '{valor}' and idClientesOffLine <> {(idClientes ?? 0)} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            else
                xQuery = $"SELECT COUNT(*) FROM {TableMobile.TB_CLIENTES} WHERE xCpfCnpj = '{valor}' and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

            count = App.Data.Connection.ExecuteScalar<int>(xQuery);

            return count > 0;

            //if (string.IsNullOrEmpty(valor)) return count > 0;
            //if (idClientes != null)
            //    count =
            //        App.Data.Connection
            //            .Table<ClientesModel>()
            //            .Count(c => c.xCpfCnpj == valor && c.idClientesOffLine != idClientes);
            //else
            //    count =
            //        App.Data.Connection
            //            .Table<ClientesModel>()
            //            .Count(c => c.xCpfCnpj == valor);

            //return count > 0;
        }

        public static bool RgIeClienteJaExiste(string valor, int? idClientes = null)
        {
            var count = 0;
            if (string.IsNullOrEmpty(valor)) return count > 0;
            if (idClientes != null)
                count =
                    App.Data.Connection
                        .Table<ClientesModel>()
                        .Count(c => c.xRgIe == valor && c.idClientesOffLine != idClientes);
            else
                count =
                    App.Data.Connection
                        .Table<ClientesModel>()
                        .Count(c => c.xRgIe == valor);

            return count > 0;

        }

        public static bool ClienteEstaEfetivado(int idClientesOffLine)
        {
            return
                App.Data.Connection.Table<ClientesModel>()
                    .FirstOrDefault(c => c.idClientesOffLine == idClientesOffLine).dEfetivacao != null;
        }

        public static ClientesModel GetClienteModel(int idClientesOffLine, bool bFull = true)
        {
            var xQuery =
                $@"SELECT * FROM {TableMobile.TB_CLIENTES} WHERE idClientesOffLine = {idClientesOffLine} 
                                                            and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var objCliente = new ClientesModel();
            var resultado = App.Data.Connection.Query<ClientesModel>(xQuery);
            if (resultado != null)
            {
                objCliente = resultado.FirstOrDefault();
                if (bFull)
                {
                    objCliente.lEndereco =
                        new ObservableCollection<EnderecoModel>(EnderecoRepository.GetAll(idClientesOffLine));
                    objCliente.lContato = new ObservableCollection<ContatoModel>(ContatoRepository.GetAll(idClientesOffLine));
                }
            }
            return objCliente;
        }



        public static List<ClientesModel> GetClientesModelsToSync()
        {
            var lUpload =
                App.Data.Connection.Table<ClientesModel>()
                    .Where(
                        c =>
                            ((c.dtUltimaAlteracao > App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime && c.dtUltimaAlteracao < DateTime.UtcNow)
                            || (c.idClientes == null))
                            && c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);


            var _list = lUpload.ToList();

            return _list;
        }

        public static string GetDisplayByIdOffLine(int idClientesOffLine)
        {

            var resultado =
                App.Data.Connection.Table<ClientesModel>()
                    .FirstOrDefault(
                        c =>
                            c.idClientesOffLine == idClientesOffLine &&
                            c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);

            return resultado != null ? resultado.xRazaoSocial : "";


        }

        public static string GetDisplayFantasiaByIdOffLine(int idClientesOffLine)
        {

            var resultado =
                App.Data.Connection.Table<ClientesModel>()
                    .FirstOrDefault(
                        c =>
                            c.idClientesOffLine == idClientesOffLine &&
                            c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);

            var retorno = resultado != null ? resultado.xFantasia : "";

            if (retorno.Length > 2)
            {
                var upper = retorno[0].ToString().ToUpper();

                var resto = retorno.Substring(1, (retorno.Length - 1)).ToLower();

                retorno = upper + resto;
            }
            return retorno;
        }

        public static string GetDisplayByIdOnLine(int idClientes)
        {

            var xquery =
                $@"select xRazaoSocial from tb_clientes
                        where idClientes  = {idClientes} and idEmpresa = {App
                    .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.ExecuteScalar<string>(xquery);
            return result;
        }

        public static double GetValorMinimoVenda(int idClientes)
        {

            var xquery =
                $@"select vLimiteMinimoVendas from tb_clientes
                        where idClientesOffLine  = {idClientes} and idEmpresa = {App
                    .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.ExecuteScalar<double>(xquery);
            return result;
        }

        public static void UpdateAfterUpload(ClientesModel objClienteModel)
        {

            //yyyy-MM-dd HH:mm:ss
            const string xQuery = @"UPDATE {0} SET idClientes = {1} 
                                        WHERE idClientesOffLine = {2} AND idEmpresa = {3}";

            App.Data.Connection.Execute(string.Format(xQuery,
                TableMobile.TB_CLIENTES,
                objClienteModel.idClientes,
                objClienteModel.idClientesOffLine,
                objClienteModel.idEmpresa));

            App.Data.Connection.Execute(string.Format(xQuery,
                TableMobile.TB_CONTATOS,
                objClienteModel.idClientes,
                objClienteModel.idClientesOffLine,
                objClienteModel.idEmpresa));

            App.Data.Connection.Execute(string.Format(xQuery,
                TableMobile.TB_ENDERECO,
                objClienteModel.idClientes,
                objClienteModel.idClientesOffLine,
                objClienteModel.idEmpresa));
        }

        public static int GetIdClienteNuvem(int idClientesOffLine)
        {

            var xquery =
               $@"select idClientes from tb_clientes
                        where idClientesOffLine  = {idClientesOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

            var result = App.Data.Connection.ExecuteScalar<int?>(xquery);

            return result ?? 0;
            //return
            //    App.Data.Connection.Table<ClientesModel>()
            //        .Where(c => c.idClientesOffLine == idClientesOffLine)
            //        .Select(c => c.idClientes ?? 0)
            //        .FirstOrDefault();
        }


        public static int GetIdClienteOffLine(int? idClientes)
        {

            //return
            //    App.Data.Connection.Table<ClientesModel>()
            //        .Where(c => c.idClientes == idClientes)
            //        .Select(c => c.idClientesOffLine ?? 0)
            //        .FirstOrDefault();

            var xquery =
               $@"select idClientesOffLine from tb_clientes
                        where idClientes  = {idClientes ?? 0} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.ExecuteScalar<int?>(xquery);
            return result ?? 0;

        }

        public static double? GetValorLimiteCredito(int? idClientesOffLine)
        {
            if (idClientesOffLine == 0) return 0;

            var xquery =
                $@"select vLimiteCredito from tb_clientes
                        where idClientesOffLine  = {idClientesOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.ExecuteScalar<double?>(xquery);

            return result;

        }

        public static int GetIdTabelaPreco(int idClientesOffLine)
        {
            return
                App.Data.Connection.Table<ClientesModel>()
                    .Where(c => c.idClientesOffLine == idClientesOffLine)
                    .Select(c => c.idTabelaPreco ?? 0)
                    .FirstOrDefault();
        }

        public static bool CanRemoveCliente(ClientesModel clientesModel)
        {
            return
                App.Data.Connection.Table<PedidoVendaModel>()
                    .Count(c => c.idClientesOffLine == clientesModel.idClientesOffLine) <= 0;
        }

        public static async Task<bool> Delete(ClientesModel objClientesModel)
        {
            try
            {
                bool removido = false;
                if (objClientesModel.idClientes == null || objClientesModel.idClientes <= 0)
                {
                    if (await UtilMessages.Exclusao())
                        if (ClienteRepository.CanRemoveCliente(objClientesModel))
                        {
                            App.Data.Connection.Delete(objClientesModel);
                            removido = true;
                        }
                        else
                            await
                                App.Messages.ShowAsync(
                                    "Já existe pedido/orçamento para esse cliente, impossível remover.");
                }
                else
                    await
                        App.Messages.ShowAsync(
                            "Não é possível excluir um registro já sincronizado pelo app, acesse o pedidoeletronico.com");
                return removido;
            }
            catch (Exception ex) // catch all other errors
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
                //Insights.Report(ex, Insights.Severity.Error);
                return false;
            }

        }

        public static int GetClientesProspect(bool bTodos)
        {
            int resultado;
            if (bTodos || App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
            {
                resultado = App.Data.Connection.Table<ClientesModel>().Count(c =>
                    c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.idEmpresa &&
                    c.stProspeccao == "CP" &&
                    c.stAtivo);
            }
            else
            {
                resultado = App.Data.Connection.Table<ClientesModel>().Count(c =>
                    c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.idEmpresa &&
                    c.idEmpresa_aspnetUsers ==
                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers &&
                    c.stProspeccao == "CP" &&
                    c.stAtivo);
            }
            return resultado;
        }




        #region Testes listview infinita

        public static List<ListItemModel> Get(int skip, int take, string xFiltro, TipoTela tipoTela = TipoTela.cadasatro)
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var filtroSemPontos = "";


                var xQuery = "";
                const string xFields =
                    "xRazaoSocial Display, (xFantasia || '  ' || xCpfCnpj) Detail, idClientesOffLine Id, xCpfCnpj";
                xQuery = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAcessoTodosClientes == 1
                    ? $"select {xFields} from {TableMobile.TB_CLIENTES} where idEmpresa = {idEmpresa} "
                    : $"select {xFields} from {TableMobile.TB_CLIENTES} where idEmpresa = {idEmpresa} and (idEmpresa_aspnetUsers = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers} or idEmpresa_aspnetUsers is null)";

                if (tipoTela == TipoTela.pedido)
                    xQuery += $" and stAtivo = 1 ";

                if (!string.IsNullOrEmpty(xFiltro))
                {
                    if (Extensions.IsDigitsOnly(xFiltro))
                    {
                        if (xFiltro.Length == 14)
                        {
                            filtroSemPontos = xFiltro;
                            xFiltro = Extensions.ToCNPJFormat(xFiltro);
                        }
                        else if (xFiltro.Length == 11)
                        {
                            filtroSemPontos = xFiltro;
                            xFiltro = Extensions.ToCpfFormat(xFiltro);
                        }
                    }
                    else if (Extensions.ApenasPontosTracos(xFiltro))
                    {
                        if (xFiltro.Length > 18)
                            xFiltro = xFiltro.Substring(0, 18);
                        filtroSemPontos = Extensions.SemPontos(xFiltro);
                    }
                    xFiltro = xFiltro.RemoverAcentos().ToUpper();
                    xQuery += $@" and (UPPER(xRazaoSocial) like('%{xFiltro}%') 
                                  or UPPER(coalesce(xDisplaySemCaracter,'')) like('%{xFiltro}%')
                                  or UPPER(coalesce(xFantasia,'')) like('%{xFiltro}%')
                                  or UPPER(xCpfCnpj) like('%{xFiltro}%')";
                    if (!filtroSemPontos.Equals(""))
                        xQuery += $"or UPPER(xCpfCnpj) like('%{filtroSemPontos}%'))";
                    else
                        xQuery += ")";
                }

                xQuery += $@" order by UPPER(xRazaoSocial)
                                            LIMIT {take} OFFSET {skip}";

                var resultado = App.Data.Connection.Query<ListItemModel>(xQuery);
                return resultado;
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
                return new List<ListItemModel>();
            }
        }

        public static List<ClientesModel> GetClientesNaoCompram(int idEmpresa, int idAspnetUsers, string filtro)
        {
            int dias = 0;

            switch (filtro)
            {
                case "mes":
                    dias = 30;
                    break;
                case "semana":
                    dias = 7;
                    break;
                case "3meses":
                    dias = 90;
                    break;
                case "6meses":
                    dias = 180;
                    break;
            }

            var queryClientes = $@"
                SELECT idClientes 
                    FROM {TableMobile.TB_CLIENTES} 
                    WHERE idEmpresa = {idEmpresa} AND idEmpresa_aspnetUsers = {idAspnetUsers}";

            List<int?> clienteIds = new List<int?>();
            var lClientes = App.Data.Connection.Query<ClientesModel>(queryClientes).ToList();

            lClientes.ForEach(x => clienteIds.Add(x.idClientes ?? 0));   

            var data = DateTime.UtcNow.AddHours(-3).AddDays(-dias);

            var ids = App.Data.Connection.Table<PedidoVendaModel>()
                            .Where(x => x.idEmpresa == idEmpresa && clienteIds.Contains(x.idClientes)
                                && x.dEmissao > data)
                                    .Select(x => x.idClientes ?? 0)
                                        .Distinct()
                                            .ToList();

            foreach (var lin in ids)
            {
                if (clienteIds.Contains(lin))
                    clienteIds.Remove(lin);

                else if (clienteIds.Count == 0)
                    break;
            }

            var idsFiltradoPlaceholders = string.Join(",", clienteIds);

            var xQueryExibirClientes =
                $@"SELECT xRazaoSocial, xFantasia, xEmails, xTelefones, idClientesOffLine, idClientes,
                    UPPER(SUBSTR(xRazaoSocial, 1, 1)) ||
                    CASE 
                    WHEN INSTR(xRazaoSocial, ' ') > 0 THEN UPPER(SUBSTR(xRazaoSocial, INSTR(xRazaoSocial, ' ') + 1, 1))
                    ELSE ''
                    END AS xAbreviacao
                    FROM {TableMobile.TB_CLIENTES}
                    WHERE idEmpresa = {idEmpresa} AND idClientes IN ({idsFiltradoPlaceholders})";

            var exibir = App.Data.Connection.Query<ClientesModel>(xQueryExibirClientes);

            return exibir.ToList();

        }

        public static ListItemModel GetRegistro(int idClientesOffLine)
        {
            var xQuery =
                $@"SELECT xRazaoSocial Display, idClientesOffLine Id FROM {TableMobile.TB_CLIENTES} WHERE idClientesOffLine = {idClientesOffLine} 
                                                            and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

            var resultado = App.Data.Connection.Query<ListItemModel>(xQuery);

            return resultado.FirstOrDefault();
        }

        public static string GetAnotacaoCliente(int idClientesOffLine)
        {

            var xQuery = $@"Select idClientesOffLine, bExibirAnotacaoNoPedido, xAnotacao from {TableMobile.TB_CLIENTES} 
                                            where idClientesOffLine = {idClientesOffLine}";


            var resultado = App.Data.Connection.Query<ClientesModel>(xQuery);

            var cliente = resultado.FirstOrDefault();

            if ((cliente.bExibirAnotacaoNoPedido ?? false))
            {
                return cliente.xAnotacao ?? "";
            }

            return "";
        }

        public static int GetIdTransportadoraCliente(int idClientesOffLine)
        {

            var xquery =
                $@"select idTransportadora from tb_clientes
                        where idClientesOffLine  = {idClientesOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.ExecuteScalar<int?>(xquery);
            return result ?? 0;
        }

        public static int? GetIdRedespachoCliente(int idClientesOffLine)
        {

            var xquery =
                $@"select idRedespacho from tb_clientes
                        where idClientesOffLine  = {idClientesOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.ExecuteScalar<int?>(xquery);
            return result ?? 0;
        }

        public static int GetIdCondicaoPagamento(int idClientesOffLine)
        {

            var xquery =
                $@"select idCondicaoPagamento from tb_clientes
                        where idClientesOffLine  = {idClientesOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.ExecuteScalar<int?>(xquery);
            return result ?? 0;
        }

        public static int? GetIdCondicaoAsync(int idClientesOffLine, int idEmpresa)
        {
            return App.Data.Connection.Table<ClientesModel>().Where(t => t.idClientesOffLine == idClientesOffLine && t.idEmpresa == idEmpresa).Select(t => t.idCondicaoPagamento).FirstOrDefault();
        }


        public static int GetIdRepresentanteDoCliente(int idClientesOffLine)
        {

            var xquery =
                $@"select idEmpresa_aspnetUsers from tb_clientes
                        where idClientesOffLine  = {idClientesOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.ExecuteScalar<int?>(xquery);
            return result ?? 0;
        }




        #endregion





    }
}
