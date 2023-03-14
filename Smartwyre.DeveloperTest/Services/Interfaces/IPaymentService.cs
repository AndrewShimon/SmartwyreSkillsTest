using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Interfaces
{
    public interface IPaymentService
    {
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
