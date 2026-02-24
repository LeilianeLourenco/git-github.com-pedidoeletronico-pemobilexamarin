using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Home;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Empresa
{
    public class EmpresaViewModel : ViewModelComum<EmpresaModel>
    {

        #region Properties

        public ICommand EntrarCommand { get; set; }

        private List<BasicPickerModel> _lempresaBasicPickerModels;
        public List<BasicPickerModel> LEmpresaBasicPickerModels
        {
            get { return _lempresaBasicPickerModels; }
            set { _lempresaBasicPickerModels = value; NotifyPropertyChanged(); }
        }

        private BasicPickerModel _currentEmpresaBasicPickerModel;
        public BasicPickerModel CurrentEmpresaBasicPickerModel
        {
            get { return _currentEmpresaBasicPickerModel; }
            set
            {
                _currentEmpresaBasicPickerModel = value;
                if (value != null)
                {
                    ChangeEmpresa(value);
                }
                NotifyPropertyChanged();
            }
        }

        #endregion


        private async void ChangeEmpresa(BasicPickerModel value)
        {
            await Task.Run(() =>
            {
                var email = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xEmail;


                var empresa =
                    App.CurrentAspnetUserModel.lEpresaAspnetUsersModel.FirstOrDefault(
                        c => c.xEmail.ToUpper() == email.ToUpper() && c.idEmpresa == value.Id);

                if (empresa != null)
                {
                    currentModel = empresa.objEmpresaModel;
                }
            });
        }


        private async void EfetivarTrocaEmpresa()
        {
            try
            {
                await Task.Run(() =>
                {
                    var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                    if (idEmpresa == CurrentEmpresaBasicPickerModel.Id)
                        return;

                    var email = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xEmail;

                    foreach (var empresa in App.CurrentAspnetUserModel.lEpresaAspnetUsersModel.Where(c => c.xEmail.ToUpper() == email.ToUpper()))
                    {
                        empresa.isAtiva = empresa.idEmpresa == CurrentEmpresaBasicPickerModel.Id;
                        if (empresa.idEmpresa == CurrentEmpresaBasicPickerModel.Id)
                        {
                            currentModel = empresa.objEmpresaModel;
                        }
                    }
                    App.EnvironmentPE.idEmpresaLogada =
                        App.CurrentAspnetUserModel.lEpresaAspnetUsersModel.FirstOrDefault(c => c.isAtiva).idEmpresa;
                    LoginRepository.UpdateUser();
                    LoginRepository.DesbloquearUser();

                    PageHomeNew.ViewModelStatic = null;
                    UtilNavidate.GoToHome();
                    //PageHome.HomeViewModel.AtualizaImagemHome();
                });
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("", ex.ToString(), "pk");
            }
        }


        public EmpresaViewModel()
        {
            var empresa = App.CurrentAspnetUserModel?.objEmpresaAspnetUsersModel?.objEmpresaModel;

            if (empresa != null)
                currentModel = empresa;

            LEmpresaBasicPickerModels = new List<BasicPickerModel>();
            EntrarCommand = new Command(EfetivarTrocaEmpresa);
            App.CurrentAspnetUserModel = LoginRepository.GetAspnetUsers();

            var empresas = App.CurrentAspnetUserModel.lEpresaAspnetUsersModel;

            var dados = (from c in empresas.Where(c => c.stAtivo && c.xEmail.ToUpper() == App.CurrentAspnetUserModel.Email.ToUpper())
                         select new
                         {
                             idEmpresa = c.objEmpresaModel.idEmpresa ?? 0,
                             xRazaoSocial = c.objEmpresaModel.xRazaoSocial
                         }).Distinct().ToList();

            foreach (var item in dados)
            {
                LEmpresaBasicPickerModels.Add(new BasicPickerModel
                {
                    Id = item.idEmpresa,
                    Display = item.xRazaoSocial
                });
            }

            CurrentEmpresaBasicPickerModel = LEmpresaBasicPickerModels.FirstOrDefault(c => c.Id == empresa.idEmpresa);
        }

        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;


            }
            return canExecuteInicial;
        }


    }
}
