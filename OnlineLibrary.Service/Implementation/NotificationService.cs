using EShop.Repository.Interface;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Implementation
{
    public class NotificationService : INotificationService
    { 
        private readonly IRepository<Notification> _notificationRepository;

        public NotificationService(IRepository<Notification> NotificationRepository)
        {
            _notificationRepository = NotificationRepository;
        }
        public void CreateNewNotification(Book b)
        {
            var notification = new Notification
            {
                BookId = b.Id,
                Book = b,
                Date = DateTime.Now
            };
            _notificationRepository.Insert(notification);
        }

        public void DeleteNotification(Guid id)
        {
            var Notification = _notificationRepository.Get(id);
            _notificationRepository.Delete(Notification);
        }

        public List<Notification> GetAllNotifications()
        {
            return _notificationRepository.GetAll().ToList();
        }

        public Notification GetDetailsForNotification(Guid? id)
        {
            return _notificationRepository.Get(id);
        }

        public List<Notification> GetLatestNotifications(int count)
        {
            return _notificationRepository.GetAll()
                .OrderByDescending(n => n.Date)
                .Take(count)
                .ToList();
        }

        public void UpdeteExistingNotification(Notification p)
        {
            _notificationRepository.Update(p);
        }
    }
}
