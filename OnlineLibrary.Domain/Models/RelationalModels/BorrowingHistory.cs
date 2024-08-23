using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Domain.Models.BaseModels;

namespace OnlineLibrary.Domain.Models.RelationalModels
{
    public class BorrowingHistory : BaseEntity
    {
        public string? MemberId{ get; set; }
        public Member User { get; set; }
        public List<BookInBorrowingHistory>? Books { get; set; }
    }
}
