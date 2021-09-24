using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    class EnderecoRepository
    {
        public static EnderecoModel Save(EnderecoModel objEnderecoModel)
        {
            if (objEnderecoModel.idEnderecoOffLine == null)
            {
                if (objEnderecoModel.dtCadastro == null)
                    objEnderecoModel.dtCadastro = DateTime.Now.ToUniversalTime();
                objEnderecoModel.idAspnetUsers = App.CurrentAspnetUserModel.Id;
                objEnderecoModel.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                App.Data.Connection.Insert(objEnderecoModel);
            }
            else
            {
                if (objEnderecoModel.idEndereco != null)
                    if (objEnderecoModel.RegistroAlterado)
                        objEnderecoModel.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();

                App.Data.Connection.Update(objEnderecoModel);
            }
            return objEnderecoModel;
        }

        public static List<EnderecoModel> GetAll(int idClienteOffLine)
        {
            var xQuery = $@"select * from {TableMobile.TB_ENDERECO} where idClientesOffLine = {idClienteOffLine}";
            var enderecos = App.Data.Connection.Query<EnderecoModel>(xQuery);

            if(enderecos?.Count() > 0)
            {
                var _melhoriaBloqueiaReceita = ConfiguracaoGeralRepositorio.GetMelhoriaEspecificaReceitaBloqueio(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
                foreach (var item in enderecos)
                {
                    item.bAplicaMelhoriaBloqueiaEnderecoReceita = _melhoriaBloqueiaReceita;
                }
            }

            return enderecos ?? new List<EnderecoModel>();
            //return
            //    App.Data.Connection.Table<EnderecoModel>()
            //        .Where(
            //            c =>
            //                c.idClientesOffLine == idClienteOffLine &&
            //                c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa)
            //        .ToList();
        }


        public static List<EnderecoModel> GetAllEnderecoModelsToSync()
        {
            var lUpload =
               App.Data.Connection.Table<EnderecoModel>()
                   .Where(
                       c =>
                           c.dtUltimaAlteracao >
                           App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime
                           && c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
            return lUpload.ToList();
        }

        public static EnderecoModel GetEndereco(int idEnderecoOffLine)
        {
            return App.Data.Connection.Table<EnderecoModel>().FirstOrDefault(c => c.idEnderecoOffLine == idEnderecoOffLine);
        }

        public static void Delete(EnderecoModel objEnderecoModel)
        {
            if (objEnderecoModel.idEndereco != null)
            {
                var logExclusao = new LogExclusaoModel
                {
                    idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                    idPK = objEnderecoModel.idEndereco ?? 0,
                    xTable = TableMobile.TB_ENDERECO
                };
                App.Data.Connection.Insert(logExclusao);
            }
            if (objEnderecoModel.idClientesOffLine != null)
                App.Data.Connection.Delete(objEnderecoModel);
        }
    }
}
