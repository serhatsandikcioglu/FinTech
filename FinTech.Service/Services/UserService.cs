using AutoMapper;
using FinTech.Core.Constans;
using FinTech.Core.DTOs.Account;
using FinTech.Core.DTOs.Role;
using FinTech.Core.DTOs.User;
using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Core.Models;
using FinTech.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(IAccountService accountService, UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManager)
        {
            _accountService = accountService;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }
        public async Task<CustomResponse<UserDTO>> CreateCustomer(UserCreateDTO UserCreateDTO)
        {
            if (_userManager.Users.Any(u => u.IdentityNumber == UserCreateDTO.IdentityNumber))
                return CustomResponse<UserDTO>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.InvalidIdNumber);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                ApplicationUser appUser = _mapper.Map<ApplicationUser>(UserCreateDTO);
                appUser.UserName = UserCreateDTO.IdentityNumber;
                var result = await _userManager.CreateAsync(appUser, UserCreateDTO.Password);
                await _userManager.AddToRoleAsync(appUser, RoleConstants.Customer);
                AccountCreateDTO accountCreateDTO = new AccountCreateDTO();
                await _accountService.CreateAccountWithoutRulesAsync(appUser.Id, accountCreateDTO);
                var userDTO = _mapper.Map<UserDTO>(appUser);
                await _unitOfWork.CommitAsync();
                return CustomResponse<UserDTO>.Success(StatusCodes.Status201Created, userDTO);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return CustomResponse<UserDTO>.Fail(StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }
        public async Task<CustomResponse<UserDTO>> CreateUser(UserCreateDTO UserCreateDTO)
        {
            if (_userManager.Users.Any(u => u.IdentityNumber == UserCreateDTO.IdentityNumber))
                return CustomResponse<UserDTO>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.InvalidIdNumber);

                ApplicationUser appUser = _mapper.Map<ApplicationUser>(UserCreateDTO);
                appUser.UserName = UserCreateDTO.IdentityNumber;
                var result = await _userManager.CreateAsync(appUser, UserCreateDTO.Password);

            if (!result.Succeeded)
                return CustomResponse<UserDTO>.Fail(StatusCodes.Status500InternalServerError, result.Errors.Select(x => x.Description).ToList());

                var userDTO = _mapper.Map<UserDTO>(appUser);
                return CustomResponse<UserDTO>.Success(StatusCodes.Status201Created, userDTO);
        }
        public async Task<CustomResponse<NoContent>> AssignRole(Guid userIdGuid, RoleDTO roleDTO)
        {
            string userId = userIdGuid.ToString();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.UserNotFound);
            var roleExists = await _roleManager.RoleExistsAsync(roleDTO.Role);
            if (!roleExists)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.RoleNotFound);

            var result = await _userManager.AddToRoleAsync(user, roleDTO.Role);
            if (result.Succeeded)
            {
                return CustomResponse<NoContent>.Success(StatusCodes.Status201Created);
            }
            else
            {
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.RoleOperationFailed);
            }
        }
        public async Task<CustomResponse<NoContent>> DeleteRole(Guid userIdGuid, RoleDTO roleDTO)
        {
            string userId = userIdGuid.ToString();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.UserNotFound);

            var roleExists = await _roleManager.RoleExistsAsync(roleDTO.Role);
            if (!roleExists)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.RoleNotFound);

            var result = await _userManager.RemoveFromRoleAsync(user, roleDTO.Role);
            if (result.Succeeded)
            {
                return CustomResponse<NoContent>.Success(StatusCodes.Status204NoContent);
            }
            else
            {
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.RoleOperationFailed);
            }
        }
        public async Task<CustomResponse<UserRolesDTO>> GetRoles(Guid userIdGuid)
        {
            string userId = userIdGuid.ToString();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return CustomResponse<UserRolesDTO>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.UserNotFound);

            var roles = await _userManager.GetRolesAsync(user);
            UserRolesDTO userRolesDTO = new UserRolesDTO();
            userRolesDTO.Roles = roles.ToList();
            return CustomResponse<UserRolesDTO>.Success(StatusCodes.Status200OK, userRolesDTO);
        }
    }
}
