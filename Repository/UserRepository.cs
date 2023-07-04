using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Dao;
using ShoeShop.Interfaces;
using ShoeShop.Models;

namespace ShoeShop.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public bool Add(User user)
        {
            context.Add(user);
            return save();
        }

        public async Task<bool> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return true;
                }

            }


            return false;
        }


        public async Task<IEnumerable<User>> getUsers()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<User> getUserById(string id)
        {
            return await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> getUserByIdNoTracking(string id)
        {
            return await userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public bool save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(User user)
        {
            context.Users.Update(user);
            return save();
        }

        public Task<User> getUserByEmail(string email)
        {
            var user = userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return user;
            }
            return null;
        }
    }

}
