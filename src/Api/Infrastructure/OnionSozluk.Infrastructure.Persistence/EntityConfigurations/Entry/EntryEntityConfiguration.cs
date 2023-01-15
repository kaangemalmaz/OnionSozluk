using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnionSozluk.Infrastructure.Persistence.Context;

namespace OnionSozluk.Infrastructure.Persistence.EntityConfigurations.Entry
{
    public class EntryEntityConfiguration : BaseEntityConfiguration<Api.Domain.Models.Entry>
    {
        public override void Configure(EntityTypeBuilder<Api.Domain.Models.Entry> builder)
        {
            base.Configure(builder);

            builder.ToTable("Entries", OnionSozlukContext.DEFAULT_SCHEMA);


            builder.HasOne(i => i.CreatedBy)
                .WithMany(i => i.Entries)
                .HasForeignKey(i => i.CreatedById);
        }
    }
}
