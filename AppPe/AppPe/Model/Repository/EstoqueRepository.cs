using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class EstoqueRepository
    {
        public static string SaveEstoqueInsuficiente(List<EstoqueInsuficienteModel> ldados, int idPedidoVendaOffLine)
        {
            try
            {
                RemoveEstoquePedido(idPedidoVendaOffLine: idPedidoVendaOffLine);
                ldados = ldados.Distinct().ToList();
                foreach (var estoque in ldados)
                {
                    estoque.idPedidoVendaOffLine = idPedidoVendaOffLine;
                    estoque.idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                    App.Data.Connection.Insert(estoque);
                }

                var _estoqueInf = App.Data.Connection.Table<EstoqueInsuficienteModel>()
                    .Where(c => c.idPedidoVendaOffLine == idPedidoVendaOffLine)
                    .OrderByDescending(c => c.idPedidoVendaOffLine)
                    .FirstOrDefault();


                var _produtoSemEstoque = App.Data.Connection.Table<ProdutoModel>()
                    .Where(p => p.idProduto == _estoqueInf.idProduto)  
                    .Select(c => new ProdutoModel
                    {
                        idProduto = c.idProduto,
                        xNome = c.xNome,
                        cAlternativo = c.cAlternativo
                    })
                    .FirstOrDefault();


                return $"Produto: {_produtoSemEstoque.cAlternativo} - {_produtoSemEstoque.xNome} / Saldo Atual: {_estoqueInf.dEstoqueAtual}";
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("EstoqueRepository.SaveEstoqueInsuficiente", ex.Message, true);

                return string.Empty;
            }

        }


        public static bool HasEstoqueInvalido(int idPedidoVendaOffLine)
        {
            try
            {

                var xQuery =
                    $"SELECT COUNT(*) FROM TB_ESTOQUE_INSUFICIENTE where idPedidoVendaOffLine = {idPedidoVendaOffLine}";

                return
                    App.Data.Connection.Table<EstoqueInsuficienteModel>()
                        .Count(c => c.idPedidoVendaOffLine == idPedidoVendaOffLine) > 0;

            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("EstoqueRepository.HasEstoqueInvalido", ex.Message, true);
                return false;
            }
        }

        //public static void RemoveEstoquePedido(int idPedidoVendaOffLine)
        //{
        //    try
        //    {
        //        var dados =
        //            App.Data.Connection.Table<EstoqueInsuficienteModel>()
        //                .Where(c => c.idPedidoVendaOffLine == idPedidoVendaOffLine)
        //                .ToList();

        //        if (dados == null) return;
        //        foreach (var estoqueInsuficienteModel in dados)
        //        {
        //            App.Data.Connection.Delete(estoqueInsuficienteModel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GoogleInsightsReportingConstants.TrakException("RemoveEstoquePedido", ex.Message, true);
        //    }
        //}


        public static void RemoveEstoquePedido(int idPedidoVendaOffLine)
        {
            try
            {
                var xQuery =
                    $"DELETE FROM TB_ESTOQUE_INSUFICIENTE WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and idPedidoVendaOffLine = {idPedidoVendaOffLine}";

                App.Data.Connection.Execute(xQuery);
            }
            catch (Exception ex)
            {
                ex.TrakException("RemoveEstoquePedido");
            }
        }
        public static void RemoveAllEstoqueSincronizacao(int idProduto)
        {
            try
            {
                var xQuery =
                    $"DELETE FROM TB_MOVIMENTOESTOQUE WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and idProduto = {idProduto}";


                App.Data.Connection.Execute(xQuery);
                
            }
            catch (Exception ex)
            {
                ex.TrakException("RemoveEstoquePedido");
            }
        }

        public static void RemoveAllEstoquePedido()
        {
            try
            {
                var xQuery =
                    $"DELETE FROM TB_ESTOQUE_INSUFICIENTE WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                App.Data.Connection.Execute(xQuery);
            }
            catch (Exception ex)
            {
                ex.TrakException("RemoveEstoquePedido");
            }
        }

        public static List<EstoqueInsuficienteModel> GetAll(int idPedidoVendaOffLine)
        {
            try
            {
                return
                    App.Data.Connection.Table<EstoqueInsuficienteModel>()
                        .Where(c => c.idPedidoVendaOffLine == idPedidoVendaOffLine)
                        .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<int> GetAllIdPedido(Lancamento.PedidoVendaModel pedidoVendaModel)
        {
            try
            {
                return
                    App.Data.Connection.Table<EstoqueInsuficienteModel>().Select(c => c.idPedidoVendaOffLine).Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
