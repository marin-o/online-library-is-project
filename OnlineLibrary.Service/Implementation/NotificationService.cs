using EShop.Repository.Interface;
using OnlineLibrary.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Implementation
{
    public class NotificationService
    {
        private readonly IRepository<Notification> _NotificationRepository;

        public NotificationService(IRepository<Notification> NotificationRepository)
        {
            _NotificationRepository = NotificationRepository;
        }
        public void CreateNewNotification(Notification p)
        {
            _NotificationRepository.Insert(p);
        }

        public void DeleteNotification(Guid id)
        {
            var Notification = _NotificationRepository.Get(id);
            _NotificationRepository.Delete(Notification);
        }

        public List<Notification> GetAllNotifications()
        {
            return _NotificationRepository.GetAll().ToList();
        }

        public Notification GetDetailsForNotification(Guid? id)
        {
            return _NotificationRepository.Get(id);
        }

        public void UpdeteExistingNotification(Notification p)
        {
            _NotificationRepository.Update(p);
        }
    }
}
