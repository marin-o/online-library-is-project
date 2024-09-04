using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Repository.Implementation
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Member> entities;
        string errorMessage = string.Empty;

        public MemberRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Member>();
        }
        public IEnumerable<Member> GetAll()
        {
            return entities.AsEnumerable();
        }

        public Member Get(string id)
        {
            return entities
               .Include(z => z.BorrowingCart)
                .ThenInclude(z => z.Books)
                .ThenInclude(z => z.Book)
               .Include(z => z.BorrowingHistories)
                .ThenInclude(z => z.Books)
                .ThenInclude(z => z.Book)
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(Member entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(Member entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(Member entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

    }
}
