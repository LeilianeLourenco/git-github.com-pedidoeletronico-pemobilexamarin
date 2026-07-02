using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_TABELAPRECO)]
    public class TabelaPrecoModel
    {
        [PrimaryKey()]
        public int idTabelaPreco { get; set; }
        private string _xNome;
        [NotNull]
        public string xNome
        {
            get { return _xNome; }
            set
            {
                _xNome = value;
                xDisplaySemCaracter = value.RemoverAcentos();
            }
        }

        public string xDisplaySemCaracter { get; set; }

        private double? _pIndiceTabela;
        public double? pIndiceTabela
        {
            get
            {
                if (_pIndiceTabela == null)
                    return 0;
                else if (_pIndiceTabela == 0)
                    return 0;
                else
                    return _pIndiceTabela;

            }
            set { _pIndiceTabela = value; }
        }
        public int? idEmpresa { get; set; }
        public byte stDefault { get; set; }
        private byte _stTabelaPreco;
        /// <summary>
        /// 0 - TABELA DE PREÇO
        /// 1 - CAMPANHA
        /// </summary>
        public byte stTabelaPreco
        {
            get { return _stTabelaPreco; }
            set
            {
                _stTabelaPreco = value;
                if (value == 0) // TABELA DE PREÇO
                {
                    iconCampanha = false;
                    iconTabelaPreco = false;
                }
                else
                {
                    iconCampanha = true;

                    iconTabelaPreco = false;
                }
            }
        }
        public double? pDescontoMaximo { get; set; }

        [NotNull]
        public bool stAtivo { get; set; }

        public bool stCampanhaRepresentante { get; set; }

        public bool stCampanhaCliente { get; set; }

        /// <summary>
        /// 0-DESCONTO
        /// 1-ACRESCIMO
        /// 2-MANUAL
        /// </summary>
        public byte stValor { get; set; }
        public DateTime? dInicial { get; set; }
        public DateTime? dFinal { get; set; }
        public double vVenda { get; set; }
        public double vLimiteMinimoVenda { get; set; }
        public bool stTabelaPrecoRepresentacao { get; set; }
        public string idAspnetUsersInclusao { get; set; }
        public double? pComissao { get; set; }
        public string xTagCliente { get; set; }
        public string xUFCliente { get; set; }

        public bool stCampanhaClienteRamoAtividade { get; set; }
        public bool stCampanhaClienteUF { get; set; }


        //propriedades que não fazem parte da tabela
        [Ignore]
        [IgnoreDataMember]
        public bool iconCampanha { get; set; }
        [Ignore]
        [IgnoreDataMember]
        public bool iconTabelaPreco { get; set; }

        [IgnoreDataMember]
        [Ignore]
        public double vTabelaCampanha { get; set; }

        [IgnoreDataMember]
        [Ignore]
        public bool bRemoveTabela { get; set; }

        [IgnoreDataMember]
        [Ignore]
        public TabelaEscalonadaModel tbEscalonada { get; set; }

        [Ignore]
        [IgnoreDataMember]
        public List<TabelaPrecoItemModel> lTabelaPrecoItens { get; set; }

        [Ignore]
        [IgnoreDataMember]
        public List<ClientesModel> lClientes { get; set; }
    }

    public class TabelaPrecoSimplificada
    {
        [IgnoreDataMember]
        [Ignore]
        public bool bRemoveTabela { get; set; }

        public double pDescontoMaximo { get; set; }
        public int idTabelaPreco { get; set; }
        //public string xTabelaPreco { get; set; }

        private string _xTabelaPreco;

        public string xTabelaPreco
        {
            get { return (_xTabelaPreco ?? "") + (bCampanha ? " (C)" : " (T)"); }
            set { _xTabelaPreco = value; }
        }

        public double pComissao { get; set; }
        public bool bCampanha { get; set; }
        public byte stDefault { get; set; }
        public double pIndice { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        /// <summary>
        /// 0-DESCONTO
        /// 1-ACRESCIMO
        /// 2-MANUAL
        /// </summary>
        public byte stValor { get; set; }
        public bool bAtivo { get; set; }
        /// <summary>
        /// Valor de venda com os impostos
        /// </summary>
        public double vVenda { get; set; }
        /// <summary>
        /// Valor de venda sem os impostos
        /// </summary>
        public double vUnitario { get; set; }
        public double vBaseComissao { get; set; }
        public bool canEditComissao { get; set; } = true;

        public bool bEscalonada { get; set; } = false;
        public bool bBloquearValorProdutoApp { get; set; }
        public bool BloquearValor
        {
            get
            {
                if(!bEscalonada && !bBloquearValorProdutoApp)
                    return false;
                else
                    return true;
            }            
        }

        [IgnoreDataMember]
        public double vVendaSemArredondamento { get; set; }

        /// <summary>
        /// Indica que o vVenda foi efetivamente calculado a partir da tabela (manual ou automática).
        /// Usado para distinguir um preço legítimo de 0,00 (ex.: 100% de desconto) de uma tabela
        /// apenas referenciada/não calculada, evitando que o 0,00 válido seja trocado pelo preço base.
        /// </summary>
        [IgnoreDataMember]
        public bool bValorCalculado { get; set; }

        public List<TabelaEscalonadaFaixaComissaoModel> lFaixaComissao { get; set; }

        public double pIpiVenda { get; set; }
        public double pStVenda { get; set; }
        public bool stCampanhaRepresentante { get; set; }
        public bool stCampanhaCliente { get; set; }
        public bool stTabelaPrecoRepresentacao { get; set; }
        public bool stCampanhaClienteRamoAtividade { get; set; }
        public bool stCampanhaClienteUF { get; set; }
        public double SelectComissaoEscalonada(double pDesconto)
        {
            if (lFaixaComissao != null && lFaixaComissao.Any())
            {
                double? dComissao = null;
                foreach (var faixa in lFaixaComissao)
                {
                    if (pDesconto >= faixa.pInicioFaixa && pDesconto <= faixa.pFimFaixa)
                    {
                        dComissao = faixa.pComissao;
                        break;
                    }
                }

                if (dComissao == null)
                {
                    dComissao = lFaixaComissao.Select(c => c.pComissao).Min();
                }

                return dComissao ?? 0;
            }
            return 0;
        }


        public TipoPrioridade stPrioridade
        {

            get
            {

                if (this.stCampanhaCliente == false && this.stCampanhaClienteRamoAtividade == false 
                    && this.stCampanhaClienteUF == false
                    && this.stCampanhaRepresentante == false && this.stTabelaPrecoRepresentacao == false)

                {
                    return TipoPrioridade.geral;
                }
                else
                {
                    return TipoPrioridade.especifica;
                }

            }

        }
    }


    /// <summary>
    /// Utilizada para pegar informações simple das tabelas de preços
    /// </summary>
    public class TabelaPrecoSimples
    {
        public int idTabelaPreco { get; set; }
        public string xNome { get; set; }
        public double pIndiceTabela { get; set; }
        public double pDescontoMaximo { get; set; }
        public byte stTabelaPreco { get; set; }
        public byte stValor { get; set; }
        public DateTime? dInicial { get; set; }
        public DateTime? dFinal { get; set; }
        public byte stDefault { get; set; }
        public int idEmpresa { get; set; }

        public bool stCampanhaRepresentante { get; set; }
        public bool stCampanhaCliente { get; set; }
        public bool stTabelaPrecoRepresentacao { get; set; }
        public bool stCampanhaClienteRamoAtividade { get; set; }
        public bool stCampanhaClienteUF { get; set; }

    }

    [Table(TableMobile.TB_TABELAPRECO_REPRESENTACOES)]
    public class TabelaPrecoRepresentacoesModel
    {
        [PrimaryKey()]
        public int? idTabelaPrecoRepresentacoes { get; set; }
        public int idTabelaPreco { get; set; }
        public int idRepresentada { get; set; }
        public int idEmpresa { get; set; }
    }

    [Table(TableMobile.TB_TABELA_PRECO_CLIENTES)]
    public class TabelaPrecoClientesModel
    {
        [PrimaryKey()]
        public int? idTabelaPrecoClientes { get; set; }
        public int idClientes { get; set; }
        public int idTabelaPreco { get; set; }
        public int idEmpresa { get; set; }
    }

    [Table(TableMobile.TB_TABELA_PRECO_REPRESENTANTES)]
    public class TabelaPrecoRepresentantesModel
    {
        [PrimaryKey()]
        public int? idTabelaPrecoRepresentantes { get; set; }
        public int idTabelaPreco { get; set; }
        public int idEmpresa_aspnetUsers { get; set; }
        public int idEmpresa { get; set; }
    }
}
