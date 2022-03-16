using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    public class StatusRepository
    {
        public static void SalvarStatusProibidos(int idStatus, IEnumerable<StatusRepresentanteProibido> statusProibido)
        {
            foreach (var item in statusProibido)
            {
                item.idStatus = idStatus;
                App.Data.Connection.Insert(item);
            }
        }

        public static StatusModel GetRegistro(int idStatus)
        {
            try
            {
                var xQuery = $@"SELECT * FROM TB_STATUS where idStatus = {idStatus}";
                var registro = App.Data.Connection.Query<StatusModel>(xQuery).FirstOrDefault();

                xQuery = $@"SELECT * FROM [{TableMobile.TB_STATUS_PROIBIDO}] WHERE [idStatus] = {idStatus}";
                registro.lRepresentantesProibidos = App.Data.Connection.Query<StatusRepresentanteProibido>(xQuery);
                return registro;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return null;
            }
        }

        /// <summary>
        /// 0 - Orçamento
        /// 1 - Pedido
        /// </summary>
        /// <param name="stLancamento"></param>
        /// <returns></returns>
        public static List<BasicPickerModel> GetListToBasicPickerModel(byte stLancamento)
        {
            try
            {
                var where = $"idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stAparecerStatus <> 1";
                if (stLancamento == 1) // pedido
                {
                    where = $"idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stAparecerStatus <> 0";
                }

                var xQuery =
                    $@"select idStatus XId, stVenda Id , xNome Display from TB_STATUS
                                                    where  stAtivo = 1 and {where}";

                var retorno = App.Data.Connection.Query<BasicPickerModel>(xQuery);
                if (retorno == null || !retorno.Any())
                {
                    // 0 - aberto
                    // 1 - cancelado 
                    // 2 - vendido
                    retorno = new List<BasicPickerModel>();
                    if (stLancamento == 0)
                    {
                        retorno.Add(new BasicPickerModel
                        {
                            Id = 0,
                            Display = "ABERTO"
                        });
                        retorno.Add(new BasicPickerModel
                        {
                            Id = 1,
                            Display = "CANCELADO"
                        });
                        retorno.Add(new BasicPickerModel
                        {
                            Id = 2,
                            Display = "VENDIDO"
                        });
                    }
                    else
                    {
                        retorno.Add(new BasicPickerModel
                        {
                            XId = "0",
                            Id = 1,
                            Display = "CANCELADO"
                        });
                        retorno.Add(new BasicPickerModel
                        {
                            Id = 2,
                            Display = "VENDIDO"
                        });
                    }
                }



                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<BasicPickerModel>();
            }

        }



        /// <summary>
        /// 0 - Orçamento
        /// 1 - Pedido
        /// </summary>
        /// <param name="stLancamento"></param>
        /// <returns></returns>
        public static List<ListItemModel> Get(int skip, int take, int idEmpresa_aspnetusers, string xFiltro, byte stLancamento)
        {
            try
            {
                var where = $"idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stAparecerStatus <> 1";
                if (stLancamento == 1) // pedido
                {
                    //where = $"idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stVenda <> 0 and stAparecerStatus <> 0";
                    where = $"idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stAparecerStatus <> 0";
                }

                if (!string.IsNullOrEmpty(xFiltro))
                {
                    xFiltro = xFiltro.RemoverAcentos().ToUpper();
                    where += $" and (UPPER(xNome) like('%{xFiltro}%') or UPPER(coalesce(xDisplaySemCaracter,'')) like('%{xFiltro}%'))";
                    //where += $" and UPPER(xNome) like('%{xFiltro.ToUpper()}%') ";
                }

                where += $" LIMIT {take} OFFSET {skip} ";

                var xQuery =
                    $@"select idStatus XId, stVenda Id , UPPER(xNome) Display from TB_STATUS
                                                    where stAtivo = 1 and {where}";

                var retorno = App.Data.Connection.Query<ListItemModel>(xQuery);

                if (take == 0 && (retorno == null || !retorno.Any()))
                {
                    // 0 - aberto
                    // 1 - cancelado 
                    // 2 - vendido
                    retorno = new List<ListItemModel>();
                    if (stLancamento == 0)
                    {
                        retorno.Add(new ListItemModel
                        {
                            Id = 0,
                            Display = "ABERTO"
                        });
                        retorno.Add(new ListItemModel
                        {
                            Id = 1,
                            Display = "CANCELADO"
                        });
                        retorno.Add(new ListItemModel
                        {
                            Id = 2,
                            Display = "VENDIDO"
                        });
                    }
                    else
                    {
                        retorno.Add(new ListItemModel
                        {
                            XId = "0",
                            Id = 1,
                            Display = "CANCELADO"
                        });
                        retorno.Add(new ListItemModel
                        {
                            Id = 2,
                            Display = "VENDIDO"
                        });
                    }
                }
                xQuery = $"SELECT * FROM [{TableMobile.TB_STATUS_PROIBIDO}]" +
                         $"WHERE [{nameof(StatusRepresentanteProibido.idEmpresa_aspnetusers)}] = {idEmpresa_aspnetusers}";

                var proibidos = App.Data.Connection.Query<StatusRepresentanteProibido>(xQuery);

                return retorno
                            .Where(wh => proibidos.FirstOrDefault(fs => fs.idStatus.ToString() == wh.XId) == null)
                            .ToList();
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<ListItemModel>();
            }

        }


        /// <summary>
        /// idStatus XId
        /// stVenda Id
        /// </summary>
        /// <param name="stLancamento"></param>
        /// <returns></returns>
        public static ListItemModel GetDefault(byte stLancamento)
        {
            try
            {
                var where = $"idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stVenda = 0 and stAparecerStatus <> 1";
                if (stLancamento == 1) // pedido
                {
                    //var empresa = EmpresaRepository.GetEmpresa();
                    //if (App.tipouser == App.TipoUser.OMIE)
                    //{
                    //    where = $"idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stAparecerStatus <> 0";
                    //}
                    //else
                    //{
                    //    where = $"idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stVenda == 2 and stAparecerStatus <> 0";
                    //}
                    where = $"idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stVenda = 2 and stAparecerStatus <> 0";
                }


                //processo de checar as flags da configuração
                if (stLancamento == 1) // pedido
                {
                    int? idStatusDefaultPedido = App.Data.Connection.ExecuteScalar<int?>($"SELECT idStatusVendaDefault FROM TB_CONFIGURACOES_GERAIS WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} ");

                    if (idStatusDefaultPedido.GetValueOrDefault() > 0)
                    {
                        where = $" idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and idStatus = {idStatusDefaultPedido.GetValueOrDefault()} ";
                    }
                }

                if (stLancamento == 0) // orçamento
                {
                    int? idStatusOrcamentoDefault = App.Data.Connection.ExecuteScalar<int?>($"SELECT idStatusOrcamentoDefault FROM TB_CONFIGURACOES_GERAIS WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} ");

                    if (idStatusOrcamentoDefault.GetValueOrDefault() > 0)
                    {
                        where = $" idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and idStatus = {idStatusOrcamentoDefault.GetValueOrDefault()} ";
                    }
                }


                var xQuery =
                    $@"select idStatus XId, stVenda Id , UPPER(xNome) Display from TB_STATUS
                                                    where stAtivo = 1 and {where}";

                var lretorno = App.Data.Connection.Query<ListItemModel>(xQuery);

                if (lretorno?.Count() == 0)
                {
                    xQuery =
                       $@"select idStatus XId, stVenda Id , UPPER(xNome) Display from TB_STATUS
                                                    where {where}";

                    lretorno = App.Data.Connection.Query<ListItemModel>(xQuery);
                }


                return lretorno.OrderBy(t => t.Display).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new ListItemModel { Display = "ocorreu um erro ao buscar o status default." };
            }

        }

        public static void RemoverProbidos(int idStatus)
        {
            try
            {
                var xQuery =
                    $"DELETE FROM [{TableMobile.TB_STATUS_PROIBIDO}] WHERE [idStatus] = {idStatus}";

                App.Data.Connection.Execute(xQuery);
            }
            catch (Exception ex)
            {
                ex.TrakException("RemoveStatusProbidos");
            }
        }
    }





}
