using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Domain.DTO;
using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository.Interface;
using Stripe.Climate;

namespace OnlineLibrary.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBorrowingHistoryRepository borrowingHistoryRepository;
        private readonly UserManager<Member> userManager;

        public AdminController(IBorrowingHistoryRepository borrowingHistoryRepository, UserManager<Member> userManager)
        {
            this.borrowingHistoryRepository = borrowingHistoryRepository;
            this.userManager = userManager;
        }

        [HttpGet("[action]")]
        public List<BorrowingHistory> GetAllBorrowingHistories()
        {
            return borrowingHistoryRepository.GetAll().ToList();
        }

        [HttpPost("[action]")]
        public BorrowingHistory GetDetailsForBorrowingHistory(BaseEntity model)
        {
            return borrowingHistoryRepository.Get(model.Id);
        }

        [HttpPost("[action]")]
        public bool ImportAllUsers(List<MemberRegistrationDTO> model)
        {
            bool status = true;
            foreach (var user in model)
            {
                var memberCheck = userManager.FindByEmailAsync(user.Email).Result;
                if (memberCheck == null)
                {
                    var neww = new Member
                    {
                        Email = user.Email,
                        UserName = user.Email,
                        NormalizedEmail = user.Email,
                        EmailConfirmed = true,
                        BorrowingCart = new BorrowingCart()
                    };
                    var result = userManager.CreateAsync(neww, user.Password).Result;
                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }
            return status;
        }
    }
}
