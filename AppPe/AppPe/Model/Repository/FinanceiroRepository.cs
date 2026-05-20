using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Financeiro;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class FinanceiroRepository
    {

        public static async Task<bool> ValidaLimiteCredito(int idClienteOffLine, int? idPedidoVendaOffLine,
            double? vTotalPedido, int? idCondicaoPagamento)
        {
            try
            {
                // verifico primeiro se não é a vista o pedido.
                double vTotalUtilizado = 0;
                if (idCondicaoPagamento != null && vTotalPedido != null)
                {
                    var tot2 = TotalPorPedidoEmAberto(idCondicaoPagamento ?? 0, vTotalPedido ?? 0);
                    if (tot2 <= 0) return true;
                    vTotalUtilizado += tot2;
                }

                //Lancamento.PedidoVendaModel pedidoVendaModel = new Lancamento.PedidoVendaModel();

                var isAdm = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador;
                var vLimiteCredito = ClienteRepository.GetValorLimiteCredito(idClienteOffLine);
                if (vLimiteCredito == null) return true;

                var idClientes = ClienteRepository.GetIdClienteNuvem(idClienteOffLine);
                string xQuery;

                // verifico o total em aberto no nome do cliente, já sincronizado
                var xWhereDesconcidere = "";
                if (idPedidoVendaOffLine != null)
                {
                    xWhereDesconcidere = $" and tb_pedidovenda.idPedidoVendaOffLine <> {idPedidoVendaOffLine}";
                }

                if (idClientes > 0)
                {
                    xQuery =
                        $@"select sum(coalesce(vTitulo,0) - coalesce(vRecebido,0)) dValor from TB_RECEBIMENTOTITULOS
                                    left join tb_pedidovenda 
                                    on TB_RECEBIMENTOTITULOS.idPedidoVenda = tb_pedidovenda.idPedidoVenda and
                                        TB_RECEBIMENTOTITULOS.idEmpresa = tb_pedidovenda.idEmpresa 
                                    where tb_pedidovenda.idClientes = {idClientes} and tb_pedidovenda.stLancamento = 1  and tb_pedidovenda.idEmpresa = {App
                            .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
                    if (xWhereDesconcidere != "")
                    {
                        xQuery = xQuery + xWhereDesconcidere;
                    }

                    var dValor = App.Data.Connection.ExecuteScalar<double>(xQuery);

                    //var dados = App.Data.Connection.Query<ResultModel>(xQuery).FirstOrDefault();

                    vTotalUtilizado += dValor;
                }

                // vamos buscar todos os pedidos em aberto e somoar o total utilizado
                xQuery =
                    $@"select tb_pedidovenda.idPedidoVendaOffLine Id, coalesce(tb_pedidovenda.idCondicaoPagamento,0) Id2,  tb_pedidovenda.VTotal dValor  from tb_pedidovenda
                                    where tb_pedidovenda.idClientesOffLine = {idClienteOffLine} and tb_pedidovenda.stLancamento = 1  and tb_pedidovenda.idPedidoVenda is null";

                if (xWhereDesconcidere != "")
                {
                    xQuery = xQuery + xWhereDesconcidere;
                }

                var pedidosAbertos = App.Data.Connection.Query<ResultModel>(xQuery);

                foreach (var pedido in pedidosAbertos)
                {
                    var tot = TotalPorPedidoEmAberto(pedido.Id2, pedido.dValor);

                    vTotalUtilizado += tot;
                }

                if (vLimiteCredito < vTotalUtilizado)
                {
                    var xMessage =
                        $"Limite de crédito excedido.{Environment.NewLine}Crédito disponível: {vLimiteCredito.ToCurrencyStringPtBr()}{Environment.NewLine}Total excedido: {(vLimiteCredito - vTotalUtilizado).ToCurrencyStringPtBr()}";
                    if (isAdm)
                    {
                        var msgAdmin = $"{xMessage}{Environment.NewLine}{Environment.NewLine}Deseja continuar ?";
                        return await App.Messages.ShowConfirmAsync(msgAdmin, "SIM", "NÃO", "LIMITE DE CRÉDITO");
                    }
                    else
                    {
                        var msgAdmin =
                            $"{xMessage}{Environment.NewLine}{Environment.NewLine}Não será possível salvar o lançamento.";
                        await App.Messages.ShowAsync(msgAdmin);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.TrakException("ValidaLimiteCredito", false);
                return true;
            }
        }

        // OS 35349 - Jessica Barbieri
        public static bool PedidoFechadoIncorretamente(int idClienteOffLine, int? idPedidoVendaOffLine,
            double? vTotalPedido, int? idCondicaoPagamento)
        {
            try
            {
                // verifico primeiro se não é a vista o pedido
                double vTotalUtilizado = 0;
                if (idCondicaoPagamento != null && vTotalPedido != null)
                {
                    var tot2 = TotalPorPedidoEmAberto(idCondicaoPagamento ?? 0, vTotalPedido ?? 0);
                    if (tot2 <= 0) return false;
                    vTotalUtilizado += tot2;
                }

                Lancamento.PedidoVendaModel pedidoVendaModel = new Lancamento.PedidoVendaModel();

                var isAdm = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador;
                if (isAdm) return false;

                var vLimiteCredito = ClienteRepository.GetValorLimiteCredito(idClienteOffLine);
                if (vLimiteCredito == null) return false;

                var idClientes = ClienteRepository.GetIdClienteNuvem(idClienteOffLine);
                string xQuery;

                // verifico o total em aberto no nome do cliente, já sincronizado
                var xWhereDesconcidere = "";
                if (idPedidoVendaOffLine != null)
                {
                    xWhereDesconcidere = $" and tb_pedidovenda.idPedidoVendaOffLine <> {idPedidoVendaOffLine}";
                }

                if (idClientes > 0)
                {
                    xQuery =
                        $@"select sum(coalesce(vTitulo,0) - coalesce(vRecebido,0)) dValor from TB_RECEBIMENTOTITULOS
                                    left join tb_pedidovenda 
                                    on TB_RECEBIMENTOTITULOS.idPedidoVenda = tb_pedidovenda.idPedidoVenda and
                                        TB_RECEBIMENTOTITULOS.idEmpresa = tb_pedidovenda.idEmpresa 
                                    where tb_pedidovenda.idClientes = {idClientes} and tb_pedidovenda.stLancamento = 1  and tb_pedidovenda.idEmpresa = {App
                            .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
                    if (xWhereDesconcidere != "")
                    {
                        xQuery = xQuery + xWhereDesconcidere;
                    }

                    var dValor = App.Data.Connection.ExecuteScalar<double>(xQuery);
                    vTotalUtilizado += dValor;
                }

                // vamos buscar todos os pedidos em aberto e somoar o total utilizado
                xQuery =
                    $@"select tb_pedidovenda.idPedidoVendaOffLine Id, coalesce(tb_pedidovenda.idCondicaoPagamento,0) Id2,  tb_pedidovenda.VTotal dValor  from tb_pedidovenda
                                    where tb_pedidovenda.idClientesOffLine = {idClienteOffLine} and tb_pedidovenda.stLancamento = 1  and tb_pedidovenda.idPedidoVenda is null";

                if (xWhereDesconcidere != "")
                {
                    xQuery = xQuery + xWhereDesconcidere;
                }

                var pedidosAbertos = App.Data.Connection.Query<ResultModel>(xQuery);

                foreach (var pedido in pedidosAbertos)
                {
                    var tot = TotalPorPedidoEmAberto(pedido.Id2, pedido.dValor);

                    vTotalUtilizado += tot;
                }

                if (vLimiteCredito > vTotalUtilizado)
                {
                    pedidoVendaModel.bPedidoFechadoIncorretamente = false;
                    return false;
                }
                else
                {
                    return true;
                }
            }

            catch (Exception ex)
            {
                ex.TrakException("bPedidoFechadoIncorretamente", true);
                return true;
            }
        }

        // OS 35349 - Jessica Barbieri
        public static bool ValidaLimiteCreditoCliente(int idClienteOffLine, int? idPedidoVendaOffLine,
            double? vTotalPedido, int? idCondicaoPagamento, double? vLimiteCredito)
        {
            try
            {
                // verifico primeiro se não é a vista o pedido
                double vTotalUtilizado = 0;
                if (idCondicaoPagamento != null && vTotalPedido != null)
                {
                    var tot2 = TotalPorPedidoEmAberto(idCondicaoPagamento ?? 0, vTotalPedido ?? 0);
                    if (tot2 <= 0) return false;
                    vTotalUtilizado += tot2;
                }

                if (vLimiteCredito == null) return false;

                var idClientes = ClienteRepository.GetIdClienteNuvem(idClienteOffLine);
                string xQuery;

                // verifico o total em aberto no nome do cliente, já sincronizado
                var xWhereDesconcidere = "";
                if (idPedidoVendaOffLine != null)
                {
                    xWhereDesconcidere = $" and tb_pedidovenda.idPedidoVendaOffLine <> {idPedidoVendaOffLine}";
                }

                if (idClientes > 0)
                {
                    xQuery =
                        $@"select sum(coalesce(vTitulo,0) - coalesce(vRecebido,0)) dValor from TB_RECEBIMENTOTITULOS
                                    left join tb_pedidovenda 
                                    on TB_RECEBIMENTOTITULOS.idPedidoVenda = tb_pedidovenda.idPedidoVenda and
                                        TB_RECEBIMENTOTITULOS.idEmpresa = tb_pedidovenda.idEmpresa 
                                    where tb_pedidovenda.idClientes = {idClientes} and tb_pedidovenda.stLancamento = 1  and tb_pedidovenda.idEmpresa = {App
                            .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
                    if (xWhereDesconcidere != "")
                    {
                        xQuery = xQuery + xWhereDesconcidere;
                    }

                    var dValor = App.Data.Connection.ExecuteScalar<double>(xQuery);
                    vTotalUtilizado += dValor;
                }

                // vamos buscar todos os pedidos em aberto e somar o total utilizado
                xQuery =
                    $@"select tb_pedidovenda.idPedidoVendaOffLine Id, coalesce(tb_pedidovenda.idCondicaoPagamento,0) Id2,  tb_pedidovenda.VTotal dValor  from tb_pedidovenda
                                    where tb_pedidovenda.idClientesOffLine = {idClienteOffLine} and tb_pedidovenda.stLancamento = 1  and tb_pedidovenda.idPedidoVenda is null";

                if (xWhereDesconcidere != "")
                {
                    xQuery = xQuery + xWhereDesconcidere;
                }

                var pedidosAbertos = App.Data.Connection.Query<ResultModel>(xQuery);

                foreach (var pedido in pedidosAbertos)
                {
                    var tot = TotalPorPedidoEmAberto(pedido.Id2, pedido.dValor);

                    vTotalUtilizado += tot;
                }

                //valida se o limite não foi excedido
                if (vLimiteCredito > vTotalUtilizado)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            catch (Exception ex)
            {
                ex.TrakException("ValidaLimiteCreditoCliente", true);
                return true;
            }
        }



        public static KeyValuePair<double, double> BuscaAbertosEVencidos(int idClienteOffLine)
        {
            try
            {
                List<int?> lIds = App.Data.Connection.Table<PedidoVendaModel>()
                  .Where(c => c.idClientesOffLine == idClienteOffLine && c.idPedidoVenda > 0)
                  .Select(p => p.idPedidoVenda)
                  .ToList();

                List<RecebimentoTitulosModel> _recebimentos =
                App.Data.Connection.Table<RecebimentoTitulosModel>()
                  .Where(c => lIds.Contains(c.idPedidoVenda) && c.vTitulo > c.vRecebido)
                  .ToList();


                decimal totalAberto = _recebimentos.Sum(p => p.vTitulo) - _recebimentos.Sum(p => p.vRecebido);
                decimal totalVencido = _recebimentos.Where(p => p.dtVencimento < DateTime.UtcNow).Sum(p => p.vTitulo) -
                    _recebimentos.Where(p => p.dtVencimento < DateTime.UtcNow).Sum(p => p.vRecebido);


                return new KeyValuePair<double, double>((double)totalAberto, (double)totalVencido);
            }
            catch (Exception ex)
            {
                return new KeyValuePair<double, double>();
            }
        }

        public static List<RecebimentoTitulosModel> BuscarTitulosEmAberto(int page, int idClienteOffLine)
        {
            try
            {
                List<int?> lIds = App.Data.Connection.Table<PedidoVendaModel>()
                  .Where(c => c.idClientesOffLine == idClienteOffLine && c.idPedidoVenda > 0)
                  .Select(p => p.idPedidoVenda)
                  .ToList();

                List<RecebimentoTitulosModel> _recebimentos =
                App.Data.Connection.Table<RecebimentoTitulosModel>()
                  .Where(c => lIds.Contains(c.idPedidoVenda) && c.vTitulo > c.vRecebido)
                  .OrderBy(x => x.dtVencimento)
                  .Skip((page - 1) * 50)
                  .Take(50)
                  .ToList();


                return _recebimentos;
            }
            catch (Exception ex)
            {
                return new List<RecebimentoTitulosModel>();
            }
        }


        private static double TotalPorPedidoEmAberto(int idCondicaoPagamento, double dValorSubTotal)
        {

            double dValorRetorno = 0;
            if (idCondicaoPagamento > 0)
            {

                var xQuery =
                    $@"SELECT xCondicaoPagamento, xFormula, stBaixa , nParcelas FROM {TableMobile.TB_CONDICAOPAGAMENTO} WHERE 
                                        idCondicaoPagamento = {idCondicaoPagamento} and idEmpresa = {App
                        .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                var _condicao = App.Data.Connection.Query<CondicaoPagamentoModel>(xQuery).FirstOrDefault();

                if (string.IsNullOrEmpty(_condicao.xFormula))
                {
                    if (_condicao.stBaixa == 0)
                    {
                        var totalparcial = (dValorSubTotal / Convert.ToDouble(_condicao.nParcelas)) *
                                           Convert.ToDouble((_condicao.nParcelas) - 1);
                        dValorRetorno += totalparcial;
                    }
                    else if (_condicao.stBaixa == 1)
                    {
                        dValorRetorno += dValorSubTotal;
                    }
                }
                else
                {
                    if (_condicao.stBaixa == 0)
                    {
                        var nParcelas = _condicao.xFormula.Split('/').Length;
                        var totalparcial = (dValorSubTotal / Convert.ToDouble(nParcelas)) *
                                           Convert.ToDouble(nParcelas - 1);
                        dValorRetorno = dValorRetorno + totalparcial;
                    }
                    else if (_condicao.stBaixa == 1)
                    {
                        dValorRetorno += dValorSubTotal;
                    }
                }
            }
            return dValorRetorno;
        }


        public static void RemoverTodosRecebimentosPedido(int idPedidoVenda)
        {
            try
            {
                App.Data.Connection.Execute($"DELETE FROM {TableMobile.TB_RECEBIMENTOTITULOS} WHERE idPedidoVenda = '{idPedidoVenda}'");
            }
            catch
            {
            }
        }

        public static async Task RemoverTodosRecebimentos<T>() where T : class
        {
            App.Data.Connection.DropTable<T>();
            await App.Data.SegundaAnalise();
            //var xTable = TableMobile.GetTableNameByModel<T>();
            //var xQuery = $"Delete from {xTable} where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

            //App.Data.Connection.Execute(xQuery);
        }


        public static bool VerificaPedidoComFinanceiroLancado(int idPedidoVenda)
        {
            try
            {
                var xQuery =
                    $@"SELECT COUNT(*) FROM {TableMobile.TB_RECEBIMENTOTITULOS} WHERE idPedidoVenda = {idPedidoVenda} and vRecebido > 0";

                var retorno = App.Data.Connection.ExecuteScalar<int>(xQuery);

                return retorno > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int GetidRecebimentoTituloOffLine(int idRecebimentoTitulo)
        {
            try
            {

                var xQuery = $@"select idRecebimentoTituloOffLine from {TableMobile.TB_RECEBIMENTOTITULOS} WHERE idRecebimentoTitulo = {idRecebimentoTitulo} ";
                var retorno = App.Data.Connection.ExecuteScalar<int>(xQuery);

                return retorno;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int GetidRecebimentoTituloMovimentacaoOffLine(int idRecebimentoTituloMovimentacao)
        {
            try
            {

                var xQuery = $@"select idRecebimentoTituloMovimentacao from {TableMobile.TB_RECEBIMENTOTITULOS_MOVIMENTACOES} WHERE idRecebimentoTituloMovimentacao = {idRecebimentoTituloMovimentacao} ";
                var retorno = App.Data.Connection.ExecuteScalar<int>(xQuery);

                return retorno;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<RecebimentoTitulosPostModel> GetByIdPedidoVenda(int idPedidoVenda, int idEmpresa)
        {
            try
            {
                var xQuery = $@"SELECT * FROM {TableMobile.TB_RECEBIMENTOTITULOS_POST} 
                        WHERE idPedidoVenda = {idPedidoVenda} AND idEmpresa = {idEmpresa}";

                var retorno = App.Data.Connection.Query<RecebimentoTitulosPostModel>(xQuery);

                var xQuery2 = $@"SELECT * FROM {TableMobile.TB_RECEBIMENTOTITULOS} 
                        WHERE idPedidoVenda = {idPedidoVenda} AND idEmpresa = {idEmpresa}";

                var retorno2 = App.Data.Connection.Query<RecebimentoTitulosModel>(xQuery2);

                return retorno ?? new List<RecebimentoTitulosPostModel>();
            }
            catch (Exception ex)
            {
                return new List<RecebimentoTitulosPostModel>();
            }
        }

        public static List<RecebimentoTitulosPostModel> GetFaturasManual(int idPedidoVenda, int idEmpresa)
        {
            try
            {
                var xQuery = $@"SELECT * FROM {TableMobile.TB_RECEBIMENTOTITULOS_POST} 
                        WHERE idPedidoVenda = {idPedidoVenda} AND idEmpresa = {idEmpresa}";

                var retorno = App.Data.Connection.Query<RecebimentoTitulosPostModel>(xQuery);
              
                return retorno ?? new List<RecebimentoTitulosPostModel>();
            }
            catch (Exception ex)
            {
                return new List<RecebimentoTitulosPostModel>();
            }
        }

        public static List<RecebimentoTitulosPostModel> GetFaturas(int idPedidoVenda, int idEmpresa)
        {
            try
            {              
                var xQuery = $@"SELECT * FROM {TableMobile.TB_RECEBIMENTOTITULOS} 
                        WHERE idPedidoVenda = {idPedidoVenda} AND idEmpresa = {idEmpresa}";

                var retorno = App.Data.Connection.Query<RecebimentoTitulosPostModel>(xQuery);

                return retorno ?? new List<RecebimentoTitulosPostModel>();
            }
            catch (Exception ex)
            {
                return new List<RecebimentoTitulosPostModel>();
            }
        }

        public static RecebimentoTitulosModel GetPixDisponivel(int idPedidoVenda, int idEmpresa)
        {
            try
            {
                var xQuery = $@"SELECT * FROM {TableMobile.TB_RECEBIMENTOTITULOS}
                                WHERE idPedidoVenda = ?
                                  AND idEmpresa = ?
                                  AND COALESCE(stPixPago, 0) = 0
                                  AND ((cCopiaCola IS NOT NULL AND cCopiaCola <> '')
                                       OR (cUrlPix IS NOT NULL AND cUrlPix <> ''))
                                LIMIT 1";

                return App.Data.Connection
                    .Query<RecebimentoTitulosModel>(xQuery, idPedidoVenda, idEmpresa)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.TrakException("FinanceiroRepository.GetPixDisponivel", false);
                return null;
            }
        }

        public static void SalvarFaturas(List<RecebimentoTitulosPostModel> recebimentoTitulosModel)
        {
            try
            {
                if (recebimentoTitulosModel != null && recebimentoTitulosModel.Any())
                {
                    foreach (var item in recebimentoTitulosModel)
                    {
                        if (item.idRecebimentoTitulo == 0)
                            App.Data.Connection.Insert(item);
                        else 
                            App.Data.Connection.Update(item);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
