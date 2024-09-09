using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApplication.Models
{
    public class BorrowingCart
    {
        public Guid Id { get; set; }
        public string? MemberId { get; set; }
        public LibraryMember? Member { get; set; }
        public List<BookInBorrowingCart>? Books { get; set; }
    }
}
