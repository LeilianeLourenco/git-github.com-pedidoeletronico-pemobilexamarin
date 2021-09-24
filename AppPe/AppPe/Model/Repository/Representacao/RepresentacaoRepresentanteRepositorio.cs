using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.Representacao;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Representacao
{
    public class RepresentacaoRepresentanteRepositorio : IRepresentacaoRepresentanteRepositorio
    {
        public List<int> ObterRepresentadas(int idRepresentante)
        {
            var _usu = App.Data.Connection.Table<EmpresaAspnetUsersModel>().FirstOrDefault(r => r.idEmpresa_aspnetUsers == idRepresentante);

            var _bAdm = _usu.stAdministrador;


            if(_bAdm == true)
            {
                return App.Data.Connection.Table<RepresentadaModel>().Select(rp => rp.idRepresentada ?? 0).ToList();
            }

            return App.Data.Connection.Table<RepresentadaAspnetUsersModel>()
                .Where(
                    c =>
                        c.idEmpresa_aspnetUsers ==
                        idRepresentante)
                        .Select(rpra => rpra.idRepresentada)
                .ToList();
        }
    }
}
