using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Domain.Models.RelationalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Repository.Interface
{
    public interface IBorrowingHistoryRepository
    {
        IEnumerable<BorrowingHistory> GetAll();
        BorrowingHistory Get(Guid? id);
        void Insert(BorrowingHistory entity);
        void Update(BorrowingHistory entity);
        void Delete(BorrowingHistory entity);
    }
}
