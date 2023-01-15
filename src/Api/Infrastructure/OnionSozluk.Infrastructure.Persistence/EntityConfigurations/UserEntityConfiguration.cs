using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnionSozluk.Api.Domain.Models;
using OnionSozluk.Infrastructure.Persistence.Context;

namespace OnionSozluk.Infrastructure.Persistence.EntityConfigurations
{
    public class UserEntityConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("Users", OnionSozlukContext.DEFAULT_SCHEMA);
        }
    }
}
