using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class CategoriaRepository
    {


        //public static List<BasicPickerModel> GetLisToFindProdutoBasicPickerModels(int idRepresentada = 0)
        //{
        //    var lreturn = new List<BasicPickerModel> { new BasicPickerModel { Id = 0, Display = "TODOS" } };
        //    var xQuery = "";
        //    List<BasicPickerModel> dadosPesquisa;
        //    if (idRepresentada != 0)
        //    {
        //        xQuery = string.Format(@"select distinct idCategoria Id from {0} Where idRepresentada = {1}", TableMobile.TB_PRODUTO, idRepresentada);
        //        dadosPesquisa = App.Data.Connection.Query<BasicPickerModel>(xQuery);
        //    }
        //    else
        //    {
        //        xQuery = string.Format(@"select distinct idCategoria Id from {0} ", TableMobile.TB_PRODUTO);
        //        dadosPesquisa = App.Data.Connection.Query<BasicPickerModel>(xQuery);
        //    }

        //    if (dadosPesquisa != null)
        //        lreturn.AddRange(dadosPesquisa.Select(categoria => GetCategoriaPickerModel(categoria.Id)));
        //    return lreturn;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idRepresentacao">Passe 0 para trazer todos</param>
        /// <param name="getItemTodos"></param>
        /// <returns></returns>
        public static List<BasicPickerModel> GetListToBasicPickerModel(int idRepresentacao, bool getItemTodos = false)
        {
            try
            {


                var lreturn = new List<BasicPickerModel>();
                if (getItemTodos)
                    lreturn.Add(new BasicPickerModel { Id = 0, Display = "TODOS" });


                if (idRepresentacao != 0)
                {
                    foreach (var categoria in App.Data.Connection.Table<CategoriaProdutoModel>().Where(c =>
                        c.stAtivo &&
                        c.idRepresentacao == idRepresentacao &&
                        c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa
                        ).OrderBy(O => O.xCategoria))
                    {
                        if (!RegistroUtilizadoComoPai(categoria.idCategoria ?? 0))
                        {
                            lreturn.Add(new BasicPickerModel
                            {
                                Id = categoria.idCategoria ?? 0,
                                Display = categoria.xCategoria
                            });

                        }
                    }
                }
                else
                {
                    var dados = App.Data.Connection.Table<CategoriaProdutoModel>().Where(c =>
                        c.stAtivo &&
                        c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa
                        ).OrderBy(O => O.xCategoria).ToList();


                    foreach (var item in dados)
                    {
                        if (!RegistroUtilizadoComoPai(item.idCategoria ?? 0))
                        {
                            lreturn.Add(new BasicPickerModel
                            {
                                Id = item.idCategoria ?? 0,
                                Display = item.xCategoria
                            });

                        }

                    }
                }
                return lreturn;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<BasicPickerModel>();
            }
        }


        public static List<ListItemModel> GetListItemModel(int idRepresentacao)
        {
            try
            {
                var lreturn = new List<ListItemModel> { new ListItemModel { Id = 0, Display = "TODOS" } };

                if (idRepresentacao != 0)
                {
                    foreach (var categoria in App.Data.Connection.Table<CategoriaProdutoModel>().Where(c =>
                        c.stAtivo &&
                        c.idRepresentacao == idRepresentacao &&
                        c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa
                        ).OrderBy(O => O.xCategoria))
                    {
                        if (!RegistroUtilizadoComoPai(categoria.idCategoria ?? 0))
                        {
                            lreturn.Add(new ListItemModel
                            {
                                Id = categoria.idCategoria ?? 0,
                                Display = categoria.xCategoria
                            });

                        }
                    }
                }
                else
                {
                    var xWhere = $"stAtivo = 1 and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} ";
                    if (!App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                    {
                        var xQueryRepresentacoes = $@"SELECT * FROM {TableMobile.TB_REPRESENTADA_ASPNETUSERS} WHERE
                                                        idEmpresa_aspnetUsers = {App.CurrentAspnetUserModel
                            .objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers}";

                        var lRepresentadasAspnetUsers =
                            App.Data.Connection.Query<RepresentadaAspnetUsersModel>(xQueryRepresentacoes);

                        var inIdRepresentada = lRepresentadasAspnetUsers.Select(c => c.idRepresentada.ToString()).ToList().Aggregate("", (current, item) => current + ((current == "" ? "" : " , ") + item));
                        xWhere += $" and idRepresentacao in ({inIdRepresentada}) ";
                    }

                    //var dados = App.Data.Connection.Table<CategoriaProdutoModel>().Where(c =>
                    //    c.stAtivo &&
                    //    c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa
                    //    ).OrderBy(O => O.xCategoria).ToList();

                    var xQuery = $@"SELECT * FROM {TableMobile.TB_CATEGORIA} WHERE
                                        {xWhere}";

                    var dados = App.Data.Connection.Query<CategoriaProdutoModel>(xQuery);



                    foreach (var item in dados)
                    {
                        if (!RegistroUtilizadoComoPai(item.idCategoria ?? 0))
                        {
                            lreturn.Add(new ListItemModel
                            {
                                Id = item.idCategoria ?? 0,
                                Display = item.xCategoria
                            });

                        }

                    }
                }
                return lreturn;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<ListItemModel>();
            }
        }

        public static bool RegistroUtilizadoComoPai(int idCategoria)
        {
            try
            {


                var icount = App.Data.Connection.ExecuteScalar<int>(
                    $@"select count(*) from {TableMobile.TB_CATEGORIA} 
                                            where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and idCategoriaPai = {idCategoria}");

                return icount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static BasicPickerModel GetCategoriaPickerModel(int idCategoria)
        {
            //var categoria =
            //    App.Data.Connection.Table<CategoriaProdutoModel>().FirstOrDefault(c => c.idCategoria == idCategoria);

            var xQuery = $@"SELECT * FROM {TableMobile.TB_CATEGORIA} WHERE idCategoria = {idCategoria}";

            var categoria = App.Data.Connection.Query<CategoriaProdutoModel>(xQuery);

            if (categoria != null && categoria.Any())
            {
                var item = categoria.FirstOrDefault();
                return new BasicPickerModel { Id = item.idCategoria ?? 0, Display = item.xCategoria };
            }
            return new BasicPickerModel { Id = 0, Display = "NÃO ENCONTRADO" };


        }

        public static string GetDisplay(int idCategoria)
        {

            var xQuery = $@"SELECT xCategoria FROM {TableMobile.TB_CATEGORIA} WHERE idCategoria = {idCategoria}";

            var retorno = App.Data.Connection.ExecuteScalar<string>(xQuery);

            return retorno;


            //return
            //    App.Data.Connection.Table<UnidadeMedidaModel>()
            //        .Where(c => c.idUnidadeMedida == idUnidadeMedida)
            //        .Select(c => c.xUnidadeMedida)
            //        .FirstOrDefault();
        }
    }
}
