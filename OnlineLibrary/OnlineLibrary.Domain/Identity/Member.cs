using Microsoft.AspNetCore.Identity;
using OnlineLibrary.Domain.Models.RelationalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Domain.Identity
{
    public class Member : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public virtual BorrowingCart? BorrowingCart { get; set; }
        public virtual ICollection<BorrowingHistory>? BorrowingHistories { get; set; }
    }
}
