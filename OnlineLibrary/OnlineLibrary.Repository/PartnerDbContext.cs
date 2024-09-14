using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Repository
{
   public class PartnerDbContext : DbContext
    {
        public PartnerDbContext(DbContextOptions<PartnerDbContext> options) : base(options)
        {
        }
    }
}
