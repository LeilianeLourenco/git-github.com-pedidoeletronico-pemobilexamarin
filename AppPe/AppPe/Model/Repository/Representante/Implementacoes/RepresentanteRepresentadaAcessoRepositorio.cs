using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Representante.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Representante.Implementacoes
{
    public class RepresentanteRepresentadaAcessoRepositorio: IRepresentanteRepresentadaAcessoRepository
    {
        public List<int> RetornarRepresentadasPermitidas()
        {
            return App.Data.Connection.Table<RepresentadaAspnetUsersModel>()
                .Where(
                    c =>
                        c.idEmpresa_aspnetUsers ==
                        App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers)
                        .Select(rpra => rpra.idRepresentada)
                .ToList();
        }

    }
}
