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
                var kniga = new Book
                {
                    Id = book.Id,
                    Title = book.Title,
                    ImageUrl = book.CoverImage,
                    ISBN = book.ISBN,
                    Quantity = 0
                };
                localBooks.Add(kniga);
            }

            return localBooks;
        }
    }
}
