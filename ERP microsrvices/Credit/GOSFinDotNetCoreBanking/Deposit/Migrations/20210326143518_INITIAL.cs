using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deposit.Migrations
{
    public partial class INITIAL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(maxLength: 550, nullable: false),
                    CustomerTypeId = table.Column<int>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    SecurityAnswered = table.Column<string>(maxLength: 256, nullable: true),
                    IsItQuestionTime = table.Column<bool>(nullable: false),
                    EnableAtThisTime = table.Column<DateTime>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmEmailCode",
                columns: table => new
                {
                    ConfirmEmailCodeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    ConfirnamationTokenCode = table.Column<string>(nullable: true),
                    IssuedDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmEmailCode", x => x.ConfirmEmailCodeId);
                });

            migrationBuilder.CreateTable(
                name: "cor_approvaldetail",
                columns: table => new
                {
                    ApprovalDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    StaffId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ArrivalDate = table.Column<DateTime>(nullable: true),
                    TargetId = table.Column<int>(nullable: false),
                    WorkflowToken = table.Column<string>(nullable: true),
                    ReferredStaffId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_approvaldetail", x => x.ApprovalDetailId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_accountreactivation",
                columns: table => new
                {
                    ReactivationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    Substructure = table.Column<int>(nullable: true),
                    AccountName = table.Column<string>(maxLength: 50, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    AccountBalance = table.Column<decimal>(nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    Balance = table.Column<decimal>(nullable: true),
                    Reason = table.Column<string>(maxLength: 50, nullable: true),
                    Charges = table.Column<string>(maxLength: 50, nullable: true),
                    ApproverName = table.Column<string>(maxLength: 50, nullable: true),
                    ApproverComment = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_accountreactivation", x => x.ReactivationId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_accountreactivationsetup",
                columns: table => new
                {
                    ReactivationSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    ChargesApplicable = table.Column<bool>(nullable: true),
                    Charge = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    ChargeType = table.Column<string>(maxLength: 50, nullable: true),
                    PresetChart = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_accountreactivationsetup", x => x.ReactivationSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_accountype",
                columns: table => new
                {
                    AccountTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    AccountNunmberPrefix = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_accountype", x => x.AccountTypeId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_bankclosure",
                columns: table => new
                {
                    BankClosureId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    SubStructure = table.Column<int>(nullable: true),
                    AccountName = table.Column<string>(maxLength: 50, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AccountBalance = table.Column<string>(maxLength: 50, nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    ClosingDate = table.Column<DateTime>(type: "date", nullable: true),
                    Reason = table.Column<string>(maxLength: 50, nullable: true),
                    Charges = table.Column<decimal>(nullable: true),
                    FinalSettlement = table.Column<string>(maxLength: 50, nullable: true),
                    Beneficiary = table.Column<string>(maxLength: 50, nullable: true),
                    ModeOfSettlement = table.Column<int>(nullable: false),
                    SettlmentAccountNumber = table.Column<string>(nullable: true),
                    TransferAccount = table.Column<string>(maxLength: 50, nullable: true),
                    AccountId = table.Column<int>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    WorkflowToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_bankclosure", x => x.BankClosureId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_bankclosuresetup",
                columns: table => new
                {
                    BankClosureSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ClosureChargeApplicable = table.Column<bool>(nullable: true),
                    Charge = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    ChargeType = table.Column<string>(maxLength: 50, nullable: true),
                    SettlementBalance = table.Column<bool>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    Percentage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_bankclosuresetup", x => x.BankClosureSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_businesscategory",
                columns: table => new
                {
                    BusinessCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_businesscategory", x => x.BusinessCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_call_over_currecies_and_amount",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Currency = table.Column<long>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    User_id = table.Column<string>(nullable: true),
                    Call_over_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_call_over_currecies_and_amount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "deposit_cashierteller_form",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: false),
                    SubStructure = table.Column<string>(nullable: true),
                    Employee_ID = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Transaction_IDs = table.Column<string>(nullable: true),
                    Approval_status = table.Column<int>(nullable: false),
                    WorkflowToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_cashierteller_form", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "deposit_category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_changeofrates",
                columns: table => new
                {
                    ChangeOfRateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    CurrentRate = table.Column<decimal>(nullable: true),
                    ProposedRate = table.Column<decimal>(nullable: true),
                    Reasons = table.Column<string>(maxLength: 500, nullable: true),
                    WorkflowToken = table.Column<string>(nullable: true),
                    ApprovalStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_changeofrates", x => x.ChangeOfRateId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customer_lite_information",
                columns: table => new
                {
                    CustomerId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CustomerTypeId = table.Column<int>(nullable: false),
                    IndividualCustomerId = table.Column<long>(nullable: false),
                    KycId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customer_lite_information", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_directors",
                columns: table => new
                {
                    DirectorsId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    AccountName = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    OtherNames = table.Column<string>(nullable: true),
                    MaritalStatus = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: false),
                    POB = table.Column<string>(nullable: true),
                    MotherMaidienName = table.Column<string>(nullable: true),
                    NameOfNextOfKin = table.Column<string>(nullable: true),
                    LGA = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    TaxIdentitfication = table.Column<string>(nullable: true),
                    MeansOfIdentification = table.Column<int>(nullable: false),
                    IdentificationNumber = table.Column<string>(nullable: true),
                    IDIssuedate = table.Column<DateTime>(nullable: false),
                    Occupation = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    ResidentPermitNumber = table.Column<string>(nullable: true),
                    PermitIssueDate = table.Column<string>(nullable: true),
                    PermitExpiryDate = table.Column<string>(nullable: true),
                    SocialSecurityNumber = table.Column<string>(nullable: true),
                    ResidentialLGA = table.Column<string>(nullable: true),
                    ResidentialCity = table.Column<string>(nullable: true),
                    ResidentialState = table.Column<string>(nullable: true),
                    MailingAddressSameWithResidentialAddress = table.Column<bool>(nullable: false),
                    MailingLGA = table.Column<string>(nullable: true),
                    MailingCity = table.Column<string>(nullable: true),
                    MailingState = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    SignatureName = table.Column<string>(nullable: true),
                    SignatureFile = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_directors", x => x.DirectorsId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_file_uploads",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    FileGuid = table.Column<string>(nullable: true),
                    FileByte = table.Column<byte[]>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FullPath = table.Column<string>(nullable: true),
                    DbPath = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Extention = table.Column<string>(nullable: true),
                    TargetId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_file_uploads", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "deposit_form",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    Structure = table.Column<int>(nullable: false),
                    TransactionId = table.Column<string>(nullable: true),
                    Account_number = table.Column<string>(nullable: true),
                    Currency = table.Column<long>(nullable: false),
                    Deposit_amount = table.Column<decimal>(nullable: false),
                    Value_date = table.Column<DateTime>(nullable: false),
                    Transaction_particulars = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Transaction_mode = table.Column<int>(nullable: false),
                    Instrument_number = table.Column<string>(nullable: true),
                    Instrument_date = table.Column<DateTime>(nullable: false),
                    Is_call_over_done = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_form", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "deposit_reactivation_form",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    Product = table.Column<int>(nullable: false),
                    Account_number = table.Column<string>(nullable: true),
                    Structure = table.Column<int>(nullable: true),
                    Substructure = table.Column<int>(nullable: true),
                    Charges = table.Column<decimal>(nullable: false),
                    Available_balance = table.Column<decimal>(nullable: false),
                    Reactivation_reason = table.Column<string>(nullable: true),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    WorkflowToken = table.Column<string>(nullable: true),
                    ReactivationSetupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_reactivation_form", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "deposit_selectedTransactioncharge",
                columns: table => new
                {
                    SelectedTransChargeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    TransactionChargeId = table.Column<int>(nullable: true),
                    AccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_selectedTransactioncharge", x => x.SelectedTransChargeId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_selectedTransactiontax",
                columns: table => new
                {
                    SelectedTransTaxId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    TransactionTaxId = table.Column<int>(nullable: true),
                    AccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_selectedTransactiontax", x => x.SelectedTransTaxId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_tillvaultform",
                columns: table => new
                {
                    TillVaultId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    TillId = table.Column<int>(nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    OpeningBalance = table.Column<decimal>(nullable: true),
                    IncomingCash = table.Column<decimal>(nullable: true),
                    OutgoingCash = table.Column<decimal>(nullable: true),
                    ClosingBalance = table.Column<decimal>(nullable: true),
                    CashAvailable = table.Column<decimal>(nullable: true),
                    Shortage = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_tillvaultform", x => x.TillVaultId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_tillvaultsetup",
                columns: table => new
                {
                    TillVaultSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    StructureTillIdPrefix = table.Column<string>(maxLength: 50, nullable: true),
                    TellerTillIdPrefix = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_tillvaultsetup", x => x.TillVaultSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transactioncharge",
                columns: table => new
                {
                    TransactionChargeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    FixedOrPercentage = table.Column<string>(maxLength: 50, nullable: true),
                    Amount_Percentage = table.Column<decimal>(nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transactioncharge", x => x.TransactionChargeId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transactioncorrectionsetup",
                columns: table => new
                {
                    TransactionCorrectionSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    JobTitleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transactioncorrectionsetup", x => x.TransactionCorrectionSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transactiontax",
                columns: table => new
                {
                    TransactionTaxId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    FixedOrPercentage = table.Column<string>(maxLength: 50, nullable: true),
                    Amount_Percentage = table.Column<decimal>(nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transactiontax", x => x.TransactionTaxId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transferform",
                columns: table => new
                {
                    TransferFormId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    ValueDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExternalReference = table.Column<string>(maxLength: 50, nullable: true),
                    TransactionReference = table.Column<string>(maxLength: 50, nullable: true),
                    PayingAccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    PayingAccountName = table.Column<string>(maxLength: 50, nullable: true),
                    PayingAccountCurrency = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    BeneficiaryAccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    BeneficiaryAccountName = table.Column<string>(maxLength: 50, nullable: true),
                    BeneficiaryAccountCurrency = table.Column<string>(maxLength: 50, nullable: true),
                    TransactionNarration = table.Column<string>(maxLength: 50, nullable: true),
                    ExchangeRate = table.Column<decimal>(nullable: true),
                    TotalCharge = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transferform", x => x.TransferFormId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transfersetup",
                columns: table => new
                {
                    TransferSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    AccountType = table.Column<int>(nullable: true),
                    DailyWithdrawalLimit = table.Column<string>(maxLength: 50, nullable: true),
                    ChargesApplicable = table.Column<bool>(nullable: true),
                    Charges = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    ChargeType = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transfersetup", x => x.TransferSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_withdrawal_form",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    Structre = table.Column<int>(nullable: false),
                    Currency = table.Column<long>(nullable: false),
                    Product = table.Column<int>(nullable: false),
                    Is_call_over_done = table.Column<bool>(nullable: false),
                    Transaction_Id = table.Column<string>(nullable: true),
                    Account_number = table.Column<string>(nullable: true),
                    Widthrawal_setup = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Transaction_description = table.Column<string>(nullable: true),
                    Transaction_date = table.Column<DateTime>(nullable: false),
                    Value_date = table.Column<DateTime>(nullable: false),
                    Withdrawal_type = table.Column<int>(nullable: false),
                    Withdrawal_instrument = table.Column<string>(nullable: true),
                    Instrument_date = table.Column<DateTime>(nullable: false),
                    Total_charge = table.Column<decimal>(nullable: false),
                    Available_balance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_withdrawal_form", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "deposit_withdrawalform",
                columns: table => new
                {
                    WithdrawalFormId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    TransactionReference = table.Column<string>(maxLength: 50, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    AccountType = table.Column<int>(nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    TransactionDescription = table.Column<string>(maxLength: 50, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    ValueDate = table.Column<DateTime>(type: "date", nullable: true),
                    WithdrawalType = table.Column<string>(maxLength: 50, nullable: true),
                    InstrumentNumber = table.Column<string>(maxLength: 50, nullable: true),
                    InstrumentDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExchangeRate = table.Column<decimal>(nullable: true),
                    TotalCharge = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_withdrawalform", x => x.WithdrawalFormId);
                });

            migrationBuilder.CreateTable(
                name: "OTPTracker",
                columns: table => new
                {
                    OTPId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    OTP = table.Column<string>(nullable: true),
                    DateIssued = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPTracker", x => x.OTPId);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Token = table.Column<string>(nullable: false),
                    JwtId = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    Used = table.Column<bool>(nullable: false),
                    Invalidated = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_accountsetup",
                columns: table => new
                {
                    DepositAccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    AccountName = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    AccountTypeId = table.Column<int>(nullable: false),
                    InitialDeposit = table.Column<decimal>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    InterestRate = table.Column<decimal>(nullable: true),
                    InterestType = table.Column<string>(maxLength: 50, nullable: true),
                    CheckCollecting = table.Column<bool>(nullable: true),
                    MaturityType = table.Column<string>(maxLength: 50, nullable: true),
                    PreTerminationLiquidationCharge = table.Column<bool>(nullable: true),
                    InterestAccrual = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: true),
                    OperatedByAnother = table.Column<bool>(nullable: true),
                    CanNominateBenefactor = table.Column<bool>(nullable: true),
                    UsePresetChartofAccount = table.Column<bool>(nullable: true),
                    TransactionPrefix = table.Column<string>(maxLength: 50, nullable: true),
                    CancelPrefix = table.Column<string>(maxLength: 50, nullable: true),
                    RefundPrefix = table.Column<string>(maxLength: 50, nullable: true),
                    Useworkflow = table.Column<bool>(nullable: true),
                    CanPlaceOnLien = table.Column<bool>(nullable: true),
                    CurrencyId = table.Column<long>(nullable: true),
                    DormancyDays = table.Column<int>(nullable: false),
                    InUse = table.Column<bool>(nullable: true),
                    DomancyDateCount = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_accountsetup", x => x.DepositAccountId);
                    table.ForeignKey(
                        name: "FK_deposit_accountsetup_deposit_accountype_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "deposit_accountype",
                        principalColumn: "AccountTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_deposit_accountsetup_deposit_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "deposit_category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_corporate_customer_information",
                columns: table => new
                {
                    CorporateCustomerId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    CertOfIncorporationNumber = table.Column<string>(nullable: true),
                    DateOfIncorporation = table.Column<DateTime>(nullable: true),
                    JurisdictionOfincorporatoin = table.Column<string>(nullable: true),
                    NatureOfBusiness = table.Column<string>(nullable: true),
                    SectorOrIndustry = table.Column<string>(nullable: true),
                    OperatingAdress1 = table.Column<string>(nullable: true),
                    OperatingAdress2 = table.Column<string>(nullable: true),
                    RegisteredAddress = table.Column<string>(nullable: true),
                    LGA = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    TaxIdentificationNumber = table.Column<string>(nullable: true),
                    SCUML = table.Column<string>(nullable: true),
                    CustomerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_corporate_customer_information", x => x.CorporateCustomerId);
                    table.ForeignKey(
                        name: "FK_deposit_corporate_customer_information_deposit_customer_lite_information_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "deposit_customer_lite_information",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customer_account_information",
                columns: table => new
                {
                    AccountInformationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    AccountNumber = table.Column<string>(nullable: true),
                    Date_to_go_dormant = table.Column<DateTime>(nullable: false),
                    AvailableBalance = table.Column<string>(nullable: true),
                    InternetBanking = table.Column<bool>(nullable: false),
                    EmailStatement = table.Column<bool>(nullable: false),
                    Card = table.Column<bool>(nullable: false),
                    SmsAlert = table.Column<bool>(nullable: false),
                    EmailAlert = table.Column<bool>(nullable: false),
                    Token = table.Column<bool>(nullable: false),
                    AccountTypeId = table.Column<int>(nullable: false),
                    Currencies = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    CustomerTypeId = table.Column<int>(nullable: false),
                    RelationshipOfficerId = table.Column<int>(nullable: true),
                    CustomerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customer_account_information", x => x.AccountInformationId);
                    table.ForeignKey(
                        name: "FK_deposit_customer_account_information_deposit_accountype_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "deposit_accountype",
                        principalColumn: "AccountTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_deposit_customer_account_information_deposit_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "deposit_category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_deposit_customer_account_information_deposit_customer_lite_information_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "deposit_customer_lite_information",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customer_kyc",
                columns: table => new
                {
                    kycId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    SociallyOrFinanciallyDisadvantaged = table.Column<bool>(nullable: false),
                    OtherDocumentsObtained = table.Column<string>(nullable: true),
                    DoesTheCustomerEnjoyTieredKYC = table.Column<bool>(nullable: false),
                    RiskCategory = table.Column<string>(nullable: true),
                    IsCustomerPoliticalyExposed = table.Column<bool>(nullable: false),
                    PoliticalyExposedDetails = table.Column<string>(nullable: true),
                    AddressVisited = table.Column<string>(nullable: true),
                    CommentOnLocation = table.Column<string>(nullable: true),
                    Location_ColorOfbuilding = table.Column<string>(nullable: true),
                    Location_DescriptionOfBuilding = table.Column<string>(nullable: true),
                    FullNameOfVisitingStaff = table.Column<string>(nullable: true),
                    DateOfVisitation = table.Column<DateTime>(nullable: false),
                    isUtilityBillSubmitted = table.Column<bool>(nullable: false),
                    DulyCompletedAccountOpenningForm = table.Column<bool>(nullable: false),
                    RecentPassportPhotograph = table.Column<bool>(nullable: false),
                    Confirmed = table.Column<bool>(nullable: false),
                    Confirmaiotnname = table.Column<string>(nullable: true),
                    ConfirmationDate = table.Column<DateTime>(nullable: false),
                    DeferralDate = table.Column<DateTime>(nullable: false),
                    NameOfDocument = table.Column<string>(nullable: true),
                    DocumentUploadDate = table.Column<DateTime>(nullable: false),
                    DocumentPath = table.Column<string>(nullable: true),
                    DeferralFullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customer_kyc", x => x.kycId);
                    table.ForeignKey(
                        name: "FK_deposit_customer_kyc_deposit_customer_lite_information_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "deposit_customer_lite_information",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customer_signatories",
                columns: table => new
                {
                    SignatoriesId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    AccountName = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    OtherNames = table.Column<string>(nullable: true),
                    ClassOfSignatory = table.Column<int>(nullable: false),
                    IdentificationType = table.Column<int>(nullable: false),
                    IdentificationNumber = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    SignatureFile = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customer_signatories", x => x.SignatoriesId);
                    table.ForeignKey(
                        name: "FK_deposit_customer_signatories_deposit_customer_lite_information_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "deposit_customer_lite_information",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deposit_customer_thumbs",
                columns: table => new
                {
                    ThumbId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Extention = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    CustomerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposit_customer_thumbs", x => x.ThumbId);
                    table.ForeignKey(
                        name: "FK_Deposit_customer_thumbs_deposit_customer_lite_information_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "deposit_customer_lite_information",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_individual_customer_information",
                columns: table => new
                {
                    IndividualCustomerId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Title = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(maxLength: 100, nullable: true),
                    Firstname = table.Column<string>(maxLength: 100, nullable: true),
                    Othername = table.Column<string>(maxLength: 50, nullable: true),
                    MaritalStatusId = table.Column<int>(nullable: true),
                    GenderId = table.Column<int>(nullable: true),
                    BirthCountryId = table.Column<long>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    MotherMaidenName = table.Column<string>(maxLength: 50, nullable: true),
                    TaxIDNumber = table.Column<string>(maxLength: 200, nullable: true),
                    BVN = table.Column<string>(maxLength: 100, nullable: true),
                    Nationality = table.Column<long>(nullable: true),
                    ResidentPermitNumber = table.Column<string>(maxLength: 50, nullable: true),
                    PermitIssueDate = table.Column<DateTime>(nullable: true),
                    PermitExpiryDate = table.Column<DateTime>(nullable: true),
                    SocialSecurityNumber = table.Column<string>(maxLength: 100, nullable: true),
                    StateOfOrigin = table.Column<long>(nullable: false),
                    LGA_of_origin = table.Column<string>(nullable: true),
                    PhoneNo = table.Column<string>(nullable: true),
                    ResidentialLGA = table.Column<string>(nullable: true),
                    ResidentialState = table.Column<long>(nullable: false),
                    ResidentialCity = table.Column<long>(nullable: false),
                    ContactDetailId = table.Column<long>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_individual_customer_information", x => x.IndividualCustomerId);
                    table.ForeignKey(
                        name: "FK_deposit_individual_customer_information_deposit_customer_lite_information_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "deposit_customer_lite_information",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account_setup_transaction_charges",
                columns: table => new
                {
                    Account_setup_transaction_chargeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositAccountId = table.Column<int>(nullable: false),
                    TransactionChargeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_setup_transaction_charges", x => x.Account_setup_transaction_chargeID);
                    table.ForeignKey(
                        name: "FK_Account_setup_transaction_charges_deposit_accountsetup_DepositAccountId",
                        column: x => x.DepositAccountId,
                        principalTable: "deposit_accountsetup",
                        principalColumn: "DepositAccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Account_setup_transaction_charges_deposit_transactioncharge_TransactionChargeId",
                        column: x => x.TransactionChargeId,
                        principalTable: "deposit_transactioncharge",
                        principalColumn: "TransactionChargeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account_setup_transaction_tax",
                columns: table => new
                {
                    Account_setup_transaction_taxID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositAccountId = table.Column<int>(nullable: false),
                    TransactionTaxId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_setup_transaction_tax", x => x.Account_setup_transaction_taxID);
                    table.ForeignKey(
                        name: "FK_Account_setup_transaction_tax_deposit_accountsetup_DepositAccountId",
                        column: x => x.DepositAccountId,
                        principalTable: "deposit_accountsetup",
                        principalColumn: "DepositAccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Account_setup_transaction_tax_deposit_transactiontax_TransactionTaxId",
                        column: x => x.TransactionTaxId,
                        principalTable: "deposit_transactiontax",
                        principalColumn: "TransactionTaxId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_cashiertellersetup",
                columns: table => new
                {
                    DepositCashierTellerSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: false),
                    Sub_strructure = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    Employee_ID = table.Column<long>(nullable: false),
                    PresetChart = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_cashiertellersetup", x => x.DepositCashierTellerSetupId);
                    table.ForeignKey(
                        name: "FK_deposit_cashiertellersetup_deposit_accountsetup_ProductId",
                        column: x => x.ProductId,
                        principalTable: "deposit_accountsetup",
                        principalColumn: "DepositAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_changeofratesetup",
                columns: table => new
                {
                    ChangeOfRateSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    CanApply = table.Column<bool>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_changeofratesetup", x => x.ChangeOfRateSetupId);
                    table.ForeignKey(
                        name: "FK_deposit_changeofratesetup_deposit_accountsetup_ProductId",
                        column: x => x.ProductId,
                        principalTable: "deposit_accountsetup",
                        principalColumn: "DepositAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_withdrawalsetup",
                columns: table => new
                {
                    WithdrawalSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    AccountType = table.Column<int>(nullable: true),
                    DailyWithdrawalLimit = table.Column<decimal>(nullable: true),
                    WithdrawalCharges = table.Column<bool>(nullable: true),
                    Charge = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    ChargeType = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_withdrawalsetup", x => x.WithdrawalSetupId);
                    table.ForeignKey(
                        name: "FK_deposit_withdrawalsetup_deposit_accountype_AccountType",
                        column: x => x.AccountType,
                        principalTable: "deposit_accountype",
                        principalColumn: "AccountTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_deposit_withdrawalsetup_deposit_accountsetup_Product",
                        column: x => x.Product,
                        principalTable: "deposit_accountsetup",
                        principalColumn: "DepositAccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customer_contact_detail",
                columns: table => new
                {
                    ContactDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    ResidentialAddressLine1 = table.Column<string>(nullable: true),
                    ResidentialAddressLine2 = table.Column<string>(nullable: true),
                    ResidentialCity = table.Column<string>(nullable: true),
                    ResidentialCountry = table.Column<long>(nullable: false),
                    ResidentialState = table.Column<long>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    MailngAddress = table.Column<string>(nullable: true),
                    IndividualCustomerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customer_contact_detail", x => x.ContactDetailId);
                    table.ForeignKey(
                        name: "FK_deposit_customer_contact_detail_deposit_individual_customer_information_IndividualCustomerId",
                        column: x => x.IndividualCustomerId,
                        principalTable: "deposit_individual_customer_information",
                        principalColumn: "IndividualCustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customerIdentification",
                columns: table => new
                {
                    IdentificationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    Identification = table.Column<int>(nullable: false),
                    IDNumber = table.Column<string>(nullable: true),
                    DateIssued = table.Column<DateTime>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    IndividualCustomerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customerIdentification", x => x.IdentificationId);
                    table.ForeignKey(
                        name: "FK_deposit_customerIdentification_deposit_individual_customer_information_IndividualCustomerId",
                        column: x => x.IndividualCustomerId,
                        principalTable: "deposit_individual_customer_information",
                        principalColumn: "IndividualCustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_employer_details",
                columns: table => new
                {
                    EmploymentDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    IsEmployed = table.Column<bool>(nullable: false),
                    IsSelfEmployed = table.Column<bool>(nullable: false),
                    IsUnEmployed = table.Column<bool>(nullable: false),
                    IsRetired = table.Column<bool>(nullable: false),
                    IsStudent = table.Column<bool>(nullable: false),
                    OtherComments = table.Column<string>(nullable: true),
                    EmployerName = table.Column<string>(nullable: true),
                    EmployerAddress = table.Column<string>(nullable: true),
                    EmployerState = table.Column<int>(nullable: true),
                    Occupation = table.Column<string>(nullable: true),
                    IndividualCustomerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_employer_details", x => x.EmploymentDetailId);
                    table.ForeignKey(
                        name: "FK_deposit_employer_details_deposit_individual_customer_information_IndividualCustomerId",
                        column: x => x.IndividualCustomerId,
                        principalTable: "deposit_individual_customer_information",
                        principalColumn: "IndividualCustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_nextofkin",
                columns: table => new
                {
                    NextOfKinId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    NextOfKinTitle = table.Column<string>(nullable: true),
                    NextOfKinSurname = table.Column<string>(nullable: true),
                    NextOfKinFirstName = table.Column<string>(nullable: true),
                    NextOfKinOtherNames = table.Column<string>(nullable: true),
                    NextOfKinDateOfBirth = table.Column<string>(nullable: true),
                    NextOfKinGender = table.Column<string>(nullable: true),
                    NextOfKinRelationship = table.Column<string>(nullable: true),
                    NextOfKinMobileNumber = table.Column<string>(nullable: true),
                    NextOfKinEmailAddress = table.Column<string>(nullable: true),
                    NextOfKinAddress = table.Column<string>(nullable: true),
                    NextOfKinCity = table.Column<string>(nullable: true),
                    NextOfKinState = table.Column<int>(nullable: false),
                    IndividualCustomerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_nextofkin", x => x.NextOfKinId);
                    table.ForeignKey(
                        name: "FK_deposit_nextofkin_deposit_individual_customer_information_IndividualCustomerId",
                        column: x => x.IndividualCustomerId,
                        principalTable: "deposit_individual_customer_information",
                        principalColumn: "IndividualCustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_signatures",
                columns: table => new
                {
                    SignatureId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    IdentificationId = table.Column<int>(nullable: false),
                    SignatureOrMarkName = table.Column<string>(nullable: true),
                    SignatureOrMarkUpload = table.Column<byte[]>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Deposit_CustomerIdentificationIdentificationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_signatures", x => x.SignatureId);
                    table.ForeignKey(
                        name: "FK_deposit_signatures_deposit_customerIdentification_Deposit_CustomerIdentificationIdentificationId",
                        column: x => x.Deposit_CustomerIdentificationIdentificationId,
                        principalTable: "deposit_customerIdentification",
                        principalColumn: "IdentificationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_setup_transaction_charges_DepositAccountId",
                table: "Account_setup_transaction_charges",
                column: "DepositAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_setup_transaction_charges_TransactionChargeId",
                table: "Account_setup_transaction_charges",
                column: "TransactionChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_setup_transaction_tax_DepositAccountId",
                table: "Account_setup_transaction_tax",
                column: "DepositAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_setup_transaction_tax_TransactionTaxId",
                table: "Account_setup_transaction_tax",
                column: "TransactionTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_accountsetup_AccountTypeId",
                table: "deposit_accountsetup",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_accountsetup_CategoryId",
                table: "deposit_accountsetup",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_cashiertellersetup_ProductId",
                table: "deposit_cashiertellersetup",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_changeofratesetup_ProductId",
                table: "deposit_changeofratesetup",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_corporate_customer_information_CustomerId",
                table: "deposit_corporate_customer_information",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customer_account_information_AccountTypeId",
                table: "deposit_customer_account_information",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customer_account_information_CategoryId",
                table: "deposit_customer_account_information",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customer_account_information_CustomerId",
                table: "deposit_customer_account_information",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customer_contact_detail_IndividualCustomerId",
                table: "deposit_customer_contact_detail",
                column: "IndividualCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customer_kyc_CustomerId",
                table: "deposit_customer_kyc",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customer_signatories_CustomerId",
                table: "deposit_customer_signatories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Deposit_customer_thumbs_CustomerId",
                table: "Deposit_customer_thumbs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customerIdentification_IndividualCustomerId",
                table: "deposit_customerIdentification",
                column: "IndividualCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_employer_details_IndividualCustomerId",
                table: "deposit_employer_details",
                column: "IndividualCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_individual_customer_information_CustomerId",
                table: "deposit_individual_customer_information",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_deposit_nextofkin_IndividualCustomerId",
                table: "deposit_nextofkin",
                column: "IndividualCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_signatures_Deposit_CustomerIdentificationIdentificationId",
                table: "deposit_signatures",
                column: "Deposit_CustomerIdentificationIdentificationId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_withdrawalsetup_AccountType",
                table: "deposit_withdrawalsetup",
                column: "AccountType");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_withdrawalsetup_Product",
                table: "deposit_withdrawalsetup",
                column: "Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account_setup_transaction_charges");

            migrationBuilder.DropTable(
                name: "Account_setup_transaction_tax");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ConfirmEmailCode");

            migrationBuilder.DropTable(
                name: "cor_approvaldetail");

            migrationBuilder.DropTable(
                name: "deposit_accountreactivation");

            migrationBuilder.DropTable(
                name: "deposit_accountreactivationsetup");

            migrationBuilder.DropTable(
                name: "deposit_bankclosure");

            migrationBuilder.DropTable(
                name: "deposit_bankclosuresetup");

            migrationBuilder.DropTable(
                name: "deposit_businesscategory");

            migrationBuilder.DropTable(
                name: "deposit_call_over_currecies_and_amount");

            migrationBuilder.DropTable(
                name: "deposit_cashierteller_form");

            migrationBuilder.DropTable(
                name: "deposit_cashiertellersetup");

            migrationBuilder.DropTable(
                name: "deposit_changeofrates");

            migrationBuilder.DropTable(
                name: "deposit_changeofratesetup");

            migrationBuilder.DropTable(
                name: "deposit_corporate_customer_information");

            migrationBuilder.DropTable(
                name: "deposit_customer_account_information");

            migrationBuilder.DropTable(
                name: "deposit_customer_contact_detail");

            migrationBuilder.DropTable(
                name: "deposit_customer_kyc");

            migrationBuilder.DropTable(
                name: "deposit_customer_signatories");

            migrationBuilder.DropTable(
                name: "Deposit_customer_thumbs");

            migrationBuilder.DropTable(
                name: "deposit_directors");

            migrationBuilder.DropTable(
                name: "deposit_employer_details");

            migrationBuilder.DropTable(
                name: "deposit_file_uploads");

            migrationBuilder.DropTable(
                name: "deposit_form");

            migrationBuilder.DropTable(
                name: "deposit_nextofkin");

            migrationBuilder.DropTable(
                name: "deposit_reactivation_form");

            migrationBuilder.DropTable(
                name: "deposit_selectedTransactioncharge");

            migrationBuilder.DropTable(
                name: "deposit_selectedTransactiontax");

            migrationBuilder.DropTable(
                name: "deposit_signatures");

            migrationBuilder.DropTable(
                name: "deposit_tillvaultform");

            migrationBuilder.DropTable(
                name: "deposit_tillvaultsetup");

            migrationBuilder.DropTable(
                name: "deposit_transactioncorrectionsetup");

            migrationBuilder.DropTable(
                name: "deposit_transferform");

            migrationBuilder.DropTable(
                name: "deposit_transfersetup");

            migrationBuilder.DropTable(
                name: "deposit_withdrawal_form");

            migrationBuilder.DropTable(
                name: "deposit_withdrawalform");

            migrationBuilder.DropTable(
                name: "deposit_withdrawalsetup");

            migrationBuilder.DropTable(
                name: "OTPTracker");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "deposit_transactioncharge");

            migrationBuilder.DropTable(
                name: "deposit_transactiontax");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "deposit_customerIdentification");

            migrationBuilder.DropTable(
                name: "deposit_accountsetup");

            migrationBuilder.DropTable(
                name: "deposit_individual_customer_information");

            migrationBuilder.DropTable(
                name: "deposit_accountype");

            migrationBuilder.DropTable(
                name: "deposit_category");

            migrationBuilder.DropTable(
                name: "deposit_customer_lite_information");
        }
    }
}
