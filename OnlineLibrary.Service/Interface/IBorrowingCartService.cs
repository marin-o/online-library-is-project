using OnlineLibrary.Domain.DTO;
using OnlineLibrary.Domain.Models.RelationalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Interface
{
    public interface IBorrowingCartService
    {
        BorrowingCartDTO getBorrowingCartInfo(string userId);
        bool deleteBookFromBorrowingCart(string userId, Guid productId);
        bool borrow(string userId);
        bool AddBookToBorrowingCart(BookInBorrowingCart model, string userId);
    }
}
