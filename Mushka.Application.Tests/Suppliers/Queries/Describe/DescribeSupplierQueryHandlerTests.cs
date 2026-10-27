using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Suppliers.Converters;
using Mushka.Application.Suppliers.Queries.Describe;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Suppliers.Queries.Describe
{
    public class DescribeSupplierQueryHandlerTests
    {
        private readonly Mock<IStorage> _storage = new Mock<IStorage>();
        private readonly Mock<ISupplierRepository> _repository = new Mock<ISupplierRepository>();
        private readonly Mock<IDescribeSupplierConverter> _converter = new Mock<IDescribeSupplierConverter>();
        private readonly DescribeSupplierQueryHandler _handler;

        public DescribeSupplierQueryHandlerTests()
        {
            _storage.Setup(storage => storage.GetRepository<ISupplierRepository>()).Returns(_repository.Object);
            _handler = new DescribeSupplierQueryHandler(_storage.Object, _converter.Object);
        }

        [Fact]
        public async Task Handle_WhenSupplierIsMissing_ThrowsNotFoundException()
        {
            var id = Guid.NewGuid();
            _repository
                .Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Supplier)null);

            var exception = await Record.ExceptionAsync(() => _handler.Handle(new DescribeSupplierQuery(id), CancellationToken.None).AsTask());

            exception.Should().BeOfType<NotFoundException>()
                .Which.ErrorKey.Should().Be(ValidationErrors.SupplierNotFound);
        }

        [Fact]
        public async Task Handle_ReturnsConvertedReadModel()
        {
            var supplier = new Supplier { Id = Guid.NewGuid(), Name = "Acme" };
            var readModel = new DescribeSupplierReadModel();
            _repository
                .Setup(repository => repository.GetByIdAsync(supplier.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(supplier);
            _converter.Setup(converter => converter.ToReadModel(supplier)).Returns(readModel);

            var actual = await _handler.Handle(new DescribeSupplierQuery(supplier.Id), CancellationToken.None);

            actual.Should().BeSameAs(readModel);
        }
    }
}
