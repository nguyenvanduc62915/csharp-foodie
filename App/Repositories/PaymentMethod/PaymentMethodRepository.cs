
using Microsoft.EntityFrameworkCore;

using AppCore.Data;
using AppCore.Models;

namespace AppCore.App.Repositories;

public class PaymentMethodRepository : IPaymentMethodRepository {
    private readonly ApplicationDbContext _dbContext;

    public PaymentMethodRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<PaymentMethod> GetPaymentMethodIdAsync(int paymentMethodId)
    {
        return await _dbContext.PaymentMethods.FindAsync(paymentMethodId);
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
    {
        return await _dbContext.PaymentMethods.ToListAsync();
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsActiveAsync() {
        return await _dbContext.PaymentMethods.Where(pm => pm.Active).ToListAsync();
    }

    public async Task<IEnumerable<PaymentMethod>> GetSomePaymentMethodsAsync(int count)
    {
        return await _dbContext.PaymentMethods.Take(count).ToListAsync();
    }

    public async Task AddPaymentMethodAsync(PaymentMethod paymentMethod)
    {
        _dbContext.PaymentMethods.Add(paymentMethod);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdatePaymentMethodAsync(PaymentMethod paymentMethod)
    {
        _dbContext.Entry(paymentMethod).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeletePaymentMethodAsync(int paymentMethodId) {
        var paymentMethod = await _dbContext.PaymentMethods.FindAsync(paymentMethodId);
        if (paymentMethod != null)
        {
            _dbContext.PaymentMethods.Remove(paymentMethod);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }


    Task<PaymentMethod> IPaymentMethodRepository.GetPaymentMethodByIdAsync(int paymentMethodId)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<PaymentMethod>> IPaymentMethodRepository.GetAllPaymentMethodsAsync()
    {
        throw new NotImplementedException();
    }

    // public Task AddProductAsync(Product product)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task UpdateProductAsync(Product product)
    // {
    //     throw new NotImplementedException();
    // }
}