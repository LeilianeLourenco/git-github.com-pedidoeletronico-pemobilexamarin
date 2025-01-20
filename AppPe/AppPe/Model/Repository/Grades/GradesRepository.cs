using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using static Xamarin.HLP.Mobile.AppPE.Model.Cadastros.ClientesModel;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Grades
{
    public class GradesRepository
    {
        public static List<int> GetGrades(object idEmpresa)
        {
            string xQuery = $@"SELECT idGrade FROM TB_GRADES WHERE idEmpresa = {idEmpresa}";
            return App.Data.Connection.Query<GradesModel>(xQuery).Select(x => x.idGrade).ToList();
        }

        public static IEnumerable<long> GetProdutosGrade(string idsProduto)
        {
            string xQuery = $"SELECT idGradeProduto FROM tb_produto_grades WHERE idProduto IN ({idsProduto})";
            return App.Data.Connection.Query<GradeVariacaoProdutoModel>(xQuery).Select(x => x.idGradeProduto).ToList();
        }
    }
}
