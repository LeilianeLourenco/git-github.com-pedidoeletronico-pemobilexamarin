using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.View.Converter.Cliente
{
    public class CollectionToBasicListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var lreturn = new ObservableCollection<BasicPickerModel>();
            if (value != null)
            {
                if (value.GetType() == typeof(ObservableCollection<ContatoModel>))
                {
                    var list = value as ObservableCollection<ContatoModel>;
                    if (list == null) return lreturn;
                    foreach (var contatoModel in list)
                    {
                        lreturn.Add(new BasicPickerModel
                        {
                            Display = contatoModel.xNome,
                            Detail = contatoModel.xDisplay,
                            Image = contatoModel.stUsaCatalogo ? "ApplicationContactBook" : "ApplicationContactBookCatalogo", 
                            Id = contatoModel.idContatoOffLine ?? 0,
                            XId = contatoModel.idGuid
                        });
                    }
                }
                else if (value.GetType() == typeof(ObservableCollection<EnderecoModel>))
                {
                    var list = value as ObservableCollection<EnderecoModel>;
                    if (list == null) return lreturn;
                    foreach (var enderecoModel in list.OrderByDescending(c => c.stPrincipal))
                    {
                        lreturn.Add(new BasicPickerModel
                        {
                            Display = enderecoModel.XTipoEndereco,
                            Detail = enderecoModel.xDisplay,
                            Image = enderecoModel.stPrincipal == false ? "ApplicationPinEnderecoPrincipal" : "ApplicationPin",
                            Id = enderecoModel.idEnderecoOffLine ?? 0,
                            XId = enderecoModel.idGuid
                        });
                    }
                }
            }
            return lreturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
