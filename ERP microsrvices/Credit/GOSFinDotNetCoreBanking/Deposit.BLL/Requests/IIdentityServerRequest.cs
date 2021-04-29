using Deposit.Contracts.Response;
using Deposit.Contracts.Response.Approvals;
using Deposit.Contracts.Response.Common;
using Deposit.Contracts.Response.Finance;
using Deposit.Contracts.Response.IdentityServer;
using Deposit.Contracts.Response.Mail;
using Deposit.DomainObjects.Auth;
using GOSLibraries.GOS_Financial_Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Deposit.Requests
{
    public interface IIdentityServerRequest
    {

        Task<AuthenticationResult> LoginAsync(string userName, string password);
        Task<UserDataResponseObj> UserDataAsync();
        Task<HttpResponseMessage> GotForApprovalAsync(GoForApprovalRequest request);
        Task<HttpResponseMessage> GetAnApproverItemsFromIdentityServer();
        Task<HttpResponseMessage> StaffApprovalRequestAsync(IndentityServerApprovalCommand request);
        Task<HttpResponseMessage> GetCanEditStatusFromIdentityServer();

        Task<CommonRespObj> GetCurrencyAsync();
        Task<CommonRespObj> GetGenderAsync();
        Task<CommonRespObj> GetMaritalStatusAsync();
        Task<CommonRespObj> GetEmploymentTypeAsync();
        Task<CommonRespObj> GetIdentiticationTypeAsync();
        Task<StaffRespObj> GetAllStaffAsync();
        Task<CompanyStructureRespObj> GetAllCompanyAsync();
        Task<OperationRespObj> GetAllOperationAsync();
        Task<FinTransacRegRespObj> PassEntryToFinance(TransactionObj request);
        Task<SubGLRespObj> GetAllSubGlAsync();
        Task<MailRespObj> SendMail(MailObj request);
        Task<SecretKeysRespObj> GetFlutterWaveKeys();


        Task<LogingFailedRespObj> CheckForFailedTrailsAsync(bool isSuccessful, int module, string userid);
        Task<SessionCheckerRespObj> CheckForSessionTrailAsync(string userid, int module);

        Task<SecurityResp> GetSettingsAsync();
        Task<QuestionsRespObj> GetQuestionsAsync();
        Task<UserRoleRespObj> GetUserRolesAsync();
        Task<ActivityRespObj> GetAllActivityAsync();
        Task<HttpResponseMessage> CheckTrackedAsync(string token, string userid);

        Task<CommonRespObj> GetAllJobTileAsync();
    }
}
