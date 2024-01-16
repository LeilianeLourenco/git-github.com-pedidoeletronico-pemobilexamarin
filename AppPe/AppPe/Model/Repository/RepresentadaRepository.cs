using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class RepresentadaRepository
    {

        public static void DeleteAllByRepresentante(int idEmpresa_aspnetUsers)
        {
            try
            {
                var xQuery =
                    $"Delete from tb_representada_aspnetusers where idEmpresa_aspnetUsers = {idEmpresa_aspnetUsers}";
                App.Data.Connection.Execute(xQuery);
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


        public static List<BasicPickerModel> GetListToBasicPickerModel(bool getItemTodos = false, int idCategoria = 0)
        {
            var retorno = new List<BasicPickerModel>();
            if (getItemTodos)
                retorno.Add(new BasicPickerModel { Id = 0, Display = "TODOS" });



            var xQuery = "";

            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
            {
                xQuery = $@"select TB_REPRESENTADA.idRepresentada Id , TB_REPRESENTADA.xRazaoSocial Display 
                                                    from TB_REPRESENTADA                                                   
                                                    where TB_REPRESENTADA.stAtivo = 1 
                                                            and TB_REPRESENTADA.idEmpresa = {App.CurrentAspnetUserModel
                    .objEmpresaAspnetUsersModel.idEmpresa}";
            }
            else
            {
                xQuery =
                    $@"select TB_REPRESENTADA.idRepresentada Id , TB_REPRESENTADA.xRazaoSocial Display from TB_REPRESENTADA
                                                    inner join tb_representada_aspnetusers 
	                                                on tb_representada.idRepresentada = tb_representada_aspnetusers.idRepresentada
	                                                where tb_representada_aspnetusers.idEmpresa_aspnetUsers = {App
                        .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers}";
            }

            var dados = App.Data.Connection.Query<BasicPickerModel>(xQuery);
            if (idCategoria != 0)
            {
                xQuery =
                    $@"SELECT DISTINCT idRepresentada Id from {TableMobile.TB_PRODUTO} WHERE IdCategoria = {idCategoria} and idEmpresa = {App
                        .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
                var resultado = App.Data.Connection.Query<ResultModel>(xQuery);
                retorno.AddRange(
                    resultado.Select(item => dados.FirstOrDefault(c => c.Id == item.Id))
                        .Where(itemAdd => itemAdd != null));
            }
            else
            {
                if (dados != null)
                    retorno.AddRange(dados.OrderBy(c => c.Display).ToList());
            }
            return retorno;
        }

        public static void RemoverTodosRepresentantes(int idEmpresa_aspnetUsers)
        {           
            App.Data.Connection.Execute($"DELETE FROM {TableMobile.TB_REPRESENTADA_ASPNETUSERS} WHERE idEmpresa_aspnetUsers = '{idEmpresa_aspnetUsers}'");            
        }

        public static List<ListItemModel> GetListItemModel(int idCategoria = 0)
        {
            var retorno = new List<ListItemModel> { new ListItemModel { Id = 0, Display = "TODOS" } };


            var xQuery = "";

            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
            {
                xQuery = $@"select TB_REPRESENTADA.idRepresentada Id , TB_REPRESENTADA.xRazaoSocial Display 
                                                    from TB_REPRESENTADA                                                   
                                                    where TB_REPRESENTADA.stAtivo = 1 
                                                            and TB_REPRESENTADA.idEmpresa = {App.CurrentAspnetUserModel
                    .objEmpresaAspnetUsersModel.idEmpresa}";
            }
            else
            {
                xQuery =
                    $@"select TB_REPRESENTADA.idRepresentada Id , TB_REPRESENTADA.xRazaoSocial Display from TB_REPRESENTADA
                                                    inner join tb_representada_aspnetusers 
	                                                on tb_representada.idRepresentada = tb_representada_aspnetusers.idRepresentada
	                                                where tb_representada_aspnetusers.idEmpresa_aspnetUsers = {App
                        .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers}";
            }

            var dados = App.Data.Connection.Query<ListItemModel>(xQuery);
            if (idCategoria != 0)
            {
                xQuery =
                    $@"SELECT DISTINCT idRepresentada Id from {TableMobile.TB_PRODUTO} WHERE IdCategoria = {idCategoria} and idEmpresa = {App
                        .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
                var resultado = App.Data.Connection.Query<ResultModel>(xQuery);
                retorno.AddRange(
                    resultado.Select(item => dados.FirstOrDefault(c => c.Id == item.Id))
                        .Where(itemAdd => itemAdd != null));
            }
            else
            {
                if (dados != null)
                    retorno.AddRange(dados.OrderBy(c => c.Display).ToList());
            }
            return retorno;
        }


        public static string GetNomeRepresentada(int idRepresentada)
        {
            var xQuery =
                $@"SELECT xRazaoSocial FROM {TableMobile.TB_REPRESENTADA} WHERE idRepresentada = {idRepresentada}";

            var retorno = App.Data.Connection.ExecuteScalar<string>(xQuery);

            return retorno;

            //var retorno = 

            //return
            //    App.Data.Connection.Table<RepresentadaModel>()
            //        .Where(c => c.idRepresentada == idRepresentada)
            //        .Select(c => c.xRazaoSocial)
            //        .FirstOrDefault();
        }


        public static byte GetParamEmailObrigatorio(int idRepresentada)
        {
            try
            {
                var xQuery = $@"Select stEmailObrigatorioPara from {TableMobile.TB_REPRESENTADA} where idRepresentada = {idRepresentada}";

                var retorno = App.Data.Connection.ExecuteScalar<byte>(xQuery);

                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException("", false);
                return 0;
            }
        }
    }
}

