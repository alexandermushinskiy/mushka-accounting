using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Supplies.Converters;
using Mushka.Application.Supplies.Queries.Search;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Models;
using Xunit;

namespace Mushka.Application.Tests.Supplies.Queries.Search
{
    public class SearchSuppliesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_UsesUnfilteredTotal_AndPassesFilter()
        {
            var productId = Guid.NewGuid();
            var supplies = new[]
            {
                new Supply
                {
                    Id = Guid.NewGuid(),
                    Description = "socks",
                    ReceivedDate = new DateTime(2024, 2, 3),
                    Cost = 10,
                    TotalCost = 12,
                    Supplier = new Supplier { Name = "Acme" },
                    Products = new List<SupplyProduct> { new SupplyProduct { Quantity = 2 }, new SupplyProduct { Quantity = 3 } }
                }
            };
            SearchSuppliesFilter capturedFilter = null;
            var repository = new Mock<ISupplyRepository>();
            repository.Setup(mock => mock.GetAllCountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(9);
            repository
                .Setup(mock => mock.GetByFilterAsync(It.IsAny<SearchSuppliesFilter>(), It.IsAny<CancellationToken>()))
                .Callback<SearchSuppliesFilter, CancellationToken>((filter, _) => capturedFilter = filter)
                .ReturnsAsync(supplies);
            var storage = new Mock<IStorage>();
            storage.Setup(mock => mock.GetRepository<ISupplyRepository>()).Returns(repository.Object);
            var handler = new SearchSuppliesQueryHandler(storage.Object, new SearchSuppliesConverter());

            var actual = await handler.Handle(new SearchSuppliesQuery("acm", productId), CancellationToken.None);

            capturedFilter.SearchKey.Should().Be("acm");
            capturedFilter.ProductId.Should().Be(productId);
            actual.Total.Should().Be(9);
            actual.Items.Should().ContainSingle();
            var item = actual.Items.Single();
            item.Description.Should().Be("socks");
            item.SupplierName.Should().Be("Acme");
            item.ProductsAmount.Should().Be(5);
            item.Cost.Should().Be(10);
            item.TotalCost.Should().Be(12);
        }
    }
}
