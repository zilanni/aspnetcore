using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using ILanni.Common.User;
using ILanni.Common.User.Repository;
using AutoMapper;
using System.Security.Claims;

namespace ILanni.Common.Identity
{
    public class UserStore : IUserStore<User>, IUserEmailStore<User>, IUserLockoutStore<User>, IUserPasswordStore<User>, IUserPhoneNumberStore<User>, IUserSecurityStampStore<User>,IUserRoleStore<User>,IUserClaimStore<User>
    {
        private UserRepository repository;
        public UserStore(UserRepository repository)
        {
            this.repository = repository;
        }

        #region IUserStore

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var model = Mapper.Map<ILanni.Common.User.DbModel.User>(user);
            var result = await repository.Add(model, cancellationToken);
            if (result.success)
            {
                user.Id = model.Id;
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Code = result.error, Description = result.error });

        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var u = await repository.FindByIdAsync(user.Id, cancellationToken);
            if (null != u)
            {
                await repository.Delete(u, cancellationToken);
            }
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            repository.Dispose();
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await repository.FindByIdAsync(userId, cancellationToken);
            return Mapper.Map<User>(user);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {

            var user = await repository.FindByNameAsync(normalizedUserName, cancellationToken);
            return Mapper.Map<User>(user);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var model = await repository.FindByIdAsync(user.Id, cancellationToken);
            if (null == model)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "用户不存在", Description = "用户不存在" });
            }
            Mapper.Map(user, model);
            var result = await repository.Update(model, cancellationToken);
            if (result.success)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Code = result.error, Description = result.error });
        }
        #endregion

        #region IUserEmailStore

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var model = repository.FindByEmailAsync(normalizedEmail, cancellationToken);
            return Mapper.Map<User>(model);
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.NormalizedEmail);
        }


        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserLockoutStore

        public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.LockoutEnd);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserPasswordStore

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash);
        }


        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        #endregion

        #region  IUserPhoneNumberStore


        public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PhoneNumberConfirmed);
        }



        public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }
        #endregion

        #region 
        //
        // 摘要:
        //     Get the security stamp for the specified user.
        //
        // 参数:
        //   user:
        //     The user whose security stamp should be set.
        //
        //   cancellationToken:
        //     The System.Threading.CancellationToken used to propagate notifications that the
        //     operation should be canceled.
        //
        // 返回结果:
        //     The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        //     the security stamp for the specified user.
        public Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.SecurityStamp);
        }
        //
        // 摘要:
        //     Sets the provided security stamp for the specified user.
        //
        // 参数:
        //   user:
        //     The user whose security stamp should be set.
        //
        //   stamp:
        //     The security stamp to set.
        //
        //   cancellationToken:
        //     The System.Threading.CancellationToken used to propagate notifications that the
        //     operation should be canceled.
        //
        // 返回结果:
        //     The System.Threading.Tasks.Task that represents the asynchronous operation.
        public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }
        #endregion

        #region IUserRoleStore
        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await repository.AddToRoleAsync(user.Id, roleName, cancellationToken);
            user.Roles.Add(new Role() { Id = roleName, Name = roleName, NormalizedName = roleName });
        }

        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult((IList<string>)user.Roles.Select(r => r.Id).ToList());
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //cancellationToken.ThrowIfCancellationRequested();
            //return repository.Set<UserRole>().Include(r => r.User).Where(x => x.RoleId == roleName).Select(r => r.User).ToListAsync();
        }


        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Roles.Any(r => r.Id == roleName));
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await repository.RemoveFromRoleAsync(user.Id, roleName, cancellationToken);
            user.Roles.RemoveAll(r => r.Id == roleName);
        }
        #endregion

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            var clains = await repository.GetClaims(user.Id, cancellationToken);
            var result = clains.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            if (!string.IsNullOrWhiteSpace(user.NormalizedEmail))
            {
                result.Add(new Claim(ClaimTypes.Email, user.NormalizedEmail));
            }
            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                result.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            }
            return result;
        }
        
        public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
