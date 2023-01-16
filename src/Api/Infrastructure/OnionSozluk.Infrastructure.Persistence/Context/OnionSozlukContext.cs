using Microsoft.EntityFrameworkCore;
using OnionSozluk.Api.Domain.Models;
using System.Reflection;

namespace OnionSozluk.Infrastructure.Persistence.Context
{
    public class OnionSozlukContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "dbo";

        public OnionSozlukContext()
        {
            // 2.bir çağırma şekli üretmek için yapılır. Yani bir option olarak sunulur.
            // api ayağa kalmadan direk olarak persistence dan migration geçilmesi buna bir örnektir.
        }
        public OnionSozlukContext(DbContextOptions options) : base(options)
        {
            //dbcontextin const. doldurursun!
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Eğer biri dbcontexi parametresi constructor üretmek isterse ve o şekilde bağlanmak istersen diye yapılan işlemdir.
            // burada eğer configurasyon geçilmemişse bu şekilde configurasyon geçebilirsin anlamına gelmektedi.r
            if (!optionsBuilder.IsConfigured)
            {
                var connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OnionSozlukDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                optionsBuilder.UseSqlServer(connStr, opt =>
                {
                    opt.EnableRetryOnFailure(); // sql servera bağlanırken bir hata oluşursa yeniden denesin diye bir örnek!
                });
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<EmailConfirmation> EmailConfirmations { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<EntryFavorite> EntryFavorites { get; set; }
        public DbSet<EntryVote> EntryVotes { get; set; }
        public DbSet<EntryComment> EntryComments { get; set; }
        public DbSet<EntryCommentFavorite> EntryCommentFavorites { get; set; }
        public DbSet<EntryCommentVote> EntryCommentVotes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //buradaki configuration nesnelerinin tamamını apply eder.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            // buradaki işlem yapılarak geri savechangesinin geri kalan halinin normal çalışması sağlanabilir.
            OnBeforeSave();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSave();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSave();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSave()
        {
            // eklenen entityleri seç onlar object olarak gelir base entitye set et! çünkü create data baseentityde diğerleride ondan türüyor.
            var addedEntities = ChangeTracker.Entries()
                                .Where(i => i.State == EntityState.Added)
                                .Select(i => (BaseEntity)i.Entity);

            PrepareAddedEntities(addedEntities);
        }


        private void PrepareAddedEntities(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.CreateDate == DateTime.MinValue) //datetime da null olmadığından min value gelir o yüzden bu kontrol edilir.
                    entity.CreateDate = DateTime.Now;
            }
        }
    }
}
