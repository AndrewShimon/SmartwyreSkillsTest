using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Services.Validators
{
    public class AutomatedPaymentSystemValidator : IPaymentValidator
    {
        public bool ValidatePayment(Account account, MakePaymentRequest request)
        {
            return account != null
                && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.AutomatedPaymentSystem)
                && account.Status == AccountStatus.Live;
        }
    }
}
