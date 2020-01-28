using FluentAssertions;
using Moonlay.MasterData.Domain.Customers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Moonlay.MasterData.Domain.UnitTests.Domain.Customers
{
    public class ServiceTest : IDisposable
    {
        private readonly MockRepository _MockRepo;
        private readonly Mock<ICustomerRepository> _CustomerRepo;

        public ServiceTest()
        {
            _MockRepo = new MockRepository(MockBehavior.Strict);
            _CustomerRepo = _MockRepo.Create<ICustomerRepository>();
        }

        public void Dispose()
        {
            _MockRepo.VerifyAll();
        }

        private CustomerUseCase CreateService(DbTestConnection db)
        {
            return new CustomerUseCase(_CustomerRepo.Object, db.Db);
        }


        [Theory(DisplayName = "CustomerService.CreateCustomer_Successfully")]
        [InlineData("Afandy", "Lamusu")]
        [InlineData("Afandy@", "Lamusu")]
        [InlineData("Afandy&77", "Lamusu")]
        public async Task CreateCustomer_Successfully(string firstName, string lastName)
        {
            using (var db = new DbTestConnection())
            {
                _CustomerRepo.Setup(s => s.DbSet).Returns(db.Db.Set<Customer>());

                // Action
                var service = CreateService(db);
                Customer customer = await service.NewCustomerAsync(firstName, lastName); ;

                // Asserts
                customer.Should().NotBeNull();
                customer.FirstName.Should().Be(firstName);
                customer.LastName.Should().Be(lastName);

            }

        }

        [Theory(DisplayName = "CustomerService.CreateCustomer_Fail_FirstName_NullOrEmpty")]
        [InlineData("", "Lamusu")]
        public async Task CreateCustomer_Fail_FirstName(string firstName, string lastName)
        {
            using (var db = new DbTestConnection())
            {
                var service = CreateService(db);

                // Action
                Func<Task> act = async () => { await service.NewCustomerAsync(firstName, lastName); };

                // Asserts
                await act.Should().ThrowAsync<ArgumentException>();
            }
        }

        [Fact(DisplayName = "CustomerService.Search_Successfully")]
        public async Task Search_Successfully()
        {
            using (var db = new DbTestConnection())
            {
                _CustomerRepo.Setup(s => s.DbSet).Returns(db.Db.Set<Customer>());

                // prepare data
                var service = CreateService(db);
                await service.NewCustomerAsync("Andy", "Hasan");
                await service.NewCustomerAsync("Andy", "Kribo");

                var customers = await service.SearchAsync(x => x.FirstName == "Andy");

                customers.Should().Contain(x => x.FirstName == "Andy");
            }
        }

        [Fact(DisplayName = "CustomerService.UpdateProfile_Successfully")]
        public async Task UpdateProfile_Successfully()
        {
            using (var db = new DbTestConnection())
            {
                _CustomerRepo.Setup(s => s.DbSet).Returns(db.Db.Set<Customer>());
                _CustomerRepo.Setup(s => s.DbSetTrail).Returns(db.DbTrail.Set<CustomerTrail>());

                // prepare data
                var service = CreateService(db);

                var newCustomer = await service.NewCustomerAsync("Andy", "Kribo");
                DateTimeOffset timeOfCreated = newCustomer.UpdatedAt;

                _CustomerRepo.Setup(x => x.With(newCustomer.Id)).Returns(Task.FromResult(newCustomer));

                // Action
                var updatedCustomer = await service.UpdateProfileAsync(newCustomer.Id, "Andy", "Hasan");

                // Assert : check the time of created not equal the time when updated.
                updatedCustomer.Id.Should().Be(newCustomer.Id);
                updatedCustomer.UpdatedAt.Should().BeOnOrAfter(timeOfCreated);

                // validate the logs, should contain 2 records.
                (await service.LogsAsync(newCustomer.Id)).Should().HaveCount(2);
            }
        }
    }
}