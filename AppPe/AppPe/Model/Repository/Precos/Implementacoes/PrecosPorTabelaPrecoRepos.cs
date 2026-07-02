using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Precos.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Representante.Implementacoes;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Representante.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model.TabelaPreco;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Precos.Implementacoes
{
    public class PrecosPorTabelaPrecoRepos : ITabelaPrecoManItensRepos
    {
        readonly bool _addNameTabela;
        readonly int? _idProduto;
        public PrecosPorTabelaPrecoRepos(bool addNameTabela, int? idProduto)
        {
            this._addNameTabela = addNameTabela;
            this._idProduto = idProduto;
        }

        public List<DisplayListaModel> ObterProdutosTabela(int idTabelaPreco)
        {
            var _objTabelaPrecoModel = TabelaPrecoRepository.GetRetistro(idTabelaPreco);

            if(_objTabelaPrecoModel.stValor != 2)
            {
                var xWhere =
                $"stAtivo = 1 and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
                if (!App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                {
                    IRepresentanteRepresentadaAcessoRepository _rpreRpraRepos =
                        new RepresentanteRepresentadaAcessoRepositorio();

                    var lRepresentadasAspnetUsers =
                        _rpreRpraRepos.RetornarRepresentadasPermitidas();

                        
                    var inIdRepresentada =
                        lRepresentadasAspnetUsers.Select(c => c.ToString())
                            .ToList()
                            .Aggregate("", (current, item) => current + ((current == "" ? "" : " , ") + item));
                    xWhere += $" and idRepresentada in ({inIdRepresentada}) ";                   
                }
                if (this._idProduto != null && this._idProduto > 0)
                {
                    xWhere += $" and idProduto = {this._idProduto}";
                }
                var xQuery =
                       $@"SELECT 
                                idProduto, 
                                idProdutoOffLine, pIpiVenda, pStVenda, 
                                (coalesce(tb_produto.cAlternativo,'') || ' - ' || xNome) xDisplay,
                                vVenda from {TableMobile.TB_PRODUTO} where {xWhere} order by xNome";                

                var lreturn = App.Data.Connection.Query<DisplayListaModel>(xQuery);

                foreach (var item in lreturn)
                {
                    item.vVenda = TabelaPrecoRepository.GetValorProdutoTabelaPreco(tbl: _objTabelaPrecoModel,
                        idProduto: item.idProduto ?? 0,
                        vVenda: item.vVenda,
                        pIpiVenda: item.pIpiVenda ?? 0,
                        pStVenda: item.pStVenda ?? 0, embuteImpostos: true);

                    if (_addNameTabela)
                    {
                        item.xDisplay =
                            $@"{(_objTabelaPrecoModel.stTabelaPreco == 0 ? "(T) " : "(C) ")}
{_objTabelaPrecoModel.xNome} - {item.vVenda.ToCurrencyStringPtBr()}";
                    }
                    else
                    {
                        item.xDisplay = $"{item.vVenda.ToCurrencyStringPtBr()} - {item.xDisplay}";
                    }
                }
                return lreturn;
            }
            else
            {
                var xWhere =
                $"stAtivo = 1 and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
                if (!App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                {
                    IRepresentanteRepresentadaAcessoRepository _rpreRpraRepos =
                        new RepresentanteRepresentadaAcessoRepositorio();

                    var lRepresentadasAspnetUsers =
                        _rpreRpraRepos.RetornarRepresentadasPermitidas();
                    var inIdRepresentada =
                        lRepresentadasAspnetUsers.Select(c => c.ToString())
                            .ToList()
                            .Aggregate("", (current, item) => current + ((current == "" ? "" : " , ") + item));
                    xWhere += $" and idRepresentada in ({inIdRepresentada}) ";                    
                }

                if (this._idProduto != null && this._idProduto > 0)
                {
                    xWhere += $" and idProduto = {this._idProduto}";
                }
                var xQuery =
                       $@"SELECT 
                                idProduto, 
                                idProdutoOffLine, pIpiVenda, pStVenda, 
                                (coalesce(tb_produto.cAlternativo,'') || ' - ' || xNome) xDisplay,
                                vVenda from {TableMobile.TB_PRODUTO} where {xWhere} order by xNome";
                

                var _lIdProdutos = App.Data.Connection.Query<DisplayListaModel>(xQuery);
                
                // Base manual = it.vVenda (igual ao backlog), somando os impostos do produto por cima.
                // NÃO usar it.vVendaComImpostos: fica desatualizado quando o item tem 100% de desconto
                // (vVenda = 0 mas vVendaComImpostos mantém o valor antigo), exibindo o preço cheio.
                var _xQueryTblItens = $@"SELECT
                    (it.vVenda + (it.vVenda * coalesce(p.pIpiVenda,0)/100.0) + (it.vVenda * coalesce(p.pStVenda,0)/100.0)) as vVenda,
                    (coalesce(p.cAlternativo,'') || ' - ' || p.xNome) as xDisplay
                    from {TableMobile.TB_TABELAPRECOITEM} as it
                    join {TableMobile.TB_PRODUTO} as p on it.idProduto = p.idProduto
                    where it.idProduto in ({
                    _lIdProdutos.Select(i => i.idProduto.ToString())
                    .Aggregate("", (current, item) => current + ((current == "" ? "" : " , ") + item))
                    }) and idTabelaPreco = {idTabelaPreco}";

                var _itensTabela = App.Data.Connection.Query<DisplayListaModel>(_xQueryTblItens);

                foreach (var item in _itensTabela)
                {

                    if (_addNameTabela)
                    {
                        item.xDisplay =
                            $@"{(_objTabelaPrecoModel.stTabelaPreco == 0 ? "(T) " : "(C) ")}
{_objTabelaPrecoModel.xNome} - {item.vVenda.ToCurrencyStringPtBr()}";
                    }
                    else
                    {
                        item.xDisplay = $"{item.vVenda.ToCurrencyStringPtBr()} - {item.xDisplay}";
                    }

                }

                return _itensTabela;
                    
            }
            

        }
    }
}
