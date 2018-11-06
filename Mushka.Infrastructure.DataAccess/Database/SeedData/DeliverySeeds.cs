using System;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.SeedData
{
    internal static class DeliverySeeds
    {
        public static void HasDeliveries(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Delivery>().HasData(
                new
                {
                    Id = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"),
                    RequestDate = DateTime.Parse("2018-02-13"),
                    DeliveryDate = DateTime.Parse("2018-04-05"),
                    PaymentMethod = PaymentMethod.TransferToCard,
                    Cost = 41319.00m,
                    TransferFee = 940.00m,
                    BankFee = 222.00m
                },
                new
                {
                    Id = Guid.Parse("32C74EF3-ADFD-4723-A319-9B8984D1B7FB"),
                    RequestDate = DateTime.Parse("2018-05-03"),
                    DeliveryDate = DateTime.Parse("2018-06-01"),
                    PaymentMethod = PaymentMethod.TransferToCard,
                    Cost = 16500.00m,
                    TransferFee = 90.00m,
                    BankFee = 90.00m
                },
                new
                {
                    Id = Guid.Parse("B2D8B13B-AA82-4820-BA85-E23501869C3A"),
                    RequestDate = DateTime.Parse("2018-08-26"),
                    DeliveryDate = DateTime.Parse("2018-09-26"),
                    PaymentMethod = PaymentMethod.TransferToCard,
                    Cost = 39720.00m,
                    TransferFee = 810.00m,
                    BankFee = 110.00m
                }
            );

            modelBuilder.Entity<DeliveryProduct>().HasData(
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("A9AB38D1-C2B2-4C50-9AB9-80335F4561F8"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 39, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("A9AB38D1-C2B2-4C50-9AB9-80335F4561F8"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 38, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("DFA69FA0-2DF3-4254-95B7-F65EB4ED6C92"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 52, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("DFA69FA0-2DF3-4254-95B7-F65EB4ED6C92"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 56, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("F9B055D3-5FD9-417F-B71D-0AF81C821029"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 53, PriceForItem = 30.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("F9B055D3-5FD9-417F-B71D-0AF81C821029"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 40, PriceForItem = 30.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("365510F0-FB1A-42CD-B249-5AD514BF2F33"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 56, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("365510F0-FB1A-42CD-B249-5AD514BF2F33"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 46, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("BA641024-D50A-4F9C-BFD9-A330FE12071E"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 52, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("BA641024-D50A-4F9C-BFD9-A330FE12071E"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 54, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("55386F8F-3234-42C0-A82A-65EA7DD50B28"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 57, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("55386F8F-3234-42C0-A82A-65EA7DD50B28"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 75, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("BDDC1231-0952-4C6D-9A30-9DE441CFA3A0"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 50, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("BDDC1231-0952-4C6D-9A30-9DE441CFA3A0"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 48, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("F869224C-80E6-43B6-94A4-2528ECD67A75"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 53, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("F869224C-80E6-43B6-94A4-2528ECD67A75"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 53, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("A9899CD5-9B2D-4241-8E28-0D1441933BAD"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 43, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("A9899CD5-9B2D-4241-8E28-0D1441933BAD"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 44, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("637B0BA2-F1D9-4BF6-B1C7-1BC685033B36"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 57, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("637B0BA2-F1D9-4BF6-B1C7-1BC685033B36"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 58, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("65801C7F-37F6-4452-A304-CEAAFC940D08"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 52, PriceForItem = 20.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("65801C7F-37F6-4452-A304-CEAAFC940D08"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 51, PriceForItem = 20.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("8636296D-E47C-4BB8-A6FD-E0CC01D4E27A"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 53, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("8636296D-E47C-4BB8-A6FD-E0CC01D4E27A"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 53, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("32AE1BAE-C186-4E7C-A6AF-D683E10D1480"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 50, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("32AE1BAE-C186-4E7C-A6AF-D683E10D1480"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 48, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("574F3353-0C6E-4148-A8EF-0DB9559F3864"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 48, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("574F3353-0C6E-4148-A8EF-0DB9559F3864"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 64, PriceForItem = 25.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("304AF5DF-1D03-40C3-AF40-9C6259898F75"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 60, PriceForItem = 20.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("304AF5DF-1D03-40C3-AF40-9C6259898F75"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 57, PriceForItem = 20.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("B62555E5-E51B-41E2-9BF8-6A750EDC0D8A"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 65, PriceForItem = 20.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("B62555E5-E51B-41E2-9BF8-6A750EDC0D8A"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 66, PriceForItem = 20.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("6BB026FC-AE0F-4A87-B0E3-845B3D55E05B"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 54, PriceForItem = 20.0m },
                new { DeliveryId = Guid.Parse("4E50F00D-4FD9-4DFE-8D56-18A2399DD7B6"), ProductId = Guid.Parse("6BB026FC-AE0F-4A87-B0E3-845B3D55E05B"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 62, PriceForItem = 20.0m }//,
                //new
                //{
                //    DeliveryId = Guid.Parse(""),
                //    ProductId = Guid.Parse(""),
                //    SizeId = Guid.Parse(""),
                //    Quantity = 0,
                //    PriceForItem = 25.0m
                //}
            );
        }
    }
}