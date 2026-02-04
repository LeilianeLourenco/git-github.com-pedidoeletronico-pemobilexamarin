using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.View.Service;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro
{
    public class EnderecoViewModel : ViewModelComum<EnderecoModel>
    {
        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                if (currentModel.idEnderecoOffLine != null || currentModel.idEnderecoOffLine > 0)
                    currentModel = TypeSerializer.Clone(currentClienteModel.objEndereco);

                CarregaPicker();
                currentModel.idClientesOffLine = currentClienteModel.idClientesOffLine;
                currentModel.idClientes = currentClienteModel.idClientes;
                currentModel.idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                if (currentModel.idClientes != null)
                    currentModel.Observable = true;

                if (currentModel.idEnderecoOffLine == null)
                {
                    if (!string.IsNullOrEmpty(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.xEstado))
                    {
                        var xEstado = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.xEstado;
                        var objEstado = LEstadosBasicPickerModels.FirstOrDefault(c => c.XId == xEstado);
                        currentModel.EstadoBasicPickerModel = objEstado ?? new BasicPickerModel { XId = "SP", Display = "SÃO PAULO" };
                    }
                    if (!string.IsNullOrEmpty(currentModel.xEstado))
                    {
                        var objEstado = LEstadosBasicPickerModels.FirstOrDefault(c => c.XId == currentModel.xEstado);
                        currentModel.EstadoBasicPickerModel = objEstado ?? new BasicPickerModel { XId = "SP", Display = "SÃO PAULO" };
                    }
                    TipoEnderecoBasicPickerModel = currentModel.LtipoEnderecoBasicPickerModels.FirstOrDefault();
                }
                else
                {
                    var objEstado = LEstadosBasicPickerModels.FirstOrDefault(c => c.XId == currentModel.xEstado);
                    currentModel.EstadoBasicPickerModel = objEstado ?? new BasicPickerModel { XId = "SP", Display = "SÃO PAULO" };

                    var tipoEndereco = currentModel.LtipoEnderecoBasicPickerModels.FirstOrDefault(c => c.XId == currentModel.stEndereco);
                    TipoEnderecoBasicPickerModel = tipoEndereco ?? new BasicPickerModel { XId = "CO", Display = "COMERCIAL" };
                }
                //FindCepCorreiosCommand = new Command(execute: () =>
                //{
                //    UtilCorreios.BuscaCep(currentModel);
                //}, canExecute: () => !currentModel.isSearching);
                currentModel.SaveCommand = new Command(Save);
                currentModel.DeleteCommand = new Command(Delete, CanDelete);
            }
            return false;
        }

        public ClientesModel currentClienteModel { get; set; }

        #region properties

        public ICommand FindCepCorreiosCommand { get; set; }

        private BasicPickerModel _tipoEnderecoBasicPickerModel;
        public BasicPickerModel TipoEnderecoBasicPickerModel
        {
            get { return _tipoEnderecoBasicPickerModel; }
            set
            {
                _tipoEnderecoBasicPickerModel = value; NotifyPropertyChanged();
                if (value != null)
                    currentModel.stEndereco = value.XId;
            }
        }

        #endregion
        public EnderecoViewModel()
        {
            currentModel = new EnderecoModel();
            CarregaPicker();
        }

        private List<BasicPickerModel> _lEstatdosBasicPickerModels = new List<BasicPickerModel>();
        public List<BasicPickerModel> LEstadosBasicPickerModels
        {
            get { return _lEstatdosBasicPickerModels; }
            set { _lEstatdosBasicPickerModels = value; NotifyPropertyChanged(); }
        }

        private List<BasicPickerModel> _lCidadesBasicPickerModels = new List<BasicPickerModel>();
        public List<BasicPickerModel> lCidadesBasicPickerModels
        {
            get { return _lCidadesBasicPickerModels; }
            set { _lCidadesBasicPickerModels = value; NotifyPropertyChanged(); }
        }

        void CarregaPicker()
        {
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "AC", Display = "ACRE" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "AL", Display = "ALAGOAS" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "AP", Display = "AMAPÁ" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "AM", Display = "AMAZONAS" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "BA", Display = "BAHIA" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "CE", Display = "CEARÁ" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "DF", Display = "DISTRITO FEDERAL" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "ES", Display = "ESPÍRITO SANTO" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "GO", Display = "GOIÁS" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "MA", Display = "MARANHÃO" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "MT", Display = "MATO GROSSO" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "MS", Display = "MATO GROSSO DO SUL" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "MG", Display = "MINAS GERAIS" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "PA", Display = "PARÁ" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "PB", Display = "PARAÍBA" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "PR", Display = "PARANÁ" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "PE", Display = "PERNAMBUCO" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "PI", Display = "PIAUÍ" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "RJ", Display = "RIO DE JANEIRO" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "RN", Display = "RIO GRANDE DO NORTE" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "RS", Display = "RIO GRANDE DO SUL" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "RO", Display = "RONDÔNIA" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "RR", Display = "RORAIMA" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "SC", Display = "SANTA CATARINA" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "SP", Display = "SÃO PAULO" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "SE", Display = "SERGIPE" });
            LEstadosBasicPickerModels.Add(new BasicPickerModel { XId = "TO", Display = "TOCANTINS" });
        }


        public void Save()
        {

            if (CanSave())
            {
                if (currentModel.idClientesOffLine != null)
                {
                    currentModel.idClientesOffLine = currentClienteModel.idClientesOffLine;
                    currentModel.idClientes = currentClienteModel.idClientes;
                    EnderecoRepository.Save(currentModel);
                }
                LimpaRegistroAntigo(currentModel.stPrincipal);
                currentClienteModel.lEndereco.Add(currentModel);
                UtilNavidate.PopAsync();
            }
            else
                UtilMessages.CamposFaltandoParaSalvar();

        }

        public bool CanSave()
        {
            return !string.IsNullOrEmpty(currentModel.xEndereco) && !string.IsNullOrEmpty(currentModel.xCidade) && !string.IsNullOrEmpty(currentModel.xEstado);
        }


        public async void Delete()
        {
            if (!await App.Messages.ShowConfirmAsync(MessageService.QuestionDelete)) return;
            EnderecoRepository.Delete(currentModel);
            LimpaRegistroAntigo();
            UtilNavidate.PopAsync();
        }

        public bool CanDelete()
        {
            return currentClienteModel.lEndereco.Any(c => c.idGuid == currentModel.idGuid);
        }


        private void LimpaRegistroAntigo(bool? stPrincipal = null)
        {
            var registro =
                currentClienteModel.lEndereco.FirstOrDefault(c => c.idGuid == currentModel.idGuid);

            if ((registro == null && stPrincipal == true) || (registro != null && registro.stPrincipal != stPrincipal))
            {
                if (stPrincipal != null && stPrincipal == true)
                {
                    foreach (var ender in currentClienteModel.lEndereco.Where(c => c != registro))
                    {
                        ender.Observable = true;
                        ender.stPrincipal = false;
                        EnderecoRepository.Save(ender);
                    }
                }
            }
            currentClienteModel.lEndereco.Remove(registro);
        }

    }
}
