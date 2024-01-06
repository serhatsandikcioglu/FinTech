﻿using FinTech.Core.DTOs.AccountActivity;
using FinTech.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface IAccountActivityService
    {
        Task<CustomResponse<AccountActivityDTO>> CreateAsync(Guid accountId , AccountActivityCreateDTO accountActivityCreateDTO,Guid? userId = null);
    }
}
