using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.PagSeguro;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.Common
{

    public class UtilHttp
    {

        #region POST

        public static async Task<Acesso> AcessoPermitido(int idEmpresa, string idAspNetUsers)
        {
            Acesso retorno = null;
            try
            {
                using (var httpAcesso = new HttpClient())
                {
                    httpAcesso.BaseAddress = new Uri(App.UrlPortal);
                    httpAcesso.DefaultRequestHeaders.Accept.Clear();
                    httpAcesso.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var acessoModel = new AcessoModel
                    {
                        idEmpresa = idEmpresa,
                        idUsuario = idAspNetUsers
                    };
                    var xJson = JsonConvert.SerializeObject(acessoModel);
                    var requestUri = App.UrlPortal + "api/ApiPagSeguro/AcessoPermitido";
                    var wcfResponse =
                        await
                            CurrentHttpClient.PostAsync(requestUri,
                                new StringContent(xJson, Encoding.UTF8, "application/json"));
                    if (wcfResponse == null) return null;
                    if (!wcfResponse.IsSuccessStatusCode) return null;
                    var responJsonText = await wcfResponse.Content.ReadAsStringAsync();
                    retorno = JsonConvert.DeserializeObject<Acesso>(responJsonText);
                }
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            catch (Exception ex)
            {
                ex.TrakException("AcessoPermitido", false);

                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                else
                    SincronizacaoNewViewModel.ocorreuErro = true;
            }
            return retorno;
        }

        /// <summary>
        /// POST REGISTROS PARA O AZURE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classe"></param>
        /// <param name="xNamePost"></param>
        /// <returns></returns>
        public static async Task<RetornoSalvar<T>> PostRegistroToCloud<T>(T classe, string xNamePost = "Post")
            where T : class
        {
            var retistroSync = new RetornoSalvar<T>();
            try
            {
                var xController = TableMobile.GetApiRegistroByModel<T>();
                if (xController == "") return null;
                var xJson = JsonConvert.SerializeObject(classe);
                var requestUri = App.UrlWebApi + $"/api/{xController}/{xNamePost}";
                var wcfResponse =
                    await
                        CurrentHttpClient.PostAsync(requestUri,
                            new StringContent(xJson, Encoding.UTF8, "application/json"));
                if (wcfResponse == null) return null;
                if (!wcfResponse.IsSuccessStatusCode) return null;
                var responJsonText = await wcfResponse.Content.ReadAsStringAsync();
                retistroSync = JsonConvert.DeserializeObject<RetornoSalvar<T>>(responJsonText);
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            catch (Exception ex)
            // catch all other errorsD:\TF\HLPMOBILE_PE\AppPE\Xamarin.HLP.Mobile.AppPE\Xamarin.HLP.Mobile.AppPE\Extensions.cs
            {
                ex.TrakException("PostRegistroToCloud", false);
                /*Insights.Report(ex, Insights.Severity.Error);*/
            }
            return retistroSync;
        }

        public static async Task<AtividadeAgendaModel> PostAgendaToCloud<T>(T classe, string xNamePost = "Post")
    where T : class
        {
            var retistroSync = new AtividadeAgendaModel();
            try
            {
                var xJson = JsonConvert.SerializeObject(classe);
                var requestUri = App.UrlWebApiMobile + $"api/ApiAtividadeAgendaMobile/SalvarAtividade";
                var wcfResponse =
                    await
                        CurrentHttpClient.PostAsync(requestUri,
                            new StringContent(xJson, Encoding.UTF8, "application/json"));
                if (wcfResponse == null) return null;
                if (!wcfResponse.IsSuccessStatusCode) return null;
                var responJsonText = await wcfResponse.Content.ReadAsStringAsync();
                retistroSync = JsonConvert.DeserializeObject<AtividadeAgendaModel>(responJsonText);
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            catch (Exception ex)
            {
                ex.TrakException("PostRegistroToCloud", false);
            }
            return retistroSync;
        }


        //public static async void PostSyncOmie(List<ClientesModel> lClientes )
        //{
        //    try
        //    {
        //        var xController = "ApiClientesOmie";
        //        var xJson = JsonConvert.SerializeObject(lClientes);
        //        var requestUri = App.UrlWebApi + $"/api/{xController}/Post";
        //        var wcfResponse = await CurrentHttpClientPost.PostAsync(requestUri, new StringContent(xJson, Encoding.UTF8, "application/json"));
        //        if (wcfResponse == null) return;
        //        if (!wcfResponse.IsSuccessStatusCode) return ;
        //        var responJsonText = await wcfResponse.Content.ReadAsStringAsync();
        //        var retorno = JsonConvert.DeserializeObject<bool>(responJsonText);
        //    }
        //    catch (System.Net.WebException)
        //    {
        //        SincronizacaoNewViewModel.bFalhaConexao = true;
        //    }
        //    catch (Exception ex) // catch all other errorsD:\TF\HLPMOBILE_PE\AppPE\Xamarin.HLP.Mobile.AppPE\Xamarin.HLP.Mobile.AppPE\Extensions.cs
        //    {
        //        ex.TrakException();
        //        /*Insights.Report(ex, Insights.Severity.Error);*/
        //    }
        //}

        public static async Task<LoginMobile> PostLogin(LoginViewModel model)
        {
            var loginSync = new LoginMobile();
            try
            {
                SincronizacaoNewViewModel.bFalhaConexao = false;
                //await App.Messages.ShowAsync("iniciou");
                var xJson = JsonConvert.SerializeObject(model);
                //await App.Messages.ShowAsync("serializou");
                var requestUri = App.UrlWebApi + "/api/APIaccount/Post";
                //await App.Messages.ShowAsync("requestUri");
                var wcfResponse =
                    await
                        CurrentHttpClient.PostAsync(requestUri,
                            new StringContent(xJson, Encoding.UTF8, "application/json"));

                //await App.Messages.ShowAsync($"response: { (wcfResponse == null ? "response null" : "response ok")}");
                if (wcfResponse == null) return null;
                if (!wcfResponse.IsSuccessStatusCode) return null;
                var responJsonText = await wcfResponse.Content.ReadAsStringAsync();
                //await App.Messages.ShowAsync(responJsonText);
                loginSync = JsonConvert.DeserializeObject<LoginMobile>(responJsonText);
            }
            catch (System.Net.WebException ex)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            catch (Exception ex)
            {
                ex.TrakException("PostLogin", false);

                //Insights.Report(ex, Insights.Severity.Error);
            }
            return loginSync;
        }

        public static async Task<SimpleResultPost> PostForgot(ForgotPasswordViewModel model)
        {

            try
            {
                var xJson = JsonConvert.SerializeObject(model);
                var requestUri = App.UrlWebApi + "/api/APIaccount/Post3";
                var wcfResponse =
                    await
                        CurrentHttpClient.PostAsync(requestUri,
                            new StringContent(xJson, Encoding.UTF8, "application/json"));
                if (wcfResponse == null) return null;
                if (!wcfResponse.IsSuccessStatusCode) return null;
                var responJsonText = await wcfResponse.Content.ReadAsStringAsync();
                var retorno = JsonConvert.DeserializeObject<SimpleResultPost>(responJsonText);

                return retorno;
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            catch (Exception ex)
            {
                //ex.TrakException();
                ex.TrakException("PostForgot", false);
            }
            return new SimpleResultPost();
        }

        public static async Task<LoginMobile> PostRegister(RegisterViewModel model)
        {

            var loginSync = new LoginMobile();
            try
            {
                var xJson = JsonConvert.SerializeObject(model);
                var requestUri = App.UrlWebApi + "/api/APIaccount/Post2";
                var wcfResponse =
                    await
                        CurrentHttpClient.PostAsync(requestUri,
                            new StringContent(xJson, Encoding.UTF8, "application/json"));
                if (wcfResponse == null) return null;
                if (!wcfResponse.IsSuccessStatusCode) return null;
                var responJsonText = await wcfResponse.Content.ReadAsStringAsync();
                loginSync = JsonConvert.DeserializeObject<LoginMobile>(responJsonText);
            }
            catch (Exception ex)
            {
                //ex.TrakException();
                ex.TrakException("PostRegister", true);
            }
            return loginSync;
        }

        public static async void SendEmailPedido(EmailPedidoModel param)
        {
            try
            {
                var xJson = JsonConvert.SerializeObject(param);
                var requestUri = App.UrlWebApi + "/api/APIEmailPedido/Post";
                var wcfResponse =
                    await
                        CurrentHttpClient.PostAsync(requestUri,
                            new StringContent(xJson, Encoding.UTF8, "application/json"));
                if (wcfResponse == null) return;
                if (!wcfResponse.IsSuccessStatusCode) return;
                var responJsonText = await wcfResponse.Content.ReadAsStringAsync();
                var EmailEnviado = JsonConvert.DeserializeObject<bool>(responJsonText);

            }
            catch (Exception ex)
            {
                ex.TrakException("SendEmail", false);
            }
        }

        #endregion

        #region GET

        public static async void SaveImagem(string xPath)
        {
            try
            {
                if (xPath.ToUpper().Contains(@"/MEDIA/PRODUTO/") || xPath.ToUpper().Contains("APPLICATIONDEFAULTIMAGE"))
                    return;
                using (var client = new HttpClient())
                {
                    var xFile = $"{App.UrlWebApi}/{xPath}";
                    var buffer = client.GetByteArrayAsync(xFile).Result;
                    //xPath = xPath.Replace("/imgs/", "").Replace(".png", "");
                    var xNameImage = xPath.PathToNameImage();
                    App.Picture.SavePictureToDisk(xNameImage, buffer);
                    await Task.Yield();
                }
            }
            catch (Exception ex)
            {
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                ex.TrakException("SaveImage", false);
            }
        }

        public static async Task<List<T>> GetListRegistroSync<T>(object param1, DateTime? param2 = null,
            object param3 = null) where T : class
        {
            var lregistros = new List<T>();
            try
            {
                var requestUri =
                    $"api/{TableMobile.GetApiRegistroByModel<T>()}/{param1}{(param2 != null ? "/" + ((DateTime)param2).ToString("yyyy-MM-ddTHH:mm:ss") : null)}{(param3 != null ? "/" + param3.ToString() : "")}";

                var _apiClient = CurrentHttpClient;

                var jsonResponse = await _apiClient.GetStringAsync(requestUri);
                lregistros = JsonConvert.DeserializeObject<List<T>>(jsonResponse);

            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetListRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                else
                    SincronizacaoNewViewModel.ocorreuErro = true;

                SincronizacaoNewViewModel.xMensagemErro = ex.Message;
            }
            return lregistros;
        }

        public static async Task<List<T>> GetListRegistroPaginadoSync<T>(object param1, DateTime? param2 = null,
            object param3 = null, int Page = 1) where T : class
        {
            var lregistros = new List<T>();
            try
            {
                var requestUri =
                    $"api/{TableMobile.GetApiRegistroByModel<T>()}/{Page}/{param1}/{(param2 != null ? "/" + ((DateTime)param2).ToString("yyyy-MM-ddTHH:mm:ss") : null)}/{(param3 != null ? "/" + param3.ToString() : "")}";

                var _apiClient = CurrentHttpClient;

                var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);
                lregistros = JsonConvert.DeserializeObject<List<T>>(jsonResponse);

            }
            catch (System.Net.WebException)
            { 
                SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetListRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                else
                    SincronizacaoNewViewModel.ocorreuErro = true;

                SincronizacaoNewViewModel.xMensagemErro = ex.Message;
            }
            return lregistros;
        }

        public static async Task<T> GetRegistroSync<T>(int? idEmpresa, int? idPK = null) where T : class
        {
            try
            {
                var requestUri = $"api/{TableMobile.GetApiRegistroByModel<T>()}/{idEmpresa}{"/" + (idPK ?? 0)}";
                var jsonResponse = await CurrentHttpClient.GetStringAsync(requestUri);
                T objRegistro = null;

                if (jsonResponse != null)
                    objRegistro = JsonConvert.DeserializeObject<T>(jsonResponse);

                return objRegistro;
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
        }

         
        public static async Task<List<T>> GetPedidosVendas<T>(int? idEmpresa, int? page, DateTime? dtUltimaAlteracao, string idAspNetUsers) where T : class
        {
            var lregistros = new List<T>();
            try
            {
                var requestUri = $"api/ApiPedidoVendaMobile/{idEmpresa}/{page}/{dtUltimaAlteracao?.ToString("yyyy-MM-ddTHH:mm:ss")}/{idAspNetUsers}";
                var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);

                lregistros = JsonConvert.DeserializeObject<List<T>>(jsonResponse);
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }

            return lregistros;
        }

        public static async Task<List<T>> GetEstoque<T>(int? idEmpresa, int? page, DateTime? dtUltimaAlteracao) where T : class
        {
            var lregistros = new List<T>();
            try
            {
                var requestUri = $"api/ApiEstoqueMobile/GetEstoque/{idEmpresa}/{page}/{dtUltimaAlteracao?.ToString("yyyy-MM-ddTHH:mm:ss")}";
                var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);

                lregistros = JsonConvert.DeserializeObject<List<T>>(jsonResponse);
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }

            return lregistros;
        }


        public static async Task<int> GetQuantidadeTotalASincronizar(object param1, DateTime param2)
        {
            try
            {
                var requestUri =
                    $"api/ApiEstoqueMobile/GetTotalASincronizar/{param1}/{param2:yyyy-MM-ddTHH:mm:ss}";
                var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);
                var lregistros = JsonConvert.DeserializeObject<int>(jsonResponse);
                return lregistros;
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return 0;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroIDSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return 0;
            }
        }

        public static async Task<List<T>> GetLocaisEstoque<T>(int idEmpresa, int page, DateTime dtUltimaAlteracao, byte stTipoBuscar) where T : class
        {
            try
            {
                var lregistros = new List<T>();

                switch (stTipoBuscar)
                {
                    case 0:
                        var requestUri =
                   $"api/ApiEstoqueMobile/GetTotalASincronizarLocais/{idEmpresa}/{page}/{dtUltimaAlteracao:yyyy-MM-ddTHH:mm:ss}";
                        var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);
                        lregistros = JsonConvert.DeserializeObject<List<T>>(jsonResponse);

                        return lregistros;
                    case 1:
                        var requestUri2 =
                   $"api/ApiEstoqueMobile/GetTotalASincronizarLocaisClientes/{idEmpresa}/{page}/{dtUltimaAlteracao:yyyy-MM-ddTHH:mm:ss}";
                        var jsonResponse2 = await CurrentApiMobileHttpClient.GetStringAsync(requestUri2);
                        lregistros = JsonConvert.DeserializeObject<List<T>>(jsonResponse2);

                        return lregistros;
                    case 2:
                        var requestUri3 =
                   $"api/ApiEstoqueMobile/GetTotalASincronizarLocaisRepresentantes/{idEmpresa}/{page}/{dtUltimaAlteracao:yyyy-MM-ddTHH:mm:ss}";
                        var jsonResponse3 = await CurrentApiMobileHttpClient.GetStringAsync(requestUri3);
                        lregistros = JsonConvert.DeserializeObject<List<T>>(jsonResponse3);

                        return lregistros;
                }

                return lregistros;
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroIDSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
        }

        public static async Task<int> GetQuantidadeTotalLocaisEstoque(int idEmpresa, DateTime dtUltimaAlteracao, byte stTipoBuscar)
        {
            try
            {
                int lregistros = 0;

                switch (stTipoBuscar)
                {
                    case 0:
                        var requestUri =
                   $"api/ApiEstoqueMobile/ObterTotalSicnronizarLocal/{idEmpresa}/{dtUltimaAlteracao:yyyy-MM-ddTHH:mm:ss}";
                        var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);
                        lregistros = JsonConvert.DeserializeObject<int>(jsonResponse);

                        return lregistros;
                    case 1:
                        var requestUri2 =
                   $"api/ApiEstoqueMobile/ObterTotalSicnronizarLocalCliente/{idEmpresa}/{dtUltimaAlteracao:yyyy-MM-ddTHH:mm:ss}";
                        var jsonResponse2 = await CurrentApiMobileHttpClient.GetStringAsync(requestUri2);
                        lregistros = JsonConvert.DeserializeObject<int>(jsonResponse2);

                        return lregistros;
                    case 2:
                        var requestUri3 =
                   $"api/ApiEstoqueMobile/ObterTotalSicnronizarLocalRepresentante/{idEmpresa}/{dtUltimaAlteracao:yyyy-MM-ddTHH:mm:ss}";
                        var jsonResponse3 = await CurrentApiMobileHttpClient.GetStringAsync(requestUri3);
                        lregistros = JsonConvert.DeserializeObject<int>(jsonResponse3);

                        return lregistros;
                }

                return lregistros;
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return 0;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroIDSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return 0;
            }
        }

        public static async Task<int> GetCountProdutos(int? idEmpresa, DateTime? dtUltimaAlteracao, string idAspNetUsers)
        {
            var count = 0;
            try
            {
                var requestUri = $"api/ApiProdutoMobile/GetCountProdutos/{idEmpresa}/{dtUltimaAlteracao?.ToString("yyyy-MM-ddTHH:mm:ss")}";
                var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);

                count = JsonConvert.DeserializeObject<int>(jsonResponse);
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return 0;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return 0;
            }

            return count;
        }

        /// <summary>
        /// Pega as Atividades alteradas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="dtUltimaAlteracao"></param>
        /// <param name="idAspNetUsers"></param>
        /// <returns></returns>
        public static async Task<int> GetCountAtividades(int? idEmpresa, DateTime? dtUltimaAlteracao, string idAspNetUsers)
        {
            var count = 0;
            try
            {
                var requestUri = $"api/ApiAtividadeAgendaMobile/GetCountAtividades/{idEmpresa}/{dtUltimaAlteracao?.ToString("yyyy-MM-ddTHH:mm:ss")}/{idAspNetUsers}";
                var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);

                count = JsonConvert.DeserializeObject<int>(jsonResponse);
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return 0;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return 0;
            }

            return count;
        }

        public static async Task<int> GetCountTipoAtividades(int? idEmpresa, DateTime? dtUltimaAlteracao)
        {
            var count = 0;
            try
            {
                var requestUri = $"api/ApiAtividadeAgendaMobile/GetCountTipoAtividades/{idEmpresa}/{dtUltimaAlteracao?.ToString("yyyy-MM-ddTHH:mm:ss")}";
                var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);

                count = JsonConvert.DeserializeObject<int>(jsonResponse);
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return 0;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return 0;
            }

            return count;
        }



        public static async Task<List<T>> GetAtividadesAgenda<T>(int? idEmpresa, int? page, DateTime? dtUltimaAlteracao, string idAspNetUsers) where T : class
        {
            var lregistros = new List<T>();
            try
            {
                var requestUri = $"api/ApiAtividadeAgendaMobile/{idEmpresa}/{page}/{dtUltimaAlteracao?.ToString("yyyy-MM-ddTHH:mm:ss")}/{idAspNetUsers}";
                var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);

                lregistros = JsonConvert.DeserializeObject<List<T>>(jsonResponse);
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }

            return lregistros;
        }

        public static async Task<List<T>> GetTipoAtividadesAgenda<T>(int? idEmpresa, int? page, DateTime? dtUltimaAlteracao) where T : class
        {
            var lregistros = new List<T>();
            try
            {
                var requestUri = $"api/ApiAtividadeAgendaMobile/GetTipoAtividades/{idEmpresa}/{page}/{dtUltimaAlteracao?.ToString("yyyy-MM-ddTHH:mm:ss")}";
                var jsonResponse = await CurrentApiMobileHttpClient.GetStringAsync(requestUri);

                lregistros = JsonConvert.DeserializeObject<List<T>>(jsonResponse);
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }

            return lregistros;
        }


        public static async Task<List<T>> GetRegistroSyncEstoque<T>(int? idEmpresa, int? idPK = null, DateTime? data = null) where T : class
        {
            try
            {
                var requestUri =
                    $"api/{TableMobile.GetApiRegistroByModel<T>()}/{idEmpresa}{"/" + (idPK ?? 0)}{(data != null ? "/" + ((DateTime)data).ToString("yyyy-MM-ddTHH:mm:ss") : null)}";

                var jsonResponse = await CurrentHttpClient.GetStringAsync(requestUri);
                var objRegistro = new List<T>();

                if (jsonResponse != null)
                    objRegistro = JsonConvert.DeserializeObject<List<T>>(jsonResponse);

                return objRegistro;
            }

            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }

            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
        }


        public static async Task<List<ImagemModel>> GetRegistroSyncImagem(int? idEmpresa, int? idPK, DateTime? data)
        {
            try
            {
                var requestUri =
                    $"api/APIimagemmobile/{idEmpresa}{"/" + (idPK ?? 0)}{(data != null ? "/" + ((DateTime)data).ToString("yyyy-MM-ddTHH:mm:ss") : null)}";

                var jsonResponse = await CurrentHttpClient.GetStringAsync(requestUri);
                var objRegistro = new List<ImagemModel>();

                if (jsonResponse != null)
                    objRegistro = JsonConvert.DeserializeObject<List<ImagemModel>>(jsonResponse);

                return objRegistro;
            }

            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }

            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
        }


        public static async Task<infoClienteApiModel> GetInfoClente(string xCNPJ)
        {
            try
            {
                xCNPJ = xCNPJ.RetiraCaracterEspecial();
                var requestUri = $"/v1/cnpj/{xCNPJ}";
                var httpClient = new HttpClient { BaseAddress = new Uri(@"https://www.receitaws.com.br/") };
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.Timeout = TimeSpan.FromSeconds(100);

                var jsonResponse = httpClient.GetStringAsync(requestUri).Result;


                var resultado = JsonConvert.DeserializeObject<infoClienteApiModel>(jsonResponse);

                if (resultado != null)
                {
                    if ((resultado.message ?? "").ToUpper().Contains("ONE OR MORE ERRORS OCCURRED"))
                    {
                        await App.Messages.ShowAsync("CNPJ não encontrado, desculpe.");
                    }

                    return resultado;
                }
                return new infoClienteApiModel { status = "ERROR", message = "NÃO ENCONRADO" };
            }
            catch (Exception ex)
            {
                return new infoClienteApiModel { status = "ERROR", message = ex.Message };
            }
        }


        public static async Task<List<PedidosToSyncModel>> GetRegistroIDSync(object param1, DateTime param2,
            string ApiController, object param3 = null)
        {
            try
            {
                var requestUri =
                    $"api/{ApiController}/{param1}/{param2:yyyy-MM-ddTHH:mm:ss}/{(param3 != null ? param3.ToString() : "")}";
                var jsonResponse = await CurrentHttpClient.GetStringAsync(requestUri);
                var lregistros = JsonConvert.DeserializeObject<List<PedidosToSyncModel>>(jsonResponse);
                return lregistros;
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return new List<PedidosToSyncModel>();
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroIDSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return new List<PedidosToSyncModel>();
            }
        }

        public static async Task<T?> GetValueSync<T>(string controller, object param1, object param2 = null,
            object param3 = null) where T : struct
        {
            try
            {
                var requestUri =
                    $"api/{controller}/{param1}/{(param2 != null ? param2.ToString() : "")}/{(param3 != null ? param3.ToString() : "")}";
                var jsonResponse = await CurrentHttpClient.GetStringAsync(requestUri);
                var objRetorno = JsonConvert.DeserializeObject<T>(jsonResponse);
                return objRetorno;
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }
            catch (Exception ex)
            {
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return null;
            }

        }

        public static async Task<DateTime?> GetDateServer()
        {
            DateTime? dt = null;
            try
            {
                var requestUri =
                    $"api/{"ApiDateTimeServer"}/0/0";
                var jsonResponse = await CurrentHttpClient.GetStringAsync(requestUri);

                dt = Convert.ToDateTime(jsonResponse.Replace("\"", ""));
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            catch (Exception ex)
            {
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            return dt;

        }

        public static async Task<List<T>> GetRegistroToRemoveSync<T>(DateTime date) where T : class
        {
            var lregistros = new List<T>();
            try
            {
                var requestUri =
                    $"api/APIremove/{App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}/{date:yyyy-MM-ddTHH:mm:ss}";
                var jsonResponse = await CurrentHttpClient.GetStringAsync(requestUri);
                lregistros = JsonConvert.DeserializeObject<List<T>>(jsonResponse);
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            catch (Exception ex)
            {
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            return lregistros;

        }


        public static async Task<GeolocalizacaoModel> GetInfoEndereco(string xParam)
        {
            try
            {
                var httpclienteGoogle = new HttpClient { BaseAddress = new Uri("https://maps.googleapis.com/") };
                httpclienteGoogle.DefaultRequestHeaders.Accept.Clear();
                httpclienteGoogle.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpclienteGoogle.Timeout = TimeSpan.FromSeconds(100);

                var requestUri = $"/maps/api/geocode/json?address=BrasilCEP{xParam.Replace(" ", "+")}&key=AIzaSyCnq3d96u9Ng6kEdDTfgVPaXQ2eeWHTkpY";
                var jsonResponse = await httpclienteGoogle.GetStringAsync(requestUri);
                var retorno = JsonConvert.DeserializeObject<GeolocalizacaoModel>(jsonResponse);
                return retorno;
            }
            catch (Exception ex)
            {
                return new GeolocalizacaoModel { status = "FAIL" };
            }
        }

        public static async Task<KeyValuePair<bool, string>> PermiteSincronizacao(int idEmpresa, int idRepresentante)
        {

            try
            {
                var requestUri = $"api/APIValidacao/{idEmpresa}{"/" + idRepresentante}";
                var jsonResponse = await CurrentHttpClient.GetStringAsync(requestUri);
                bool objRegistro = false;

                if (jsonResponse != null)
                    objRegistro = JsonConvert.DeserializeObject<bool>(jsonResponse);

                return new KeyValuePair<bool, string>(key: objRegistro, value: "Vendedor Inativo");
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
                return new KeyValuePair<bool, string>(key: false, value: "Vendedor Inativo");
            }
            catch (Exception ex)
            {
                ex.TrakException("GetRegistroSync", false);
                if (!await App.IsConected() || ex.Message?.ToUpper() == "THE OPERATION WAS CANCELED")
                    SincronizacaoNewViewModel.bFalhaConexao = true;
                return new KeyValuePair<bool, string>(key: false, value: "Vendedor Inativo");
            }

        }

        //ALTERADO
        //OS 35349 - Jessica Barbieri
        //public static async Task<ClientesModel> GetRegistroClienteToSync<ClientesModel>(int param1, int param2)
        //    //where ClientesModel : struct
        //{
        //    try
        //    {
        //        var requestUri =
        //            $"api/GetRegistroClienteToSync/{param1}/{param2}";
        //        var jsonResponse = await CurrentHttpClient.GetStringAsync(requestUri);
        //        var objRetorno = JsonConvert.DeserializeObject<ClientesModel>(jsonResponse);
        //        return objRetorno;
        //    }

        //    catch (System.Net.WebException)
        //    {
        //        SincronizacaoNewViewModel.bFalhaConexao = true;
        //        return default(ClientesModel);
        //    }

        //    catch (Exception ex)
        //    {
        //        if (!await App.IsConected())
        //            SincronizacaoNewViewModel.bFalhaConexao = true;
        //        return default(ClientesModel);
        //    }
        //}

        public static async Task<double?> GetLimiteCreditoCliente(int param1, int param2)
        {
            try
            {
                var requestUri =
                    $"api/APICreditoCliente/GetLimiteCreditoCliente?param1={param1}&param2={param2}";

                var jsonResponse = await CurrentHttpClient.GetStringAsync(requestUri);
                var retorno = JsonConvert.DeserializeObject<double>(jsonResponse);

                return retorno;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
        //FIM

        #endregion

        #region DELETE
        public static async Task<bool> DeleteAsync<T>(object idPk) where T : class
        {
            var bRetorno = false;
            try
            {
                var api = TableMobile.GetApiRegistroByModel<T>();
                var client = new HttpClient { BaseAddress = new Uri(App.UrlWebApi) };
                var requestUri = $"/api/{api}/{Convert.ToInt32(idPk)}/0";
                var response = await client.DeleteAsync(requestUri);
                bRetorno = response.IsSuccessStatusCode;
            }
            catch (System.Net.WebException)
            {
                SincronizacaoNewViewModel.bFalhaConexao = true;
            }
            catch (Exception)
            {
                return false;
            }
            return bRetorno;
        }

        #endregion

        #region properties
        public enum TipoRetornoInfoClass { PrimaryKey, ApiRegistro, TableName }
        private static HttpClient _currentHttpClient = null;
        public static HttpClient CurrentHttpClient
        {
            get
            {
                if (_currentHttpClient != null)
                {
                    return _currentHttpClient;
                }
                _currentHttpClient = new HttpClient { BaseAddress = new Uri(App.UrlWebApi) };
                _currentHttpClient.DefaultRequestHeaders.Accept.Clear();
                _currentHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _currentHttpClient.Timeout = TimeSpan.FromSeconds(100); 
                return _currentHttpClient;
            }
            set { _currentHttpClient = value; }
        }

        private static HttpClient _currentApiMobileHttpClient = null;
        public static HttpClient CurrentApiMobileHttpClient
        {
            get
            {
                if (_currentApiMobileHttpClient != null)
                {
                    return _currentApiMobileHttpClient;
                }
                _currentApiMobileHttpClient = new HttpClient { BaseAddress = new Uri(App.UrlWebApiMobile) };
                _currentApiMobileHttpClient.DefaultRequestHeaders.Accept.Clear();
                _currentApiMobileHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _currentApiMobileHttpClient.Timeout = TimeSpan.FromSeconds(100);
                return _currentApiMobileHttpClient;
            }
            set { _currentApiMobileHttpClient = value; }
        }


        #endregion
    }
}
