using System;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class EmpresaRepository
    {

        

        public static int GetnDiasValidadeOrcamento()
        {
            try
            {
                var xQuery = string.Format("select coalesce(nDiasValidadeOrcamento,0) from {0} where idEmpresa = {1}",
                    TableMobile.TB_EMPRESA,
                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.idEmpresa);

                var resultado = App.Data.Connection.ExecuteScalar<string>(xQuery);
                return Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static EmpresaModel GetEmpresa()
        {
            //return
            //    App.Data.Connection.Table<EmpresaModel>()
            //        .FirstOrDefault(c => c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);

            var xQuery =
               $@"SELECT * FROM {TableMobile.TB_EMPRESA} WHERE 
                                    idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var resultado = App.Data.Connection.Query<EmpresaModel>(xQuery);
            return resultado.FirstOrDefault();
        }


        public static EmpresaModel GetDadosLimiteCreditoEmpresa()
        {
            
            var xQuery =
               $@"SELECT stForcaLimiteCreditoCliente FROM {TableMobile.TB_EMPRESA} WHERE 
                                    idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var resultado = App.Data.Connection.Query<EmpresaModel>(xQuery);
            return resultado.FirstOrDefault();
        }

        //OS 35349 - Jessica Barbieri
        public static bool GetForcaLimiteCreditoCliente(int idEmpresa)
        {
            var xQuery =
               $@"SELECT stForcaLimiteCreditoCliente FROM {TableMobile.TB_EMPRESA} WHERE 
                                    idEmpresa = {idEmpresa}";
            var resultado = App.Data.Connection.ExecuteScalar<bool>(xQuery);
            return resultado;
        }

        /// <summary>
        /// Campo de parâmetro que indica se a xAnotacao na tb_empresa deve aparecer no pedido de venda.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public static bool MostraAnotacaoPedidoDaEmpresa(int idEmpresa)
        {
            var xQuery =
               $@"SELECT bExibirAnotacaoEmpresaNoPedido FROM {TableMobile.TB_EMPRESA} WHERE 
                                    idEmpresa = {idEmpresa}";
            var resultado = App.Data.Connection.ExecuteScalar<bool>(xQuery);
            return resultado;
        }

        /// <summary>
        /// Get das informações adicionais da empresa.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public static string GetAnotacaoEmpresaParaPedido(int idEmpresa)
        {
            var xQuery =
               $@"SELECT xAnotacao FROM {TableMobile.TB_EMPRESA} WHERE 
                                    idEmpresa = {idEmpresa}";
            var resultado = App.Data.Connection.ExecuteScalar<string>(xQuery);
            return "Observação Empresa: " + resultado + "\n";
        }

        public static string GetNameEmpresa(int idEmpresaAspnetUser)
        {
            try
            {
                var item =
                    App.Data.Connection
                        .Table<EmpresaAspnetUsersModel>()
                        .FirstOrDefault(c => c.idEmpresa_aspnetUsers == idEmpresaAspnetUser);

                return item != null ? item.xNome : "Nome da empresa";
            }
            catch (Exception)
            {
                return "Nome da empresa";
            }
        }

    }
}
