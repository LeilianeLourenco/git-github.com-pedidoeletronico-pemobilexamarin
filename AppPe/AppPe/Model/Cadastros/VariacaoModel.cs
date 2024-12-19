using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_GRADES)]
    public class GradesModel
    {
        [PrimaryKey()]
        public int idGrade { get; set; }
        public string xGrade { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        public string idAspnetUserEdicao { get; set; }
        public int idEmpresa { get; set; }
        public bool bExcluido { get; set; }        
    }

    [Table(TableMobile.TB_GRADES_COMPOSICAO)]
    public class GradesComposicaoModel
    {
        [PrimaryKey()]
        public int idGradeComposicao { get; set; }
        public string xNomeGrade { get; set; }
        public int idGrade { get; set; }
        public bool bExcluido { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        public string xImgPath { get; set; }
        public string xCor { get; set; }
    }

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
        public long idGradeProduto { get; set; }
        public int idGradeComposicao { get; set; } 
        public string xGradeComposicao { get; set; }  
    }

    public class VariacaoModel
    {       
        public string NomeVariacao { get; set; }
        public int idProduto { get; set; }
        public List<int> idProdutoLista { get; set; }
    }

}
