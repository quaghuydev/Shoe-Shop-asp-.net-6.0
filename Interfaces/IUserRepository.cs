using ShoeShop.Models;

namespace ShoeShop.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> getUsers();
        Task<User> getUserById(string id);
        Task<User> getUserByIdNoTracking(string id);
        Task<User> getUserByEmail(string email);
        bool Add(User user);
        bool Update(User user);
        Task<bool> Delete(string id);
        bool save();
    }
}
