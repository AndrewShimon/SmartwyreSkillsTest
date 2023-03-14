using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Services.Validators;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private static IDictionary<PaymentScheme, IPaymentValidator> CreatePaymentValidators()
        {
            return new Dictionary<PaymentScheme, IPaymentValidator>
            {
                { PaymentScheme.AutomatedPaymentSystem, new AutomatedPaymentSystemValidator() },
                { PaymentScheme.BankToBankTransfer, new BankToBankTransferValidator() },
                { PaymentScheme.ExpeditedPayments, new ExpeditedPaymentsValidator() }
            };
        }

        [Fact]
        public void MakePayment_Successful()
        {
            // Arrange
            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(x => x.GetAccount("1234")).Returns(new Account()
            {
                AccountNumber = "1234",
                Balance = 100,
                AllowedPaymentSchemes = AllowedPaymentSchemes.AutomatedPaymentSystem,
                Status = AccountStatus.Live
            });

            var paymentValidators = CreatePaymentValidators();

            var paymentService = new PaymentService(accountDataStore.Object, paymentValidators);
            var request = new MakePaymentRequest()
            {
                Amount = 50,
                DebtorAccountNumber = "1234",
                PaymentScheme = PaymentScheme.AutomatedPaymentSystem
            };

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            Assert.True(result.Success);
            accountDataStore.Verify(x => x.GetAccount("1234"), Times.Once);
            accountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public void MakePayment_Overdraft_Successful()
        {
            ///
            // Nothing said if overdrafts were allowed or not so I left them possible.
            // This could be updated to be a failure if that overdrafts should not be possible.
            ///

            // Arrange
            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(x => x.GetAccount("5678")).Returns(new Account()
            {
                AccountNumber = "5678",
                Balance = 100,
                AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer,
                Status = AccountStatus.Live
            });

            var paymentValidators = CreatePaymentValidators();

            var paymentService = new PaymentService(accountDataStore.Object, paymentValidators);
            var request = new MakePaymentRequest()
            {
                Amount = 200,
                DebtorAccountNumber = "5678",
                PaymentScheme = PaymentScheme.BankToBankTransfer
            };

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            Assert.True(result.Success);
            accountDataStore.Verify(x => x.GetAccount("5678"), Times.Once);
            accountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public void MakePayment_AccountHasWrongAllowedPaymentSchemes_Falure()
        {
            // Arrange
            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(x => x.GetAccount("2468")).Returns(new Account()
            {
                AccountNumber = "2468",
                Balance = 100,
                AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer,
                Status = AccountStatus.Live
            });

            var paymentValidators = CreatePaymentValidators();

            var paymentService = new PaymentService(accountDataStore.Object, paymentValidators);
            var request = new MakePaymentRequest()
            {
                Amount = 50,
                DebtorAccountNumber = "2468",
                PaymentScheme = PaymentScheme.ExpeditedPayments
            };

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            Assert.False(result.Success);
            accountDataStore.Verify(x => x.GetAccount("2468"), Times.Once);
            accountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }
    }
}
