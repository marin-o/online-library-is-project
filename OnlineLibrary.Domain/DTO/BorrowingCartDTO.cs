using OnlineLibrary.Domain.Models.RelationalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Domain.DTO
{
    public class BorrowingCartDTO
    {
        List<BookInBorrowingCart>? BooksInCart { get; set; }
    }
}
