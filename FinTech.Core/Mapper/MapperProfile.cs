using AutoMapper;
using FinTech.Core.DTOs;
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
        }
    }
}
