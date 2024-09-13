using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Repository.Interface;
using OnlineLibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Implementation
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            this.memberRepository = memberRepository;
        }

        public void CreateNewMember(Member m)
        {
            memberRepository.Insert(m);
        }

        public void DeleteMember(string id)
        {
            memberRepository.Delete(memberRepository.Get(id));
        }

        public List<Member> GetAllMembers()
        {
            return memberRepository.GetAll().ToList();
        }

        public Member GetDetailsForMember(string? id)
        {
            return memberRepository.Get(id);
        }

        public void UpdeteExistingMember(Member m)
        {
            memberRepository.Update(m);
        }
    }
}
