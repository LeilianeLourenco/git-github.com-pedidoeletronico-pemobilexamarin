using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_CONDICAOPAGAMENTO)]
    public class CondicaoPagamentoModel : ModelComum
    {
        public CondicaoPagamentoModel()
        {
            this.stBaixa = 1;
        }

        [PrimaryKey()]
        public int? idCondicaoPagamento { get; set; }

        private string _xCondicaoPagamento;
        public string xCondicaoPagamento
        {
            get { return _xCondicaoPagamento ?? "PRAZO DE PGTO"; }
            set
            {
                _xCondicaoPagamento = value; base.NotifyPropertyChanged("xCondicaoPagamento");
                xDisplaySemCaracter = value.RemoverAcentos();
            }
        }

        /// <summary>
        /// Valor de desconto/acrescimo na condição de pagamento que influência no valor do produto na hora de fazer o pedido de venda.
        /// </summary>
        public double? vDescCondicao { get; set; }

        public string xDisplaySemCaracter { get; set; }

        public string xFormula { get; set; }

        private int? _nParcelas = 0;
        public int? nParcelas
        {
            get { return _nParcelas; }
            set { _nParcelas = value; }
        }


        public bool? stAtivo { get; set; }


        private byte? _stBaixa = 0;
        public byte? stBaixa
        {
            get { return _stBaixa; }
            set { _stBaixa = value; }
        }

        private int? _idTabelaPreco;


        /// <summary>
        /// Tabela de preço vinculada a condição do pagamento, onde é utilizada para forçar a tabela de preço dos produtos no pedido.
        /// </summary>
        public int? idTabelaPreco
        {
            get { return _idTabelaPreco; }
            set { _idTabelaPreco = value; }
        }


        [Ignore]
        public string xDisplayBaixa => _stBaixa == 0 ? "NÃO" : "SIM";

        public int idEmpresa { get; set; }

        [Ignore]
        public string xDisplay2
        {
            get
            {
                if (!string.IsNullOrEmpty(xFormula))
                {
                    return xFormula;
                }
                else
                {
                    return nParcelas.ToString() + " - parcelas";
                }
            }
        }


        [Ignore]
        public string xAbreviacao => this.xCondicaoPagamento[0].ToString().ToUpper() + this.xCondicaoPagamento[1].ToString().ToUpper();
    }
}
