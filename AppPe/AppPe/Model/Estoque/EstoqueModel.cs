using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Estoque
{
    /// <summary>
    /// Essa modelagem apenas traz o ultimo valor de estoque de cada grade cor e tamanho e também do produto
    /// </summary>
    [Table(TableMobile.TB_MOVIMENTOESTOQUE)]
    public class EstoqueModel : ModelComum
    {
        [PrimaryKey, AutoIncrement]
        public int? idMovimentoEstoqueMobile { get; set; }

        public double vEstoque { get; set; }

        public int idProduto { get; set; }

        public int idEmpresa { get; set; }

        public int? idGradeTamanho { get; set; }

        public int? idGradeCor { get; set; }

        public DateTime dtUltimaAlteracao { get; set; }

        public string xGradeCor { get; set; }

        public string xGradeTamanho { get; set; }  

        public int? idLocalEstoque { get; set; } 
    }
}
