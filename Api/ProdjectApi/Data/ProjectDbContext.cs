using Microsoft.EntityFrameworkCore;
using ProdjectApi.Domain.Models;

namespace ProdjectApi.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Currencies> Currencies { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<BandMember> BandMembers { get; set; }  
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<ExpenseParticipants> ExpenseParticipants {  get; set; }
        public DbSet<Debt> Debts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //верификация почты
            modelBuilder.Entity<EmailVerification>(entity =>
            {
                entity.ToTable("email_verification");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(6);
                entity.Property(e => e.ExpiresAt).IsRequired();
                entity.Property(e => e.IsUsed).IsRequired();

                entity.HasOne<Users>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            //комната
            modelBuilder.Entity<Rooms>()
               .HasOne(r => r.Currency)
               .WithMany()
               .HasForeignKey(r => r.CurrenciesId);

            modelBuilder.Entity<Rooms>()
                .HasOne(r => r.CreatorUser)
                .WithMany()
                .HasForeignKey(r => r.Creator);
            // связь юзера и комнаты
            modelBuilder.Entity<BandMember>(entity =>
            {
                entity.ToTable("band_members");
                entity.HasOne<BandMember>()
                    .WithMany()
                    .HasForeignKey(bm => bm.UsersId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne<BandMember>()
                    .WithMany()
                    .HasForeignKey(bm => bm.RoomsId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            // расходы
            modelBuilder.Entity<Expenses>()
                .ToTable("expenses")
                .Property(x => x.Name)
                .HasColumnName("name");

            modelBuilder.Entity<Expenses>()
                .Property(x => x.ReceiptAmount)
                .HasColumnName("receipt_amount");

            modelBuilder.Entity<Expenses>()
                .Property(x => x.PaymentDate)
                .HasColumnName("payment_date");

            modelBuilder.Entity<Expenses>()
                .Property(x => x.PayerId)
                .HasColumnName("payer_id");

            modelBuilder.Entity<Expenses>()
                .Property(x => x.DistributionType)
                .HasColumnName("distribution_type");

            modelBuilder.Entity<Expenses>()
                .Property(x => x.RoomsId)
                .HasColumnName("rooms_id");

            modelBuilder.Entity<Expenses>()
                .Property(x => x.CurrenciesId)
                .HasColumnName("currencies_id");
            // участники расходов
            modelBuilder.Entity<ExpenseParticipants>().HasKey(ep => ep.Id);

            modelBuilder.Entity<ExpenseParticipants>()
                .HasOne(ep => ep.Expense)
                .WithMany(e => e.Participants)
                .HasForeignKey(ep => ep.ExpensesId);

            modelBuilder.Entity<ExpenseParticipants>()
                .HasOne(ep => ep.User)
                .WithMany(u => u.ExpenseParticipants)
                .HasForeignKey(ep => ep.UsersId);

            modelBuilder.Entity<ExpenseParticipants>()
                .Property(ep => ep.TotalDebt)
                .HasColumnType("decimal(10,2)");
            //долги
            modelBuilder.Entity<Debt>().HasKey(d => d.Id);

            modelBuilder.Entity<Debt>()
                .HasOne(d => d.Room)
                .WithMany(r => r.Debts)
                .HasForeignKey(d => d.RoomsId);

            modelBuilder.Entity<Debt>()
                .HasOne(d => d.Debtor)
                .WithMany(u => u.DebtsAsDebtor)
                .HasForeignKey(d => d.DebtorId);

            modelBuilder.Entity<Debt>()
                .HasOne(d => d.Lender)
                .WithMany(u => u.DebtsAsLender)
                .HasForeignKey(d => d.LenderId);

            modelBuilder.Entity<Debt>()
                .Property(d => d.Amount)
                .HasColumnType("decimal(10,2)");
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
