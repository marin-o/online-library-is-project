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
        public List<BookInBorrowingCart>? BooksInCart { get; set; }
        public int NumBorrowedBooks { get; set; }
    }
}
