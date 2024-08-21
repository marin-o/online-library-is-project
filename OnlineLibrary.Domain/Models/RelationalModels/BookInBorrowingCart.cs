using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLibrary.Domain.Models.BaseModels;

namespace OnlineLibrary.Domain.Models.RelationalModels
{
    public class BookInBorrowingCart : BaseEntity
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid BorrowingCartId { get; set; }
        public BorrowingCart BorrowingCart { get; set; }
    }
}
