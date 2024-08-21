using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Domain.Models.BaseModels
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public string Biography { get; set; }
    }
}
