using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Supplies.Converters;
using Mushka.Application.Supplies.Queries.Describe;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Supplies.Queries.Describe
{
    public class DescribeSupplyQueryHandlerTests
    {
        private readonly Mock<IStorage> _storage = new Mock<IStorage>();
        private readonly Mock<ISupplyRepository> _repository = new Mock<ISupplyRepository>();
        private readonly Mock<IDescribeSupplyConverter> _converter = new Mock<IDescribeSupplyConverter>();
        private readonly DescribeSupplyQueryHandler _handler;

        public DescribeSupplyQueryHandlerTests()
        {
            _storage.Setup(storage => storage.GetRepository<ISupplyRepository>()).Returns(_repository.Object);
            _handler = new DescribeSupplyQueryHandler(_storage.Object, _converter.Object);
        }

        [Fact]
        public async Task Handle_WhenSupplyIsMissing_ThrowsNotFoundException()
        {
            var id = Guid.NewGuid();
            _repository
                .Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Supply)null);

            var exception = await Record.ExceptionAsync(() => _handler.Handle(new DescribeSupplyQuery(id), CancellationToken.None).AsTask());

            exception.Should().BeOfType<NotFoundException>()
                .Which.ErrorKey.Should().Be(ValidationErrors.SupplyNotFound);
        }

        [Fact]
        public async Task Handle_ReturnsConvertedReadModel()
        {
            var supply = new Supply { Id = Guid.NewGuid() };
            var readModel = new DescribeSupplyReadModel();
            _repository
                .Setup(repository => repository.GetByIdAsync(supply.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(supply);
            _converter.Setup(converter => converter.ToReadModel(supply)).Returns(readModel);

            var actual = await _handler.Handle(new DescribeSupplyQuery(supply.Id), CancellationToken.None);

            actual.Should().BeSameAs(readModel);
        }
    }
}
