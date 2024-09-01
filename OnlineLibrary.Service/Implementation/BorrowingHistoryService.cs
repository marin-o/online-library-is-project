using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository.Interface;
using OnlineLibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Implementation
{
    public class BorrowingHistoryService : IBorrowingHistoryService
    {
        private readonly IBorrowingHistoryRepository _borrowingHistoryRepository;

        public BorrowingHistoryService(IBorrowingHistoryRepository borrowingHistoryRepository)
        {
            _borrowingHistoryRepository = borrowingHistoryRepository;
        }

        public void Delete(BorrowingHistory entity)
        {
            _borrowingHistoryRepository.Delete(entity);
        }

        public BorrowingHistory Get(Guid? id)
        {
            return _borrowingHistoryRepository.Get(id);
        }

        public IEnumerable<BorrowingHistory> GetAll()
        {
            return _borrowingHistoryRepository.GetAll();
        }

        public void Insert(BorrowingHistory entity)
        {
            _borrowingHistoryRepository.Insert(entity);
        }

        public void Update(BorrowingHistory entity)
        {
            _borrowingHistoryRepository.Update(entity);

        }

        public bool Exists(Guid id)
        {
            return _borrowingHistoryRepository.GetAll().Any(x => x.Id == id);
        }
    }
}
