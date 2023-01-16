using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnionSozluk.Api.Domain.Models;
using OnionSozluk.Infrastructure.Persistence.Context;

namespace OnionSozluk.Infrastructure.Persistence.EntityConfigurations.EntryComment
{
    public class EntryCommentFavoriteEntityConfiguration : BaseEntityConfiguration<EntryCommentFavorite>
    {
        public override void Configure(EntityTypeBuilder<EntryCommentFavorite> builder)
        {
            base.Configure(builder);

            builder.ToTable("EntryCommentFavorites", OnionSozlukContext.DEFAULT_SCHEMA);


            builder.HasOne(i => i.EntryComment)
                .WithMany(i => i.EntryCommentFavorites)
                .HasForeignKey(i => i.EntryCommentId);

            builder.HasOne(i => i.CreatedBy)
                .WithMany(i => i.EntryCommentFavorites)
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

/*
 Cascade (kademeli): Eğer ilişkili ana satır (parent row) silinirse yada güncellenirse bağlı olduğu alt satır (child row) da silinir yada güncellenir.

Restrict (sınırlama): Fakat Restrict kullanırsanız o zaman ana satır (parent row) silinirse yada güncellenirse bağlı olduğu alt satır (child row) silinmez yada güncellenmez. Bir hata mesajı (error) verir. Örneğin "Silmeye çalıştığınız satırdaki .... alanı başka bir tabloda kullanılıyor". Bu hata işlemini parent row silerseniz çıkar. Bağlı olduğu child row'u silseniz bir hata almazsınız ki olması gereken de budur.

Set Null (Boş): Parent row silinirse yada güncellenirse child row'a Boş (Null) değeri atanır.

No Action (Hiçbirşey yapma): Hiç bir işlem yapmaz. Yani parent row silinse de güncellense de child row da bir işlem yapılmaz.
 */