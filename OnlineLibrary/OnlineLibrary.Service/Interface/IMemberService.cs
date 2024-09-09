using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Interface
{
    public interface IMemberService
    {
        List<Member> GetAllMembers();
        Member GetDetailsForMember(string? id);
        void CreateNewMember(Member m);
        void UpdeteExistingMember(Member m);
        void DeleteMember(string id);
    }
}
