using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OnlineLibrary.Domain.Models.BaseModels;

public interface IPartnerService
{
    public IEnumerable<Book> GetPartnerBooksAsync();
}
