using ShoeShop.Models;

namespace ShoeShop.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> getContactById(int id);
        Task<Contact> getContactByIdNoTracking(int id);
        Task<IEnumerable<Contact>> getContacts();
        bool update(Contact contact);
        bool delete(Contact contact);
        bool add(Contact contact);
        bool save();

    }
}
