using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Deposit.DomainObjects.Auth;
using Deposit.Repository.Interface.Deposit;
using Deposit.Repository.Implement.Deposit;
using Deposit.Requests;
using Deposit.Repository.Implement;
using Deposit.Handlers.Details;
using Deposit.Data;
using Deposit.Managers.Interface;
using Deposit.Repository.Interface;
using Deposit.Managers.Implement;
using Deposit.Managers.InterfaceManagers;

namespace Deposit.Installers
{
    public class DbInstaller : IInstaller
    { 
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                   options.UseSqlServer(
                       configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAccountInformationService, AccountInformationService>();

            services.AddScoped<IChangeOfRate, ChangeOfRate>();
            services.AddScoped<IDepositBankClosure, DepositBankClosure>();
            services.AddScoped<IWorkflowDetailService, WorkflowDetailService>();
            services.AddScoped<IDepositAccountypeService, DepositAccountypeService>();
            services.AddScoped<IBusinessCategoryService, BusinessCategoryService>();
            services.AddScoped<ICashierTellerService, CashierTellerService>();
            services.AddScoped<IDepositCategoryService, DepositCategoryService>();
            services.AddScoped<ITransactionChargeService, TransactionChargeService>();
            services.AddScoped<ITransactionTaxService, TransactionTaxService>();
            services.AddScoped<IAccountSetupService, AccountSetupService>(); 
            services.AddScoped<IChangeOfRatesService, ChangeOfRatesService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IWithdrawalService, WithdrawalService>();
            services.AddScoped<ITransferService, TransferService>();
 
            services.AddScoped<IIdentityServerRequest, IdentityServerRequest>();
         
            services.AddScoped<IFlutterWaveRequest, FlutterWaveRequest>();
 

            services.AddTransient<ICommonService, CommonService>();
            services.AddDefaultIdentity<ApplicationUser>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddAutoMapper(typeof(Startup)); 
            services.AddMediatR(typeof(Startup));
            services.AddMvc();  

            

        }
    }
}
