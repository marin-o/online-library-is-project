using OnlineLibrary.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Repository.Interface
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAll();
        Member Get(string? id);
        void Insert(Member entity);
        void Update(Member entity);
        void Delete(Member entity);
    }
}
