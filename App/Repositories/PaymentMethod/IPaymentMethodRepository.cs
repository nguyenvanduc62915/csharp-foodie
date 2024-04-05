
using AppCore.Models;

namespace AppCore.App.Repositories;

public interface IPaymentMethodRepository {

    Task<PaymentMethod> GetPaymentMethodByIdAsync(int paymentMethodId);
    Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync();
    Task AddPaymentMethodAsync(PaymentMethod paymentMethod);
    Task UpdatePaymentMethodAsync(PaymentMethod paymentMethod);
    Task<bool> DeletePaymentMethodAsync(int paymentMethodId);

}