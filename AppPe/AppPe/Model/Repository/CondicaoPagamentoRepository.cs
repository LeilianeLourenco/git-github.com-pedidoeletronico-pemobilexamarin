using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class CondicaoPagamentoRepository
    {

        public static List<ListItemModel> Get(int skip, int take, string xFiltro, int? idClienteOffline = null, int? idCliente = null)
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var idRepresentante = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers;
                var xQuery = "";
                const string xFields = @"idCondicaoPagamento , xCondicaoPagamento, xFormula, nParcelas, idEmpresa, idTabelaPreco";
                xQuery = $"select {xFields} from {TableMobile.TB_CONDICAOPAGAMENTO} where idEmpresa = {idEmpresa} and stAtivo = 1";

                if (!string.IsNullOrEmpty(xFiltro))
                {
                    xFiltro = xFiltro.RemoverAcentos().ToUpper();
                    xQuery += $" and (UPPER(xCondicaoPagamento) like('%{xFiltro}%') or UPPER(coalesce(xDisplaySemCaracter,'')) like('%{xFiltro}%'))";
                    //xQuery += $" and UPPER(xCondicaoPagamento) like('%{xFiltro.ToUpper()}%') ";
                }

                var _condicoesPermitidas = new List<ClientesCondicoesPagamentoModel>();
                if (idCliente.GetValueOrDefault() > 0)
                {
                    _condicoesPermitidas = ClienteRepository.GetClientesCondicaoPagamento(idCliente ?? 0, idEmpresa);
                    if (_condicoesPermitidas?.Any() == true)
                    {
                        var idsPermitidos = string.Join(",", _condicoesPermitidas.Select(x => x.idCondicaoPagamento));
                        xQuery += $" and idCondicaoPagamento in ({idsPermitidos})";
                    }
                }

                xQuery += $@" order by UPPER(xCondicaoPagamento)
                                            LIMIT {take} OFFSET {skip}";


                var dados = App.Data.Connection.Query<CondicaoPagamentoModel>(xQuery);


                // método de utilização de tabela de preço na condição de pagamento.
                if (idClienteOffline.GetValueOrDefault() > 0)
                {
                    var _cliente = ClienteRepository.GetClienteModel(idClienteOffline.GetValueOrDefault(), false);
                    var _tabelasPermitidas = TabelaPrecoRepository.GetTabelaPrecoSimplificadas(0, 1, idEmpresa, null, null, null, _cliente.idClientes);

                    var _idsTabelas = _tabelasPermitidas.Select(t => (int?)t.idTabelaPreco).ToList();

                    dados = dados.Where(c => c.idEmpresa == idEmpresa && (c.idTabelaPreco == null || _idsTabelas.Contains(c.idTabelaPreco))).ToList();
                }

                if (dados?.Count == 0 && skip == 0)
                {
                    xQuery = $"select {xFields} from {TableMobile.TB_CONDICAOPAGAMENTO} where idEmpresa = {idEmpresa} limit 1";

                    dados = App.Data.Connection.Query<CondicaoPagamentoModel>(xQuery);
                }

                if (dados == null)
                    return new List<ListItemModel>();


                return dados.ToList().Select(item =>
                {
                    if (item.idCondicaoPagamento != null)
                        return new ListItemModel
                        {
                            IdOnline = (int)item.idCondicaoPagamento,
                            Id = (int)item.idCondicaoPagamento,
                            Display = item.xCondicaoPagamento,
                            Detail = item.xDisplay2
                        };
                    return null;
                }).ToList();
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
                return new List<ListItemModel>();
            }
        }

        public static ListItemModel GetFirstItem()
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var xQuery = "";
                const string xFields = @"idCondicaoPagamento , xCondicaoPagamento, xFormula, nParcelas";
                xQuery = $"select {xFields} from {TableMobile.TB_CONDICAOPAGAMENTO} where idEmpresa = {idEmpresa} limit 1";

                var dados = App.Data.Connection.Query<CondicaoPagamentoModel>(xQuery);
                if (dados == null)
                    return new ListItemModel();

                var item = dados.FirstOrDefault();
                return new ListItemModel
                {
                    IdOnline = (int)item.idCondicaoPagamento,
                    Id = (int)item.idCondicaoPagamento,
                    Display = item.xCondicaoPagamento,
                    Detail = item.xDisplay2
                };
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
                return new ListItemModel();
            }
        }

        public static ListItemModel GetItem(int idCondicaoPagamento, int? idClienteOffLine = null)
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var xQuery = "";
                const string xFields = @"idCondicaoPagamento , xCondicaoPagamento, xFormula, nParcelas, vDescCondicao, idEmpresa, idTabelaPreco";

                int idClientes = ClienteRepository.GetIdClienteNuvem(idClienteOffLine ?? 0);

                if (idClientes > 0)
                {
                    var _condicoesPermitidas = ClienteRepository.GetClientesCondicaoPagamento(idClientes, idEmpresa);
                    if (_condicoesPermitidas?.Any() == true)
                    {
                        if (!_condicoesPermitidas.Select(x => x.idCondicaoPagamento).Contains(idCondicaoPagamento))
                            idCondicaoPagamento = _condicoesPermitidas.FirstOrDefault().idCondicaoPagamento;
                    }
                }

                xQuery = $"select {xFields} from {TableMobile.TB_CONDICAOPAGAMENTO} where idEmpresa = {idEmpresa} and idCondicaoPagamento = {idCondicaoPagamento} ";

                var dados = App.Data.Connection.Query<CondicaoPagamentoModel>(xQuery);

                //if (dados.Count == 0)
                //{
                //    xQuery = $"select {xFields} from {TableMobile.TB_CONDICAOPAGAMENTO} where idEmpresa = {idEmpresa} limit 1";

                //    dados = App.Data.Connection.Query<CondicaoPagamentoModel>(xQuery);
                //}

                // método de utilização de tabela de preço na condição de pagamento.
                if (idClienteOffLine.GetValueOrDefault() > 0 && dados?.Count() > 0)
                {
                    var _cliente = ClienteRepository.GetClienteModel(idClienteOffLine.GetValueOrDefault(), false);
                    var _tabelasPermitidas = TabelaPrecoRepository.GetTabelaPrecoSimplificadas(0, 1, idEmpresa, null, null, null, _cliente.idClientes);

                    var _idsTabelas = _tabelasPermitidas.Select(t => (int?)t.idTabelaPreco).ToList();

                    var _condicoesExistentes = dados.Where(c => c.idEmpresa == idEmpresa && (c.idTabelaPreco == null || _idsTabelas.Contains(c.idTabelaPreco))).ToList();

                    var _condicaoClienteExiste = _condicoesExistentes.Where(t => t.idCondicaoPagamento == _cliente.idCondicaoPagamento).FirstOrDefault();

                    if (_condicaoClienteExiste != null)
                    {
                        dados = dados.Where(c => c.idCondicaoPagamento == _condicaoClienteExiste.idCondicaoPagamento).ToList();
                    }
                }

                if (dados?.Count() == 0)
                    return new ListItemModel();

                var item = dados.FirstOrDefault();
                if (item != null)
                {
                    return new ListItemModel
                    {
                        IdOnline = (int)item.idCondicaoPagamento,
                        Id = (int)item.idCondicaoPagamento,
                        Display = item.xCondicaoPagamento,
                        Detail = item.xDisplay2,
                        idTabelaPreco = item.idTabelaPreco,
                        vDescCondicao = item.vDescCondicao //alterado
                    };
                }
                else
                {
                    return new ListItemModel { Display = "-" };
                }
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
                return new ListItemModel();
            }
        }

        [Obsolete]
        public static List<BasicPickerModel> GetListToBasicPickerModel()
        {
            var xQuery = string.Format(@"select idCondicaoPagamento , xCondicaoPagamento, xFormula, nParcelas, vDescCondicao from {0} 
                                                    where idEmpresa = {1}", TableMobile.TB_CONDICAOPAGAMENTO, App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
            try
            {
                var dados = App.Data.Connection.Query<CondicaoPagamentoModel>(xQuery);
                if (dados == null)
                    return new List<BasicPickerModel>();

                return dados.ToList().Select(item =>
                {
                    if (item.idCondicaoPagamento != null)
                        return new BasicPickerModel
                        {
                            bTrazerImagem = false,
                            IdOnline = (int)item.idCondicaoPagamento,
                            Id = (int)item.idCondicaoPagamento,
                            Display = item.xCondicaoPagamento,
                            Detail = item.xDisplay2,
                            vDescCondicao = item.vDescCondicao
                        };
                    return null;
                }).ToList().OrderBy(c => c.Display).ToList();
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("CondicaoPagamentoRepository.GetListToBasicPickerModel", ex.Message, true);
                return new List<BasicPickerModel>();
            }

        }


        public static List<ListItemModel> BuscaFormasPagamentoPorCondicao(string xFiltro, int? idCondicaoPagamento)
        {
            string _formasPermitidas = string.Empty;
            if (idCondicaoPagamento.GetValueOrDefault() > 0)
                _formasPermitidas = App.Data.Connection.Table<CondicaoPagamentoModel>().Where(t => t.idCondicaoPagamento == idCondicaoPagamento).Select(t => t.xFormaPagamentoPermitidas).FirstOrDefault();

            if (!string.IsNullOrEmpty(_formasPermitidas))
            {
                return _formasPermitidas.ToUpper().Split(';').Select(t => new ListItemModel
                {
                    Display = t,
                    Id = 0,
                }).OrderBy(t => t.Display).ToList();
            }

            var _listRetorno = App.Data.Connection.Table<FormaPagamentoModel>().Select(t => new ListItemModel
            {
                Display = t.xFormaPagamento,
                Id = t.idFormaPagamento,
            }).OrderBy(t => t.Display).ToList();


            if (_listRetorno?.Count() == 0)
            {
                _listRetorno = new List<ListItemModel>();

                _listRetorno.Add(new ListItemModel
                {
                    Display = "SEM FORMA DE PAGAMENTO",
                    Id = 0
                });
            }

            return _listRetorno;
        }

        public static string GetDisplay(int idCondicaoPagamento)
        {
            try
            {
                var xquery = $@"select xCondicaoPagamento from TB_CONDICAOPAGAMENTO
                        where idCondicaoPagamento  = {idCondicaoPagamento} and idEmpresa = {App.CurrentAspnetUserModel
                    .objEmpresaAspnetUsersModel.idEmpresa}";
                var result = App.Data.Connection.ExecuteScalar<string>(xquery);
                return result;
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("CondicaoPagamentoRepository.GetDisplay", ex.Message, true);
                return "";
            }

        }

        public static int GetIdTabelaPrecoCondicao(int idCondicaoPagamento)
        {
            try
            {
                var xquery = $@"select idTabelaPreco from TB_CONDICAOPAGAMENTO
                        where idCondicaoPagamento  = {idCondicaoPagamento} and idEmpresa = {App.CurrentAspnetUserModel
                    .objEmpresaAspnetUsersModel.idEmpresa}";
                var result = App.Data.Connection.ExecuteScalar<int>(xquery);
                return result;
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("CondicaoPagamentoRepository.GetIdTabelaPrecoCondicao", ex.Message, true);
                return 0;
            }

        }

        public static int CreateCondicaoManual()
        {
            try
            {
                int idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                var xquery = $@"select * from {TableMobile.TB_CONDICAOPAGAMENTO}
                        where xCondicaoPagamento = 'Configurado Manualmente' and idEmpresa = {idEmpresa}";

                var condicao = App.Data.Connection.Query<CondicaoPagamentoModel>(xquery).FirstOrDefault();

                if (condicao != null)
                    return condicao.idCondicaoPagamento ?? 0;

                CondicaoPagamentoModel condicaoPagamento = new CondicaoPagamentoModel
                {
                    idEmpresa = idEmpresa,
                    xCondicaoPagamento = "Configurado Manualmente",
                    stAtivo = true,
                };

                App.Data.Connection.Insert(condicaoPagamento);

                return condicaoPagamento.idCondicaoPagamento ?? 0;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
    }
}