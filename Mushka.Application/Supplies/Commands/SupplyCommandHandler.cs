using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Supplies.Converters;
using Mushka.Domain.Comparers;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Supplies.Commands
{
    public sealed class SupplyCommandHandler :
        ICommandHandler<CreateSupplyCommand>,
        ICommandHandler<UpdateSupplyCommand>,
        ICommandHandler<DeleteSupplyCommand>
    {
        private readonly IStorage _storage;
        private readonly ISupplyRepository _supplyRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICreateSupplyConverter _createSupplyConverter;
        private readonly IUpdateSupplyConverter _updateSupplyConverter;

        public SupplyCommandHandler(
            IStorage storage,
            ICreateSupplyConverter createSupplyConverter,
            IUpdateSupplyConverter updateSupplyConverter)
        {
            _storage = storage;
            _supplyRepository = storage.GetRepository<ISupplyRepository>();
            _productRepository = storage.GetRepository<IProductRepository>();
            _createSupplyConverter = createSupplyConverter;
            _updateSupplyConverter = updateSupplyConverter;
        }

        public async ValueTask<Unit> Handle(CreateSupplyCommand command, CancellationToken cancellationToken)
        {
            var supply = _createSupplyConverter.ToSupply(command);

            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await _productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    throw new NotFoundException(ValidationErrors.ProductNotFound);
                }

                storedProduct.Quantity += supplyProduct.Quantity;
                _productRepository.Update(storedProduct);
            }

            _supplyRepository.Add(supply);
            await _storage.SaveAsync(cancellationToken);

            return Unit.Value;
        }

        public async ValueTask<Unit> Handle(UpdateSupplyCommand command, CancellationToken cancellationToken)
        {
            var storedSupply = await _supplyRepository.GetByIdAsync(command.Id, cancellationToken);

            if (storedSupply == null)
            {
                throw new NotFoundException(ValidationErrors.SupplyNotFound);
            }

            var supply = _updateSupplyConverter.ToSupply(command);

            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await _productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    throw new NotFoundException(ValidationErrors.ProductNotFound);
                }

                var storedSupplyQuantity = storedSupply.Products
                    .FirstOrDefault(product => product.ProductId == storedProduct.Id)?.Quantity ?? 0;

                if (storedSupplyQuantity != supplyProduct.Quantity)
                {
                    storedProduct.Quantity = storedProduct.Quantity - storedSupplyQuantity + supplyProduct.Quantity;
                    _productRepository.Update(storedProduct);
                }
            }

            foreach (var removedProduct in storedSupply.Products.Except(supply.Products, new SupplyProductComparer()))
            {
                var storedProduct = await _productRepository.GetByIdAsync(removedProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    throw new NotFoundException(ValidationErrors.ProductNotFound);
                }

                storedProduct.Quantity -= removedProduct.Quantity;
                _productRepository.Update(storedProduct);
            }

            _supplyRepository.Update(supply);
            await _storage.SaveAsync(cancellationToken);

            return Unit.Value;
        }

        public async ValueTask<Unit> Handle(DeleteSupplyCommand command, CancellationToken cancellationToken)
        {
            var supply = await _supplyRepository.GetByIdAsync(command.Id, cancellationToken);

            if (supply == null)
            {
                throw new NotFoundException(ValidationErrors.SupplyNotFound);
            }

            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await _productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    throw new NotFoundException(ValidationErrors.ProductNotFound);
                }

                storedProduct.Quantity -= supplyProduct.Quantity;
                _productRepository.Update(storedProduct);
            }

            _supplyRepository.Delete(supply);
            await _storage.SaveAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
