using Microsoft.EntityFrameworkCore;
using ShoeShop.Dao;
using ShoeShop.Interfaces;
using ShoeShop.Models;

namespace ShoeShop.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext context;

        public ContactRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public bool add(Contact contact)
        {
            context.Contacts.Add(contact);
            return save();
        }

        public bool delete(Contact contact)
        {
            context.Contacts.Remove(contact);
            return save();
        }

        public async Task<Contact> getContactById(int id)
        {
            return await context.Contacts.FindAsync(id);
        }

        public async Task<Contact> getContactByIdNoTracking(int id)
        {
            return await context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Contact>> getContacts()
        {
            return await context.Contacts.ToListAsync();
        }

        public bool save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ? true : false;
        }



        public bool update(Contact contact)
        {
            context.Contacts.Update(contact);
            return save();
        }
    }
}
