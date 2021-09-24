using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro
{
    public class TelefoneClienteViewModel : NotifyCommon
    {

        private ClientesModel _currentModel = new ClientesModel();

        public ClientesModel currentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; NotifyPropertyChanged(); }
        }


        private ObservableCollection<Group<string, EnderecoFoneClienteContatoModel>> _lEmailTelefoneAgrupado;
        public ObservableCollection<Group<string, EnderecoFoneClienteContatoModel>> lEmailTelefoneAgrupado
        {
            get { return _lEmailTelefoneAgrupado; }
            set { _lEmailTelefoneAgrupado = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<EnderecoFoneClienteContatoModel> _lEmailTelefone = new ObservableCollection<EnderecoFoneClienteContatoModel>();
        public ObservableCollection<EnderecoFoneClienteContatoModel> lEmailTelefone
        {
            get { return _lEmailTelefone; }
            set { _lEmailTelefone = value; NotifyPropertyChanged(); }
        }



        private void CarregarFoneAndEmail()
        {
            lEmailTelefone = new ObservableCollection<EnderecoFoneClienteContatoModel>();
            var icount = 0;
            foreach (var email in (currentModel.xEmails ?? "").Split(',').Where(email => !string.IsNullOrEmpty(email) && email.IsValidEmailAddress()))
            {
                icount++;
                lEmailTelefone.Add(new EnderecoFoneClienteContatoModel
                {
                    xDescricao = $"E-MAIL {icount}",
                    xEmail = email,
                    Agrupamento = "EMPRESA"
                });
            }
            icount = 0;
            foreach (var fone in (currentModel.xTelefones ?? "").Split(',').Where(fone => !string.IsNullOrEmpty(fone)))
            {
                icount++;
                lEmailTelefone.Add(new EnderecoFoneClienteContatoModel
                {
                    xDescricao = $"FONE {icount}",
                    xFone = fone,
                    Agrupamento = "EMPRESA"
                });
            }

            foreach (var contatoModel in currentModel.lContato)
            {
                if (!string.IsNullOrEmpty(contatoModel.xEmail) || !string.IsNullOrEmpty(contatoModel.xTelefone))
                {
                    lEmailTelefone.Add(new EnderecoFoneClienteContatoModel
                    {
                        xDescricao = contatoModel.xNome.ToUpper(),
                        xFone = (contatoModel.xTelefone ?? "").ToPhoneFormat(),
                        xEmail = contatoModel.xEmail,
                        Agrupamento = "CONTATOS"
                    });
                }
            }

            lEmailTelefoneAgrupado =
                         new ObservableCollection<Group<string, EnderecoFoneClienteContatoModel>>(
                             from registro in lEmailTelefone
                             orderby registro.xDescricao
                             group registro by registro.Agrupamento
                                 into grupos
                             select new Group<string, EnderecoFoneClienteContatoModel>(grupos.Key, grupos));
        }


        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                CarregarFoneAndEmail();
            }
            return canExecuteInicial;
        }


        public async void SendEmail(EnderecoFoneClienteContatoModel item)
        {
            const string format = "mailto:{0}?body={1}{1}PedidoEletronico.com";
            var uri = string.Format(format, item.xEmail, Environment.NewLine);
            if (lEmailTelefone.Count(c => c.hasEmail) > 1)
            {
                if (await App.Messages.ShowConfirmAsync("Deseja utilizar todos os emails do cadastro ?"))
                {
                    var emails = lEmailTelefone.Where(c => c.hasEmail)
                        .Select(c => c.xEmail).ToList().Where(email => email.IsValidEmailAddress())
                        .Aggregate("", (i, email) => i + (email + ";"));
                    uri = string.Format(format, emails, Environment.NewLine);
                }
            }
            Device.OpenUri(new Uri(uri));
        }
        public void Call(EnderecoFoneClienteContatoModel item)
        {
            Device.OpenUri(new Uri("tel:" + item.xFone.RetiraCaracterEspecial()));
        }
    }
}
