using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Repository.Implementation
{
    public class BorrowingCartRepository : IBorrowingCartRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<BorrowingCart> entities;
        string errorMessage = string.Empty;

        public BorrowingCartRepository(ApplicationDbContext context)
        {
            _context = context;
            this.entities = context.BorrowingCarts;
        }

        public void Delete(BorrowingCart entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
        }

        public BorrowingCart Get(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            return entities
                .Include(z => z.Books)
                .ThenInclude(z => z.Book)
                .Include(z => z.Member)
                .SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<BorrowingCart> GetAll()
        {
            return entities
                .Include(z => z.Books)
                .ThenInclude(z => z.Book)
                .Include(z => z.Member)
                .AsEnumerable();
        }

        public void Insert(BorrowingCart entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(BorrowingCart entity)
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
