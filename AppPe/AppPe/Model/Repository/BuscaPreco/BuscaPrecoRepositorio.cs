using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class BuscaPrecoRepositorio
    {
        public List<TabelaPrecoModel> BuscarCampanhasCliente(int idEmpresa, int idCliente, string filtro = null)
        {
            List<TabelaPrecoModel> _lRetornoAux;
            List<IBuscaPrecoRepositorio> _lReposisBusca = new List<IBuscaPrecoRepositorio>();
            List<TabelaPrecoModel> _lRetorno = new List<TabelaPrecoModel>();
            _lReposisBusca.Add(new BuscaPrecoClienteRepositorio());
            _lReposisBusca.Add(new BuscaPrecoClienteUfRepository());
            _lReposisBusca.Add(new BuscaPrecoClienteRamoRepositorio());
            _lReposisBusca.Add(new BuscaPrecoClienteJoinRamoRepositorio());
            _lReposisBusca.Add(new BuscaPrecoClienteUfJoinRamoRepositorio());

            foreach (var rep in _lReposisBusca)
            {
                _lRetornoAux = rep.RetornaPrecos(idEmpresa: idEmpresa, id: idCliente, stBusca: TipoPrecoBusca.cmp, filtro: filtro);

                if (_lRetornoAux != null && _lRetornoAux.Count > 0)
                {
                    _lRetorno.AddRange(_lRetornoAux);
                }
            }
            return _lRetorno;
        }

        public List<TabelaPrecoModel> BuscarCampanhasRepresentante(int idEmpresa, int idUsuario, string filtro = null)
        {
            BuscaPrecoRepresentanteRepositorio rep = new BuscaPrecoRepresentanteRepositorio();
            List<TabelaPrecoModel> _lRetornoAux = rep.RetornaPrecos(idEmpresa: idEmpresa, id: idUsuario, stBusca: TipoPrecoBusca.cmp, filtro: filtro);

            return _lRetornoAux;
        }

        public List<TabelaPrecoModel> BuscarCampanhasRepresentacao(int idEmpresa, int idRepresentacao, string filtro = null)
        {
            BuscaPrecoRepresentacaoRepositorio _rep = new BuscaPrecoRepresentacaoRepositorio();
            List<TabelaPrecoModel> _lRetornoAux = _rep.RetornaPrecos(idEmpresa: idEmpresa, id: idRepresentacao, stBusca: TipoPrecoBusca.cmp, filtro: filtro);

            return _lRetornoAux;
        }

        public List<TabelaPrecoModel> BuscarCampanhasGerais(int idEmpresa, string filtro = null)
        {
            BuscaPrecoGeralRepositorio _rep = new BuscaPrecoGeralRepositorio();
            var _lRetornoAux = _rep.RetornaPrecos(idEmpresa: idEmpresa, id: 0, stBusca: TipoPrecoBusca.cmp, filtro: filtro);

            return _lRetornoAux;
        }
    }
}
