using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class ImagemRepository
    {
        public static List<ImagemModel> GetAllImages(int idProduto)
        {
            try
            {
                return App.Data.Connection.Table<ImagemModel>().Where(c => c.idProduto == idProduto).OrderByDescending(p => p.stPrincipal).ToList();
            }
            catch (Exception)
            {
                return new List<ImagemModel>();
            }
        }
    }
}
