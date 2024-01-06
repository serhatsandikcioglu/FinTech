using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Constans
{
    public static class ErrorMessageConstants
    {
        public const string AccountNotFound = "Account Not Found";
        public const string LoanApplicationNotFound = "Loan Application Not Found";
        public const string InsufficientFunds = "Insufficient Funds";
        public const string InitialBalanceError = "Initial Account Balance Must Be Above The Account Opening Limit";
        public const string MinBalanceError = "Balance Cannot Be Less Than 0";
        public const string LoanApplicationProcessed = "Loan Application Has Been Processed";
        public const string DailyTransferLimitError = "Daily Transfer Limit Exceeded";
        public const string TransferLimitError = "Transfer Limit Exceeded";
        public const string IncorrectUsernameOrPassword = "Incorrect Username Or Password";
        public const string InvalidIdNumber = "ID Number Is Invalid";
        public const string RoleOperationFailed = "User Does Not Have This Role";
        public const string UserNotFound = "User Not Found";
        public const string RoleNotFound = "Role Not Found";
        public const string SupportTicketNotFound = "Support Ticket Not Found";
        public const string SupportTicketPrioritized = "Support Ticket Prioritized Already";
        public const string SupportTicketProcessed = "Support Ticket Processed Already";
        public const string NameAndSurnameDontMatch = "Name And Surname Dont Match";
        public const string AutomaticPaymentNotFound = "Automatic Payment Not Found";
        public const string ForbiddenAccount = "You Are Not Authorized To Process On This Account.";
    }
}
