using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Suppliers.Converters;
using Mushka.Application.Suppliers.Queries.Search;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Suppliers.Queries.Search
{
    public class SearchSuppliersQueryHandlerTests
    {
        [Fact]
        public async Task Handle_OrdersSuppliersByName_AndReturnsReadModel()
        {
            var suppliers = new[]
            {
                new Supplier { Name = "Beta" },
                new Supplier { Name = "Alpha" }
            };
            var repository = new Mock<ISupplierRepository>();
            repository
                .Setup(mock => mock.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(suppliers);
            var storage = new Mock<IStorage>();
            storage.Setup(mock => mock.GetRepository<ISupplierRepository>()).Returns(repository.Object);
            var handler = new SearchSuppliersQueryHandler(storage.Object, new SearchSuppliersConverter());

            var actual = await handler.Handle(new SearchSuppliersQuery(), CancellationToken.None);

            actual.Total.Should().Be(2);
            actual.Items.Select(item => item.Supplier.Name).Should().Equal("Alpha", "Beta");
        }
    }
}
