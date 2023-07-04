namespace ShoeShop.Dto
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public string? Feedback { get; set; }
    }
}
