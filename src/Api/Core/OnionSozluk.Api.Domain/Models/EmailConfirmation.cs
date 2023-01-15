namespace OnionSozluk.Api.Domain.Models
{
    public class EmailConfirmation:BaseEntity
    {
        public string OldEmailAddress { get; set; }
        public string NewEmalAddress { get; set; }
    }
}
