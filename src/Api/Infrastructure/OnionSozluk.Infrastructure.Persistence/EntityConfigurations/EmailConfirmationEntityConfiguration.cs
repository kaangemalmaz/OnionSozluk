using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnionSozluk.Api.Domain.Models;
using OnionSozluk.Infrastructure.Persistence.Context;

namespace OnionSozluk.Infrastructure.Persistence.EntityConfigurations
{
    public class EmailConfirmationEntityConfiguration : BaseEntityConfiguration<EmailConfirmation>
    {
        public override void Configure(EntityTypeBuilder<EmailConfirmation> builder)
        {
            base.Configure(builder);

            builder.ToTable("Emailconfirmations", OnionSozlukContext.DEFAULT_SCHEMA);
        }
    }
}
