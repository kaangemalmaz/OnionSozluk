using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnionSozluk.Api.Domain.Models;
using OnionSozluk.Common.Infrastructure;

namespace OnionSozluk.Infrastructure.Persistence.Context
{
    public class SeedData
    {
        // md5 geri dönülemez bir algoritma yaratıyor.
        private static List<User> GetUsers()
        {
            var result = new Faker<User>("tr")
                    .RuleFor(i => i.Id, i => Guid.NewGuid())
                    .RuleFor(i => i.CreateDate,
                                i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                    .RuleFor(i => i.FirstName, i => i.Person.FirstName)
                    .RuleFor(i => i.LastName, i => i.Person.LastName)
                    .RuleFor(i => i.EmailAddress, i => i.Internet.Email())
                    .RuleFor(i => i.UserName, i => i.Internet.UserName())
                    .RuleFor(i => i.Password, i => PasswordEncryptor.Encrpt(i.Internet.Password()))
                    .RuleFor(i => i.EmailConfirmed, i => i.PickRandom(true, false))
                .Generate(500);

            return result;
        }

        public async Task SeedAsync(IConfiguration configuration)
        {
            var dbContextBuilder = new DbContextOptionsBuilder();
            dbContextBuilder.UseSqlServer(configuration["OnionSozlukDbConnectionDbContext"]);
            var context = new OnionSozlukContext(dbContextBuilder.Options); // 2.const. çağırıyoruz.

            //burası eğer seed datayı kaldırmayı unutursan diye koruma amaçlı koyulmuştur.
            //if (context.Users.Any())
            //{
            //    await Task.CompletedTask;
            //    return;
            //}

            var users = GetUsers();
            var userIds = users.Select(i => i.Id); //burada userin idlerini çekiyoruz çünkü entry ve entrycommentde kullanacağız.

            await context.Users.AddRangeAsync(users);


            var guids = Enumerable.Range(0, 150).Select(i => Guid.NewGuid()).ToList();
            int counter = 0; // global counter yapılması

            var entries = new Faker<Entry>("tr")
                    .RuleFor(i => i.Id, i => guids[counter++]) //global counter sıra sıra atama yapar.
                    .RuleFor(i => i.CreateDate,
                                i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                    .RuleFor(i => i.Subject, i => i.Lorem.Sentence(5, 5)) // loremden 5 cümle al demektir.
                    .RuleFor(i => i.Content, i => i.Lorem.Paragraph(2)) // loremden 2 paragram al.
                    .RuleFor(i => i.CreatedById, i => i.PickRandom(userIds)) // herhangi bir userid yi ata
                .Generate(150);

            await context.Entries.AddRangeAsync(entries);

            var comments = new Faker<EntryComment>("tr")
                    .RuleFor(i => i.Id, i => Guid.NewGuid())
                    .RuleFor(i => i.CreateDate,
                                i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                    .RuleFor(i => i.Content, i => i.Lorem.Paragraph(2))
                    .RuleFor(i => i.CreatedById, i => i.PickRandom(userIds))
                    .RuleFor(i => i.EntryId, i => i.PickRandom(guids))
                .Generate(1000);

            await context.EntryComments.AddRangeAsync(comments);
            await context.SaveChangesAsync();
        }
    }
}
