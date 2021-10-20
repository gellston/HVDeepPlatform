using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionDeepTool.Helper
{
    public class ToastMessageHelper
    {
        static private readonly NotificationManager notificaitonManager = new NotificationManager();


        static public void ShowToastErrorMessage(string title, string message)
        {
            notificaitonManager.ShowAsync(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = Notifications.Wpf.Core.NotificationType.Error
            });
        }

        static public void ShowToastSuccessMessage(string title, string message)
        {
            notificaitonManager.ShowAsync(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = Notifications.Wpf.Core.NotificationType.Success
            });
        }
    }
}
