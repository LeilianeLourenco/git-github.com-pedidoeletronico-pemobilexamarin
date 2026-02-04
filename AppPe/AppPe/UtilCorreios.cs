using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Cidades;

namespace Xamarin.HLP.Mobile.AppPE
{
    public class UtilCorreios
    {
        static WebRequest request = null;

        private static string RemoverCaracteres(string texto)
        {
            string resultado = texto;

            resultado = resultado.Replace("\n", "");
            resultado = resultado.Replace("\r", "");
            resultado = resultado.Replace("\t", "");
            resultado = resultado.Replace(".", "");
            resultado = resultado.Replace("-", "");
            resultado = resultado.Trim();



            return resultado;
        }

        private static EnderecoModel objEnderecoModel { get; set; }
        public static async void BuscaCep(EnderecoModel _objEnderecoModel)
        {
            if (_objEnderecoModel.isSearching == false)
            {
                objEnderecoModel = _objEnderecoModel;
                if (objEnderecoModel == null) return;
                if (await App.IsConected() == false)
                {
                    await App.Messages.ShowAsync("Sem conexão com internet.");
                    return;
                }
                try
                {
                    objEnderecoModel.isSearching = true;
                    var cep = RemoverCaracteres(objEnderecoModel.xCep);
                    //var parametros = "cepEntrada=" + cep + "&tipoCep=&cepTemp=&metodo=buscarCep";
                    request = WebRequest.Create("https://viacep.com.br/ws/"+cep+"/json/");
                    request.Method = "GET";
                    //request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentType = "application/json";
                    request.BeginGetResponse(FinishWebRequest, null);
                }
                catch (Exception)
                {
                    objEnderecoModel.isSearching = false;
                }
            }

        }
        static async void FinishWebRequest(IAsyncResult result)
        {
            try
            {
                var response = request.EndGetResponse(result);
                var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                var dados = stream.ReadToEnd();
                EnderecoCepModel ender = JsonConvert.DeserializeObject<EnderecoCepModel>(dados);

                if (!string.IsNullOrEmpty(ender.localidade))
                {
                    objEnderecoModel.xEndereco = ender.logradouro;
                    objEnderecoModel.xBairro = ender.bairro;
                    //objEnderecoModel.xCidade = ender.localidade;

                    var pickerUF =
                        objEnderecoModel.LEstadosBasicPickerModels.FirstOrDefault(c => c.XId.ToUpper() == ender.uf.ToUpper());
                    objEnderecoModel.EstadoBasicPickerModel = pickerUF ?? objEnderecoModel.LEstadosBasicPickerModels.FirstOrDefault();

                    SelecionarCidade(ender.localidade);
                }


                //var count = 0;
                //const string ExpressaoRegular = "<span class=\"respostadestaque\">(.*?)</span>";
                //var endereco = Regex.Matches(dados, ExpressaoRegular, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                //foreach (Match resultado in endereco)
                //{
                //    count++;

                //    switch (count) //Preencho um resultado por vez
                //    {
                //        case 1:
                //            objEnderecoModel.xEndereco = resultado.Groups[1].Value.Trim();
                //            break;
                //        case 2:
                //            objEnderecoModel.xBairro = RemoverCaracteres(resultado.Groups[1].Value.Trim());
                //            break;
                //        case 3:
                //            {
                //                try
                //                {
                //                    objEnderecoModel.xCidade = RemoverCaracteres(resultado.Groups[1].Value.Trim().Split('/')[0]);
                //                }
                //                catch (Exception)
                //                {
                //                }
                //                try
                //                {
                //                    var uf = RemoverCaracteres(resultado.Groups[1].Value.Trim().Split('/')[1]);
                //                    var pickerUF =
                //                        objEnderecoModel.LEstadosBasicPickerModels.FirstOrDefault(c => c.XId.ToUpper() == uf.ToUpper());
                //                    objEnderecoModel.EstadoBasicPickerModel = pickerUF ?? objEnderecoModel.LEstadosBasicPickerModels.FirstOrDefault();
                //                }
                //                catch (Exception)
                //                {
                //                }

                //            }
                //            break;
                //    }
                //}

                var cep = objEnderecoModel.xCep.Replace(".", "").Replace("-", "");
                cep = cep.PadLeft(8, '0');
                int icep = 0;
                int.TryParse(cep, out icep);

                if (icep > 0)
                {
                    cep = icep.ToString("#####-###");
                    objEnderecoModel.xCep = cep;
                }

                var infoGeo = await UtilHttp.GetInfoEndereco(objEnderecoModel.xCep);
                if (infoGeo.status.ToUpper().Equals("OK"))
                {
                    if (infoGeo.results.Any())
                    {
                        foreach (var resultado in infoGeo.results)
                        {
                            objEnderecoModel.xLongitude = resultado.geometry.location.lng.ToString();
                            objEnderecoModel.xLatitude = resultado.geometry.location.lat.ToString();
                            break;
                        }
                    }
                }

                objEnderecoModel.isSearching = false;
            }
            catch (Exception ex)
            {
                objEnderecoModel.isSearching = false;
            }

        }

        private static void SelecionarCidade(string cidade)
        {
            var estado = objEnderecoModel.EstadoBasicPickerModel;

            if (estado != null)
            {
                var cidades = App.Data.Connection
                 .Table<CidadesModel>()
                 .Where(c => c.uf == estado.XId)
                 .OrderBy(c => c.nome)
                 .ToList();

                var pickerItems = cidades.Select(c => new BasicPickerModel
                {
                    XId = c.codigoIBGE?.ToString(),
                    Display = c.nome
                }).ToList();

                objEnderecoModel.CidadeBasicPickerModel = pickerItems.FirstOrDefault(x => x.Display.ToLower() == cidade.ToLower());
            }
        }
    }
}
