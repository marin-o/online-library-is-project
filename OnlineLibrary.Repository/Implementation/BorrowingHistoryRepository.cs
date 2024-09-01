using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Repository.Implementation
{
    public class BorrowingHistoryRepository : IBorrowingHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<BorrowingHistory> entities;
        string errorMessage = String.Empty;

        public BorrowingHistoryRepository(ApplicationDbContext context)
        {
            this.entities = context.BorrowingHistories;
            _context = context;
        }

        public void Delete(BorrowingHistory entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
        }

        public BorrowingHistory Get(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            return entities
                .Include(z => z.Books)
                .ThenInclude(z => z.Book)
                .Include(z => z.Member)
                .SingleOrDefault(z => z.Id == id);
        }

        public IEnumerable<BorrowingHistory> GetAll()
        {
            return entities
                .Include(z => z.Books)
                .ThenInclude(z => z.Book)
                .Include(z => z.Member)
                .AsEnumerable();
        }

        public void Insert(BorrowingHistory entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(BorrowingHistory entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            _context.SaveChanges();
        }
    }
}
