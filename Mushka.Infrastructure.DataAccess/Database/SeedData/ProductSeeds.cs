﻿using System;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.SeedData
{
    internal static class ProductSeeds
    {
        public static void HasProducts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("A9AB38D1-C2B2-4C50-9AB9-80335F4561F8"), Name = "Airplane", Code = "AIR001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("DFA69FA0-2DF3-4254-95B7-F65EB4ED6C92"), Name = "Bike", Code = "BIK001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("F9B055D3-5FD9-417F-B71D-0AF81C821029"), Name = "Motobike", Code = "MOT001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("365510F0-FB1A-42CD-B249-5AD514BF2F33"), Name = "Dartik", Code = "DRT001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("BA641024-D50A-4F9C-BFD9-A330FE12071E"), Name = "Shturmovik", Code = "DRV001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("55386F8F-3234-42C0-A82A-65EA7DD50B28"), Name = "Yoda", Code = "DRJ001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("BDDC1231-0952-4C6D-9A30-9DE441CFA3A0"), Name = "Badminton", Code = "BDM001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("F869224C-80E6-43B6-94A4-2528ECD67A75"), Name = "Beer", Code = "BER001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("A9899CD5-9B2D-4241-8E28-0D1441933BAD"), Name = "Smile", Code = "CAM001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("637B0BA2-F1D9-4BF6-B1C7-1BC685033B36"), Name = "CoffeeOk", Code = "CFF001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("65801C7F-37F6-4452-A304-CEAAFC940D08"), Name = "Avo-avocado", Code = "DGY001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("8636296D-E47C-4BB8-A6FD-E0CC01D4E27A"), Name = "Beboss dot", Code = "DRW001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("32AE1BAE-C186-4E7C-A6AF-D683E10D1480"), Name = "Apelsinka", Code = "DOW001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("574F3353-0C6E-4148-A8EF-0DB9559F3864"), Name = "Spaceman", Code = "GAL001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("304AF5DF-1D03-40C3-AF40-9C6259898F75"), Name = "Limono", Code = "SWY001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("B62555E5-E51B-41E2-9BF8-6A750EDC0D8A"), Name = "Cherry", Code = "SWR001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("6BB026FC-AE0F-4A87-B0E3-845B3D55E05B"), Name = "Navy", Code = "SBY001", CreatedOn = DateTime.Parse("2018-11-06") },
                new { CategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Id = Guid.Parse("A6F3CC9A-BD32-49F5-8A5E-CAD1262298F8"), Name = "", Code = "", CreatedOn = DateTime.Parse("2018-11-06") }
            );

            modelBuilder.Entity<ProductSize>().HasData(
                new { ProductId = Guid.Parse("A9AB38D1-C2B2-4C50-9AB9-80335F4561F8"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 0 },
                new { ProductId = Guid.Parse("A9AB38D1-C2B2-4C50-9AB9-80335F4561F8"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 0 },
                new { ProductId = Guid.Parse("DFA69FA0-2DF3-4254-95B7-F65EB4ED6C92"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 0 },
                new { ProductId = Guid.Parse("DFA69FA0-2DF3-4254-95B7-F65EB4ED6C92"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 0 },
                new { ProductId = Guid.Parse("F9B055D3-5FD9-417F-B71D-0AF81C821029"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 0 },
                new { ProductId = Guid.Parse("F9B055D3-5FD9-417F-B71D-0AF81C821029"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 0 },
                new { ProductId = Guid.Parse("365510F0-FB1A-42CD-B249-5AD514BF2F33"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 0 },
                new { ProductId = Guid.Parse("365510F0-FB1A-42CD-B249-5AD514BF2F33"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 0 },
                new { ProductId = Guid.Parse("BA641024-D50A-4F9C-BFD9-A330FE12071E"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 0 },
                new { ProductId = Guid.Parse("BA641024-D50A-4F9C-BFD9-A330FE12071E"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 0 },
                new { ProductId = Guid.Parse("55386F8F-3234-42C0-A82A-65EA7DD50B28"), SizeId = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Quantity = 0 },
                new { ProductId = Guid.Parse("55386F8F-3234-42C0-A82A-65EA7DD50B28"), SizeId = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Quantity = 0 },
                new { ProductId = Guid.Parse("BDDC1231-0952-4C6D-9A30-9DE441CFA3A0"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("BDDC1231-0952-4C6D-9A30-9DE441CFA3A0"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 },
                new { ProductId = Guid.Parse("F869224C-80E6-43B6-94A4-2528ECD67A75"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("F869224C-80E6-43B6-94A4-2528ECD67A75"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 },
                new { ProductId = Guid.Parse("A9899CD5-9B2D-4241-8E28-0D1441933BAD"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("A9899CD5-9B2D-4241-8E28-0D1441933BAD"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 },
                new { ProductId = Guid.Parse("637B0BA2-F1D9-4BF6-B1C7-1BC685033B36"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("637B0BA2-F1D9-4BF6-B1C7-1BC685033B36"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 },
                new { ProductId = Guid.Parse("65801C7F-37F6-4452-A304-CEAAFC940D08"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("65801C7F-37F6-4452-A304-CEAAFC940D08"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 },
                new { ProductId = Guid.Parse("8636296D-E47C-4BB8-A6FD-E0CC01D4E27A"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("8636296D-E47C-4BB8-A6FD-E0CC01D4E27A"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 },
                new { ProductId = Guid.Parse("32AE1BAE-C186-4E7C-A6AF-D683E10D1480"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("32AE1BAE-C186-4E7C-A6AF-D683E10D1480"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 },
                new { ProductId = Guid.Parse("574F3353-0C6E-4148-A8EF-0DB9559F3864"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("574F3353-0C6E-4148-A8EF-0DB9559F3864"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 },
                new { ProductId = Guid.Parse("304AF5DF-1D03-40C3-AF40-9C6259898F75"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("304AF5DF-1D03-40C3-AF40-9C6259898F75"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 },
                new { ProductId = Guid.Parse("B62555E5-E51B-41E2-9BF8-6A750EDC0D8A"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("B62555E5-E51B-41E2-9BF8-6A750EDC0D8A"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 },
                new { ProductId = Guid.Parse("6BB026FC-AE0F-4A87-B0E3-845B3D55E05B"), SizeId = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Quantity = 0 },
                new { ProductId = Guid.Parse("6BB026FC-AE0F-4A87-B0E3-845B3D55E05B"), SizeId = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Quantity = 0 }//,
                //new { ProductId = Guid.Parse(""), SizeId = Guid.Parse(""), Quantity = 0 }
            );
        }
    }
}