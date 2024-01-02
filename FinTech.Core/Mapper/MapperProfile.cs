using AutoMapper;
using FinTech.Core.DTOs.Account;
using FinTech.Core.DTOs.AccountActivity;
using FinTech.Core.DTOs.Balance;
using FinTech.Core.DTOs.LoanApplication;
using FinTech.Core.DTOs.MoneyTransfer;
using FinTech.Core.DTOs.SupportTicket;
using FinTech.Core.DTOs.User;
using FinTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<Account , AccountDTO>().ReverseMap();
            CreateMap<Account , AccountCreateDTO>().ReverseMap();

            CreateMap<AccountActivity,AccountActivityDTO>().ReverseMap();
            CreateMap<AccountActivity,AccountActivityCreateDTO>().ReverseMap();

            CreateMap<MoneyTransfer,MoneyTransferCreateDTO>().ReverseMap();

            CreateMap<LoanApplication, LoanApplicationCreateDTO>().ReverseMap();
            CreateMap<LoanApplication, LoanApplicationDTO>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserCreateDTO>().ReverseMap();

            CreateMap<BalanceUpdateDTO, BalanceDTO>().ReverseMap();

            CreateMap<SupportTicket, SupportTicketDTO>().ReverseMap();
            CreateMap<SupportTicket, SupportTicketCreateDTO>().ReverseMap();
            CreateMap<SupportTicket, SupportTicketCreatedDTO>().ReverseMap();

        }
    }
}
