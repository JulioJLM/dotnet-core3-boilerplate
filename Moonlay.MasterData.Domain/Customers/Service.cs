using Moonlay.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moonlay.MasterData.Domain.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IDbContext _db;

        public CustomerService(ICustomerRepository repository, IDbContext db)
        {
            _customerRepo = repository;
            _db = db;
        }

        private async Task SaveChangesAsync()
        {
            await this._db.SaveChangesAsync();
        }

        public async Task<Customer> NewCustomerAsync(string firstName, string lastName, Action<Customer> beforeSave = null)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException("message", nameof(firstName));
            }

            var newCustomerId = Guid.NewGuid();

            var customer = await this._customerRepo.StoreAsync(newCustomerId, firstName, lastName);

            beforeSave?.Invoke(customer);

            await SaveChangesAsync();

            return customer;
        }

        public Task<List<Customer>> SearchAsync(Func<Customer, bool> criteria = null, int page = 0, int size = 25)
        {
            if (criteria == null)
                criteria = x => true;

            var result = _customerRepo.DbSet.Where(criteria).OrderBy(o => o.LastName).Skip(page * size).Take(size).ToList();

            return Task.FromResult(result);
        }

        public Task<List<CustomerTrail>> LogsAsync(Guid id)
        {
            var customer = _customerRepo.With(id);

            var logs = _customerRepo.DbSetTrail.Where(o => o.CustomerId == id).ToList();

            return Task.FromResult(logs);
        }

        public async Task<Customer> UpdateProfileAsync(Guid id, string firstName, string lastName, Action<Customer> beforeSave = null)
        {
            var customer = _customerRepo.With(id);
            customer.FirstName = firstName;
            customer.LastName = lastName;

            await _customerRepo.UpdateAsync(customer);

            beforeSave?.Invoke(customer);

            await SaveChangesAsync();

            return customer;
        }
    }

}