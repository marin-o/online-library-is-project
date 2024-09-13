using OnlineLibrary.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Interface
{
    public interface INotificationService
    {
        List<Notification> GetAllNotifications();
        Notification GetDetailsForNotification(Guid? id);
        void CreateNewNotification(Book b);
        void UpdeteExistingNotification(Notification b);
        void DeleteNotification(Guid id);
        List<Notification> GetLatestNotifications(int count);
    }
}
