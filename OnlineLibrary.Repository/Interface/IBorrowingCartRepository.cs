using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Domain.Models.RelationalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Repository.Interface
{
    public interface IBorrowingCartRepository
    {
        IEnumerable<BorrowingCart> GetAll();
        BorrowingCart Get(Guid? id);
        void Insert(BorrowingCart entity);
        void Update(BorrowingCart entity);
        void Delete(BorrowingCart entity);
    }
}
