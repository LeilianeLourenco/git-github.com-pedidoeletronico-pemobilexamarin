using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Agenda
{
    public class AgendaRepository
    {
        public static List<AgendaListarModel> GetInfinit(int skip, int take, string xFiltro, int idEmpresa, bool bTrazerRealizados, int? idAtividadeOffline, int? idClienteOffline, bool bTrazerCancelados, bool bTrazerTodosRepresentantes, bool bOrdenarFormaCrescente)
        {
            var retorno = new List<AgendaListarModel>();
            try
            {
                string xQuery = string.Empty;
                string xWhere = string.Empty;
                if (!string.IsNullOrEmpty(xFiltro))
                {
                    xFiltro = xFiltro.RemoverAcentos().ToUpper();
                    xWhere += $@" and (UPPER(coalesce(c.xRazaoSocial,'')) like('%{xFiltro}%')
                                        or UPPER(coalesce(c.xFantasia,'')) like('%{xFiltro}%')
                                        or coalesce(a.xDtInicioEvento,'') like('%{xFiltro}%')
                                        or c.xCpfCnpj like('%{xFiltro}%'))";
                }


                if (idAtividadeOffline.GetValueOrDefault() > 0)
                    xWhere += $@" and a.idAtividadeOffline = {idAtividadeOffline}";

                if (idClienteOffline.GetValueOrDefault() > 0)
                    xWhere += $@" and idClienteOffline = {idClienteOffline}";


                if (!bTrazerTodosRepresentantes)
                    xWhere += $@" and a.xVendedorVinculado like '%{App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xApelido}%'";



                if (bTrazerRealizados)
                    if (bTrazerRealizados)
                        xWhere += $@" and (a.bRealizado = 1 or a.bEventoCancelado = 1)";
                    else
                        xWhere += $@" and a.bRealizado = 1";
                else
                    if (bTrazerCancelados)
                    xWhere += $@" and (a.bRealizado = 0 or a.bEventoCancelado = 1)";
                else
                    xWhere += $@" and a.bRealizado = 0";


                if (bTrazerCancelados)
                    if (bTrazerRealizados)
                        xWhere += $@" and (a.bEventoCancelado = 1 or a.bRealizado = 1)";
                    else
                        xWhere += $@" and a.bEventoCancelado = 1";
                else
                   if (bTrazerRealizados)
                    xWhere += $@" and (a.bEventoCancelado = 0 or a.bRealizado = 1)";
                else
                    xWhere += $@" and a.bEventoCancelado = 0";




                xQuery = $@"select distinct
		                    a.idAtividade,  a.idAtividadeOffline, a.idClienteOffline, a.xVendedorVinculado, a.bEventoCancelado, c.xTelefones as xTelefoneCliente,
                            a.xDescricaoAtividade, a.dtInicioEvento, t.bVerificaLocalizacao, a.bRealizado, a.xEnderecoCompleto,
                            a.dtFimEvento, t.xCor, c.xFantasia as xNomeCliente, t.xDescricaoAtividade as xNomeAtividade, a.xAnotacaoPrivada
		                    from TB_ATIVIDADES a
                            LEFT JOIN TB_CLIENTES c on a.idClienteOffline = c.idClientesOffline
                            INNER JOIN TB_TIPOATIVIDADESCRM t on a.idTipoAtividade = t.idTipoAtividade
                            Where a.idEmpresa = {idEmpresa} and a.bExcluido = 0 {xWhere}";

                if (bOrdenarFormaCrescente)
                {
                    xQuery += $@" ORDER BY a.dtInicioEvento
                                LIMIT {take} OFFSET {skip}";
                }
                else
                {
                    xQuery += $@" ORDER BY a.dtInicioEvento DESC
                                LIMIT {take} OFFSET {skip}";
                }

                retorno = App.Data.Connection.Query<AgendaListarModel>(xQuery);


                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
            return retorno;
        }

        public static List<AgendaListarModel> GetInfinitParaEdicao(int skip, int take, string xFiltro, int idEmpresa, bool bTrazerRealizados, int? idAtividadeOffline, int? idClienteOffline, bool bTrazerCancelados, bool bTrazerTodosRepresentantes)
        {
            var retorno = new List<AgendaListarModel>();
            try
            {
                string xQuery = string.Empty;
                string xWhere = string.Empty;
                if (!string.IsNullOrEmpty(xFiltro))
                {
                    xFiltro = xFiltro.RemoverAcentos().ToUpper();
                    xWhere += $@" and (UPPER(coalesce(c.xRazaoSocial,'')) like('%{xFiltro}%')
                                        or UPPER(coalesce(c.xFantasia,'')) like('%{xFiltro}%')
                                        or coalesce(a.xDtInicioEvento,'') like('%{xFiltro}%')
                                        or c.xCpfCnpj like('%{xFiltro}%'))";
                }


                if (idAtividadeOffline.GetValueOrDefault() > 0)
                    xWhere += $@" and a.idAtividadeOffline = {idAtividadeOffline}";

                if (idClienteOffline.GetValueOrDefault() > 0)
                    xWhere += $@" and idClienteOffline = {idClienteOffline}";


                xQuery = $@"select distinct
		                    a.idAtividade,  a.idAtividadeOffline, a.idClienteOffline, a.xVendedorVinculado, a.bEventoCancelado, c.xTelefones as xTelefoneCliente,
                            a.xDescricaoAtividade, a.dtInicioEvento, t.bVerificaLocalizacao, a.bRealizado, a.xEnderecoCompleto,
                            a.dtFimEvento, t.xCor, c.xFantasia as xNomeCliente, t.xDescricaoAtividade as xNomeAtividade, a.xAnotacaoPrivada
		                    from TB_ATIVIDADES a
                            LEFT JOIN TB_CLIENTES c on a.idClienteOffline = c.idClientesOffline
                            INNER JOIN TB_TIPOATIVIDADESCRM t on a.idTipoAtividade = t.idTipoAtividade
                            Where a.idEmpresa = {idEmpresa} and a.bExcluido = 0 {xWhere}";


                xQuery += $@" ORDER BY a.dtInicioEvento DESC
                                LIMIT {take} OFFSET {skip}";

                retorno = App.Data.Connection.Query<AgendaListarModel>(xQuery);


                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
            return retorno;
        }



        public static void RealizarAdiantamento(int idAtividadeOffline, DateTime dtInicioEvento, DateTime dtFimEvento)
        {
            try
            {
                var xQuery =
                  $"SELECT * FROM {TableMobile.TB_ATIVIDADES} WHERE  idAtividadeOffline = {idAtividadeOffline}";

                var obj = (App.Data.Connection.Query<AtividadeAgendaModel>(xQuery)).FirstOrDefault();

                if (obj != null)
                {
                    obj.dtInicioEvento = dtInicioEvento;
                    obj.dtFimEvento = dtFimEvento;
                    obj.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                    obj.idAspnetUsers = App.CurrentAspnetUserModel.Id;

                    App.Data.Connection.Update(obj);
                }

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public static bool ExcluirEvento(int idAtividadeOffline)
        {
            try
            {
                var xQuery =
                  $"SELECT * FROM {TableMobile.TB_ATIVIDADES} WHERE  idAtividadeOffline = {idAtividadeOffline}";

                var obj = (App.Data.Connection.Query<AtividadeAgendaModel>(xQuery)).FirstOrDefault();

                if (obj != null)
                {
                    obj.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                    obj.idAspnetUsers = App.CurrentAspnetUserModel.Id;
                    obj.bExcluido = true;
                    App.Data.Connection.Update(obj);
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return false;
            }
        }

        public static void RealizarEncerramento(int idAtividadeOffline)
        {
            try
            {
                var xQuery =
                 $"SELECT * FROM {TableMobile.TB_ATIVIDADES} WHERE  idAtividadeOffline = {idAtividadeOffline}";

                var obj = (App.Data.Connection.Query<AtividadeAgendaModel>(xQuery)).FirstOrDefault();

                if (obj != null)
                {
                    obj.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                    obj.idAspnetUsers = App.CurrentAspnetUserModel.Id;
                    obj.bRealizado = true;
                    obj.bEventoCancelado = false;
                    App.Data.Connection.Update(obj);
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public static AtividadeAgendaModel GetDadosEvento(int idAtividadeOffline)
        {
            try
            {
                var xQuery =
                $"SELECT dtTempoInitCheck, dtTempoCheck, xLocalCheckIn, xLocalCheckOut, tsDuracaoCheck FROM {TableMobile.TB_ATIVIDADES} WHERE idAtividadeOffline = {idAtividadeOffline}";

                return (App.Data.Connection.Query<AtividadeAgendaModel>(xQuery)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new AtividadeAgendaModel();
            }
        }

        public static void SalvarCheckIn(AgendaListarModel agenda)
        {
            try
            {
                var xQuery =
                $"SELECT * FROM {TableMobile.TB_ATIVIDADES} WHERE idAtividadeOffline = {agenda.idAtividadeOffline}";

                var obj = (App.Data.Connection.Query<AtividadeAgendaModel>(xQuery)).FirstOrDefault();

                if (obj != null)
                {
                    obj.dtTempoInitCheck = DateTime.UtcNow.AddHours(-3);
                    obj.xLocalCheckIn = agenda.xLocalCheckIn;
                    obj.dtUltimaAlteracao = DateTime.UtcNow.AddHours(-3);
                    App.Data.Connection.Update(obj);
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public static AtividadeAgendaModel SalvarCheckOut(AgendaListarModel agenda)
        {
            try
            {
                var xQuery =
                $"SELECT * FROM {TableMobile.TB_ATIVIDADES} WHERE idAtividadeOffline = {agenda.idAtividadeOffline}";

                var obj = (App.Data.Connection.Query<AtividadeAgendaModel>(xQuery)).FirstOrDefault();

                if (obj != null)
                {
                    obj.tsDuracaoCheck = DateTime.UtcNow.AddHours(-3) - obj.dtTempoInitCheck;
                    obj.xLocalCheckOut = agenda.xLocalCheckOut;
                    obj.dtUltimaAlteracao = DateTime.UtcNow.AddHours(-3);
                    App.Data.Connection.Update(obj);
                }

                return obj;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new AtividadeAgendaModel();
            }
        }

        public static void RealizarReabertura(int idAtividadeOffline)
        {
            try
            {
                var xQuery =
                $"SELECT * FROM {TableMobile.TB_ATIVIDADES} WHERE  idAtividadeOffline = {idAtividadeOffline}";

                var obj = (App.Data.Connection.Query<AtividadeAgendaModel>(xQuery)).FirstOrDefault();

                if (obj != null)
                {
                    obj.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                    obj.idAspnetUsers = App.CurrentAspnetUserModel.Id;
                    obj.bRealizado = false;
                    obj.bEventoCancelado = false;
                    App.Data.Connection.Update(obj);
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public static void CancelarEvento(int idAtividadeOffline)
        {
            try
            {
                var xQuery =
                $"SELECT * FROM {TableMobile.TB_ATIVIDADES} WHERE  idAtividadeOffline = {idAtividadeOffline}";

                var obj = (App.Data.Connection.Query<AtividadeAgendaModel>(xQuery)).FirstOrDefault();

                if (obj != null)
                {
                    obj.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                    obj.idAspnetUsers = App.CurrentAspnetUserModel.Id;
                    obj.bRealizado = false;
                    obj.bEventoCancelado = true;
                    App.Data.Connection.Update(obj);
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public static List<ListItemModel> Get(int skip, int take, string xFiltro)
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var xQuery = "";
                const string xFields =
                    "xDescricaoAtividade Display, idTipoAtividade Id";
                xQuery = $"select {xFields} from {TableMobile.TB_TIPOATIVIDADESCRM} where idEmpresa = {idEmpresa} and stAtivo = 1";

                if (!string.IsNullOrEmpty(xFiltro))
                {
                    xFiltro = xFiltro.RemoverAcentos().ToUpper();
                    xQuery += $" and (UPPER(xDescricaoAtividade) like('%{xFiltro}%'))";

                    //xQuery += $" and UPPER(xRazaoSocial) like('%{xFiltro.ToUpper()}%') ";
                }


                xQuery += $@" order by UPPER(xDescricaoAtividade)
                                            LIMIT {take} OFFSET {skip}";

                var resultado = App.Data.Connection.Query<ListItemModel>(xQuery);
                return resultado;
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
                return new List<ListItemModel>();
            }
        }

        public static string GetEnderecoClienteEscolhido(int idEmpresa, int idClientesOffline)
        {
            try
            {
                string _endereco = string.Empty;
                var _enderecoPrincipalCliente = App.Data.Connection.Table<EnderecoModel>().Where(e => e.idEmpresa == idEmpresa && e.idClientesOffLine == idClientesOffline && e.stPrincipal).FirstOrDefault();

                if (_enderecoPrincipalCliente != null)
                {
                    return $"{_enderecoPrincipalCliente.xEndereco},{_enderecoPrincipalCliente.cNumero.ToString()} - {_enderecoPrincipalCliente.xBairro}, {_enderecoPrincipalCliente.xCidade} - {_enderecoPrincipalCliente.xEstado}, {_enderecoPrincipalCliente.xCep}";
                }
                else
                {
                    return $"Sem endereço principal selecionado";
                }

            }
            catch (Exception ex)
            {
                return $"Sem endereço principal selecionado";
            }
        }

        public static AtividadeAgendaModel GetAtividadeModel(int idAtividadeOffline)
        {
            AtividadeAgendaModel obj = new AtividadeAgendaModel();
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                var xQuery =
                    $"SELECT * FROM {TableMobile.TB_ATIVIDADES} WHERE idAtividadeOffline = {idAtividadeOffline} and idEmpresa = {idEmpresa}";

                obj = (App.Data.Connection.Query<AtividadeAgendaModel>(xQuery)).FirstOrDefault();

                var xQueryAnexo =
                   $"SELECT * FROM {TableMobile.TB_ANEXOS} WHERE idAtividade = {idAtividadeOffline} and idEmpresa = {idEmpresa}";

                var teste = (App.Data.Connection.Query<AnexosModel>(xQueryAnexo)).ToList();

                obj.lAnexosAtividade = new ObservableCollection<AnexosModel>(teste);
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

            return obj;
        }

        public static void SaveAtividade(AtividadeAgendaModel obj)
        {
            try
            {
                //if (obj.dtInicioEvento != null)
                //    if ((obj.dtInicioEvento ?? DateTime.Now).Kind != DateTimeKind.Local)
                //        obj.dtInicioEvento = (obj.dtInicioEvento ?? DateTime.Now).ToLocalTime();


                //if (obj.dtFimEvento != null)
                //    if ((obj.dtFimEvento ?? DateTime.Now).Kind != DateTimeKind.Local)
                //        obj.dtFimEvento = (obj.dtFimEvento ?? DateTime.Now).ToLocalTime();

                obj.idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                obj.idAspnetUsers = App.CurrentAspnetUserModel.Id;
                obj.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                obj.xVendedorVinculado = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xApelido;

                if (obj.idAtividadeOffline == 0)
                    App.Data.Connection.Insert(obj);
                else
                    App.Data.Connection.Update(obj);

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

        }

        public static List<AtividadeAgendaModel> GetAtividadeAgendaParaUploadModel()
        {
            var lUpload =
               App.Data.Connection.Table<AtividadeAgendaModel>()
                   .Where(
                       c =>
                           (c.dtUltimaAlteracao >
                           App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime)
                           && c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
            return lUpload.ToList();
        }

        public static ListItemModel GetRegistroParaListagem(int idTipoAtividade)
        {
            var xQuery =
                $@"SELECT xDescricaoAtividade Display, idTipoAtividade Id FROM {TableMobile.TB_TIPOATIVIDADESCRM} WHERE idTipoAtividade = {idTipoAtividade} 
                                                            and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

            var resultado = App.Data.Connection.Query<ListItemModel>(xQuery);

            return resultado.FirstOrDefault();
        }
    }
}
