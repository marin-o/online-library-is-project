using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Domain.Models.BaseModels
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Year { get; set; }
        public int Pages { get; set; }
        public int Quantity { get; set; }
        public bool Available { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
