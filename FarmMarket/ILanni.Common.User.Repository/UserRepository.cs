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
    public class UserRepository
    {
        private UserDbContext context;
        public UserRepository(UserDbContext context)
        {
            this.context = context;
        }

        public async Task<(bool success, string error)> Add(DbModel.User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            context.Users.Add(user);
            try
            {
                var result = await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return (false, "用户名已经存在");
            }
            return (true, null);
        }

        public async Task Delete(DbModel.User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            context.Users.Remove(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<(bool success, string error)> Update(DbModel.User user, CancellationToken cancellationToken)
        {
            context.Users.Update(user);
            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return (false, "用户数据已更改");
            }
            catch (DbUpdateException)
            {
                return (false, "用户名冲突");
            }
            return (true, null);
        }


        public Task<DbModel.User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);//.FindAsync(new object[] { userId }, cancellationToken);
        }

        public Task<DbModel.User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return context.Users.Include(u => u.Roles).FirstOrDefaultAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);
        }


        public Task<DbModel.User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return context.Users.Include(u => u.Roles).FirstOrDefaultAsync(user => user.NormalizedEmail == normalizedEmail, cancellationToken);
        }
        public void Dispose()
        {
            context.Dispose();
        }

        public Task AddToRoleAsync(string userId, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userRole = new UserRole() { RoleId = roleName, User = new DbModel.User() { Id = userId } };
            context.Add(userRole);
            return context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveFromRoleAsync(string userId, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await FindByIdAsync(userId, cancellationToken);
            var role = user.Roles.FirstOrDefault(r => r.RoleId == roleName);
            if (null != user)
            {
                if (null != role)
                {
                    user.Roles.Remove(role);
                    context.Set<UserRole>().Remove(role);
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public Task<List<UserClaim>> GetClaims(string userId, CancellationToken cancellationToken)
        {
            return context.Set<UserClaim>().Where(c => c.UserId == userId).ToListAsync(cancellationToken);
        }

        public Task AddClaims(IEnumerable<UserClaim> claims, CancellationToken cancellationToken)
        {
            context.Set<UserClaim>().AddRange(claims);
            return context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTag(long id, string tag)
        {
            using (var tx = context.Database.BeginTransaction())
            {
                var model = context.Set<UserTag>().FirstOrDefault(t => t.Id == id);
                if (null != model)
                {
                    model.Tag = tag;
                    model.Version++;
                    context.Update(model);
                    context.SaveChanges();
                    //await context.SaveChangesAsync();
                }
                tx.Commit();
            }
        }

    }
}
