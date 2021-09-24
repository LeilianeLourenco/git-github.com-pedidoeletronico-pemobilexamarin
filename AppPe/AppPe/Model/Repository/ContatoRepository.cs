using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class ContatoRepository
    {
        public static ContatoModel Save(ContatoModel objContatoModel)
        {
            if (objContatoModel.idContatoOffLine == null)
            {
                objContatoModel.dtCadastro = DateTime.Now.ToUniversalTime();
                objContatoModel.idAspnetUsers = App.CurrentAspnetUserModel.Id;
                objContatoModel.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                App.Data.Connection.Insert(objContatoModel);
            }
            else
            {
                if (objContatoModel.idContatos != null) // é pq ja esta sincronizado
                    if (objContatoModel.RegistroAlterado)
                        objContatoModel.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                App.Data.Connection.Update(objContatoModel);
            }

            return objContatoModel;
        }

        public static List<ContatoModel> GetAll(int idClienteOffLine)
        {
            var xQuery = $@"select * from {TableMobile.TB_CONTATOS} where idClientesOffLine = {idClienteOffLine}";
            var dados = App.Data.Connection.Query<ContatoModel>(xQuery);
            return dados ?? new List<ContatoModel>();

            //return
            //    App.Data.Connection.Table<ContatoModel>()
            //        .Where(
            //            c =>
            //                c.idClientesOffLine == idClienteOffLine &&
            //                c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa)
            //        .ToList();
        }

        public static List<ContatoModel> GetAllContatoModelsToSync()
        {
            var lUpload =
                App.Data.Connection.Table<ContatoModel>()
                    .Where(
                        c =>
                            c.dtUltimaAlteracao >
                            App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime
                            && c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
            return lUpload.ToList();
        }

        public static ContatoModel GetContato(int idContatoOffLine)
        {
            return App.Data.Connection.Table<ContatoModel>().FirstOrDefault(c => c.idContatoOffLine == idContatoOffLine);
        }

        public static void Delete(ContatoModel objContatoModel)
        {
            if (objContatoModel.idContatos != null)
            {
                var logExclusao = new LogExclusaoModel
                {
                    idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                    idPK = objContatoModel.idContatos ?? 0,
                    xTable = TableMobile.TB_CONTATOS
                };
                App.Data.Connection.Insert(logExclusao);
            }
            if (objContatoModel.idContatoOffLine != null)
                App.Data.Connection.Delete(objContatoModel);
        }


       
    }
}
