using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Services.Interfaces
{
    public interface IPaymentValidator
    {
        bool ValidatePayment(Account account, MakePaymentRequest request);
    }
}
