using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.BuscaPreco;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public partial class TabelaPrecoRepository
    {

        public static string GetNameRegistro(int idPkOffLine)
        {
            try
            {
                //var xQuery = $@"select xNome Display from {TableMobile.TB_TABELAPRECO} where idTabelaPreco = {idPkOffLine}";
                //var resultado = App.Data.Connection.Query<ListItemModel>(xQuery);
                //return resultado != null ? resultado.FirstOrDefault().Display : "";

                var xquery = $@"select xNome from {TableMobile.TB_TABELAPRECO}
                        where idTabelaPreco  = {idPkOffLine}";
                var result = App.Data.Connection.ExecuteScalar<string>(xquery);
                return result;
            }
            catch (Exception)
            {
                return "-";
            }
        }

        public static List<BasicPickerModel> GetListToBasicPickerModel(bool bTrasCampanha = false)
        {

            string xQuery =
                $@"select idTabelaPreco Id , xNome Display, ('Indice de ' || coalesce(pIndiceTabela,'0' || ' %')) Detail, stTabelaPreco status, dInicial Date, dFinal Date2 from tb_tabelapreco where idEmpresa = {App
                    .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stAtivo = 1 {(bTrasCampanha ? "" : "and stTabelaPreco = 0")}";
            try
            {
                var dados = App.Data.Connection.Query<BasicPickerModel>(xQuery);
                if (dados == null)
                    return new List<BasicPickerModel>();

                if (bTrasCampanha)
                {
                    dados = dados.Where(c => c.status == 0 || (c.iDate <= DateTime.Now.ToIntFull() && c.iDate2 >= DateTime.Now.ToIntFull())).ToList();
                }

                foreach (var item in dados)
                {
                    item.Display += (item.status == 0 ? " (T)" : " (C)");
                    item.ColorDisplay = (item.status == 0 ? ColorStaticModel.VerdePedido : ColorStaticModel.Orcamento);
                    item.bTrazerImagem = false;
                }


                return dados.OrderBy(c => c.Display).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ListItemModel GetItem(int idTabelaPreco)
        {
            var xQuery = $@"select idTabelaPreco Id , 
                    xNome Display,
                    ('Indice de ' || coalesce(pIndiceTabela, '0' || ' %')) Detail,
                    stTabelaPreco status from {TableMobile.TB_TABELAPRECO}
                                                    where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and idTabelaPreco = {idTabelaPreco}";

            var dados = App.Data.Connection.Query<BasicPickerModel>(xQuery);
            if (dados == null)
                return new ListItemModel();
            var item = dados.FirstOrDefault();
            return new ListItemModel
            {
                Display = item.Display + (item.status == 0 ? " (T)" : " (C)"),
                Detail = item.Detail,
                Id = item.Id,
                IdOnline = item.Id
            };
        }


        public static ListItemModel GetFirstItem()
        {
            var xQuery = $@"select idTabelaPreco Id , 
                    xNome Display,
                    ('Indice de ' || coalesce(pIndiceTabela, '0' || ' %')) Detail,
                    stTabelaPreco status from {TableMobile.TB_TABELAPRECO}
                                                    where stAtivo = 1 and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} limit 1";

            var dados = App.Data.Connection.Query<BasicPickerModel>(xQuery);
            if (dados?.Count() == 0)
                return new ListItemModel();


            var item = dados.FirstOrDefault();
            return new ListItemModel
            {
                Display = item.Display + (item.status == 0 ? " (T)" : " (C)"),
                Detail = item.Detail,
                Id = item.Id,
                IdOnline = item.Id
            };
        }

        public static List<ListItemModel> Get(int skip, int take, string xFiltro, bool bTrasCampanha = false)
        {
            var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

            const string xFields = @"idTabelaPreco Id , 
                    xNome Display,
                    ('Indice de ' || coalesce(pIndiceTabela, '0' || ' %')) Detail,
                    stTabelaPreco status,
                    dInicial Date,
                    dFinal Date2 ";
            var xQuery = $"select {xFields} from {TableMobile.TB_TABELAPRECO} where idEmpresa = {idEmpresa} and stAtivo = 1 {(bTrasCampanha ? "" : "and stTabelaPreco = 0")}";

            if (!string.IsNullOrEmpty(xFiltro))
            {
                xFiltro = xFiltro.RemoverAcentos().ToUpper();
                xQuery += $" and (UPPER(Display) like('%{xFiltro}%') or UPPER(coalesce(xDisplaySemCaracter,'')) like('%{xFiltro}%'))";
                //xQuery += $" and UPPER(Display) like('%{xFiltro.ToUpper()}%') ";
            }

            xQuery += $@" order by UPPER(Display)
                                            LIMIT {take} OFFSET {skip}";

            try
            {
                var dados = App.Data.Connection.Query<BasicPickerModel>(xQuery);
                if (dados == null)
                    return new List<ListItemModel>();

                dados = dados.Where(c =>
                (c.Date == null || c.Date <= DateTime.UtcNow)
                && (c.Date == null || c.Date2 >= DateTime.UtcNow)).ToList();

                var retorno = new List<ListItemModel>();
                foreach (var item in dados)
                {
                    retorno.Add(new ListItemModel
                    {
                        Display = item.Display + (item.status == 0 ? " (T)" : " (C)"),
                        Detail = item.Detail,
                        Id = item.Id,
                        IdOnline = item.Id
                    });
                }
                return retorno;
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
                return new List<ListItemModel>();
            }
        }

        /// <summary>
        /// Método para filtro de tabelas preço e campanhas
        /// </summary>
        /// <param name="stTabelaCampanha">0 - Ambos; 1 - Campanha; 2 - Tabela Preço</param>
        /// <param name="stAtivoInativo">0 - Ambos; 1 - Ativo; 2 - Inativo</param>
        /// <param name="idEmpresa"></param>
        /// <param name="idProduto"></param>
        /// <param name="idRepresentada">Informar se necessário filtrar por Representação</param>
        /// <param name="idRepresentante">Informar se necessário filtrar por Representante</param>
        /// <param name="idCliente">Informar se necessário filtrar por Cliente</param>
        /// <returns></returns>
        public static List<TabelaPrecoSimplificada> GetTabelaPrecoSimplificadas(
            byte stTabelaCampanha,
            byte stAtivoInativo,
            int idEmpresa,
            int? idProduto,
            int[] idRepresentada = null,
            int? idRepresentante = null,
            int? idCliente = null)
        {
            try
            {
                var tabelascampanhas = new List<TabelaPrecoModel>();
                var retorno = new List<TabelaPrecoModel>();

                //Retornado primeiramente da base Tabelas/Campanhas de acordo com filtro ativo/inativo
                //Diferenciado linq de tabela e campanha porque comportamento de filtro de ativo/inativo é diferente nas duas situações
                if (stTabelaCampanha == 0 || stTabelaCampanha == 2)
                {

                    var lTabelas = (from tbl in App.Data.Connection.Table<TabelaPrecoModel>()
                                    where tbl.stTabelaPreco == 0 &&
                                    tbl.stAtivo &&
                                    tbl.idEmpresa == idEmpresa
                                    select tbl).ToList();

                    tabelascampanhas.AddRange(lTabelas);
                }

                if (stTabelaCampanha == 0 || stTabelaCampanha == 1)
                {
                    //Além de ser verificado campo stAtivo é necessário verificar também se data 
                    //atual está no intervalo entre dInicial e dFinal para estar ativo, senão inativo
                    var lCampanhas = new List<TabelaPrecoModel>();

                    var dados = (from tbl in App.Data.Connection.Table<TabelaPrecoModel>()
                                 where tbl.stTabelaPreco == 1 && tbl.idEmpresa == idEmpresa
                                 select tbl).ToList();

                    foreach (var i in dados)
                    {
                        if (stAtivoInativo == 0)
                        {
                            lCampanhas.Add(i);
                        }
                        else if (stAtivoInativo == 1)
                        {
                            if (i.stAtivo &&
                                DateTime.Now.ToUniversalTime() >= i.dInicial &&
                                DateTime.Now.ToUniversalTime() <= i.dFinal)
                            {
                                lCampanhas.Add(i);
                            }
                        }
                        else if (stAtivoInativo == 2)
                            if (i.stAtivo == false || (DateTime.Now.ToUniversalTime() < i.dInicial || DateTime.Now.ToUniversalTime() > i.dFinal))
                                lCampanhas.Add(item: i);
                    }

                    tabelascampanhas.AddRange(lCampanhas);
                }

                retorno = tabelascampanhas.Where(i => i.stTabelaPrecoRepresentacao == false
                     && i.stCampanhaCliente == false && i.stCampanhaRepresentante == false).ToList();



                //Filtro de representações, onde será filtrada tabelas preço que sejam para todas representadas stTabelaPrecoRepresentacao == 0
                //ou que representações informadas estejam especificadas na tabela tb_tabelapreco_representacoes
                if (idRepresentada != null)
                {
                    var tabelasAllRepresentadas = (from tbl in tabelascampanhas
                                                   join tblrepr in App.Data.Connection.Table<TabelaPrecoRepresentacoesModel>() on
                                                                    tbl.idTabelaPreco equals tblrepr.idTabelaPreco
                                                   where
                                                       tbl.stTabelaPrecoRepresentacao == true &&
                                                       idRepresentada.Contains(tblrepr.idRepresentada) &&
                                                       tbl.stTabelaPreco == 0
                                                   select tbl
                               ).ToList().Distinct();
                    retorno.AddRange(collection: tabelasAllRepresentadas);
                }

                //Filtro de representantes, regra semelhante a filtro de representações
                if (idRepresentante != null)
                {
                    retorno.AddRange(collection:
                        (from tbl in tabelascampanhas
                         join tblrepr in App.Data.Connection.Table<TabelaPrecoRepresentantesModel>() on tbl.idTabelaPreco equals tblrepr.idTabelaPreco
                         where
                             tbl.stCampanhaRepresentante == true &&
                             tblrepr.idTabelaPrecoRepresentantes == idRepresentante &&
                             tbl.stTabelaPreco == 1
                         select tbl
                               ).ToList().Distinct());
                }


                //Filtro de clientes, regra semelhante a filtro de representações
                if (idCliente != null)
                {
                    retorno.AddRange(collection:
                        (from tbl in tabelascampanhas
                         join tblcli in App.Data.Connection.Table<TabelaPrecoClientesModel>() on tbl.idTabelaPreco equals tblcli.idTabelaPreco
                         where
                             tbl.stCampanhaCliente == true &&
                             tblcli.idClientes == idCliente &&
                             tbl.stTabelaPreco == 1
                         select tbl
                               ).ToList());
                }
                return retorno.Distinct().Select(i => new TabelaPrecoSimplificada
                {
                    idTabelaPreco = i.idTabelaPreco,
                    xTabelaPreco = i.xNome,
                    bCampanha = i.stTabelaPreco == 1,
                    stDefault = i.stDefault,
                    stValor = i.stValor,
                    pDescontoMaximo = i.pDescontoMaximo ?? 100,
                    vVenda = GetValorProdutoTabelaPreco(i.idTabelaPreco, idProduto),
                    pIndice = i.pIndiceTabela ?? 0,
                    bAtivo = i.stTabelaPreco == 1
                        ? ((i.dInicial < DateTime.Now.ToUniversalTime() && i.dFinal > DateTime.Now.ToUniversalTime()) || i.stAtivo)
                        : i.stAtivo
                }).ToList();

            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<TabelaPrecoSimplificada>();
            }
        }

        #region NEW METHODS

        /// <summary>
        /// Método para filtro de tabelas preço e campanhas
        /// </summary>
        /// <param name="stTabelaCampanha">0 - Ambos; 1 - Campanha; 2 - Tabela Preço</param>
        /// <param name="stAtivoInativo">0 - Ambos; 1 - Ativo; 2 - Inativo</param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public static List<TabelaPrecoModel> GetTabelasCampanhas(byte stTabelaCampanha, byte stAtivoInativo, int idEmpresa)
        {
            try
            {
                var tabelascampanhas = new List<TabelaPrecoModel>();

                //Retornado primeiramente da base Tabelas/Campanhas de acordo com filtro ativo/inativo
                //Diferenciado linq de tabela e campanha porque comportamento de filtro de ativo/inativo é diferente nas duas situações
                if (stTabelaCampanha == 0 || stTabelaCampanha == 2)
                {

                    var lTabelas = (from tbl in App.Data.Connection.Table<TabelaPrecoModel>()
                                    where tbl.stTabelaPreco == 0
                                    && tbl.stAtivo
                                    && tbl.idEmpresa == idEmpresa
                                    select tbl
                                                     ).ToList();

                    tabelascampanhas.AddRange(collection: lTabelas);
                }

                if (stTabelaCampanha == 0 || stTabelaCampanha == 1)
                {
                    //Além de ser verificado campo stAtivo é necessário verificar também se data atual está no intervalo entre dInicial e dFinal para estar ativo, senão inativo
                    var lCampanhas = new List<TabelaPrecoModel>();

                    var dados = (from tbl in App.Data.Connection.Table<TabelaPrecoModel>()
                                 where tbl.stTabelaPreco == 1
                                 && tbl.idEmpresa == idEmpresa
                                 select tbl
                                                  ).ToList();

                    foreach (var i in dados)
                    {
                        if (stAtivoInativo == 0)
                        {
                            lCampanhas.Add(item: i);
                        }
                        else if (stAtivoInativo == 1)
                        {
                            if (i.stAtivo && DateTime.UtcNow >= i.dInicial && DateTime.UtcNow <= i.dFinal)
                            {
                                lCampanhas.Add(item: i);
                            }
                        }
                        else if (stAtivoInativo == 2)
                        {
                            if (i.stAtivo == false || (DateTime.UtcNow < i.dInicial || DateTime.UtcNow > i.dFinal))
                            {
                                lCampanhas.Add(item: i);
                            }
                        }
                    }
                    tabelascampanhas.AddRange(collection: lCampanhas);
                }
                return tabelascampanhas;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<TabelaPrecoModel>();
            }
        }


        public static double GetValorProdutoTabelaPreco(TabelaPrecoModel tbl, int idProduto, double vVenda, double pIpiVenda, double pStVenda, bool embuteImpostos = false)
        {
            if (tbl == null)
                return 0;
            double vVendaTabela;

            if (tbl.stDefault == 1)
            {
                vVendaTabela = vVenda;

            }
            else
            {
                if (tbl.stValor != 2) //Tabela Automática
                {
                    var valor = tbl.stValor == 0 ? (vVenda * (1 - (tbl.pIndiceTabela / 100)) ?? 0)
                        : ((vVenda * (1 + (tbl.pIndiceTabela / 100)) ?? 0));

                    vVendaTabela = valor;
                }
                else
                {
                    var item = App.Data.Connection.Table<TabelaPrecoItemModel>().FirstOrDefault(i => i.idTabelaPreco == tbl.idTabelaPreco && i.idProduto == idProduto);
                    vVendaTabela = item != null ? item.vVenda ?? 0 : 0;
                }
            }

            if (embuteImpostos == false)
                return vVendaTabela.ArredondarValorDecimal();

            var _pIpiVenda = pIpiVenda / 100;
            var _pStVenda = pStVenda / 100;
            return (vVendaTabela + (_pIpiVenda * vVendaTabela) + (_pStVenda * vVendaTabela)).ArredondarValorDecimal();
        }

        public static double GetValorProdutoTabelaPrecoSemArredondamento(int idTabelaPreco, int idProduto, double vVenda, double pIpiVenda, double pStVenda, bool embuteImpostos = false)
        {
            double vVendaTabela;

            var item = App.Data.Connection.Table<TabelaPrecoItemModel>().FirstOrDefault(i => i.idTabelaPreco == idTabelaPreco && i.idProduto == idProduto);
            vVendaTabela = item != null ? item.vVenda ?? 0 : 0;

            if (embuteImpostos == false)
                return vVendaTabela;

            var _pIpiVenda = pIpiVenda / 100;
            var _pStVenda = pStVenda / 100;
            return (vVendaTabela + (_pIpiVenda * vVendaTabela) + (_pStVenda * vVendaTabela));
        }




        public static double GetValorProdutoTabelaPreco(TabelaPrecoModel tbl, PedidoVendaItensModel itemProduto, bool embuteImpostos = false)
        {
            return GetValorProdutoTabelaPreco(tbl, itemProduto.idProduto ?? 0, itemProduto.vVenda, itemProduto.pIpiVenda ?? 0,
                itemProduto.pStVenda ?? 0, embuteImpostos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTabelaPreco"></param>
        /// <param name="idProduto"> só será utilizado quando não for null, pois ai o sistema saberá que o produto já esta sincronizado</param>
        /// <returns></returns>
        public static double GetValorProdutoTabelaPreco(int idTabelaPreco, int? idProduto)
        {
            try
            {
                var tbl = App.Data.Connection.Query<TabelaPrecoSimples>(
                $@"select * from {TableMobile.TB_TABELAPRECO} 
                    where idTabelaPreco = {idTabelaPreco}").FirstOrDefault();

                if (tbl.stDefault == 1)
                {
                    var vVenda = App.Data.Connection.Table<ProdutoModel>().Where(i => i.idProduto == idProduto).Select(c => c.vVenda).FirstOrDefault();
                    return vVenda;
                }
                if (tbl.stValor != 2) //Tabela Automática
                {
                    var vVenda = App.Data.Connection.Table<ProdutoModel>().Where(i => i.idProduto == idProduto).Select(c => c.vVenda).FirstOrDefault();
                    return CalcularValorTabelaAutomatica(tbl, vVenda); // tbl.stValor == 0 ? (vVenda * (1 - (tbl.pIndiceTabela / 100)) ?? 0) : ((vVenda * (1 + (tbl.pIndiceTabela / 100)) ?? 0));
                }

                if (idProduto != null) // será null quando o produto não estiver sincronizado
                {
                    var vVendaManual = App.Data.Connection.Table<TabelaPrecoItemModel>().Where(i => i.idTabelaPreco == idTabelaPreco && i.idProduto == idProduto).Select(c => c.vVenda).FirstOrDefault();
                    return (vVendaManual ?? 0);
                }
                return 0;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return 0;
            }
        }

        /// <summary>
        /// pego a tabela de preço após alteração de condições de pagamento
        /// </summary>
        /// <param name="idTabelaPreco"></param>
        /// <param name="idProduto"></param>
        /// <returns></returns>
        public static byte? GetTabelaPrecoParaPedidoVenda(int idTabelaPreco)
        {
            try
            {
                return App.Data.Connection.ExecuteScalar<byte?>(
                $@"select stValor from {TableMobile.TB_TABELAPRECO} 
                    where idTabelaPreco = {idTabelaPreco}");
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return null;
            }
        }

        /// <summary>
        /// buscando o item da tabela de itens
        /// </summary>
        /// <param name="idTabelaPreco"></param>
        /// <param name="idProduto"></param>
        /// <returns></returns>
        public static TabelaPrecoItemModel GetTabelaPrecoItemParaPedidoVenda(int idTabelaPreco, int? idProduto)
        {
            try
            {
                return App.Data.Connection.Query<TabelaPrecoItemModel>(
                $@"select * from {TableMobile.TB_TABELAPRECOITEM} 
                    where idTabelaPreco = {idTabelaPreco} and idProduto = {idProduto}").FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new TabelaPrecoItemModel();
            }
        }

        public static double CalcularValorTabelaAutomatica(TabelaPrecoSimples tbl, double vVenda)
        {
            return tbl.stValor == 0 ? (vVenda * (1 - (tbl.pIndiceTabela / 100))) : ((vVenda * (1 + (tbl.pIndiceTabela / 100))));
        }

        public static PedidoVendaItensModel SetTabelaPrecoByProduto(PedidoVendaItensModel item, int idClienteOffLine, int? idCliente, int _idRepresentante, int? idTabelaPrecoCondicao = null)
        {
            try
            {
                var _idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                IPrecoRepositorio _buscaPreco = new PrecoRepositorio();

                var retorno = _buscaPreco.Buscar(idEmpresa: _idEmpresa,
                    idCliente: idCliente ?? 0, idClienteOffLine: idClienteOffLine,
                    idProduto: item.idProduto ?? 0,
                    idRepresentacao: item.idRepresentada,
                    idRepresentante: _idRepresentante,
                    stBusca: Interfaces.TipoPrecoBusca.tbl);

                List<TabelaPrecoSimplificada> _lTabelas = new List<TabelaPrecoSimplificada>();
                TabelaPrecoSimplificada _tblAux;

                //se a condição de pagamento possuir a regra de tabela de preço, ele vai buscar apenas ela
                if (idTabelaPrecoCondicao.GetValueOrDefault() > 0)
                {
                    retorno = retorno.Where(c => c.idTabelaPreco == idTabelaPrecoCondicao).ToList();
                }

                var _infProd = ProdutoRepository.GetProdutoParaCalculoDePreco(idProdutoOffLine: item.idProdutoOffLine);
                double _vTabela;
                TabelaPrecoItemModel _itemManualAux;

                foreach (var tbl in retorno)
                {
                    if (_lTabelas.Count(t => t.idTabelaPreco == tbl.idTabelaPreco) == 0)
                    {
                        _tblAux = new TabelaPrecoSimplificada
                        {
                            idTabelaPreco = tbl.idTabelaPreco,
                            xTabelaPreco = tbl.xNome,
                            bCampanha = tbl.stTabelaPreco == 1,
                            stDefault = tbl.stDefault,
                            pComissao = tbl.pComissao ?? 0,
                            stValor = tbl.stValor,
                            pDescontoMaximo = tbl.pDescontoMaximo ?? 100,
                            pIndice = tbl.pIndiceTabela ?? 0,
                            bEscalonada = tbl.tbEscalonada != null,
                            lFaixaComissao = tbl.tbEscalonada != null ? tbl.tbEscalonada.lFaixaComissao : null,
                            stCampanhaCliente = tbl.stCampanhaCliente,
                            stCampanhaClienteRamoAtividade = tbl.stCampanhaClienteRamoAtividade,
                            stCampanhaClienteUF = tbl.stCampanhaClienteUF,
                            stCampanhaRepresentante = tbl.stCampanhaRepresentante,
                            stTabelaPrecoRepresentacao = tbl.stTabelaPrecoRepresentacao
                        };

                        if (tbl.stValor == 2)
                        {
                            try
                            {
                                BuscaPrecoCalculoHelper.CalcularTabelaManual(idTabelaPreco: tbl.idTabelaPreco,
                                idProduto: _infProd.idProduto ?? 0, tblAux: _tblAux);
                                _lTabelas.Add(_tblAux);
                            }
                            catch (NullReferenceException ex)
                            {
                                //Produto não incluido nesta lista manual   
                            }
                        }
                        else
                        {
                            BuscaPrecoCalculoHelper.CalcularTabelaAutomatica(tblAux: _tblAux,
                                infProd: _infProd, stValor: tbl.stValor, pIndiceTabela: tbl.pIndiceTabela ?? 0);
                            _lTabelas.Add(_tblAux);
                        }
                    }
                }

                if (item.lTabelaPreco.Count > 0) //Verificar se item já possui campanhas carregadas, consequentemente, já definido um preço de venda para o item
                {
                    var _tblsJaInseridas = item.lTabelaPreco.Select(tb => tb.idTabelaPreco).ToList();
                    var _bTeveTabelaRemovida = false;

                    foreach (var t in item.lTabelaPreco)
                    {
                        if (t.xTabelaPreco.Contains("(T)") || t.xTabelaPreco == "")
                        {
                            var _exist = _lTabelas.Where(c => t.idTabelaPreco == c.idTabelaPreco).FirstOrDefault();

                            if (_exist == null || _exist.idTabelaPreco <= 0)
                            {
                                item.lTabelaPreco.Remove(t);
                                _bTeveTabelaRemovida = true;
                            }

                            // fórmula encontrada para evitar erro de foreach
                            if (item.lTabelaPreco?.Count() <= 0)
                                break;
                        }
                    }

                    item.lTabelaPreco.AddRange(_lTabelas.Where(t => !_tblsJaInseridas.Contains(t.idTabelaPreco)).ToList());

                    if (_bTeveTabelaRemovida)
                    {
                        var _group = _lTabelas.GroupBy(gr => gr.stPrioridade)
                        .OrderByDescending(gr => gr.FirstOrDefault().stPrioridade).FirstOrDefault();

                        item.currentTabelaPreco = _group.OrderBy(gr => gr.vUnitario).FirstOrDefault();
                    }
                }
                else
                {
                    item.lTabelaPreco = _lTabelas;
                    //item.currentTabelaPreco =
                    //    (from tb in _lTabelas
                    //     orderby tb.stCampanhaCliente, tb.stCampanhaClienteRamoAtividade, tb.stCampanhaClienteUF,
                    //     tb.stCampanhaRepresentante, tb.stTabelaPrecoRepresentacao                         
                    //     select tb).LastOrDefault();

                    var _group = _lTabelas.GroupBy(gr => gr.stPrioridade)
                        .OrderByDescending(gr => gr.FirstOrDefault().stPrioridade).FirstOrDefault();


                    if (_group != null)
                        item.currentTabelaPreco = _group.OrderBy(gr => gr.vUnitario).FirstOrDefault();
                }
                item.bTabelasCarregadas = true;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
            return item;
        }




        private static void ValidaCamposTabela(TabelaPrecoSimplificada c, TabelaEscalonadaModel minhaEscalonada, PedidoVendaItensModel item, double pDescontoMaxEscalonada, double pDescontoMinEscalonada)
        {
            c.bEscalonada = true;
            if (minhaEscalonada.lFaixaComissao.Any())
            {
                c.pComissao = minhaEscalonada.lFaixaComissao.Select(o => o.pComissao).Max();
                c.pDescontoMaximo = pDescontoMaxEscalonada;
                item.pComissaoOriginal = c.pComissao;
            }
            //if (c.pDescontoMaximo > pDescontoMaxEscalonada)
            //    c.pDescontoMaximo = pDescontoMaxEscalonada;
            //if (c.pDescontoMaximo < pDescontoMinEscalonada)
            //    c.pDescontoMaximo = pDescontoMinEscalonada;
            c.canEditComissao = false;

        }

        public static List<TabelaPrecoSimplificada> GetAllTables()
        {
            var lTabelaPreco = new List<TabelaPrecoSimplificada>();
            try
            {
                var idRepresentante = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers;
                var retorno = StaticModel.lTabelasPrecoCampanhas.Where(i => i.stCampanhaCliente == false && i.stCampanhaRepresentante == false).ToList();

                var representadas = App.Data.Connection.Table<RepresentadaAspnetUsersModel>()
                     .Where(
                         c =>
                             c.idEmpresa_aspnetUsers ==
                             App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers).
                             ToList();

                foreach (var t in StaticModel.lTabelasPrecoCampanhas.Where(i => i.stTabelaPrecoRepresentacao == true && i.stTabelaPreco == 0))
                {
                    var t1 = t;
                    foreach (var idRepresentada in representadas.Select(c => c.idRepresentada))
                    {
                        if ((from tr in App.Data.Connection.Table<TabelaPrecoRepresentacoesModel>()
                             where tr.idTabelaPreco == t1.idTabelaPreco
                             select tr.idRepresentada).Count(tr => tr == idRepresentada) > 0)
                        {
                            if (retorno.Count(r => r.idTabelaPreco == t.idTabelaPreco) == 0)
                            {
                                retorno.Add(item: t);
                            }
                        }
                    }
                }

                //Filtro de representantes, regra semelhante a filtro de representações
                foreach (var t in StaticModel.lTabelasPrecoCampanhas.Where(i => i.stCampanhaRepresentante == true && i.stCampanhaCliente == false && i.stTabelaPreco <= 1)
                    .ToList())
                {
                    var t1 = t;

                    if ((from c in App.Data.Connection.Table<TabelaPrecoRepresentantesModel>()
                         where c.idTabelaPreco == t1.idTabelaPreco
                         select c.idEmpresa_aspnetUsers).Count(r => r == idRepresentante) > 0)
                    {
                        if (retorno.Count(r => r.idTabelaPreco == t.idTabelaPreco) == 0)
                        {
                            retorno.Add(item: t);
                        }
                    }
                }

                lTabelaPreco = retorno.Distinct().Select(i => new TabelaPrecoSimplificada
                {
                    idTabelaPreco = i.idTabelaPreco,
                    xTabelaPreco = i.xNome,
                    bCampanha = i.stTabelaPreco == 1,
                    stDefault = i.stDefault,
                    pComissao = i.pComissao ?? 0,
                    stValor = i.stValor,
                    pDescontoMaximo = i.pDescontoMaximo ?? 100,
                    pIndice = i.pIndiceTabela ?? 0,
                    stCampanhaCliente = i.stCampanhaCliente,
                    stCampanhaClienteRamoAtividade = i.stCampanhaClienteRamoAtividade,
                    stCampanhaClienteUF = i.stCampanhaClienteUF,
                    stCampanhaRepresentante = i.stCampanhaRepresentante,
                    stTabelaPrecoRepresentacao = i.stTabelaPrecoRepresentacao,
                    bAtivo = i.stTabelaPreco == 1 ? ((i.dInicial < DateTime.Now.ToUniversalTime() && i.dFinal > DateTime.Now.ToUniversalTime()) || i.stAtivo) : i.stAtivo
                }).ToList();

                // OS 35398 - Jessica Barbieri
                //if (lTabelaPreco?.Count() > 0)
                //{
                //    foreach (var tabela in lTabelaPreco)
                //    {
                //        int filtros = 0;

                //        if (tabela.stCampanhaCliente)
                //            filtros++;

                //        if (tabela.stCampanhaClienteRamoAtividade)
                //            filtros++;

                //        if (tabela.stCampanhaClienteUF)
                //            filtros++;

                //        if (tabela.stCampanhaRepresentante)
                //            filtros++;

                //        if (tabela.stTabelaPrecoRepresentacao)
                //            filtros++;

                //        var _contagemDeTabelas = lTabelaPreco.Where(t => t.idTabelaPreco == tabela.idTabelaPreco).Count();

                //        if (filtros > 0 && _contagemDeTabelas != filtros)
                //        {
                //            tabela.bRemoveTabela = true;
                //        }
                //    }

                //    lTabelaPreco.RemoveAll(t => t.bRemoveTabela == true);
                //}

            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
            }
            return lTabelaPreco;
        }
        public static TabelaPrecoModel GetRetistro(int idTabelaPreco)
        {
            return App.Data.Connection.Table<TabelaPrecoModel>().FirstOrDefault(c => c.idTabelaPreco == idTabelaPreco);
        }

        #endregion
        public static List<ModelEscalonadaDisplay> GetDadosEscalonada(int idTabelaEscalonada)
        {
            try
            {
                int icount = 1;
                var retorno = (from c in App.Data.Connection.Table<TabelaEscalonadaFaixaComissaoModel>()
                               where c.idTabelaEscalonada == idTabelaEscalonada
                               orderby c.pInicioFaixa
                               select new ModelEscalonadaDisplay
                               {
                                   pComissao = c.pComissao.ToCurrencyStringSimplesPtBr() + " %",
                                   xDescontoDe = c.pInicioFaixa.ToCurrencyStringSimplesPtBr() + " %",
                                   xDescontoAte = c.pFimFaixa.ToCurrencyStringSimplesPtBr() + " %",
                                   order = icount++
                               }).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<ModelEscalonadaDisplay>();
            }
        }

        public static bool GetAcessoEscalonadaPorEmpresa(int idEmpresa)
        {
            try
            {
                return App.Data.Connection.Table<ConfiguracaoGeralModel>().Where(c => c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa).Select(t => t.bMostraFaixaTabelaEscalonada).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static TabelaEscalonadaModel VerificaSeUtilizaEscalonada()
        {
            TabelaEscalonadaModel retorno = null;
            try
            {

                var ids = new List<int>();
                var xquery =
               $@"select idTabelaEscalonada from {TableMobile.TB_TABELAESCALONADA_REPRESENTANTE}
                        where idRepresentante  = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers} and coalesce(idTabelaEscalonada,0) > 0";

                ids = App.Data.Connection.Query<TabelaEscalonadaRepresentanteModel>(xquery).Select(c => c.idTabelaEscalonada ?? 0).ToList();

                //var ids = new List<int>();
                //ids = (App.Data.Connection.Table<TabelaEscalonadaRepresentanteModel>()
                //           .Where(
                //               c =>
                //                   c.idRepresentante == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers &&
                //                   c.idTabelaEscalonada != null)
                //           .Select(c => c.idTabelaEscalonada ?? 0)).ToList();

                if (ids != null && ids.Any())
                {


                    var tabela = (from c in App.Data.Connection.Table<TabelaEscalonadaModel>()
                                  where ids.Contains(c.idTabelaPrecoEscalonada) && c.stAtivo
                                  select c).FirstOrDefault();

                    if (tabela != null)
                    {
                        xquery =
                            $"SELECT * FROM {TableMobile.TB_TABELAESCALONADA_FAIXACOMISSAO} WHERE idTabelaEscalonada = {tabela.idTabelaPrecoEscalonada}";

                        tabela.lFaixaComissao = App.Data.Connection.Query<TabelaEscalonadaFaixaComissaoModel>(xquery);

                        //tabela.lFaixaComissao = 
                        //    App.Data.Connection.Table<TabelaEscalonadaFaixaComissaoModel>()
                        //        .Where(c => c.idTabelaEscalonada == tabela.idTabelaPrecoEscalonada)
                        //        .ToList();
                        retorno = tabela;
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return retorno;
            }
        }


        /// <summary>
        /// retorna a lista de escalonadas
        /// </summary>
        /// <returns></returns>
        public static List<TabelaEscalonadaModel> VerificaSeUtilizaEscalonadas()
        {
            List<TabelaEscalonadaModel> retorno = new List<TabelaEscalonadaModel>();
            try
            {

                var ids = new List<int>();
                var xquery =
               $@"select idTabelaEscalonada from {TableMobile.TB_TABELAESCALONADA_REPRESENTANTE}
                        where idRepresentante  = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers} and coalesce(idTabelaEscalonada,0) > 0";

                ids = App.Data.Connection.Query<TabelaEscalonadaRepresentanteModel>(xquery).Select(c => c.idTabelaEscalonada ?? 0).ToList();

                //var ids = new List<int>();
                //ids = (App.Data.Connection.Table<TabelaEscalonadaRepresentanteModel>()
                //           .Where(
                //               c =>
                //                   c.idRepresentante == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers &&
                //                   c.idTabelaEscalonada != null)
                //           .Select(c => c.idTabelaEscalonada ?? 0)).ToList();

                if (ids != null && ids.Any())
                {


                    var tabela = (from c in App.Data.Connection.Table<TabelaEscalonadaModel>()
                                  where ids.Contains(c.idTabelaPrecoEscalonada) && c.stAtivo
                                  select c).ToList();

                    if (tabela?.Count() > 0)
                    {
                        foreach (var item in tabela)
                        {
                            xquery =
                          $"SELECT * FROM {TableMobile.TB_TABELAESCALONADA_FAIXACOMISSAO} WHERE idTabelaEscalonada = {item.idTabelaPrecoEscalonada}";

                            item.lFaixaComissao = App.Data.Connection.Query<TabelaEscalonadaFaixaComissaoModel>(xquery);

                            //tabela.lFaixaComissao = 
                            //    App.Data.Connection.Table<TabelaEscalonadaFaixaComissaoModel>()
                            //        .Where(c => c.idTabelaEscalonada == tabela.idTabelaPrecoEscalonada)
                            //        .ToList();
                            retorno.Add(item);
                        }
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return retorno;
            }
        }


        public static List<BasicPickerModel> GetTabelaEscalonadaToDisplay()
        {
            try
            {
                var tabelas = new List<BasicPickerModel>();

                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var ids = new List<int>();
                if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                {
                    ids = (App.Data.Connection.Table<TabelaEscalonadaRepresentanteModel>()
                           .Where(c => c.idTabelaEscalonada != null && c.idEmpresa == idEmpresa)
                           .Select(c => c.idTabelaEscalonada ?? 0)).Distinct().ToList();
                }
                else
                {
                    ids = (App.Data.Connection.Table<TabelaEscalonadaRepresentanteModel>()
                           .Where(
                               c =>
                                c.idEmpresa == idEmpresa
                                && c.idRepresentante == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers
                                && c.idTabelaEscalonada != null)
                           .Select(c => c.idTabelaEscalonada ?? 0)).ToList();
                }

                if (ids.Any())
                {


                    var lTabela = (from c in App.Data.Connection.Table<TabelaEscalonadaModel>()
                                   where ids.Contains(c.idTabelaPrecoEscalonada) && c.stAtivo
                                   select c).ToList();

                    if (lTabela != null)
                    {
                        tabelas.AddRange(lTabela.Select(item => new BasicPickerModel
                        {
                            Id = item.idTabelaPrecoEscalonada,
                            Display = item.xNomeTabela
                        }));
                    }
                }

                return tabelas;

            }
            catch (Exception ex)
            {
                return new List<BasicPickerModel>();
            }
        }
    }


    public partial class TabelaPrecoRepository
    {
        public List<KeyValuePair<TabelaPrecoModel, KeyValuePair<TipoPrioridade, decimal>>> ObterListaTabelasParaVenda(
            int idEmpresa, int idCliente, string xEmailRepresentante, int idProduto)
        {
            var _retorno = new List<KeyValuePair<TabelaPrecoModel, KeyValuePair<TipoPrioridade, decimal>>>();

            //Verificar se representante possui tabela escalonada . . .

            return _retorno;
        }

    }
}
