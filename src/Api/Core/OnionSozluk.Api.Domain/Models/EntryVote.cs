using OnionSozluk.Common.ViewModels;

namespace OnionSozluk.Api.Domain.Models
{
    public class EntryVote : BaseEntity
    {
        public Guid EntryId { get; set; }
        public VoteType VoteType { get; set; }
        public Guid CreateById { get; set; }

        public virtual Entry Entry { get; set; }

    }
}
