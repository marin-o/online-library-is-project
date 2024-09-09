using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApplication.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Book? Book { get; set; }
        public DateTime Date { get; set; }

        public string GetMessage()
        {
            return $"The book {Book?.Title} is the most read book this week.";
        }
    }
}
