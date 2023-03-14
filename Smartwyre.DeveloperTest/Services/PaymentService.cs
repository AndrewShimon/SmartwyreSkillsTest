using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IDictionary<PaymentScheme, IPaymentValidator> _paymentValidators;

        public PaymentService(IAccountDataStore accountDataStore, IDictionary<PaymentScheme, IPaymentValidator> paymentValidators)
        {
            _accountDataStore = accountDataStore;
            _paymentValidators = paymentValidators;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult();

            if (_paymentValidators.TryGetValue(request.PaymentScheme, out IPaymentValidator validator))
            {
                result.Success = validator.ValidatePayment(account, request);
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;

                _accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}
