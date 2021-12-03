using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{

    [Table(TableMobile.TB_PRODUTO_GRADES)]
    public class GradeVariacaoProdutoModel
    {
        [PrimaryKey()]
        public long idGradeProduto { get; set; }  
        public int nSequencia { get; set; } 
        public int idProduto { get; set; }  
        public string xGrade { get; set; }  
    }

    [Table(TableMobile.TB_PRODUTO_GRADES_COMPOSICAO)]
    public class GradeVariacaoProdutoComposicaoModel
    {
        [PrimaryKey()]
        public long idGradeProdutoComposicao { get; set; } 
        public string xGradeComposicao { get; set; }  
        public long idGradeProduto { get; set; }
    }

}
