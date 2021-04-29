using GOSLibraries.GOS_Financial_Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Auth
{
    public class RegistrationCommand : IRequest<AuthResponse>
    {
        public string UserName { get; set; }
        public int CustomerTypeId { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public int QuestionId { get; set; }
        public string SecurityAnswered { get; set; }
    }
}
