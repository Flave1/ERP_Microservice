using System;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.GeneralExtension;
using Deposit.DomainObjects;
using Deposit.DomainObjects.Approval;
using Deposit.DomainObjects.Auth;
using Deposit.DomainObjects.Deposit;
using GODP.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Deposit.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext() { }

        private readonly IHttpContextAccessor _accessor;
        public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor accessor)
            : base(options) { _accessor = accessor; }

        public DbSet<deposit_call_over_currecies_and_amount> deposit_call_over_currecies_and_amount { get; set; }
        public DbSet<deposit_withdrawal_form> deposit_withdrawal_form { get; set; }
        public DbSet<deposit_reactivation_form> deposit_reactivation_form { get; set; }
        public DbSet<deposit_form> deposit_form { get; set; }
        public DbSet<deposit_customer_accountdetails> deposit_customer_accountdetails { get; set; }
        public DbSet<deposit_nextofkin> deposit_nextofkin { get; set; }
        public DbSet<deposit_directors> deposit_directors { get; set; }
        public DbSet<deposit_keycontactpersons> deposit_keycontactpersons { get; set; }
        public DbSet<deposit_kyc> deposit_kyc { get; set; }
        public DbSet<deposit_signatories> deposit_signatories { get; set; }
        public DbSet<deposit_signatures> deposit_signatures { get; set; }
        public DbSet<deposit_file_uploads> deposit_file_uploads { get; set; }
        public DbSet<deposit_customerIdentifications> deposit_customerIdentification { get; set; }
        public DbSet<OTPTracker> OTPTracker { get; set; }
        public DbSet<ConfirmEmailCode> ConfirmEmailCode { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<deposit_accountopening> deposit_accountopening { get; set; }
        public DbSet<deposit_accountreactivation> deposit_accountreactivation { get; set; }
        public DbSet<deposit_accountreactivationsetup> deposit_accountreactivationsetup { get; set; }
        public DbSet<deposit_accountsetup> deposit_accountsetup { get; set; }
        public DbSet<deposit_accountype> deposit_accountype { get; set; }
        public DbSet<deposit_bankclosure> deposit_bankclosure { get; set; }
        public DbSet<deposit_bankclosuresetup> deposit_bankclosuresetup { get; set; }
        public DbSet<deposit_businesscategory> deposit_businesscategory { get; set; }
        public DbSet<deposit_cashierteller_form> deposit_cashierteller_form { get; set; }
        public DbSet<deposit_cashiertellersetup> deposit_cashiertellersetup { get; set; }
        public DbSet<deposit_category> deposit_category { get; set; }
        public DbSet<deposit_changeofrates> deposit_changeofrates { get; set; }
        public DbSet<deposit_changeofratesetup> deposit_changeofratesetup { get; set; }
        public DbSet<cor_approvaldetail> cor_approvaldetail { get; set; }
        public DbSet<deposit_withdrawalsetup> deposit_withdrawalsetup { get; set; }
        public DbSet<deposit_tillvaultsetup> deposit_tillvaultsetup { get; set; }
        public DbSet<deposit_transactioncorrectionsetup> deposit_transactioncorrectionsetup { get; set; }
        public DbSet<deposit_selectedTransactioncharge> deposit_selectedTransactioncharge { get; set; }
        public DbSet<deposit_selectedTransactiontax> deposit_selectedTransactiontax { get; set; }
        public DbSet<deposit_tillvaultform> deposit_tillvaultform { get; set; }
        public DbSet<deposit_transactioncharge> deposit_transactioncharge { get; set; }
        public DbSet<deposit_transactiontax> deposit_transactiontax { get; set; }
        public DbSet<deposit_transferform> deposit_transferform { get; set; }
        public DbSet<deposit_transfersetup> deposit_transfersetup { get; set; }
        public DbSet<deposit_withdrawalform> deposit_withdrawalform { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var userid = _accessor?.HttpContext?.User?.FindFirst(e => e.Type == "userId")?.Value ?? string.Empty;

            foreach (var entry in ChangeTracker.Entries<GeneralEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Active = true;
                    entry.Entity.Deleted = false;
                    entry.Entity.Active = false;
                    entry.Entity.CreatedBy = userid;
                    entry.Entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entry.Entity.UpdatedOn = DateTime.Now;
                    entry.Entity.UpdatedBy = userid;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var userid = _accessor?.HttpContext?.User?.FindFirst(e => e.Type == "userId")?.Value ?? string.Empty;

            foreach (var entry in ChangeTracker.Entries<GeneralEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Active = true;
                    entry.Entity.Deleted = false;
                    entry.Entity.Active = false;
                    entry.Entity.CreatedBy = userid;
                    entry.Entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entry.Entity.UpdatedOn = DateTime.Now;
                    entry.Entity.UpdatedBy = userid;
                }
            }
            return base.SaveChanges();
        }
    }
}
