using FinTech.Core.DTOs.Role;
using FinTech.Core.DTOs.User;
using FinTech.Core.Entities;
using FinTech.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface IUserService
    {
        Task<CustomResponse<UserDTO>> CreateCustomerAsync(UserCreateDTO userCreateDTO);
        Task<CustomResponse<UserDTO>> CreateUserAsync(UserCreateDTO UserCreateDTO);
        Task<CustomResponse<UserRolesDTO>> GetRolesAsync(Guid userIdGuid);
        Task<CustomResponse<NoContent>> DeleteRoleAsync(Guid userIdGuid, RoleDTO roleDTO);
        Task<CustomResponse<NoContent>> AssignRoleAsync(Guid userIdGuid, RoleDTO roleDTO);
    }
}
