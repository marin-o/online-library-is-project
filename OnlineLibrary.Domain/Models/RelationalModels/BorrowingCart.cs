using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Domain.Models.RelationalModels
{
    public class BorrowingCart : BaseEntity
    {
        public string? MemberId { get; set; }
        public Member? Member { get; set; }
        public List<BookInBorrowingCart>? Books { get; set; }
    }
}
