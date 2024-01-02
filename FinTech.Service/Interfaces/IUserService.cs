using FinTech.Core.DTOs.Role;
using FinTech.Core.DTOs.User;
using FinTech.Core.Entities;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface IUserService
    {
        Task<CustomResponse<UserDTO>> CreateCustomer(UserCreateDTO userCreateDTO);
        Task<CustomResponse<UserDTO>> CreateUser(UserCreateDTO UserCreateDTO);
        Task<CustomResponse<UserRolesDTO>> GetRoles(Guid userIdGuid);
        Task<CustomResponse<NoContent>> DeleteRole(Guid userIdGuid, RoleDTO roleDTO);
        Task<CustomResponse<NoContent>> AssignRole(Guid userIdGuid, RoleDTO roleDTO);
    }
}
