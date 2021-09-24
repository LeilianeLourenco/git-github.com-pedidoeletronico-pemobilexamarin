using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.custom;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.View.Cliente
{


    public partial class PageBuscaPorCnpj : PopupPage
    {
        public bool _forcarPesquisa { get; set; }

        public PageBuscaPorCnpj(bool forcarPesquisa = false)
        {
            InitializeComponent();
            _forcarPesquisa = forcarPesquisa;
            this.BindingContext = PageCliente.StaticViewModel;
            GridManualmente.Command = new Command(() =>
            {
                if (ModalAberto)
                {
                    UtilNavidate.PopPopupNew();
                    ModalAberto = false;
                }
            });
            btnExecutePesquisa.Command = new Command(ExecutarPesquisa);
            EntryCnpj.Completed += (sender, e) => { ExecutarPesquisa(); };
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_forcarPesquisa)
                EntryCnpj.Focus();

            ModalAberto = true;

            if (EntryCnpj.Text.isValidCNPJ() && _forcarPesquisa)
            {
                ExecutarPesquisa();
            }
        }

        private void EntryCpfCnpj_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var entry = sender as ExtendedEntry;
                if (entry != null && entry.IsFocused)
                {
                    entry.TextColor = !entry.Text.isValidCNPJ()
                        ? ColorStaticModel.VermelhoPrincipal
                        : ColorStaticModel.Preto;
                }
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }
        }

        public bool ModalAberto { get; set; }
        public bool bPesquisando { get; set; }

        private async void ExecutarPesquisa()
        {
            if (!bPesquisando)
            {
                bPesquisando = true;

                if (EntryCnpj.Text.isValidCNPJ())
                {
                    if (await App.IsConected() == false)
                    {
                        bPesquisando = Loader.IsVisible = false;
                        await App.Messages.ShowAsync("Sem conexão com internet");
                        return;
                    }

                    Loader.IsVisible = true;
                    bPesquisando = true;
                    var infoCli = await UtilHttp.GetInfoClente(EntryCnpj.Text);


                    if (infoCli.status.ToUpper().Equals("OK"))
                    {
                        PageCliente.StaticViewModel.bBuscouPorCnpj = true;
                        var cliente = PageCliente.StaticViewModel.currentModel;
                        cliente.xRazaoSocial = infoCli.nome.ToUpper();
                        cliente.xFantasia = string.IsNullOrEmpty(infoCli.fantasia)
                            ? cliente.xRazaoSocial
                            : infoCli.fantasia;

                        cliente.xRazaoSocial.ValidaMaxLength(200);
                        cliente.xFantasia.ValidaMaxLength(200);
                        cliente.idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                        if (!string.IsNullOrEmpty(cliente.xCpfCnpj))
                        {
                            // OS 35293 - Jessica Barbieri
                            var cdalter = cliente.xCpfCnpj.Split(' ')[0];
                            cdalter = cdalter.RetiraCaracterEspecial();
                            //var cdalter = cliente.xFantasia.Split(' ')[0];
                            cliente.cAlternativo = cdalter;
                        }

                        cliente.xTelefones = (infoCli.telefone ?? "").Replace("/", ";");
                        if (infoCli.atividade_principal != null && infoCli.atividade_principal.Any())
                            cliente.xAnotacao = infoCli.atividade_principal.FirstOrDefault().text;
                        cliente.xEmails = (infoCli.email ?? "").Replace("/", ";");

                        if (!string.IsNullOrEmpty(cliente.xEmails))
                            PageCliente.StaticViewModel.bQuantidadeEmailCnpj = cliente.xEmails.Split(';').Count();


                        if (!string.IsNullOrEmpty(cliente.xTelefones))
                            PageCliente.StaticViewModel.bQuantidadeTelefoneCnpj = cliente.xTelefones.Split(';').Count();


                        var endereco = new EnderecoModel
                        {
                            xBairro = infoCli.bairro,
                            cNumero = infoCli.numero.TryToInt(),
                            xCep = infoCli.cep.Replace(".", ""),
                            xCidade = infoCli.municipio,
                            xEndereco = infoCli.logradouro,
                            xEstado = infoCli.uf,
                            xComplemento = infoCli.complemento,
                            bBuscaFeitoDaReceita = true,
                            stPrincipal = true,
                            idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa
                        };

                        if (cliente.lEndereco.Any(c => c.xBairro == endereco.xBairro) == false)
                        {
                            //var infoGeo = await UtilHttp.GetInfoEndereco(endereco.xCep);
                            //if (infoGeo.status.ToUpper().Equals("OK"))
                            //{
                            //    if (infoGeo.results.Any())
                            //    {
                            //        foreach (var resultado in infoGeo.results)
                            //        {
                            //            endereco.xLongitude = resultado.geometry.location.lng.ToString();
                            //            endereco.xLatitude = resultado.geometry.location.lat.ToString();
                            //            break;
                            //        }
                            //    }
                            //}
                            cliente.lEndereco.Add(endereco);
                        }


                        if (infoCli.qsa != null && infoCli.qsa.Any())
                        {
                            foreach (var qsa in infoCli.qsa)
                            {
                                try
                                {
                                    var cargo = qsa.qual.Split('-')[0];
                                    cargo = qsa.qual.Replace(cargo + "-", "");

                                    var contato = new ContatoModel
                                    {
                                        xNome = qsa.nome,
                                        xCargo = cargo,
                                        idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa
                                    };
                                    if (cliente.lContato.Any(c => c.xNome == contato.xNome) == false)
                                        cliente.lContato.Add(contato);
                                }
                                catch (Exception ex)
                                {
                                    ex.TrakException("", false);
                                }
                            }
                        }

                        PageCliente.StaticViewModel.bQuantidadeContatoCnpj = cliente.lContato.Count();

                        PageCliente.StaticViewModel.CountLabel();
                    }
                    bPesquisando = false;
                }
                else
                {
                    await App.Messages.ShowAsync("CNPJ inválido.");
                    EntryCnpj.Focus();
                    bPesquisando = Loader.IsVisible = false;
                    return;
                }
                if (ModalAberto)
                {
                    UtilNavidate.PopPopupNew();
                    ModalAberto = false;
                }
            }
        }


    }
}
