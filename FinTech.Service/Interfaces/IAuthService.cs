using FinTech.Core.DTOs.Authentication;
using FinTech.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface IAuthService
    {
        Task<CustomResponse<TokenDTO>> Login(LoginDTO loginDTO);
    }
}
