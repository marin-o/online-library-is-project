using OnlineLibrary.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Domain.Models.RelationalModels
{
    public class BookInBorrowingHistory : BaseEntity
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid BorrowingHistoryId { get; set; }
        public BorrowingHistory BorrowingHistory { get; set; }
        public DateTime BorrowedAt { get; set; }
        public bool Returned { get; set; }
        public DateTime ReturnedAt { get; set; }
    }
}