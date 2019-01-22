using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Providers;
using Mushka.Tests.Common;
using Xunit;

namespace Mushka.Tests.Service.Providers
{
    public class CostPriceProviderTest : ServiceTestBase
    {
        private const string CategoryName = Component.Service + nameof(CostPriceProvider);
        private const string CalculateAsyncMethodName = nameof(CostPriceProvider.CalculateAsync) + ". ";

        private static readonly Guid ProductId = Guid.Parse("00000000000000000000000000000111");
        private static readonly Guid OtherProductId = Guid.Parse("00000000000000000000000000000222");

        private readonly Mock<IOrderRepository> orderRepositoryMock;
        private readonly Mock<ISupplyRepository> supplyRepositoryMock;
        private readonly CostPriceProvider costPriceProvider;

        public CostPriceProviderTest()
        {
            orderRepositoryMock = MockRepository.Create<IOrderRepository>();
            supplyRepositoryMock = MockRepository.Create<ISupplyRepository>();

            costPriceProvider = new CostPriceProvider(supplyRepositoryMock.Object, orderRepositoryMock.Object);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = CalculateAsyncMethodName + "One supply and nothing sold yet")]
        public async Task CalculateAsyncOneSupplyAndNothingSoldTest()
        {
            var supplyProducts = new[] { CreateSupplyProduct(ProductId, 10, 1.2m) };

            orderRepositoryMock
                .SetupAsync(repo => repo.GetSoldProductCount(ProductId, default(CancellationToken)), 0);

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByProductAsync(ProductId, default(CancellationToken)), supplyProducts);

            var actual = await costPriceProvider.CalculateAsync(ProductId, 1);
            actual.Should().Be(1.2m);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = CalculateAsyncMethodName + "All sold from first supply")]
        public async Task CalculateAsyncAllSoldFromFirstSupplyTest()
        {
            var supplyProducts = new[]
            {
                CreateSupplyProduct(ProductId, 10, 1.2m),
                CreateSupplyProduct(ProductId, 13, 1.5m)
            };

            orderRepositoryMock
                .SetupAsync(repo => repo.GetSoldProductCount(ProductId, default(CancellationToken)), 10);

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByProductAsync(ProductId, default(CancellationToken)), supplyProducts);

            var actual = await costPriceProvider.CalculateAsync(ProductId, 1);
            actual.Should().Be(1.5m);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = CalculateAsyncMethodName + "Calculate from different supplies")]
        public async Task CalculateAsyncCalculateFromDifferentSuppliesTest()
        {
            var supplyProducts = new[]
            {
                CreateSupplyProduct(ProductId, 10, 1.2m),
                CreateSupplyProduct(ProductId, 13, 2.5m)
            };

            orderRepositoryMock
                .SetupAsync(repo => repo.GetSoldProductCount(ProductId, default(CancellationToken)), 8);

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByProductAsync(ProductId, default(CancellationToken)), supplyProducts);

            var actual = await costPriceProvider.CalculateAsync(ProductId, 4);
            actual.Should().Be(1.85m);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = CalculateAsyncMethodName + "Use certain product")]
        public async Task CalculateAsyncUseCertainProductTest()
        {
            var supplyProducts = new[]
            {
                CreateSupplyProduct(ProductId, 10, 1.2m),
                CreateSupplyProduct(OtherProductId, 13, 12.7m)
            };

            orderRepositoryMock
                .SetupAsync(repo => repo.GetSoldProductCount(ProductId, default(CancellationToken)), 2);

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByProductAsync(ProductId, default(CancellationToken)), supplyProducts);

            var actual = await costPriceProvider.CalculateAsync(ProductId, 4);
            actual.Should().Be(1.2m);
        }

        private static SupplyProduct CreateSupplyProduct(Guid productId, int quantity, decimal costPrice) =>
            new SupplyProduct
            {
                ProductId = productId,
                CostPrice = costPrice,
                Quantity = quantity
            };
    }
}