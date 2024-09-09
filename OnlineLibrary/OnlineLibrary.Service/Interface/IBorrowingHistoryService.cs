using OnlineLibrary.Domain.Models.RelationalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Interface
{
    public interface IBorrowingHistoryService
    {
        IEnumerable<BorrowingHistory> GetAll();
        BorrowingHistory Get(Guid? id);
        void Insert(BorrowingHistory entity);
        void Update(BorrowingHistory entity);
        void Delete(BorrowingHistory entity);
        bool Exists(Guid id);
        List<BorrowingHistory> GetBorrowingHistoriesForUser(string userId);
        bool ReturnBooks(Guid borrowingHistoryId, string userId);
        public bool ReturnBook(Guid borrowingHistoryId, Guid bookId, string userId);
    }
}
