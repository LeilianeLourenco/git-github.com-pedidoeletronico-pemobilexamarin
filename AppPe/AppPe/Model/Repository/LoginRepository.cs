using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class LoginRepository
    {
        public static void RefreshTipoUsuario()
        {
            var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.idEmpresa;
            var query = $"SELECT * FROM {TableMobile.TB_EMPRESA} WHERE idEmpresa = {idEmpresa}";
            var empresa = App.Data.Connection.Query<EmpresaModel>(query).FirstOrDefault();

            if (!string.IsNullOrEmpty(empresa?.xBlingApiKey))
                App.tipouser = App.TipoUser.BLING;

            else if (!string.IsNullOrEmpty(empresa?.xOmieAppKey))
                App.tipouser = App.TipoUser.OMIE;

            else if (empresa?.idEcommerceTiny > 0)
                App.tipouser = App.TipoUser.TINY;

            else
                App.tipouser = App.TipoUser.NORMAL;
        }
        public static bool HasLogin()
        {
            try
            {

                var icount =
                    App.Data.Connection.ExecuteScalar<int>(
                        $"select count(*) from {TableMobile.CurrentUserLogin} where bLogado = 1");


                return icount > 0;

                //return App.Data.Connection.Table<CurrentUserLoginModel>().Any(c => c.bLogado);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static AspNetUsersModel SaveAspnetUsers(AspNetUsersModel model)
        {
            try
            {
                var icount = 0;
                icount =
                    App.Data.Connection.ExecuteScalar<int>(
                        $"select count(*) from {TableMobile.AspNetUsers} where Id = '{model.Id.ToString()}'");

                if (icount == 0)
                    App.Data.Connection.Insert(model);
                else
                    App.Data.Connection.Update(model);

                SaveCurrentUserLog(model);

                foreach (var empresa in model.lEpresaAspnetUsersModel)
                {

                    icount =
                        App.Data.Connection.ExecuteScalar<int>(
                            $"select count(*) from {TableMobile.TB_EMPRESA_ASPNETUSERS} where idEmpresa_aspnetUsers = {empresa.idEmpresa_aspnetUsers.ToString()}");

                    if (icount == 0)
                        App.Data.Connection.Insert(empresa);
                    else
                    {
                        empresa.UltimaSyncDateTime = App.Data.Connection.Table<EmpresaAspnetUsersModel>()
                            .FirstOrDefault(c => c.idEmpresa_aspnetUsers == empresa.idEmpresa_aspnetUsers)
                            .UltimaSyncDateTime;
                        App.Data.Connection.Update(empresa);
                    }

                    icount =
                        App.Data.Connection.ExecuteScalar<int>(
                            $"select count(*) from {TableMobile.TB_EMPRESA} where idEmpresa = {empresa.idEmpresa.ToString()}");

                    if (icount == 0)
                        App.Data.Connection.Insert(empresa.objEmpresaModel);
                    else
                        App.Data.Connection.Update(empresa.objEmpresaModel);
                }
                return model;
            }
            catch (Exception ex)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }

        }


        public static void UpdateUser()
        {
            try
            {
                var item = App.EnvironmentPE;
                App.Data.Connection.Update(item);
            }
            catch (Exception ex)
            {

                ex.TrakException();
            }

        }

        public static CurrentUserLoginModel GetUserLoginModel()
        {
            if (App.Data.Connection != null && !App.Data.Connection.Table<CurrentUserLoginModel>().Any())
            {
                var currrentUser = new CurrentUserLoginModel
                {
                    Id = App.CurrentAspnetUserModel.Id,
                    Email = App.CurrentAspnetUserModel.Email,
                    idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa
                };
                App.Data.Connection.Insert(currrentUser);
            }

            var retorno =
                App.Data.Connection.Table<CurrentUserLoginModel>()
                    .FirstOrDefault(c => c.Id == App.CurrentAspnetUserModel.Id) ??
                App.Data.Connection.Table<CurrentUserLoginModel>().FirstOrDefault();

            retorno.bLogado = true;


            // looping de representantes
            foreach (var representante in App.CurrentAspnetUserModel.lEpresaAspnetUsersModel)
            {
                representante.isAtiva = false;

                if (representante.xEmail.ToUpper() == retorno.Email.ToUpper() && representante.idEmpresa == retorno.idEmpresaLogada)
                {
                    representante.isAtiva = true;
                }
            }

            if (!App.CurrentAspnetUserModel.lEpresaAspnetUsersModel.Any(c => c.isAtiva))
            {
                var first = App.CurrentAspnetUserModel.lEpresaAspnetUsersModel.FirstOrDefault();
                first.isAtiva = true;
            }


            if (retorno.idEmpresa == 0)
            {
                retorno.idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
            }


            App.Data.Connection.Execute($@"UPDATE {TableMobile.CurrentUserLogin} set bUltimoUserLogado = 0");

            retorno.bUltimoUserLogado = true;
            App.Data.Connection.Update(retorno);

            return retorno;
        }


        public static string GetUltimoEmailLogin()
        {
            try
            {
                var xQuery = $"SELECT Email from {TableMobile.CurrentUserLogin} where bUltimoUserLogado = 1 limit 1";
                var result = App.Data.Connection.ExecuteScalar<string>(xQuery);
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static void SaveCurrentUserLog(AspNetUsersModel user)
        {
            try
            {
                var update = $@"UPDATE {TableMobile.CurrentUserLogin} set bUltimoUserLogado = 0, bLogado = 0";

                App.Data.Connection.Execute(update);

                CurrentUserLoginModel currentUsu = null;

                if (App.Data.Connection.Table<CurrentUserLoginModel>().Any(c => c.Id == user.Id) == false)
                {
                    currentUsu = new CurrentUserLoginModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        idEmpresa = user.objEmpresaAspnetUsersModel.idEmpresa,
                        idEmpresaLogada = user.objEmpresaAspnetUsersModel.idEmpresa,
                        bLogado = true,
                        bUltimoUserLogado = true

                    };
                    App.Data.Connection.Insert(currentUsu);
                }
                else
                {
                    var xQuery = $@"SELECT * FROM {TableMobile.CurrentUserLogin} where Id = '{user.Id}'";
                    currentUsu = App.Data.Connection.Query<CurrentUserLoginModel>(xQuery).FirstOrDefault();

                    if (currentUsu != null)
                    {
                        currentUsu.bUltimoUserLogado = currentUsu.bLogado = true;
                        App.Data.Connection.Update(currentUsu);
                    }
                }
                App.EnvironmentPE = currentUsu;
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException($"SaveCurrentUserLog", ex.Message, true);
            }
        }

        public static AspNetUsersModel GetAspnetUsers()
        {
            try
            {
                //var lPedido = App.Data.Connection.Query<PedidoVendaListarModel>(xQuery);
                //var currentUser = App.Data.Connection.Table<CurrentUserLoginModel>().FirstOrDefault(c => c.bLogado);
                //var user = App.Data.Connection.Table<AspNetUsersModel>().FirstOrDefault(c => c.Email == currentUser.Email);


                var xQuery = $@"SELECT * FROM {TableMobile.CurrentUserLogin} where bLogado = 1";
                var currentUser = App.Data.Connection.Query<CurrentUserLoginModel>(xQuery).FirstOrDefault();
                xQuery = $@"SELECT * FROM {TableMobile.AspNetUsers} where Email = '{currentUser.Email}'";
                var user = App.Data.Connection.Query<AspNetUsersModel>(xQuery).FirstOrDefault();


                user.lEpresaAspnetUsersModel = new List<EmpresaAspnetUsersModel>();
                var idsEmpresas = App.Data.Connection.Table<EmpresaAspnetUsersModel>().Where(c => c.xEmail.ToUpper() == currentUser.Email.ToUpper()).Select(c => c.idEmpresa).Distinct().ToList();

                foreach (var idEmpresa in idsEmpresas)
                {
                    xQuery = $@"SELECT * FROM {TableMobile.TB_EMPRESA_ASPNETUSERS} where idEmpresa = {idEmpresa}";
                    user.lEpresaAspnetUsersModel.AddRange(App.Data.Connection.Query<EmpresaAspnetUsersModel>(xQuery));
                    xQuery = $@"SELECT * FROM {TableMobile.TB_PERMISSOES_REPRESENTANTES} where idEmpresa = {idEmpresa}";
                    user.lPermissoesRepresentantesModel.AddRange(App.Data.Connection.Query<PermissoesRepresentantesModel>(xQuery));
                    user.objEmpresaAspnetUsersModel.permissoesRepresentantesModel = user.lPermissoesRepresentantesModel.FirstOrDefault(x => x.idEmpresa_aspnetusers == user.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers);
                }

                EmpresaModel _empresa = null;
                foreach (var emp in user.lEpresaAspnetUsersModel.OrderBy(c => c.idEmpresa))
                {
                    emp.isAtiva = false;
                    xQuery = $"SELECT * FROM {TableMobile.TB_EMPRESA} where idEmpresa = {emp.idEmpresa}";
                    if (_empresa == null)
                        _empresa = App.Data.Connection.Query<EmpresaModel>(xQuery).FirstOrDefault();

                    if (emp.idEmpresa != _empresa.idEmpresa)
                        _empresa = App.Data.Connection.Query<EmpresaModel>(xQuery).FirstOrDefault();

                    emp.objEmpresaModel = _empresa;
                    emp.permissoesRepresentantesModel = user.lPermissoesRepresentantesModel.FirstOrDefault(x => x.idEmpresa_aspnetusers == emp.idEmpresa_aspnetUsers);
                }

                EmpresaAspnetUsersModel userDefault = null;
                if (user.lEpresaAspnetUsersModel.Any())
                {
                    if (currentUser.idEmpresaLogada > 0)
                        userDefault = user.lEpresaAspnetUsersModel.FirstOrDefault(c => c.xEmail.ToUpper() == currentUser.Email.ToUpper() && c.idEmpresa == currentUser.idEmpresaLogada);


                    if (userDefault == null)
                        userDefault = user.lEpresaAspnetUsersModel.FirstOrDefault(c => c.xEmail.ToUpper() == currentUser.Email.ToUpper());

                    if (userDefault != null)
                    {
                        userDefault.isAtiva = true;

                        if (currentUser.idEmpresaLogada == 0)
                        {
                            currentUser.idEmpresaLogada = userDefault.idEmpresa;
                            App.Data.Connection.Update(currentUser);
                        }
                    }
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static EmpresaAspnetUsersModel GetEmpresaAspnetUsersModel(int idEmpresa_aspnetUsers)
        {
            var xQuery = $"SELECT * FROM {TableMobile.TB_EMPRESA_ASPNETUSERS} where idEmpresa_aspnetUsers = {idEmpresa_aspnetUsers}";
            return App.Data.Connection.Query<EmpresaAspnetUsersModel>(xQuery).FirstOrDefault();

        }

        public static bool StatusBloqueio()
        {
            try
            {
                var icount = App.Data.Connection.ExecuteScalar<int>(
                    $"SELECT COUNT(*) FROM {TableMobile.CurrentUserLogin} WHERE bBloqueado = 1");

                return icount > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void BloquearUser()
        {
            try
            {
                App.Data.Connection.Execute($@"UPDATE {TableMobile.CurrentUserLogin} set bBloqueado = 1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DesbloquearUser()
        {
            try
            {
                App.Data.Connection.Execute($@"UPDATE {TableMobile.CurrentUserLogin} set bBloqueado = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Loggout()
        {
            try
            {
                App.EnvironmentPE.bLogado = false;
                UpdateUser();
                App.CurrentAspnetUserModel = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string GetNameByIdAspnetUsers(object idAspnetUsers)
        {
            try
            {
                if (string.IsNullOrEmpty(idAspnetUsers.ToString())) return "";
                var result = "";
                var xquery = $@"select Email from AspNetUsers 
                        where Id  = '{idAspnetUsers}' ";
                var mail = App.Data.Connection.ExecuteScalar<string>(xquery);
                if (mail == null) return result;
                xquery =
                    $@"select xApelido from tb_empresa_aspnetusers 
                        where xEmail  = '{mail}' and idEmpresa = {App
                        .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
                result = App.Data.Connection.ExecuteScalar<string>(xquery);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
