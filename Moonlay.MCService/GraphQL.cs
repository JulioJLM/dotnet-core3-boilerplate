using GraphQL;
using GraphQL.Types;
using Moonlay.MasterData.Domain.Customers;
using Moonlay.MasterData.Domain.Customers.GraphQL;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Moonlay.MasterData.WebApi
{
    public class DQuery : ObjectGraphType<object>
    {
        public DQuery()
        {

            // var scope = provider.CreateScope();
            // var service = scope.ServiceProvider.GetRequiredService<ICustomerService>();

            Name = "DQuery";

            Field<ListGraphType<CustomerType>>("customers",

                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "limit" },
                    new QueryArgument<IntGraphType> { Name = "offset" }
                ),
                resolve: context =>
                {
                    return new List<Customer>();
                }

            );

        }
    }

    public class DMutation : ObjectGraphType
    {
        public DMutation()
        {
            Name = "Mutation";

            Field<CustomerType>(
                "createCustomer",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }
                ),
                resolve: context =>
                {
                    var human = context.GetArgument<string>("name");
                    return new Customer(Guid.NewGuid());
                });
        }
    }

    public class DSchema : Schema
    {
        public DSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<DQuery>();
            Mutation = resolver.Resolve<DMutation>();
        }
    }

    internal class GraphQLUserContext : Dictionary<string, object>
    {
        public ClaimsPrincipal User { get; set; }
    }
}