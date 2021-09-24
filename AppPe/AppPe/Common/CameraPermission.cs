using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Permissions.Abstractions;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    public class CameraPermission
    {
        private readonly IPermissions permissions;

        public CameraPermission(IPermissions permissions)
        {
            this.permissions = permissions;
        }

        public async Task<bool> RequestCameraPermissionIfNeeded()
        {
            try
            {
                var status = await permissions.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    var results = await permissions.RequestPermissionsAsync(new[] { Permission.Camera });

                    status = results[Permission.Camera];
                }

                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }
    }
}
