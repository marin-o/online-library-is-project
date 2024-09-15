using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OnlineLibrary.Domain.Models.BaseModels;

public class PartnerService : IPartnerService
{
    private readonly string _connectionString;

    public PartnerService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PartnerConnection") ?? throw new System.ArgumentNullException(nameof(configuration));
    }

    public IEnumerable<Book> GetPartnerBooks()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var books = connection.Query("SELECT * FROM Books");

            List<Book> localBooks = new List<Book>();

            foreach (var book in books)
            {
                var formattedISBN = FormatISBN(book.ISBN);

                var kniga = new Book
                {
                    Id = book.Id,
                    Title = $"{book.Title} <i>- From Partners</i>",
                    ImageUrl = book.CoverImage,
                    ISBN = formattedISBN,
                    Quantity = 0
                };
                localBooks.Add(kniga);
            }

            return localBooks;
        }
    }

    private string FormatISBN(string isbn)
    {
        if (isbn.Length != 10)
        {
            return isbn + " - could not format.";
        }
        return $"{isbn.Substring(0, 1)}-{isbn.Substring(1, 4)}-{isbn.Substring(5, 4)}-{isbn.Substring(9, 1)}";
    }
}
