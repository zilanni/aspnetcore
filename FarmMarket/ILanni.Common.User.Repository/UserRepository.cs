using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ILanni.Common.User.DbModel;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ILanni.Common.User.Repository
{
    public class UserRepository : IUserStore<ILanni.Common.User.DbModel.User>, IUserEmailStore<DbModel.User>, IUserLockoutStore<DbModel.User>, IUserPasswordStore<DbModel.User>, IUserPhoneNumberStore<DbModel.User>, IUserSecurityStampStore<DbModel.User>,IUserRoleStore<DbModel.User>
    {
        private UserDbContext context;
        public UserRepository(UserDbContext context)
        {
            this.context = context;
        }

        #region IUserStore

        public async Task<IdentityResult> CreateAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            context.Users.Add(user);
            var r = await context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
            //return context.AddAsync(user, cancellationToken).ContinueWith<IdentityResult>(t => IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            context.Users.Remove(user);
            var result = await context.SaveChangesAsync();
            return result > 0 ? IdentityResult.Success: IdentityResult.Failed(new IdentityError() { Code = "", Description = "删除失败" });
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Task<DbModel.User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);//.FindAsync(new object[] { userId }, cancellationToken);
        }

        public Task<DbModel.User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return context.Users.Include(u=>u.Roles).FirstOrDefaultAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(DbModel.User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(DbModel.User user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }
        #endregion

        #region IUserEmailStore

        public Task<DbModel.User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }
        public Task<string> GetEmailAsync(DbModel.User user, CancellationToken cancellationToken)
        {

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(DbModel.User user, CancellationToken cancellationToken)
        {

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.NormalizedEmail);
        }


        public Task SetEmailAsync(DbModel.User user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(DbModel.User user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(DbModel.User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserLockoutStore

        public Task<int> GetAccessFailedCountAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.LockoutEnd);
        }

        public Task<int> IncrementAccessFailedCountAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(DbModel.User user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(DbModel.User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserPasswordStore

        public Task<string> GetPasswordHashAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash);
        }


        public Task<bool> HasPasswordAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(DbModel.User user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        #endregion

        #region  IUserPhoneNumberStore
       

        public Task<string> GetPhoneNumberAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        

        public Task SetPhoneNumberAsync(DbModel.User user, string phoneNumber, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberConfirmedAsync(DbModel.User user, bool confirmed, CancellationToken cancellationToken)
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
        public Task<string> GetSecurityStampAsync(DbModel.User user, CancellationToken cancellationToken)
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
        public Task SetSecurityStampAsync(DbModel.User user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }
        #endregion

        #region IUserRoleStore
        public Task AddToRoleAsync(DbModel.User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userRole = new UserRole() { RoleId = roleName, User =user };
            user.Roles.Add(userRole);
            context.Add(userRole);
            return context.SaveChangesAsync(cancellationToken);
        }

        public Task<IList<string>> GetRolesAsync(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult((IList<string>)user.Roles.Select(r => r.RoleId).ToList());
        }

        public Task<IList<DbModel.User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //cancellationToken.ThrowIfCancellationRequested();
            //return context.Set<UserRole>().Include(r => r.User).Where(x => x.RoleId == roleName).Select(r => r.User).ToListAsync();
        }


        public Task<bool> IsInRoleAsync(DbModel.User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Roles.Any(r => r.RoleId == roleName));
        }

        public Task RemoveFromRoleAsync(DbModel.User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var role = user.Roles.FirstOrDefault(r => r.RoleId == roleName);
            if (null != role)
            {
                user.Roles.Remove(role);
                context.Set<UserRole>().Remove(role);
                return context.SaveChangesAsync(cancellationToken);
            }
            return Task.CompletedTask;
        }
        #endregion

    }
}
