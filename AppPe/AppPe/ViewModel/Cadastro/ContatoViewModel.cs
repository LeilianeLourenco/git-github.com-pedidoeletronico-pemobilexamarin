using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Service;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro
{
    public class ContatoViewModel : ViewModelComum<ContatoModel>
    {

        #region properties

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private BasicPickerModel _departamentoBasicPickerModel = new BasicPickerModel
        {
            XId = "AD",
            Display = "ADMINISTRATIVO"
        };

        public BasicPickerModel DepartamentoBasicPickerModel
        {
            get { return _departamentoBasicPickerModel; }
            set
            {
                _departamentoBasicPickerModel = value;
                base.NotifyPropertyChanged();
                if (value != null)
                {
                    this.currentModel.stDepartamento = value.XId;
                }
            }
        }

        public ClientesModel currentClienteModel { get; set; }
        #endregion
        public ContatoViewModel()
        {
            currentModel = new ContatoModel();
            CarregarPiker();
        }

        private List<BasicPickerModel> _lDepartamentoBasicPickerModels = new List<BasicPickerModel>();
        public List<BasicPickerModel> LDepartamentoBasicPickerModels
        {
            get { return _lDepartamentoBasicPickerModels; }
            set { _lDepartamentoBasicPickerModels = value; NotifyPropertyChanged(); }
        }

        void CarregarPiker()
        {
            LDepartamentoBasicPickerModels = new List<BasicPickerModel>
            {
                new BasicPickerModel
                {
                    XId = "AD",
                    Display = "ADMINISTRATIVO"
                },
                new BasicPickerModel
                {
                    XId = "CP",
                    Display = "COMPRAS"
                },
                new BasicPickerModel
                {
                    XId = "FA",
                    Display = "COMERCIAL/FATURAMENTO"
                },
                new BasicPickerModel
                {
                    XId = "FI",
                    Display = "FINANCEIRO"
                }
            };

        }


        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                if (currentModel.idContatoOffLine != null || currentModel.idContatoOffLine > 0)
                    currentModel = TypeSerializer.Clone(currentClienteModel.objContatoModel);

                currentModel.idClientesOffLine = currentClienteModel.idClientesOffLine;
                currentModel.idClientes = currentClienteModel.idClientes;
                currentModel.idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                if (currentModel.idClientes != null)
                    currentModel.Observable = true;

                DepartamentoBasicPickerModel = currentModel.stDepartamento != null
                    ? LDepartamentoBasicPickerModels.FirstOrDefault(c => c.XId == currentModel.stDepartamento)
                    : new BasicPickerModel { XId = "AD", Display = "ADMINISTRATIVO" };

                SaveCommand = new Command(Save);
                DeleteCommand = new Command(Delete, CanDelete);
            }
            return canExecuteInicial;
        }

        private async void Delete()
        {
            try
            {
                if (!await App.Messages.ShowConfirmAsync(MessageService.QuestionDelete)) return;
                ContatoRepository.Delete(currentModel);
                LimpaRegistroAntigo();
                UtilNavidate.PopAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CanDelete()
        {
            return currentClienteModel.lContato.Any(c => c.idGuid == currentModel.idGuid);
        }

        private void Save()
        {
            if (CanSave())
            {

                if (currentModel.stUsaCatalogo && (currentModel.xEmail.IsValidEmailAddress() == false))
                {
                    App.Messages.ShowAsync("Opção de usar catalogo online, obriga o campo de email no contato.");
                    return;
                }
                if (currentModel.idClientesOffLine != null)
                {
                    currentModel.idClientesOffLine = currentClienteModel.idClientesOffLine;
                    currentModel.idClientes = currentClienteModel.idClientes;
                    ContatoRepository.Save(currentModel);
                }

                LimpaRegistroAntigo();
                currentClienteModel.lContato.Add(currentModel);
                UtilNavidate.PopAsync();
            }
            else
                UtilMessages.CamposFaltandoParaSalvar();
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(currentModel.xNome);
        }


        private void LimpaRegistroAntigo()
        {
            var registro =
                   currentClienteModel.lContato.FirstOrDefault(c => c.idGuid == currentModel.idGuid);
            currentClienteModel.lContato.Remove(registro);
        }



    }
}
