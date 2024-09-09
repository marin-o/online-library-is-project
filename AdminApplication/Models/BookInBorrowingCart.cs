using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdminApplication.Models
{
    public class BookInBorrowingCart
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Book? Book { get; set; }
        public Guid BorrowingCartId { get; set; }
        public BorrowingCart? BorrowingCart { get; set; }
    }
}
