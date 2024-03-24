using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Chart.Horizontal;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;
using Xamarin.HLP.Mobile.AppPE.Model.Home;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Services;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class PedidoRepository
    {
        public static List<PedidoVendaListarModel> GetInfinit(int skip, int take, string xFiltro,
            string idRepresentantePedido = null, int? idClienteOffLine = null, int idPedidoVendaOffLine = 0)
        {
            var retorno = new List<PedidoVendaListarModel>();
            try
            {

                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                var xQuery =
                    $@"select distinct
		                    tb_pedidovenda.idPedidoVenda, 
                            tb_pedidovenda.idPedidoVendaOffLine,
                            tb_pedidovenda.stPedidoVenda,
                            tb_pedidovenda.idPedidoDisplay,
                            tb_pedidovenda.xDisplayIntegracao,
	                        tb_pedidovenda.stLancamento,	   
                            tb_pedidovenda.XdEmissao,                                    
                            tb_pedidovenda.vSeguroPed,                              
                            tb_pedidovenda.xObsInterna,                     
                            tb_pedidovenda.xFormaPagamento as DisplayForma,
                            tb_pedidovenda.vOutrasPed,
                            tb_pedidovenda.vFretePed,
                            tb_pedidovenda.idRepresentadaPdf,
                            tb_pedidovenda.VTotal as VTotal, 
                            tb_pedidovenda.xAssinatura as xAssinatura, 
                            TB_EMPRESA_ASPNETUSERS.xEmail DisplayEmail, 
                            tb_condicaopagamento.xCondicaoPagamento DisplayPrazo, 
                            tb_clientes.xRazaoSocial DisplayCliente,
                            TB_STATUS.idStatus ,
                            TB_STATUS.xSigla ,
                            TB_STATUS.xCor,
                            TB_STATUS.xNome xNomeStatus,
                            coalesce(TB_ESTOQUE_INSUFICIENTE.idPedidoVendaOffLine, 0) iEstoqueInvalido
		                from tb_pedidovenda
				                 left join TB_EMPRESA_ASPNETUSERS on tb_pedidovenda.idRepresentantePedido = TB_EMPRESA_ASPNETUSERS.idEmpresa_aspnetUsers
                                 left join TB_STATUS on tb_pedidovenda.idStatus = TB_STATUS.idStatus
				                 left join tb_condicaopagamento on tb_pedidovenda.idCondicaoPagamento = tb_condicaopagamento.idCondicaoPagamento
				                 left join tb_clientes on tb_pedidovenda.idClientesOffLine = tb_clientes.idClientesOffLine 
                                 left join TB_ESTOQUE_INSUFICIENTE on tb_pedidovenda.idPedidoVendaOffLine = TB_ESTOQUE_INSUFICIENTE.idPedidoVendaOffLine 
                         Where tb_pedidovenda.idEmpresa = {idEmpresa} ";

                if (idPedidoVendaOffLine > 0)
                    xQuery += $" and TB_PEDIDOVENDA.idPedidoVendaOffLine = '{idPedidoVendaOffLine}'";
                else
                {

                    var queryUser = $@"SELECT * FROM {TableMobile.CurrentUserLogin} where bLogado = 1";
                    var currentUser = App.Data.Connection.Query<CurrentUserLoginModel>(queryUser).FirstOrDefault();
                    queryUser = $@"SELECT * FROM {TableMobile.AspNetUsers} where Email = '{currentUser.Email}'";
                    var user = App.Data.Connection.Query<AspNetUsersModel>(queryUser).FirstOrDefault();

                    user.lEpresaAspnetUsersModel = new List<EmpresaAspnetUsersModel>();

                    queryUser = $@"SELECT * FROM {TableMobile.TB_EMPRESA_ASPNETUSERS} where idEmpresa = {idEmpresa}";
                    user.lEpresaAspnetUsersModel.AddRange(App.Data.Connection.Query<EmpresaAspnetUsersModel>(queryUser));
                    queryUser = $@"SELECT * FROM {TableMobile.TB_PERMISSOES_REPRESENTANTES} where idEmpresa = {idEmpresa}";
                    user.lPermissoesRepresentantesModel.AddRange(App.Data.Connection.Query<PermissoesRepresentantesModel>(queryUser));
                    user.objEmpresaAspnetUsersModel.permissoesRepresentantesModel = user.lPermissoesRepresentantesModel.FirstOrDefault(x => x.idEmpresa_aspnetusers == user.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers);

                    int stPermissaoPedidoVenda = 1;

                    if (user.objEmpresaAspnetUsersModel.permissoesRepresentantesModel != null)
                        stPermissaoPedidoVenda = (byte)user.objEmpresaAspnetUsersModel.permissoesRepresentantesModel.stPermissaoPedidoVenda;

                    if (!string.IsNullOrEmpty(idRepresentantePedido) && idRepresentantePedido != "0")
                    {
                        switch (stPermissaoPedidoVenda)
                        {
                            case 0:
                                xQuery += $" and TB_PEDIDOVENDA.idRepresentantePedido = '{0}'";
                                break;
                            case 1:
                                xQuery += $" and TB_PEDIDOVENDA.idRepresentantePedido = '{idRepresentantePedido}'";
                                break;
                            case 2:
                                var xQueryIdEquipe = $@"SELECT * FROM {TableMobile.TB_EQUIPE_REPRESENTANTES} WHERE idEmpresa_aspnetusers = {idRepresentantePedido}";
                                var listaIdEquipe = App.Data.Connection.Query<EquipeRepresentantesModel>(xQueryIdEquipe).ToList();

                                string xQueryEquipes = "SELECT * FROM " + TableMobile.TB_EQUIPE_REPRESENTANTES + " WHERE ";

                                for (int i = 0; i < listaIdEquipe.Count; i++)
                                {
                                    if (i != 0)
                                        xQueryEquipes += " OR ";

                                    xQueryEquipes += $"idEquipe = {listaIdEquipe[i].idEquipe}";

                                }

                                List<EquipeRepresentantesModel> listaEquipe = new List<EquipeRepresentantesModel>();

                                if (listaIdEquipe.Count > 0)
                                    listaEquipe = App.Data.Connection.Query<EquipeRepresentantesModel>(xQueryEquipes).ToList();

                                xQuery += $" and TB_PEDIDOVENDA.idRepresentantePedido = '{idRepresentantePedido}'";

                                foreach (var item in listaEquipe)
                                {
                                    if (item.idEmpresa_aspnetusers != Convert.ToInt32(idRepresentantePedido))
                                        xQuery += $" or TB_PEDIDOVENDA.idRepresentantePedido = '{item.idEmpresa_aspnetusers}'";
                                }
                                break;
                        }
                    }

                    if (idClienteOffLine != null)
                        xQuery += $" and TB_PEDIDOVENDA.idClientesOffLine = '{idClienteOffLine}'";
                }

                if (!string.IsNullOrEmpty(xFiltro))
                {
                    xFiltro = xFiltro.RemoverAcentos().ToUpper();
                    xQuery += $@" and (
                                        tb_pedidovenda.idPedidoDisplay like('%{xFiltro}%') 
                                        or UPPER(coalesce(tb_clientes.xRazaoSocial,'')) like('%{xFiltro}%')
                                        or UPPER(coalesce(TB_EMPRESA_ASPNETUSERS.xEmail,'')) like('%{xFiltro}%')
                                        or coalesce(tb_pedidovenda.xDisplayIntegracao,'') like('%{xFiltro}%')
                                        or coalesce(tb_pedidovenda.XdEmissao,'') like('%{xFiltro}%') 
                                        or UPPER(coalesce(tb_condicaopagamento.xCondicaoPagamento,'')) like('%{xFiltro}%')
                                        or tb_clientes.xCpfCnpj like('%{xFiltro}%'))"; // OS 35425 - Jessica Barbieri
                }

                xQuery += $@" ORDER BY COALESCE(tb_pedidovenda.idPedidoDisplay, 99999999999) DESC
                                            LIMIT {take} OFFSET {skip}";

                retorno = App.Data.Connection.Query<PedidoVendaListarModel>(xQuery);

                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
            return retorno;
        }

        public static List<PedidoVendaModel> GetPedidosAlteradosStatus()
        {
            try
            {
                var xQuery = $@"SELECT * FROM {TableMobile.TB_PEDIDOVENDA} 
                                WHERE idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and bChangedStatus = 1";

                var retorno = App.Data.Connection.Query<PedidoVendaModel>(xQuery);

                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<PedidoVendaModel>();
            }
        }

        public static List<int> GetPedidosToSync(DateTime dtUltimaSync)
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                var xQuery = $@"Select idPedidoVendaOffLine Id from {TableMobile.TB_PEDIDOVENDA} 
                                    where idEmpresa = {idEmpresa} AND idAspnetUsers = '{App.CurrentAspnetUserModel.Id}' 
                                    and idPedidoVenda is null
                                    order by idPedidoVendaOffLine";

                var dados = App.Data.Connection.Query<BasicPickerModel>(xQuery);
                List<int> ids = dados.Select(c => c.Id).ToList();
                return ids;

            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return null;
            }
        }

        public static List<PedidoVendaModel> GetAllPedidosToSync()
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var xQuery = $@"Select xAssinatura, idPedidoVenda, idPedidoVendaOffLine, idEmpresa from {TableMobile.TB_PEDIDOVENDA} 
                                    where idEmpresa = {idEmpresa} AND idAspnetUsers = '{App.CurrentAspnetUserModel.Id}'                                    
                                    order by idPedidoVendaOffLine";

                var _lPedidos = App.Data.Connection.Query<PedidoVendaModel>(xQuery);
                List<PedidoVendaModel> _return = new List<PedidoVendaModel>();
                foreach (var obj in _lPedidos)
                {
                    if (!string.IsNullOrEmpty(obj.xAssinatura))
                    {
                        IFileService _fileService = DependencyService.Get<IFileService>();
                        obj.xAssinaturaBase64 = _fileService.GetImageBase64(obj.xAssinatura);
                        _return.Add(obj);
                    }
                }
                return _return;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return null;
            }
        }

        public static byte? GetDataRelatorio(int idEmpresa)
        {
            var xQuery =
             $"select stDataRelatorios from {TableMobile.TB_EMPRESA} where idEmpresa = {idEmpresa}";


            return App.Data.Connection.ExecuteScalar<byte?>(xQuery);
        }

        public static bool EmpresaUtilizaLocaisEstoque(int idEmpresa)
        {
            List<int> _listaLocais = App.Data.Connection.Table<LocalEstoqueModel>().Where(c => c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa && c.bExcluido == false).Select(t => t.idLocalEstoque).ToList();

            return _listaLocais?.Count() > 0;
        }

        /// <summary>
        /// Mostra as variações dos produtos 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public static bool MostrarProdutoVariacoesVenda(int idEmpresa)
        {
            return App.Data.Connection.Table<ConfiguracaoGeralModel>().Where(c => c.idEmpresa == idEmpresa).Select(t => t.bMostraProdutosVariacoesNaVenda).FirstOrDefault() ?? false;
        }

        /// <summary>
        /// Aqui já busca o local ordenado por prioridade
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="idRepresentante"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public static Dictionary<int, string> BuscarLocaisEstoqueParaListas(int? idCliente, int? idRepresentante, int idEmpresa)
        {
            Dictionary<int, string> locais = App.Data.Connection.Table<LocalEstoqueModel>().Where(c => c.bExcluido == false && c.idEmpresa == idEmpresa).Distinct().ToDictionary(t => t.idLocalEstoque, t => t.xNomeLocal);
            Dictionary<int, string> dicLocais = new Dictionary<int, string>();
            string _xUfCliente = App.Data.Connection.Table<EnderecoModel>().Where(c => c.idClientes == idCliente && c.stPrincipal == true && c.idEmpresa == idEmpresa).Select(t => t.xEstado).FirstOrDefault();
            int _idRamoAtividade = App.Data.Connection.Table<ClientesModel>().Where(c => c.idClientes == idCliente && c.idEmpresa == idEmpresa).Select(t => t.idRamoAtividade).FirstOrDefault();
            List<int> lIdsLocais = locais.Select(t => t.Key).ToList();

            List<int> _listaLocais = App.Data.Connection.Table<LocalEstoqueModel>().Where(c => lIdsLocais.Contains(c.idLocalEstoque)).Select(t => t.idLocalEstoque).Distinct().ToList();
            List<int> _listaLocaisPorCliente = App.Data.Connection.Table<LocalEstoqueClientesModel>().Where(c => c.idClientes == idCliente && lIdsLocais.Contains(c.idLocalEstoque)).Select(t => t.idLocalEstoque).Distinct().ToList();
            List<int> _listaLocaisPorUf = App.Data.Connection.Table<LocalEstoqueUfModel>().Where(c => c.xUf == _xUfCliente && lIdsLocais.Contains(c.idLocalEstoque)).Select(t => t.idLocalEstoque).Distinct().ToList();
            List<int> _listaLocaisPorRamo = App.Data.Connection.Table<LocalEstoqueClienteRamoAtividadesDataModel>().Where(c => c.idRamoAtividade == _idRamoAtividade && lIdsLocais.Contains(c.idLocalEstoque)).Select(t => t.idLocalEstoque).Distinct().ToList();
            List<int> _listaLocaisPorRepresentante = App.Data.Connection.Table<LocalEstoqueRepresentantesModel>().Where(c => c.idEmpresa_aspnetUsers == idRepresentante && lIdsLocais.Contains(c.idLocalEstoque)).Select(t => t.idLocalEstoque).Distinct().ToList();

            int cont = 0;
            if (_listaLocaisPorCliente?.Count() > 0)
            {
                foreach (var item in _listaLocaisPorCliente)
                {
                    if (dicLocais.Where(t => t.Key == item).Count() == 0)
                    {
                        dicLocais.Add(item, locais.Where(t => t.Key == item).Select(t => t.Value).FirstOrDefault());
                        cont++;
                    }
                }
            }

            if (_listaLocaisPorUf?.Count() > 0)
            {
                foreach (var item in _listaLocaisPorUf)
                {
                    if (dicLocais.Where(t => t.Key == item).Count() == 0)
                    {
                        dicLocais.Add(item, locais.Where(t => t.Key == item).Select(t => t.Value).FirstOrDefault());
                        cont++;
                    }
                }
            }

            if (_listaLocaisPorRamo?.Count() > 0)
            {
                foreach (var item in _listaLocaisPorRamo)
                {
                    if (dicLocais.Where(t => t.Key == item).Count() == 0)
                    {
                        dicLocais.Add(item, locais.Where(t => t.Key == item).Select(t => t.Value).FirstOrDefault());
                        cont++;
                    }
                }
            }

            if (_listaLocaisPorRepresentante?.Count() > 0)
            {
                foreach (var item in _listaLocaisPorRepresentante)
                {
                    if (dicLocais.Where(t => t.Key == item).Count() == 0)
                    {
                        dicLocais.Add(item, locais.Where(t => t.Key == item).Select(t => t.Value).FirstOrDefault());
                        cont++;
                    }
                }
            }
            if (cont == 0)
            {
                foreach (var item in _listaLocais)
                {
                    if (dicLocais.Where(t => t.Key == item).Count() == 0)
                        dicLocais.Add(item, locais.Where(t => t.Key == item).Select(t => t.Value).FirstOrDefault());

                }
            }

            dicLocais.Add(0, "Local Padrão");


            return dicLocais;
        }

        public static PedidoVendaItensModel SetLocalEstoque(PedidoVendaItensModel item, int? idCliente, int _idRepresentante)
        {
            try
            {
                //ProdutoRepository.ObterEstoqueProduto(idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa, idProduto: item.idProduto ?? 0, idLocalEstoque: item.idLocalEstoque);
                Dictionary<int, string> dicRetorno = BuscarLocaisEstoqueParaListas(idCliente, _idRepresentante, item.idEmpresa);
                item.lLocaisEstoque = new List<LocalEstoqueSimplificado>();

                foreach (var local in dicRetorno)
                {
                    item.lLocaisEstoque.Add(new LocalEstoqueSimplificado
                    {
                        idLocalEstoque = local.Key,
                        xNomeLocal = local.Value
                    });
                }

                item.currentLocalEstoque = item.lLocaisEstoque.Where(t => t.idLocalEstoque == item.idLocalEstoque).Select(t => new LocalEstoqueSimplificado
                {
                    idEmpresa = t.idEmpresa,
                    idLocalEstoque = t.idLocalEstoque,
                    xNomeLocal = t.xNomeLocal
                }).FirstOrDefault();

                item.bLocaisCarregados = true;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

            return item;
        }

        #region Jornada de trabalho
        public static bool bPermiteJornada(int idEmpresa, int idRepresentante)
        {
            try
            {

                if (idRepresentante == 0)
                    return true;


                var xQuery = $"Select * from {TableMobile.TB_EMPRESA_ASPNETUSERS} where idEmpresa_aspnetUsers = {idRepresentante}";

                var _representante = App.Data.Connection.Query<EmpresaAspnetUsersModel>(xQuery).FirstOrDefault();

                if (_representante.idJornada.GetValueOrDefault() == 0)
                    return true;

                var _jornadaHorarios =
                 App.Data.Connection.Table<JornadaHorariosModel>()
                     .Where(c => c.idJornada == _representante.idJornada).ToList();

                var _dateNow = DateTime.Now.TimeOfDay;
                var _dayOfWeek = (int)DateTime.Now.DayOfWeek;

                //se o count for > 0 é porque existe um horário permitido e ele pode entrar na tela de pedido.
                return _jornadaHorarios.Where(t => t.nDiaSemana == _dayOfWeek && _dateNow >= t.tHorarioInicio && _dateNow <= t.tHorarioFim).ToList().Count() > 0;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return false;

                //Insights.Report(ex, Insights.Severity.Error);
            }

        }

        public static void RemoveHorariosJornadaNova(int idJornada)
        {
            try
            {
                var xQuery =
                    $"DELETE FROM TB_JORNADA_TRABALHO_HORARIOS WHERE idJornada = {idJornada}";

                App.Data.Connection.Execute(xQuery);
            }
            catch (Exception ex)
            {
                ex.TrakException("RemoveHorariosJornadaNova");
            }
        }

        #endregion



        public static PedidoVendaModel GetPedidoVendaModel(int idPedidoVendaOffLine)
        {
            PedidoVendaModel objPedidoVendaModel = null;
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                var xQuery =
                    $"SELECT * FROM {TableMobile.TB_PEDIDOVENDA} WHERE  idPedidoVendaOffLine = {idPedidoVendaOffLine} and idEmpresa = {idEmpresa}";

                objPedidoVendaModel = (App.Data.Connection.Query<PedidoVendaModel>(xQuery)).FirstOrDefault();

                xQuery =
                    $"select distinct idProdutoOffLine, idItemAgrupamento from {TableMobile.TB_PEDIDOVENDAITENS} where idPedidoVendaOffLine = {idPedidoVendaOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";




                // OS 35452 - Jessica Barbieri
                if (objPedidoVendaModel.idClientes == null || objPedidoVendaModel.idClientes == 0)
                    objPedidoVendaModel.idClientes = ClienteRepository.GetIdClienteNuvem(idClientesOffLine: objPedidoVendaModel.idClientesOffLine);



                // BUSCA DE TODOS OS ITENS DO PEDIDO
                var Itens = App.Data.Connection.Query<PedidoVendaItensModel>(xQuery).ToList();

                // OS 35452 - Jessica Barbieri
                if (objPedidoVendaModel.idClientes == null || objPedidoVendaModel.idClientes == 0)
                {
                    objPedidoVendaModel.idClientes = ClienteRepository.GetIdClienteNuvem(idClientesOffLine: objPedidoVendaModel.idClientesOffLine);
                }

                foreach (var itemPedido in Itens)
                {
                    // Busca das informações completas do produto, já com a tabela de preço e grade
                    var itemCompleto = ProdutoRepository.GetProdutoToDisplay(itemPedido.idProdutoOffLine,
                        objPedidoVendaModel.idClientesOffLine, objPedidoVendaModel.idClientes, objPedidoVendaModel.idRepresentantePedido ?? 0);

                    if (itemCompleto.HasGrade)
                        itemCompleto.ItensGrade =
                            new ObservableCollection<PedidoVendaItensModel>(
                                ProdutoRepository.GetGradeItem(itemCompleto));

                    var idProdutoOffLine = itemPedido.idProdutoOffLine;
                    var idItemAgrupamento = itemPedido.idItemAgrupamento;

                    // BUSCA DO(S) PRODUTO(S), EM CASO DE GRADE, PODE VIR MAIS DE UM PRODUTO.
                    var lItens =
                        App.Data.Connection.Table<PedidoVendaItensModel>()
                            .Where(
                                c => c.idProdutoOffLine == idProdutoOffLine &&
                                     c.idItemAgrupamento == idItemAgrupamento &&
                                     c.idPedidoVendaOffLine == objPedidoVendaModel.idPedidoVendaOffLine).ToList();

                    foreach (var item in lItens)
                    {
                        item.lTabelaPreco = itemCompleto.lTabelaPreco;
                        item.currentTabelaPreco =
                            itemCompleto.lTabelaPreco.Count(c => c.idTabelaPreco == item.idTabelaPreco) > 0
                                ? itemCompleto.lTabelaPreco.FirstOrDefault(
                                    c => c.idTabelaPreco == item.idTabelaPreco)
                                : itemCompleto.currentTabelaPreco;

                        item.lLocaisEstoque = itemCompleto.lLocaisEstoque;
                        item.currentLocalEstoque =
                            itemCompleto.lLocaisEstoque.Count(c => c.idLocalEstoque == item.idLocalEstoque) > 0
                                ? itemCompleto.lLocaisEstoque.FirstOrDefault(
                                    c => c.idLocalEstoque == item.idLocalEstoque)
                                : itemCompleto.currentLocalEstoque;

                        if (!itemCompleto.xInfAdicionais.Contains(item.xInfAdicionais))
                            itemCompleto.xInfAdicionais += $"{item.xInfAdicionais} ";

                        if (itemCompleto.HasGrade)
                        {
                            if (itemCompleto.ItensGrade.Count(c =>
                                        c.idProdutoOffLine == item.idProdutoOffLine &&
                                        c.idGradeCor == item.idGradeCor &&
                                        c.idGradeTamanho == item.idGradeTamanho
                                ) > 0)
                            {
                                var itemtoremove = itemCompleto.ItensGrade.FirstOrDefault(c =>
                                        c.idProdutoOffLine == item.idProdutoOffLine &&
                                        c.idGradeCor == item.idGradeCor &&
                                        c.idGradeTamanho == item.idGradeTamanho
                                );
                                //modificação feita 
                                item.xCor = itemtoremove.xCor;
                                SetPropriedadesTexto(item, itemtoremove);
                                itemCompleto.ItensGrade.Remove(itemtoremove);
                                itemCompleto.ItensGrade.Add(item);

                                PedidoVendaCalculos.CalculoValorComissao(item);
                            }
                            itemCompleto.SetDetalheItem();
                        }
                        else
                        {
                            SetPropriedadesTexto(item, itemCompleto);
                            itemCompleto = item;
                            itemCompleto.ItensGrade = new ObservableCollection<PedidoVendaItensModel>() { itemCompleto };
                            itemCompleto.SetDetalheItem();
                            break;
                        }
                    }

                    if (itemCompleto.HasGrade && itemCompleto.ItensGrade.Any())
                    {
                        var itemparam = itemCompleto.ItensGrade.FirstOrDefault(c => c.vQtdItem > 0);
                        if (itemparam != null)
                            itemCompleto.idTabelaPreco = itemparam.idTabelaPreco;

                    }

                    objPedidoVendaModel.lItens.Add(itemCompleto);
                }

                objPedidoVendaModel.EstoqueInvalido = EstoqueRepository.HasEstoqueInvalido(idPedidoVendaOffLine);
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
            }
            return objPedidoVendaModel;
        }

        private static void SetPropriedadesTexto(PedidoVendaItensModel item, PedidoVendaItensModel itemCompleto)
        {
            item.xDescricao = itemCompleto.xDescricao;
            item.cAlternativo = itemCompleto.cAlternativo;
            item.xFileImagePrincipal = itemCompleto.xFileImagePrincipal;
            item.xSigla = itemCompleto.xSigla;
        }

        public static PedidoVendaModel GetPedidoVendaModelToSync(int idPedidoVendaOffLine)
        {
            var objPedidoVendaModel = App.Data.Connection.Table<PedidoVendaModel>()
                    .FirstOrDefault(c => c.idPedidoVendaOffLine == idPedidoVendaOffLine);
            try
            {

                if (objPedidoVendaModel.idClientes == null)
                {
                    objPedidoVendaModel.idClientes = App.Data.Connection.Table<ClientesModel>()
                            .Where(c => c.idClientesOffLine == objPedidoVendaModel.idClientesOffLine)
                            .Select(s => s.idClientes).FirstOrDefault();
                }

                var itens = App.Data.Connection.Table<PedidoVendaItensModel>()
                        .Where(c => c.idPedidoVendaOffLine == idPedidoVendaOffLine).ToList();

                if (itens != null)
                    objPedidoVendaModel.lItens = new ObservableCollection<PedidoVendaItensModel>(itens);

                if (!string.IsNullOrEmpty(objPedidoVendaModel.xAssinatura))
                {
                    IFileService _fileService = DependencyService.Get<IFileService>();
                    objPedidoVendaModel.xAssinaturaBase64 = _fileService.GetImageBase64(objPedidoVendaModel.xAssinatura);
                }

                return objPedidoVendaModel;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
            }
            return objPedidoVendaModel;
        }

        public static void SavePedidoVenda(PedidoVendaModel objPedido)
        {
            try
            {

                if (objPedido.idPedidoDisplay != null && objPedido.idPedidoDisplay <= 0)
                    objPedido.idPedidoDisplay = null;

                double vTotalPedido = 0;
                double vDescontoTotal = 0;
                foreach (var item in objPedido.lItens)
                {
                    if (item.ItensGrade != null && item.ItensGrade.Any())
                    {
                        var _qtdadeTotal = item.ItensGrade.Where(itemgrade => itemgrade.vQtdItem > 0).Sum(itemgrade => itemgrade.vQtdItem);
                        var _itemAux = item.ItensGrade.Where(itemgrade => itemgrade.vQtdItem > 0).FirstOrDefault();
                        double _descontoUnitario = 0;

                        if (_itemAux != null)
                            _descontoUnitario = _itemAux.vDesconto;

                        vDescontoTotal += _qtdadeTotal * _descontoUnitario;
                        vTotalPedido += item.ItensGrade.Where(itemgrade => itemgrade.vQtdItem > 0).Sum(itemgrade => itemgrade.vSubTotal);
                    }
                    else
                    {
                        vDescontoTotal += (item.vDesconto * item.vQtdItem);
                        vTotalPedido += item.vSubTotal;
                    }
                }

                objPedido.stValidaEnvioParaRepresentada = objPedido.stEnviadoRepresentacao;

                objPedido.vTotalProduto = vTotalPedido;

                if (objPedido.idPedidoVenda == null || objPedido.idPedidoVenda == 0)
                    vTotalPedido = vTotalPedido + objPedido.vFretePed + objPedido.vSeguroPed + objPedido.vOutrasPed;

                objPedido.VTotal = vTotalPedido;
                objPedido.vDescontoPed = vDescontoTotal;
                objPedido.idEmpresa = (int)App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                objPedido.idAspnetUsers = App.CurrentAspnetUserModel.Id;

                if (objPedido.idPedidoVenda == null)
                    objPedido.dtUltimaAlteracao = DateTime.UtcNow.ToDateTimeSync();

                //if (objPedido.stLancamento == 0)
                //    objPedido.dtValidadeOrcamento = null;

                if (objPedido.stLancamento == 1)
                    objPedido.dtValidadeOrcamento = null;

                if (objPedido.idPedidoVendaOffLine == null)
                {
                    if (objPedido.idAspnetUsers == null)
                        objPedido.idAspnetUsers = App.CurrentAspnetUserModel.Id;
                    App.Data.Connection.Insert(objPedido);
                }
                else
                    App.Data.Connection.Update(objPedido);

                foreach (var itemRemovido in objPedido.ItensRemovidos)
                {
                    App.Data.Connection.Delete(itemRemovido);
                }


                foreach (var item in objPedido.lItens)
                {

                    var anotacao = ProdutoRepository.GetAnotacaoProduto(item.idProdutoOffLine);

                    if (!string.IsNullOrEmpty(anotacao))
                    {
                        if (!item.xInfAdicionais.ToUpper().Contains(anotacao.ToUpper()))
                        {
                            item.xInfAdicionais += string.IsNullOrEmpty(item.xInfAdicionais)
                                ? anotacao
                                : Environment.NewLine + anotacao;
                        }
                    }

                    if (item.HasGrade || item.ItensGrade != null)
                    {
                        if (item.idItemAgrupamento == null)
                            item.idItemAgrupamento = objPedido.GetNextValidAgrupamento();

                        foreach (var itemGrade in item.ItensGrade)
                        {
                            itemGrade.xInfAdicionais = item.xInfAdicionais;
                            itemGrade.idItemAgrupamento = item.idItemAgrupamento;
                            SaveItemPedido(objPedido, itemGrade);
                        }
                    }
                    else
                    {
                        if (item.idItemAgrupamento == null)
                            item.idItemAgrupamento = objPedido.GetNextValidAgrupamento();
                        SaveItemPedido(objPedido, item);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
            }

        }

        public static double SumFieldItem(int idPedidoVendaOffLine, string xField)
        {
            var xQuery =
                $"select sum({xField}) from {TableMobile.TB_PEDIDOVENDAITENS} where idPedidoVendaOffLine = {idPedidoVendaOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

            var retorno = App.Data.Connection.ExecuteScalar<double>(xQuery);

            return retorno;
        }

        public static double SumDescontoItens(int idPedidoVendaOffLine)
        {
            //var xQuery =
            //    $"select sum(vDesconto*vQtdItem) from {TableMobile.TB_PEDIDOVENDAITENS} where idPedidoVendaOffLine = {idPedidoVendaOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

            var xQuery =
                $"select vDescontoPed from {TableMobile.TB_PEDIDOVENDA} where idPedidoVendaOffLine = {idPedidoVendaOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

            var retorno = App.Data.Connection.ExecuteScalar<double>(xQuery);

            return retorno;
        }

        private static void SaveItemPedido(PedidoVendaModel objPedido, PedidoVendaItensModel item)
        {
            try
            {

                item.idEmpresa = objPedido.idEmpresa;
                item.idPedidoVendaOffLine = objPedido.idPedidoVendaOffLine;
                item.idPedidoVenda = objPedido.idPedidoVenda;
                item.idClientesOffLine = objPedido.idClientesOffLine;
                // item.bPedidoFechadoIncorretamente = objPedido.bPedidoFechadoIncorretamente;

                if (item.idProdutoOffLine == 0)
                    item.idProdutoOffLine = ProdutoRepository.GetIdOffLineByIdOnline(item.idProduto ?? 0);

                if (item.vVenda <= 0)
                {
                    item.vUnitarioVenda = item.vUnitarioVendaComImpostos;
                    item.vVenda = item.vUnitarioVendaComImpostos;
                }


                if (item.idPedidoVendaItemOffLine == null && item.vQtdItem > 0)
                {
                    item.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                    item.SeqToLastSales = Convert.ToInt32((item.dtUltimaAlteracao ?? DateTime.UtcNow).ToString("yyMMdd"));
                    App.Data.Connection.Insert(item);
                }
                else
                {
                    if (item.idPedidoVendaItemOffLine != null)
                    {
                        if (item.vQtdItem > 0)
                            App.Data.Connection.Update(item);
                        else
                            App.Data.Connection.Delete(item);
                    }
                }


                ProdutoRepository.AtualizarEstoqueProduto(idEmpresa: item.idEmpresa,
                    idProduto: item.idProduto, idLocalEstoque: item.idLocalEstoque, vQtdItem: item.vQtdItem);

                //if (item.vQtdEstoque != null && item.vQtdItem > 0)
                //{
                //    EstoqueModel _retornoEstoqueProdutoMobile = new EstoqueModel();

                //    if (item.idGradeCor != null || item.idGradeTamanho != null)
                //    {
                //        _retornoEstoqueProdutoMobile = ProdutoRepository.ObterRegistroEstoqueComGradeProduto(item.idEmpresa, item.idProduto ?? 0, item.idGradeCor, item.idGradeTamanho);
                //    }
                //    else
                //    {
                //        _retornoEstoqueProdutoMobile = ProdutoRepository.ObterRegistroEstoqueProduto(item.idEmpresa, item.idProduto ?? 0);
                //    }

                //    if (_retornoEstoqueProdutoMobile == null)
                //    {
                //        _retornoEstoqueProdutoMobile = new EstoqueModel
                //        {
                //            idProduto = item.idProduto ?? 0,
                //            idEmpresa = item.idEmpresa,
                //            idGradeCor = item.idGradeCor,
                //            idGradeTamanho = item.idGradeTamanho,
                //            vEstoque = 0
                //        };
                //    }

                //    _retornoEstoqueProdutoMobile.vEstoque -= item.vQtdItem;
                //    App.Data.Connection.Update(_retornoEstoqueProdutoMobile);
                //}


            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
            }
        }

        public static bool Delete(int idPedidoVendaOffLine)
        {
            try
            {
                var xQuery = "Delete from {0} where idPedidoVendaOffLine = {1}";

                var xQueryDeletePedido = string.Format(xQuery, TableMobile.TB_PEDIDOVENDA, idPedidoVendaOffLine);
                var xQueryDeleteItens = string.Format(xQuery, TableMobile.TB_PEDIDOVENDAITENS, idPedidoVendaOffLine);

                App.Data.Connection.Execute(xQueryDeletePedido);
                App.Data.Connection.Execute(xQueryDeleteItens);

                return true;
            }
            catch (Exception ex)
            {
                //ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return false;
            }
        }

        public static int GetIdOffLine(int? idPedidoVenda)
        {
            try
            {
                var xquery =
                    $@"select idPedidoVendaOffLine from {TableMobile.TB_PEDIDOVENDA}
                        where idPedidoVenda  = {idPedidoVenda ?? 0} ";
                var result = App.Data.Connection.ExecuteScalar<int?>(xquery);
                return result ?? 0;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return 0;
            }
        }

        public static int GetIdClienteOffLine(int? idPedidoVendaOffLine)
        {
            try
            {
                var xquery =
                    $@"select idClientesOffLine from {TableMobile.TB_PEDIDOVENDA}
                        where idPedidoVendaOffLine  = {idPedidoVendaOffLine ?? 0} ";
                var result = App.Data.Connection.ExecuteScalar<int?>(xquery);
                return result ?? 0;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return 0;
            }
        }


        public static List<int> GetUltimasCompras(int idClientesOffLine)
        {
            try
            {
                var xQuery = $@"select distinct tb_pedidovendaitens.idProdutoOffLine from tb_pedidovenda  
		                            inner join tb_pedidovendaitens 
		                            on tb_pedidovenda.idPedidoVendaOffLine = tb_pedidovendaitens.idPedidoVendaOffLine
		                            where tb_pedidovenda.stLancamento = 1
		                            and tb_pedidovenda.stPedidoVenda = 2
                                    and tb_pedidovenda.idClientesOffLine = {idClientesOffLine}";


                var dados = App.Data.Connection.Query<PedidoVendaItensModel>(xQuery);

                if (dados != null)
                {
                    return dados.OrderByDescending(c => c.dtUltimaAlteracao).Select(c => c.idProdutoOffLine).ToList();
                }
                return new List<int>();

                //return (from item in App.Data.Connection.Table<PedidoVendaItensModel>()
                //        where item.idClientesOffLine == idClientesOffLine
                //        orderby item.idPedidoVendaOffLine descending
                //        select item.idProdutoOffLine).Distinct().ToList();




            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<int>();
            }
        }

        public static bool PedidoPrecisaSerAtualizado(int idPedidoVenda, DateTime dtUltimaAtualizadaoNuvem)
        {
            try
            {
                if (App.Data.Connection.Table<PedidoVendaModel>().Any(c => c.idPedidoVenda == idPedidoVenda))
                {
                    var dateCompare = App.Data.Connection.Table<PedidoVendaModel>()
                        .FirstOrDefault(c => c.idPedidoVenda == idPedidoVenda).dtUltimaAlteracao;
                    return dateCompare < dtUltimaAtualizadaoNuvem;
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }

        [Obsolete]
        public static PedidoVendaModel GerarPedidoByOrcamento(PedidoVendaListarModel currentPedido)
        {
            try
            {

                App.Data.Connection.Update(currentPedido);

                var registro = GetPedidoVendaModel(currentPedido.idPedidoVendaOffLine);
                registro.idOrcamentoOrigem = registro.idPedidoOrigem = null;

                if (registro.stLancamento == 0)
                {
                    registro.idOrcamentoOrigem = registro.idPedidoVenda;
                    registro.idOrcamentoOrigemOffLine = currentPedido.idPedidoVendaOffLine;
                }
                else
                    registro.idPedidoOrigem = registro.idPedidoVenda;


                registro.idPedidoVenda = registro.idPedidoVendaOffLine = null;
                foreach (var item in registro.lItens)
                {
                    item.idPedidoVendaOffLine = item.idPedidoVendaItemOffLine = null;
                }

                registro.idPedidoDisplay = null;
                registro.stLancamento = 1;
                registro.stPedidoVenda = 2;
                registro.dEmissao = DateTime.UtcNow.ToDateTimeSync();

                var status =
                    App.Data.Connection.Table<StatusModel>()
                        .FirstOrDefault(c => c.idStatus == registro.idStatus && c.idEmpresa == registro.idEmpresa);

                var idStatus = 0;

                if (status?.stAparecerStatus == 2)
                    idStatus = status.idStatus;

                if (idStatus == 0)
                {
                    status =
                        App.Data.Connection.Table<StatusModel>()
                            .FirstOrDefault(
                                c =>
                                        c.stVenda == registro.stPedidoVenda && c.idEmpresa == registro.idEmpresa);

                    if (status != null)
                    {
                        idStatus = status.idStatus;
                    }
                }

                registro.idStatus = idStatus;

                SavePedidoVenda(registro);
                return registro;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return new PedidoVendaModel();
            }
        }

        public static async Task<PedidoVendaModel> DuplicarPedido(PedidoVendaListarModel pedido)
        {
            try
            {
                if (pedido.idPedidoVenda == null && pedido.idPedidoVenda == 0)
                {
                    await
                        App.Messages.ShowAsync("Lançamento não se encontra sincronizado, impossível realizar essa ação.");
                    return null;
                }

                var registro = GetPedidoVendaModel(pedido.idPedidoVendaOffLine);

                if (await
                        FinanceiroRepository.ValidaLimiteCredito(registro.idClientesOffLine, null, registro.VTotal,
                            registro.idCondicaoPagamento) == false)
                {
                    return null;
                }

                if (await App.Messages.ShowConfirmAsync("DESEJA REALMENTE DUPLICAR ESSE PEDIDO ?") == false)
                {
                    return null;
                }


                registro.idPedidoVendaOffLine =
                    registro.idPedidoVenda = registro.idOrcamentoOrigem = registro.idPedidoOrigem = null;
                registro.idPedidoOrigem = pedido.idPedidoVenda;

                foreach (var item in registro.lItens)
                {
                    item.idPedidoVenda = item.idPedidoVendaOffLine = item.idPedidoVendaItemOffLine = null;
                }

                registro.idPedidoDisplay = null;
                registro.xDisplayIntegracao = null;
                registro.dEmissao = DateTime.UtcNow.ToDateTimeSync();

                var status =
                    App.Data.Connection.Table<StatusModel>()
                        .FirstOrDefault(c => c.idStatus == registro.idStatus && c.idEmpresa == registro.idEmpresa);

                var idStatus = 0;

                if (status?.stVenda == 1)
                {
                    status = App.Data.Connection.Table<StatusModel>()
                          .FirstOrDefault(c => c.stVenda == 2 && c.idEmpresa == registro.idEmpresa);

                    if (status != null)
                        idStatus = status.idStatus;
                }

                if (status?.stAparecerStatus == 2)
                    idStatus = status.idStatus;

                if (idStatus == 0)
                {
                    status =
                        App.Data.Connection.Table<StatusModel>()
                            .FirstOrDefault(
                                c =>
                                        c.stVenda == registro.stPedidoVenda && c.idEmpresa == registro.idEmpresa);

                    if (status != null)
                    {
                        idStatus = status.idStatus;
                    }
                }

                registro.idStatus = idStatus;

                SavePedidoVenda(registro);
                return registro;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return new PedidoVendaModel();
            }
        }

        public static async Task<PedidoVendaModel> GerarPedidoByOrcamentoNew(PedidoVendaListarModel pedido)
        {
            try
            {
                if (pedido.idPedidoVenda == null && pedido.idPedidoVenda == 0)
                {
                    await
                        App.Messages.ShowAsync("Lançamento não se encontra sincronizado, impossível realizar essa ação.");
                    return null;
                }

                var registro = GetPedidoVendaModel(pedido.idPedidoVendaOffLine);

                if (await
                        FinanceiroRepository.ValidaLimiteCredito(registro.idClientesOffLine, null, registro.VTotal,
                            registro.idCondicaoPagamento) == false)
                {
                    return null;
                }

                await App.Messages.ShowAsync("Sistema irá gerar um pedido baseado nesse orçamento.");

                registro.idOrcamentoOrigem = registro.idPedidoOrigem = null;

                if (registro.stLancamento == 0)
                {
                    registro.idOrcamentoOrigem = pedido.idPedidoVenda;
                    registro.idOrcamentoOrigemOffLine = pedido.idPedidoVendaOffLine;
                }


                registro.idPedidoVenda = registro.idPedidoVendaOffLine = null;
                foreach (var item in registro.lItens)
                {
                    item.idPedidoVendaOffLine = item.idPedidoVendaItemOffLine = null;
                }

                registro.idPedidoDisplay = null;
                registro.stLancamento = 1;
                registro.stPedidoVenda = 2;
                registro.dEmissao = DateTime.UtcNow.ToDateTimeSync();

                var status =
                    App.Data.Connection.Table<StatusModel>()
                        .FirstOrDefault(c => c.idStatus == registro.idStatus && c.idEmpresa == registro.idEmpresa);

                var idStatus = 0;

                if (status?.stAparecerStatus == 2)
                    idStatus = status.idStatus;

                if (idStatus == 0)
                {
                    status =
                        App.Data.Connection.Table<StatusModel>()
                            .FirstOrDefault(
                                c =>
                                        c.stVenda == registro.stPedidoVenda && c.idEmpresa == registro.idEmpresa);

                    if (status != null)
                    {
                        idStatus = status.idStatus;
                    }
                }

                registro.idStatus = idStatus;

                SavePedidoVenda(registro);
                return registro;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return new PedidoVendaModel();
            }
        }

        public static bool ExistemPedidosByOrcamento(int idPedidoVenda)
        {
            try
            {
                var xQuery =
                    $@"SELECT COUNT(*) FROM {TableMobile.TB_PEDIDOVENDA}  WHERE idOrcamentoOrigem = {idPedidoVenda}";

                var count = App.Data.Connection.ExecuteScalar<int>(xQuery);

                return count > 0;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return false;
            }
        }


        public static string GetPedidosFilhos(int? idPedidoVenda)
        {
            var retorno = "";
            if (idPedidoVenda != null)
            {
                foreach (
                    var idPedidoDisplay in
                    App.Data.Connection.Table<PedidoVendaModel>()
                        .Where(c => (c.idPedidoOrigem == idPedidoVenda) || (c.idOrcamentoOrigem == idPedidoVenda))
                        .Select(c => c.idPedidoDisplay))
                {
                    if (idPedidoDisplay != null)
                        retorno += "#" + idPedidoDisplay.ToString().PadLeft(6, '0') + "; ";
                }
            }

            if (retorno != "")
            {
                return "Pedidos vinculados: " + retorno;
            }
            return "";

        }

        public static double GetFaturamento(DateTime date, bool bTodosUsuarios)
        {
            try
            {
                var xDate = date.ToString("dd/MM/yyyy");

                double resultado;
                if (bTodosUsuarios)
                {
                    resultado = App.Data.Connection.Table<PedidoVendaModel>().Where(c => c.stLancamento == 1
                                                                                         && c.XdEmissao == xDate
                                                                                         &&
                                                                                         c.idEmpresa ==
                                                                                         App.CurrentAspnetUserModel
                                                                                             .objEmpresaAspnetUsersModel
                                                                                             .idEmpresa
                                                                                         && c.stPedidoVenda == 2)
                        .Sum(c => c.VTotal);
                }
                else
                {
                    resultado = App.Data.Connection.Table<PedidoVendaModel>().Where(c => c.stLancamento == 1
                                                                                         && c.XdEmissao == xDate
                                                                                         &&
                                                                                         c.idRepresentantePedido ==
                                                                                         App.CurrentAspnetUserModel
                                                                                             .objEmpresaAspnetUsersModel
                                                                                             .idEmpresa_aspnetUsers
                                                                                         &&
                                                                                         c.idEmpresa ==
                                                                                         App.CurrentAspnetUserModel
                                                                                             .objEmpresaAspnetUsersModel
                                                                                             .idEmpresa
                                                                                         && c.stPedidoVenda == 2)
                        .Sum(c => c.VTotal);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("GetFaturamento", ex.Message, true);
                return 0;
            }
        }

        public static double GetFaturamentoPorDataFaturamento(DateTime date, bool bTodosUsuarios)
        {
            try
            {
                var xDate = date.ToString("dd/MM/yyyy");

                double resultado;
                if (bTodosUsuarios)
                {
                    resultado = App.Data.Connection.Table<PedidoVendaModel>().Where(c => c.stLancamento == 1
                                                                                         && c.XdtFaturamento == xDate
                                                                                         &&
                                                                                         c.idEmpresa ==
                                                                                         App.CurrentAspnetUserModel
                                                                                             .objEmpresaAspnetUsersModel
                                                                                             .idEmpresa
                                                                                         && c.stPedidoVenda == 2)
                        .Sum(c => c.VTotal);
                }
                else
                {
                    resultado = App.Data.Connection.Table<PedidoVendaModel>().Where(c => c.stLancamento == 1
                                                                                         && c.XdtFaturamento == xDate
                                                                                         &&
                                                                                         c.idRepresentantePedido ==
                                                                                         App.CurrentAspnetUserModel
                                                                                             .objEmpresaAspnetUsersModel
                                                                                             .idEmpresa_aspnetUsers
                                                                                         &&
                                                                                         c.idEmpresa ==
                                                                                         App.CurrentAspnetUserModel
                                                                                             .objEmpresaAspnetUsersModel
                                                                                             .idEmpresa
                                                                                         && c.stPedidoVenda == 2)
                        .Sum(c => c.VTotal);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("GetFaturamentoPorDataFaturamento", ex.Message, true);
                return 0;
            }
        }


        public static int GetOrcamentosAbertos(bool bTodosUsuarios)
        {
            int resultado = 0;
            try
            {
                if (bTodosUsuarios)
                {
                    resultado = App.Data.Connection.Table<PedidoVendaModel>().Count(c => c.stLancamento == 0
                                                                                         &&
                                                                                         c.idEmpresa ==
                                                                                         App.CurrentAspnetUserModel
                                                                                             .objEmpresaAspnetUsersModel
                                                                                             .idEmpresa
                                                                                         &&
                                                                                         c.dtValidadeOrcamento >
                                                                                         DateTime.UtcNow
                                                                                         && c.stPedidoVenda == 0);
                }
                else
                {
                    resultado = App.Data.Connection.Table<PedidoVendaModel>().Count(c => c.stLancamento == 0
                                                                                         &&
                                                                                         c.idRepresentantePedido ==
                                                                                         App.CurrentAspnetUserModel
                                                                                             .objEmpresaAspnetUsersModel
                                                                                             .idEmpresa_aspnetUsers
                                                                                         &&
                                                                                         c.dtValidadeOrcamento >
                                                                                         DateTime.UtcNow
                                                                                         &&
                                                                                         c.idEmpresa ==
                                                                                         App.CurrentAspnetUserModel
                                                                                             .objEmpresaAspnetUsersModel
                                                                                             .idEmpresa
                                                                                         && c.stPedidoVenda == 0);
                }
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("GetOrcamentosAbertos", ex.Message, true);
            }
            return resultado;
        }


        public static string GetTotalVendasMesAtual()
        {
            var xMes = DateTime.Today.ToString("MM/yyyy");
            var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
            var idRepresentante = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers;

            var xQuery = $@"SELECT SUM(VTotal + vFretePed + vOutrasPed + vSeguroPed) FROM {TableMobile.TB_PEDIDOVENDA} 
                                WHERE 
                                      XdEmissao like '%{xMes}%' and stLancamento = 1 and
                                      stPedidoVenda = 2 and
                                      idEmpresa = {idEmpresa} and
                                      idRepresentantePedido = {idRepresentante}";

            var dfaturado = App.Data.Connection.ExecuteScalar<double>(xQuery);


            return dfaturado.ToCurrencyStringPtBr();
        }



        public static DashMetaMensalModel GetDadosDashMensal(double widthGridBoxDash)
        {
            try
            {
                var retorno = new DashMetaMensalModel();
                var xMes = DateTime.Today.ToString("MM/yyyy");
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var idRepresentante = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers;

                var xQuery = $@"SELECT SUM(VTotal + vFretePed + vOutrasPed + vSeguroPed) FROM {TableMobile.TB_PEDIDOVENDA} 
                                WHERE 
                                      XdEmissao like '%{xMes}%' and stLancamento = 1 and
                                      stPedidoVenda = 2 and
                                      idEmpresa = {idEmpresa} and
                                      idRepresentantePedido = {idRepresentante}";
                var dfaturado = App.Data.Connection.ExecuteScalar<double>(xQuery);


                xQuery = $@"SELECT * FROM {TableMobile.TB_PEDIDOVENDA} 
                                WHERE 
                                      XdEmissao like '%{xMes}%' and stLancamento = 1 and
                                      stPedidoVenda = 2 and
                                      idEmpresa = {idEmpresa} and
                                      idRepresentantePedido = {idRepresentante}";



                //xQuery = $@"SELECT vMetaCorrente FROM {TableMobile.CurrentUserLogin} where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and Email = '{App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xEmail}'";

                var registro =
                    App.Data.Connection.Table<CurrentUserLoginModel>()
                        .FirstOrDefault(
                            c => c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa &&
                                 c.Email.ToUpper() ==
                                 App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xEmail.ToUpper());

                if (registro != null)
                {
                    retorno.dMeta = registro.vMetaCorrente;
                }

                //retorno.dMeta = App.Data.Connection.ExecuteScalar<double>(xQuery);


                //xQuery = $@"SELECT xvMetaCorrente FROM {TableMobile.TB_EMPRESA_ASPNETUSERS} where idEmpresa_aspnetUsers = {idRepresentante}";

                //var teste  = App.Data.Connection.ExecuteScalar<string>(xQuery);

                //retorno.dMeta = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.vMetaCorrente;
                if (App.EnvironmentPE != null)
                    retorno.dMeta = App.EnvironmentPE.vMetaCorrente;

                retorno.xDisplay1 = $"Dados referente ao mês de {DateTime.Today.Month.ToDisplayMonth()}";
                retorno.xMeta = $"Meta = {retorno.dMeta.ToCurrencyStringPtBr()}";
                retorno.dVendido = dfaturado;

                if (retorno.dMeta > 0)
                {
                    retorno.dFaltante = retorno.dMeta - retorno.dVendido;

                    if (retorno.dFaltante < 0)
                        retorno.dFaltante = 0;

                    var pVendido = ((retorno.dVendido * 100) / retorno.dMeta);

                    retorno.pVendido = Convert.ToInt32(pVendido).ToString() + " %";

                    if (pVendido < 100)
                    {
                        var pFaltante = ((retorno.dFaltante * 100) / retorno.dMeta);
                        retorno.pFaltante = Convert.ToInt32(pFaltante).ToString() + " %";

                        retorno.WidthGridBoxDashVendido = (pVendido * widthGridBoxDash) / 100;

                        retorno.WidthGridBoxDashFaltante = (pFaltante * widthGridBoxDash) / 100;
                    }
                    else
                    {
                        retorno.WidthGridBoxDashVendido = widthGridBoxDash;
                        retorno.WidthGridBoxDashFaltante = 0;
                    }

                }
                else
                {
                    retorno.xDisplay2 = "Nenhuma meta configurada no pedidoeletronico.com";
                    retorno.WidthGridBoxDashVendido = 0;
                    retorno.WidthGridBoxDashFaltante = widthGridBoxDash;
                }



                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new DashMetaMensalModel();
            }
        }

        public static DashMetaMensalModel GetDadosDashMensalPorDataFaturamento(double widthGridBoxDash)
        {
            try
            {
                var retorno = new DashMetaMensalModel();
                var xMes = DateTime.Today.ToString("MM/yyyy");
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var idRepresentante = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers;

                var xQuery = $@"SELECT SUM(VTotal + vFretePed + vOutrasPed + vSeguroPed) FROM {TableMobile.TB_PEDIDOVENDA} 
                                WHERE 
                                      XdtFaturamento like '%{xMes}%' and stLancamento = 1 and
                                      stPedidoVenda = 2 and
                                      idEmpresa = {idEmpresa} and
                                      idRepresentantePedido = {idRepresentante}";
                var dfaturado = App.Data.Connection.ExecuteScalar<double>(xQuery);


                xQuery = $@"SELECT * FROM {TableMobile.TB_PEDIDOVENDA} 
                                WHERE 
                                      XdtFaturamento like '%{xMes}%' and stLancamento = 1 and
                                      stPedidoVenda = 2 and
                                      idEmpresa = {idEmpresa} and
                                      idRepresentantePedido = {idRepresentante}";



                //xQuery = $@"SELECT vMetaCorrente FROM {TableMobile.CurrentUserLogin} where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and Email = '{App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xEmail}'";

                var registro =
                    App.Data.Connection.Table<CurrentUserLoginModel>()
                        .FirstOrDefault(
                            c => c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa &&
                                 c.Email.ToUpper() ==
                                 App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xEmail.ToUpper());

                if (registro != null)
                {
                    retorno.dMeta = registro.vMetaCorrente;
                }

                //retorno.dMeta = App.Data.Connection.ExecuteScalar<double>(xQuery);


                //xQuery = $@"SELECT xvMetaCorrente FROM {TableMobile.TB_EMPRESA_ASPNETUSERS} where idEmpresa_aspnetUsers = {idRepresentante}";

                //var teste  = App.Data.Connection.ExecuteScalar<string>(xQuery);

                //retorno.dMeta = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.vMetaCorrente;
                if (App.EnvironmentPE != null)
                    retorno.dMeta = App.EnvironmentPE.vMetaCorrente;

                retorno.xDisplay1 = $"Dados referente ao mês de {DateTime.Today.Month.ToDisplayMonth()}";
                retorno.xMeta = $"Meta = {retorno.dMeta.ToCurrencyStringPtBr()}";
                retorno.dVendido = dfaturado;

                if (retorno.dMeta > 0)
                {
                    retorno.dFaltante = retorno.dMeta - retorno.dVendido;

                    if (retorno.dFaltante < 0)
                        retorno.dFaltante = 0;

                    var pVendido = ((retorno.dVendido * 100) / retorno.dMeta);

                    retorno.pVendido = Convert.ToInt32(pVendido).ToString() + " %";

                    if (pVendido < 100)
                    {
                        var pFaltante = ((retorno.dFaltante * 100) / retorno.dMeta);
                        retorno.pFaltante = Convert.ToInt32(pFaltante).ToString() + " %";

                        retorno.WidthGridBoxDashVendido = (pVendido * widthGridBoxDash) / 100;

                        retorno.WidthGridBoxDashFaltante = (pFaltante * widthGridBoxDash) / 100;
                    }
                    else
                    {
                        retorno.WidthGridBoxDashVendido = widthGridBoxDash;
                        retorno.WidthGridBoxDashFaltante = 0;
                    }

                }
                else
                {
                    retorno.xDisplay2 = "Nenhuma meta configurada no pedidoeletronico.com";
                    retorno.WidthGridBoxDashVendido = 0;
                    retorno.WidthGridBoxDashFaltante = widthGridBoxDash;
                }



                return retorno;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new DashMetaMensalModel();
            }
        }

        public static ChartHorizontalModel GetChartFaturamentoInLine(bool bTodosUsuarios)
        {

            var chart = new ChartHorizontalModel
            {
                Title = "Vendas semestral",
                CorTemplate = ColorStaticModel.AzulChart
            };


            var param = DateTime.Today.AddMonths(-6);
            param = param.AddDays(((param.Day - 1) * -1));
            var iparam = param.ToLocalTime().ToInt();
            Expression<Func<PedidoVendaModel, bool>> query = null;
            var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
            var idRepresentante = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers;

            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador && bTodosUsuarios)
            {
                query = pedido => pedido.idEmissao >= iparam
                                  && pedido.stLancamento == 1
                                  && pedido.stPedidoVenda == 2
                                  && pedido.idEmpresa == idEmpresa;
            }
            else
            {
                query = pedido => pedido.idEmissao >= iparam
                                  && pedido.stLancamento == 1
                                  && pedido.stPedidoVenda == 2
                                  && pedido.idEmpresa == idEmpresa
                                  && pedido.idRepresentantePedido == idRepresentante;
            }

            var dados = (from pedido in App.Data.Connection.Table<PedidoVendaModel>().Where(query)
                         group pedido by pedido.XdEmissao.Substring(3, pedido.XdEmissao.Length - 3)
                into p
                         select new
                         {
                             mes = p.Key,
                             total = p.Sum(c => c.VTotal + c.vFretePed + c.vSeguroPed + c.vOutrasPed),
                             order = p.Key.Split('/')[1] + p.Key.Split('/')[0]
                         }).OrderBy(c => c.order).ToList();



            for (var i = 0; i < 6; i++)
            {
                var mes = DateTime.Today.AddMonths((i * -1)).ToString("MM/yyyy");

                var item = dados.FirstOrDefault(c => c.mes == mes);
                if (item != null)
                {
                    chart.Series.Add(new SerieHorizontalModel
                    {
                        CorLine = ColorStaticModel.AzulChart,
                        Display = item.mes.ToDisplayMonthNew(),
                        Valor = item.total
                    });
                }
                else
                {
                    chart.Series.Add(new SerieHorizontalModel
                    {
                        CorLine = ColorStaticModel.AzulChart,
                        Display = mes.ToDisplayMonthNew(),
                        Valor = 0
                    });
                }
            }
            return chart;
        }

        public static ChartHorizontalModel GetChartFaturamentoPorDataFaturamentoInLine(bool bTodosUsuarios)
        {

            var chart = new ChartHorizontalModel
            {
                Title = "Vendas semestral",
                CorTemplate = ColorStaticModel.AzulChart
            };


            var param = DateTime.Today.AddMonths(-6);
            param = param.AddDays(((param.Day - 1) * -1));
            var iparam = param.ToLocalTime().ToInt();
            Expression<Func<PedidoVendaModel, bool>> query = null;
            var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
            var idRepresentante = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers;

            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador && bTodosUsuarios)
            {
                query = pedido => pedido.idFaturamento >= iparam
                                  && pedido.stLancamento == 1
                                  && pedido.stPedidoVenda == 2
                                  && pedido.idEmpresa == idEmpresa;
            }
            else
            {
                query = pedido => pedido.idFaturamento >= iparam
                                  && pedido.stLancamento == 1
                                  && pedido.stPedidoVenda == 2
                                  && pedido.idEmpresa == idEmpresa
                                  && pedido.idRepresentantePedido == idRepresentante;
            }

            var dados = (from pedido in App.Data.Connection.Table<PedidoVendaModel>().Where(query)
                         group pedido by pedido.XdtFaturamento.Substring(3, pedido.XdtFaturamento.Length - 3)
                into p
                         select new
                         {
                             mes = p.Key,
                             total = p.Sum(c => c.VTotal + c.vFretePed + c.vSeguroPed + c.vOutrasPed),
                             order = p.Key.Split('/')[1] + p.Key.Split('/')[0]
                         }).OrderBy(c => c.order).ToList();



            for (var i = 0; i < 6; i++)
            {
                var mes = DateTime.Today.AddMonths((i * -1)).ToString("MM/yyyy");

                var item = dados.FirstOrDefault(c => c.mes == mes);
                if (item != null)
                {
                    chart.Series.Add(new SerieHorizontalModel
                    {
                        CorLine = ColorStaticModel.AzulChart,
                        Display = item.mes.ToDisplayMonthNew(),
                        Valor = item.total
                    });
                }
                else
                {
                    chart.Series.Add(new SerieHorizontalModel
                    {
                        CorLine = ColorStaticModel.AzulChart,
                        Display = mes.ToDisplayMonthNew(),
                        Valor = 0
                    });
                }
            }

            return chart;
        }

        public static void UpdateAfterUpload(int idPedidoVendaOffLine, int idPedidoVenda)
        {
            var xQuery =
                $"UPDATE {TableMobile.TB_PEDIDOVENDA} SET idOrcamentoOrigem = {idPedidoVenda} WHERE idOrcamentoOrigemOffLine = {idPedidoVendaOffLine}";
            App.Data.Connection.Execute(xQuery);
        }

        public static void UpdateStatus(int idPedidoVendaOffLine, int idStatus, int idStatusAtual, int stPedidoVenda, string xMotivoCancelamento)
        {
            try
            {
                //pedido.dtUltimaAlteracao = dtUltimaAlteracao ?? DateTime.UtcNow.ToDateTimeSync();
                var xQuery = $@"UPDATE {TableMobile.TB_PEDIDOVENDA} SET 
                            idStatus = {idStatus}, 
                            idStatusOld = {idStatusAtual},
                            stPedidoVenda = {stPedidoVenda}, 
                            bChangedStatus = 1, 
                            xMotivoCancelamento = '{xMotivoCancelamento}'
                           WHERE idPedidoVendaOffLine = {idPedidoVendaOffLine}";
                App.Data.Connection.Execute(xQuery);
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public static void VoltarParaStatusAnterior(int idPedidoVendaOffLine, StatusModel statusOld)
        {
            try
            {
                if (statusOld == null) return;

                var xQuery = $@"UPDATE {TableMobile.TB_PEDIDOVENDA} SET 
                            idStatus = {statusOld.idStatus},                             
                            stPedidoVenda = {statusOld.stVenda}, 
                            bChangedStatus = 0                            
                           WHERE idPedidoVendaOffLine = {idPedidoVendaOffLine}";
                App.Data.Connection.Execute(xQuery);
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public static void UpdateStatusParaNaoAlterado(int idPedidoVenda)
        {
            try
            {
                //pedido.dtUltimaAlteracao = dtUltimaAlteracao ?? DateTime.UtcNow.ToDateTimeSync();
                var xQuery = $@"UPDATE {TableMobile.TB_PEDIDOVENDA} SET                             
                            bChangedStatus = 0
                           WHERE idPedidoVenda = {idPedidoVenda}";
                App.Data.Connection.Execute(xQuery);
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        /// <summary>
        /// Aplicação de rateio de desconto no pedido  de venda
        /// </summary>
        /// <param name="_itens"></param>
        /// <param name="vlrDesconto"></param>
        /// <returns></returns>
        public static ObservableCollection<PedidoVendaItensModel> AplicarRateio(ObservableCollection<PedidoVendaItensModel> _itens, decimal vlrDesconto)
        {
            var _vTotalItens = (decimal)_itens.Sum(i => i.vVenda * i.vQtdItem);
            _vTotalItens = Math.Round(_vTotalItens, 2, MidpointRounding.AwayFromZero);
            decimal _pProporcaoItem = 0;
            decimal _vResto = 0;
            decimal _vDescontoTotalAplicado = 0;
            decimal vSubTotal = 0;
            decimal _vDesconto = 0;
            if (_vTotalItens > 0)
            {
                //_pProporcaoItem = Math.Round((vlrDesconto / _vTotalItens), decimals: 2, mode: MidpointRounding.AwayFromZero);
                _pProporcaoItem = Math.Round((vlrDesconto / _vTotalItens), decimals: 6, mode: MidpointRounding.AwayFromZero); //erick insuess 505
            }

            foreach (var item in _itens)
            {
                if (_pProporcaoItem > 0 && item.vQtdItem > 0)
                {
                    _vDesconto = Math.Round((_pProporcaoItem * (decimal)item.vVenda), decimals: 6, mode: MidpointRounding.AwayFromZero);
                    _vDescontoTotalAplicado += Math.Round(d: _vDesconto * (decimal)item.vQtdItem, decimals: 2, mode: MidpointRounding.AwayFromZero);
                }

                if (item.Equals(_itens.LastOrDefault()))
                {
                    if (_vDescontoTotalAplicado != vlrDesconto)
                    {
                        // _vResto = Math.Round(d: _vDescontoTotalAplicado - vlrDesconto, decimals: 2, mode: MidpointRounding.AwayFromZero);
                        _vResto = Math.Round(d: _vDescontoTotalAplicado - vlrDesconto, decimals: 6, mode: MidpointRounding.AwayFromZero);   //erick insuess 505
                    }
                }

                vSubTotal += item.SetValorDesconto(vDesconto: (double)_vDesconto, vResto: (double)_vResto);
                if (item.Equals(_itens.LastOrDefault()))
                {
                    if (vSubTotal != (_vTotalItens - vlrDesconto))
                    {
                        var _vsubrestoaux = (_vTotalItens - vlrDesconto) - vSubTotal;
                        item.vSubTotal = (double)Math.Round(d: (decimal)item.vSubTotal + _vsubrestoaux, decimals: 2, mode: MidpointRounding.AwayFromZero);

                        item.SetValorTotal(vSubTotalAux: item.vSubTotal);
                    }
                }


            }

            return _itens;
        }

        public decimal BuscaDescontoMaximo(int idEmpresa, int idTabelaPreco, int idProduto)
        {
            var _tbl = App.Data.Connection.Table<TabelaPrecoModel>()
                        .Where(c => c.idTabelaPreco == idTabelaPreco &&
                                    c.idEmpresa == idEmpresa).FirstOrDefault();

            if (_tbl == null)
            {
                return 0;
            }
            else
            {
                if (_tbl.stValor != 2)
                {
                    return (decimal?)_tbl.pDescontoMaximo ?? decimal.Zero;
                }
                else
                {
                    var _tblItem = App.Data.Connection.Table<TabelaPrecoItemModel>().FirstOrDefault(i => i.idEmpresa == idEmpresa && i.idTabelaPreco == idTabelaPreco
                    && i.idProduto == idProduto);

                    if (_tblItem == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return _tblItem.pDescontoMaximo ?? decimal.Zero;
                    }
                }
            }
        }

        public static bool SalvarCaminhoImagemAssinaturaPedidoVenda(string xCaminhoImgAssinatura, int idPedidoVendaOffLine)
        {
            //App.Data.Connection.Query<PedidoVendaListarModel>(xQuery);
            var _pedidovenda = App.Data.Connection.Table<PedidoVendaModel>()
              .Where(c => c.idPedidoVendaOffLine == idPedidoVendaOffLine &&
                          c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa).FirstOrDefault();

            _pedidovenda.xAssinatura = xCaminhoImgAssinatura;
            _pedidovenda.dtUltimaAlteracao = DateTime.Now;

            if (_pedidovenda.idPedidoVendaOffLine > 0)
                App.Data.Connection.Update(_pedidovenda);
            return true;
        }
        public static string BuscarAssAtualizada(int idPedidoVendaOffLine)
        {
            var xQuery = $"select xAssinatura from {TableMobile.TB_PEDIDOVENDA} where idPedidoVendaOffLine = {idPedidoVendaOffLine} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var retorno = App.Data.Connection.ExecuteScalar<string>(xQuery);
            return retorno;

        }

    }
}

