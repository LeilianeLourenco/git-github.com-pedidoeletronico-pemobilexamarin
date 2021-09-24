using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Input;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Annotations;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    public class ModelComum : INotifyPropertyChanged
    {

        public ModelComum()
        {
            ltrasformationCircle = new List<ITransformation> { new CircleTransformation(), new CropTransformation(1, 0, 0) };
            lTransformationsCorners.Add(new CropTransformation(0.6,0,0));
        }

        private List<ITransformation> _ltrasformationCircle = new List<ITransformation>();
        [Ignore]
        [IgnoreDataMember]
        public List<ITransformation> ltrasformationCircle
        {
            get { return _ltrasformationCircle; }
            set { _ltrasformationCircle = value; NotifyPropertyChanged(); }
        }

        private List<ITransformation> _lTransformationsCorners = new List<ITransformation>();
        [Ignore]
        [IgnoreDataMember]
        public List<ITransformation> lTransformationsCorners
        {
            get { return _lTransformationsCorners; }
            set { _lTransformationsCorners = value;NotifyPropertyChanged(); }
        }

        [IgnoreDataMember]
        [Ignore]
        public double DefultFontSize => SettingsModel.DefultFontSize;


        [IgnoreDataMember]
        [Ignore]
        public ICommand SaveCommand { get; set; }
        [IgnoreDataMember]
        [Ignore]
        public ICommand DeleteCommand { get; set; }
        [IgnoreDataMember]
        [Ignore]
        public ICommand AddCommand { get; set; }

        [IgnoreDataMember]
        [Ignore]
        public bool Observable { get; set; }
        [IgnoreDataMember]
        [Ignore]
        public bool RegistroAlterado { get; set; }


        [IgnoreDataMember]
        [Ignore]
        public string ApplicationViewCellSelect => "Xamarin.HLP.Mobile.AppPE.Images.MenuIcon.ApplicationArrowRightMenuDefault.svg";

        private bool _needRefresh;
        [IgnoreDataMember]
        [Ignore]
        public bool needRefresh
        {
            get { return _needRefresh; }
            set { _needRefresh = value; NotifyPropertyChanged(); }
        }

        public string xMeuID { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Observable)
                RegistroAlterado = true;

            SaveCommand?.ChangeCanExecute();
        }

        public void PublicNotifyPropertyChanged(string _propertyName)
        {
            NotifyPropertyChanged(_propertyName);
        }


    }
}
