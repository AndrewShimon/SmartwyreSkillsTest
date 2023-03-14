using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Services.Validators;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var accountDataStore = new AccountDataStore();

            var paymentValidators = new Dictionary<PaymentScheme, IPaymentValidator>
            {
                { PaymentScheme.AutomatedPaymentSystem, new AutomatedPaymentSystemValidator() },
                { PaymentScheme.BankToBankTransfer, new BankToBankTransferValidator() },
                { PaymentScheme.ExpeditedPayments, new ExpeditedPaymentsValidator() }
            };

            var paymentService = new PaymentService(accountDataStore, paymentValidators);

            Console.Write("Enter account number: ");
            var accountNumber = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("1 for AutomatedPaymentSystem");
            Console.WriteLine("2 for BankToBankTransfer");
            Console.WriteLine("3 for ExpeditedPayments");
            Console.Write("Enter payment scheme: ");
            if (!int.TryParse(Console.ReadLine(), out var paymentSchemeInt))
            {
                Console.WriteLine("Invalid Number.");
                return;
            }
            if (!Enum.IsDefined(typeof(PaymentScheme), paymentSchemeInt))
            {
                Console.WriteLine("Invalid payment scheme.");
                return;
            }
            var paymentScheme = (PaymentScheme)paymentSchemeInt;


            Console.WriteLine();
            Console.Write("Enter payment amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out var amount))
            {
                Console.WriteLine("Invalid payment amount.");
                return;
            }

            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = accountNumber,
                PaymentScheme = paymentScheme,
                Amount = amount
            };

            var result = paymentService.MakePayment(request);


            Console.WriteLine();
            if (result.Success)
            {
                Console.WriteLine("Payment succeeded.");
            }
            else
            {
                Console.WriteLine("Payment failed.");
            }
        }
    }
}
