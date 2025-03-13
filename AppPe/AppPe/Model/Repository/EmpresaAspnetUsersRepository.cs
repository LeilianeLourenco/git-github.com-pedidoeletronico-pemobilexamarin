using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class EmpresaAspnetUsersRepository
    {


        public static List<ListItemModel> Get(int skip, int take, string xFiltro)
        {
            var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.idEmpresa;
            var where = $@"idEmpresa = {idEmpresa} and stAtivo = 1 ";

            if (!string.IsNullOrEmpty(xFiltro))
            {
                xFiltro = xFiltro.RemoverAcentos().ToUpper();
                where += $" and (UPPER(xNome) like('%{xFiltro}%') or UPPER(coalesce(xDisplaySemCaracter,'')) like('%{xFiltro}%'))";
                //where += $" and UPPER(xNome) like('%{xFiltro.ToUpper()}%') ";
            }

            where += $" LIMIT {take} OFFSET {skip} ";

            var xQuery =
              $@"select idEmpresa_aspnetUsers Id , UPPER(xNome) Display, xEmail Detail from {TableMobile.TB_EMPRESA_ASPNETUSERS}
                                                    where {where}";


            var retorno = App.Data.Connection.Query<ListItemModel>(xQuery);

            return retorno;
        }

        public static List<int> GetListaRepsLinkados(int idEmpresa)
        {   
            var xQuery =
              $@"select  idEmpresa_aspnetUsers Id from {TableMobile.TB_EMPRESA_ASPNETUSERS}
                                                    where idEmpresa = {idEmpresa} and stAtivo = 1 ";
             
            var retorno = App.Data.Connection.Query<ListItemModel>(xQuery);

            return retorno.Select(t => t.Id).ToList();
        }

        public static ListItemModel GetRegistro(int idEmpresa_aspnetUsers)
        {
            var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.idEmpresa;
            var where = $@"idEmpresa = {idEmpresa} and idEmpresa_aspnetUsers = {idEmpresa_aspnetUsers} ";


            var xQuery =
              $@"select idEmpresa_aspnetUsers Id , UPPER(xNome) Display, xEmail Detail from {TableMobile.TB_EMPRESA_ASPNETUSERS}
                                                    where {where}";


            var retorno = App.Data.Connection.Query<ListItemModel>(xQuery);
            if (retorno == null) return new ListItemModel { Display = "-" };
            return retorno.FirstOrDefault();
        }


        public static List<BasicPickerModel> GetAllBasicPickerModelsEmail(bool addItemTodos = true)
        {
            var ldados = new List<BasicPickerModel>();
            if (addItemTodos)
            {
                ldados.Add(new BasicPickerModel { Id = 0, Display = "todos" });
            }
            var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.idEmpresa;
            var dados = App.Data.Connection.Table<EmpresaAspnetUsersModel>().Where(c => c.idEmpresa == idEmpresa && c.stAtivo).ToList();
            //var dados = App.Data.Connection.Table<EmpresaAspnetUsersModel>().Where(c => c.idEmpresa == idEmpresa ).ToList();
            foreach (var empresaAspnetUsersModel in dados)
            {
                ldados.Add(new BasicPickerModel
                {
                    Id = empresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0,
                    Display = empresaAspnetUsersModel.xNome,
                    Detail = empresaAspnetUsersModel.xEmail,
                    bTrazerImagem = false,
                    ColorDisplay = ColorStaticModel.CinzaPrincipal,
                    ColorDetail = ColorStaticModel.AzulPrincipal
                });
            }
            return ldados;
        }

        public static EmpresaAspnetUsersModel GetUsuario()
        {

            var xQuery =
                $"Select * from {TableMobile.TB_EMPRESA_ASPNETUSERS} where idEmpresa_aspnetUsers = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers}";
            var resultado = App.Data.Connection.Query<EmpresaAspnetUsersModel>(xQuery);
            return resultado.FirstOrDefault();
        }

        public static EmpresaAspnetUsersModel GetEmpresaAspnetUsers(int idEmpresa_aspnetUsers)
        {

            var xQuery =
                $"Select * from {TableMobile.TB_EMPRESA_ASPNETUSERS} where idEmpresa_aspnetUsers = {idEmpresa_aspnetUsers}";

            var resultado = App.Data.Connection.Query<EmpresaAspnetUsersModel>(xQuery);
            return resultado.FirstOrDefault();
        }


        public static string GetDisplay(int idEmpresa_aspnetUsers)
        {
            try
            {
                var xquery = $@"select xNome from {TableMobile.TB_EMPRESA_ASPNETUSERS}
                        where idEmpresa_aspnetUsers  = {idEmpresa_aspnetUsers}";
                var result = App.Data.Connection.ExecuteScalar<string>(xquery);
                return result;
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("CondicaoPagamentoRepository.GetDisplay", ex.Message, true);
                return "";
            }
        }

        public static string GetEmail(int idEmpresa_aspnetUsers)
        {
            try
            {
                var xquery = $@"select xEmail from {TableMobile.TB_EMPRESA_ASPNETUSERS}
                        where idEmpresa_aspnetUsers  = {idEmpresa_aspnetUsers}";
                var result = App.Data.Connection.ExecuteScalar<string>(xquery);
                return result;
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("CondicaoPagamentoRepository.GetDisplay", ex.Message, true);
                return "";
            }
        }

        public static bool GetGravaLoc(int idEmpresa_aspnetUsers)
        {
            var xQuery =
              $@"select bGravaLocRepresentante from {TableMobile.TB_EMPRESA_ASPNETUSERS}
                                                    where idEmpresa_aspnetUsers = {idEmpresa_aspnetUsers}";
  
            return App.Data.Connection.ExecuteScalar<bool>(xQuery);
        }

        public static EmpresaAspnetUsersModel AtualizaEmpresaAspnetUsersModel(
            EmpresaAspnetUsersModel objEmpresaAspnetUsersModel)
        {
            App.Data.Connection.Update(obj: objEmpresaAspnetUsersModel);
            return objEmpresaAspnetUsersModel;
        }
    }
}
