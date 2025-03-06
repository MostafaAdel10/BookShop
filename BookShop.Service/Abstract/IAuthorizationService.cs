﻿using BookShop.DataAccess.Entities.Identity;
using BookShop.DataAccess.Requests;
using BookShop.DataAccess.Results;

namespace BookShop.Service.Abstract
{
    public interface IAuthorizationService
    {
        public Task<string> AddRoleAsync(string roleName);
        public Task<bool> IsRoleExistByName(string roleName);
        public Task<string> EditRoleAsync(EditRoleRequest request);
        public Task<string> DeleteRoleAsync(int roleId);
        public Task<bool> IsRoleExistById(int roleId);
        public Task<List<Role>> GetRolesList();
        public Task<Role> GetRoleById(int id);
        public Task<ManageUserRolesResult> ManageUserRolesData(ApplicationUser user);
        public Task<string> UpdateUserRoles(UpdateUserRolesRequest request);
        public Task<ManageUserClaimsResult> ManageUserClaimData(ApplicationUser user);
        public Task<string> UpdateUserClaims(UpdateUserClaimsRequest request);
    }
}
