namespace OnionSozluk.Api.Domain.Models
{
    public class Entry : BaseEntity
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public Guid CreatedById { get; set; }


        // ilişkiler virtual kurulması gerekir. çünkü include ettiğinde hata verme olasılığını azaltırsın ve include etmesen de virtual olduğu için hata vermez.
        // https://stackoverflow.com/questions/5597760/what-effects-can-the-virtual-keyword-have-in-entity-framework-4-1-poco-code-fi
        // virtual lazy loading olarak yüklemeni sağlar.
        public virtual User CreatedBy { get; set; }
        public virtual ICollection<EntryComment> EntryComments { get; set; }
        public virtual ICollection<EntryVote> EntryVotes { get; set; }
        public virtual ICollection<EntryFavorite> EntryFavorites { get; set; }

    }
}
