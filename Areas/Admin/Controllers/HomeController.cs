using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Dto;
using ShoeShop.Interfaces;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("admin"))]

    public class HomeController : Controller
    {
        private readonly IContactRepository contactRepository;
        private readonly IUserRepository userRepository;
        private readonly IEmailService emailService;

        public HomeController(IContactRepository contactRepository, IUserRepository userRepository, IEmailService emailService)
        {
            this.contactRepository = contactRepository;
            this.userRepository = userRepository;
            this.emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Contact()
        {
            IEnumerable<Contact> contacts = await contactRepository.getContacts();
            return View(contacts);
        }
        public async Task<IActionResult> Feedback(int id)
        {
            var contact = await contactRepository.getContactById(id);
            if (contact == null)
            {
                return View("Contact");
            }
            var contactDto = new ContactDto
            {
                Id = id,
                Email = contact.Email,
                Subject = contact.Subject,
                Content = contact.Content,
                Feedback = contact.Feedback,

            };

            return View(contactDto);
        }
        [HttpPost]

        public async Task<IActionResult> Feedback(ContactDto contactDto)
        {
            if (ModelState.IsValid)
            {
                var contact = await contactRepository.getContactById(contactDto.Id);
                if (contact == null)
                {
                    ModelState.AddModelError("", "phản hồi thất bại");
                    return View("Feedback");
                }

                contact.Feedback = contactDto.Feedback;
                contact.Status = Models.Enums.ContactStatus.Processed;
                contactRepository.update(contact);

                var user = await userRepository.getUserById(contact.UserId);
                if (user == null)
                {
                    ModelState.AddModelError("", "phản hồi thất bại");
                    return View("Feedback");
                }
                emailService.SendEmailAsync(contact.Email, user.UserName, contact.Subject, contact.Feedback);
                return RedirectToAction("Contact");
            }
            ModelState.AddModelError("", "phản hồi thất bại");
            return View("Feedback");
        }
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await contactRepository.getContactByIdNoTracking(id);
            if (contact == null)
            {
                return View("Contact");
            }
            contactRepository.delete(contact);
            return RedirectToAction("Contact");
        }
    }

}
