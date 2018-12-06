﻿USE [Mushka]

-- Sizes
INSERT INTO [Sizes] ([Id], [Name])
VALUES
('ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2', '36-39'),
('2DFA21EF-5EED-462F-B5E5-06EE31281BA2', '41-45'),
('FB8356A5-1629-4F9F-9B51-3D40E0E55F84', '39-42'),
('6E519491-8FD8-45F2-992E-270B01F25971', '43-46');

-- Categories
INSERT INTO [Categories] ([Id], [Name], [Order])
VALUES
('88CD0F34-9D4A-4E45-BE97-8899A97FB82C', N'Носки', 1),
('0E7BE1DE-267C-4C0A-8EE9-ABA0A267F27A', N'Упаковка', 2),
('B425D75B-2E72-45F0-A55D-3BA400051E5F', N'Другое', 3);

-- Products
INSERT INTO [Products] ([Id], [CategoryId], [Code], [CreatedOn], [Name])
VALUES 
('a9ab38d1-c2b2-4c50-9ab9-80335f4561f8', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'AIR001', '2018-11-06T00:00:00.0000000', N'Airplane'),
('8823f027-9074-4fa9-a5ef-552a5b08ef5e', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'FLM001', '2018-11-06T00:00:00.0000000', N'Flamingo'),
('76f1b29c-edac-4ca9-b529-da383c04905b', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'GRE001', '2018-11-06T00:00:00.0000000', N'Deep Green'),
('eabc3ce7-3c55-465e-9f27-11033bcc4f33', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'CAC001', '2018-11-06T00:00:00.0000000', N'Cactus'),
('d772b195-65e3-4250-8b2c-e2d59e7d24da', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BEE001', '2018-11-06T00:00:00.0000000', N'Bumble-bee'),
('380e3a08-08c5-40b1-b401-ec6b57d2e549', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SAI001', '2018-11-06T00:00:00.0000000', N'Sailor'),
('e536c61e-c2c5-41ec-9205-660726baa18b', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'PAS001', '2018-11-06T00:00:00.0000000', N'Royal Passion'),
('5bf3988b-ba17-4802-90ad-b77abe68677a', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'LAM001', '2018-11-06T00:00:00.0000000', N'Lamp'),
('db645119-1b9f-4161-966d-97a7cca8d2c7', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'PEP001', '2018-11-06T00:00:00.0000000', N'Pepper'),
('5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BAN001', '2018-11-06T00:00:00.0000000', N'Banana'),
('85ceb6f2-c29b-4809-b30a-5ccf427a0447', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'YOG001', '2018-11-06T00:00:00.0000000', N'Yoga'),
('09cfb881-d707-49e5-a2c1-730ce136b710', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'EIN001', '2018-11-06T00:00:00.0000000', N'Einstein'),
('84c601ce-a32d-432d-99e2-c23916cf4d1f', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'STE001', '2018-11-06T00:00:00.0000000', N'Jobsy'),
('6bb026fc-ae0f-4a87-b0e3-845b3d55e05b', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SBY001', '2018-11-06T00:00:00.0000000', N'Navy'),
('1054708a-aa30-4ba6-84f7-321eac6aa041', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'MST001', '2018-11-06T00:00:00.0000000', N'Multi stripe'),
('b62555e5-e51b-41e2-9bf8-6a750edc0d8a', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SWR001', '2018-11-06T00:00:00.0000000', N'Cherry'),
('574f3353-0c6e-4148-a8ef-0db9559f3864', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'GAL001', '2018-11-06T00:00:00.0000000', N'Spaceman'),
('32ae1bae-c186-4e7c-a6af-d683e10d1480', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DOW001', '2018-11-06T00:00:00.0000000', N'Apelsinka'),
('8636296d-e47c-4bb8-a6fd-e0cc01d4e27a', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRW001', '2018-11-06T00:00:00.0000000', N'Beboss dot'),
('65801c7f-37f6-4452-a304-ceaafc940d08', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DGY001', '2018-11-06T00:00:00.0000000', N'Avo-avocado'),
('637b0ba2-f1d9-4bf6-b1c7-1bc685033b36', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'CFF001', '2018-11-06T00:00:00.0000000', N'CoffeeOk'),
('a9899cd5-9b2d-4241-8e28-0d1441933bad', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'CAM001', '2018-11-06T00:00:00.0000000', N'Smile'),
('f869224c-80e6-43b6-94a4-2528ecd67a75', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BER001', '2018-11-06T00:00:00.0000000', N'Beer'),
('bddc1231-0952-4c6d-9a30-9de441cfa3a0', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BDM001', '2018-11-06T00:00:00.0000000', N'Badminton'),
('55386f8f-3234-42c0-a82a-65ea7dd50b28', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRJ001', '2018-11-06T00:00:00.0000000', N'Yoda'),
('ba641024-d50a-4f9c-bfd9-a330fe12071e', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRV001', '2018-11-06T00:00:00.0000000', N'Shturmovik'),
('365510f0-fb1a-42cd-b249-5ad514bf2f33', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRT001', '2018-11-06T00:00:00.0000000', N'Dartik'),
('f9b055d3-5fd9-417f-b71d-0af81c821029', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'MOT001', '2018-11-06T00:00:00.0000000', N'Motobike'),
('dfa69fa0-2df3-4254-95b7-f65eb4ed6c92', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BIK001', '2018-11-06T00:00:00.0000000', N'Bike'),
('304af5df-1d03-40c3-af40-9c6259898f75', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SWY001', '2018-11-06T00:00:00.0000000', N'Limono'),
('297af444-055f-4b76-a3ee-fbe65b9752f6', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'ORA001', '2018-11-06T00:00:00.0000000', N'Orange mood');


-- Product sizes
INSERT INTO [ProductSizes] ([ProductId], [SizeId], [Quantity])
VALUES 
('380e3a08-08c5-40b1-b401-ec6b57d2e549', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 107),
('5bf3988b-ba17-4802-90ad-b77abe68677a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 57),
('365510f0-fb1a-42cd-b249-5ad514bf2f33', '6e519491-8fd8-45f2-992e-270b01f25971', 46),
('365510f0-fb1a-42cd-b249-5ad514bf2f33', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 56),
('e536c61e-c2c5-41ec-9205-660726baa18b', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 122),
('380e3a08-08c5-40b1-b401-ec6b57d2e549', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 115),
('e536c61e-c2c5-41ec-9205-660726baa18b', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 110),
('5bf3988b-ba17-4802-90ad-b77abe68677a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 61),
('65801c7f-37f6-4452-a304-ceaafc940d08', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 52),
('f9b055d3-5fd9-417f-b71d-0af81c821029', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 53),
('d772b195-65e3-4250-8b2c-e2d59e7d24da', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 108),
('d772b195-65e3-4250-8b2c-e2d59e7d24da', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 111),
('eabc3ce7-3c55-465e-9f27-11033bcc4f33', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 98),
('eabc3ce7-3c55-465e-9f27-11033bcc4f33', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 92),
('dfa69fa0-2df3-4254-95b7-f65eb4ed6c92', '6e519491-8fd8-45f2-992e-270b01f25971', 56),
('76f1b29c-edac-4ca9-b529-da383c04905b', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 105),
('76f1b29c-edac-4ca9-b529-da383c04905b', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 112),
('dfa69fa0-2df3-4254-95b7-f65eb4ed6c92', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 52),
('8823f027-9074-4fa9-a5ef-552a5b08ef5e', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 115),
('8823f027-9074-4fa9-a5ef-552a5b08ef5e', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 114),
('a9ab38d1-c2b2-4c50-9ab9-80335f4561f8', '6e519491-8fd8-45f2-992e-270b01f25971', 38),
('1054708a-aa30-4ba6-84f7-321eac6aa041', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 110),
('1054708a-aa30-4ba6-84f7-321eac6aa041', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 160),
('a9ab38d1-c2b2-4c50-9ab9-80335f4561f8', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 39),
('f9b055d3-5fd9-417f-b71d-0af81c821029', '6e519491-8fd8-45f2-992e-270b01f25971', 40),
('ba641024-d50a-4f9c-bfd9-a330fe12071e', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 52),
('5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 47),
('db645119-1b9f-4161-966d-97a7cca8d2c7', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 52),
('f869224c-80e6-43b6-94a4-2528ecd67a75', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 53),
('304af5df-1d03-40c3-af40-9c6259898f75', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 57),
('304af5df-1d03-40c3-af40-9c6259898f75', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 60),
('574f3353-0c6e-4148-a8ef-0db9559f3864', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 64),
('574f3353-0c6e-4148-a8ef-0db9559f3864', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 48),
('a9899cd5-9b2d-4241-8e28-0d1441933bad', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 43),
('297af444-055f-4b76-a3ee-fbe65b9752f6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 132),
('a9899cd5-9b2d-4241-8e28-0d1441933bad', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 44),
('32ae1bae-c186-4e7c-a6af-d683e10d1480', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 50),
('8636296d-e47c-4bb8-a6fd-e0cc01d4e27a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 53),
('8636296d-e47c-4bb8-a6fd-e0cc01d4e27a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 53),
('637b0ba2-f1d9-4bf6-b1c7-1bc685033b36', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 57),
('637b0ba2-f1d9-4bf6-b1c7-1bc685033b36', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 58),
('65801c7f-37f6-4452-a304-ceaafc940d08', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 51),
('32ae1bae-c186-4e7c-a6af-d683e10d1480', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 48),
('db645119-1b9f-4161-966d-97a7cca8d2c7', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 49),
('b62555e5-e51b-41e2-9bf8-6a750edc0d8a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 65),
('f869224c-80e6-43b6-94a4-2528ecd67a75', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 53),
('ba641024-d50a-4f9c-bfd9-a330fe12071e', '6e519491-8fd8-45f2-992e-270b01f25971', 54),
('5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 46),
('55386f8f-3234-42c0-a82a-65ea7dd50b28', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 57),
('85ceb6f2-c29b-4809-b30a-5ccf427a0447', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 53),
('85ceb6f2-c29b-4809-b30a-5ccf427a0447', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 13),
('55386f8f-3234-42c0-a82a-65ea7dd50b28', '6e519491-8fd8-45f2-992e-270b01f25971', 75),
('b62555e5-e51b-41e2-9bf8-6a750edc0d8a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 66),
('09cfb881-d707-49e5-a2c1-730ce136b710', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 63),
('bddc1231-0952-4c6d-9a30-9de441cfa3a0', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 50),
('84c601ce-a32d-432d-99e2-c23916cf4d1f', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 48),
('84c601ce-a32d-432d-99e2-c23916cf4d1f', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 51),
('bddc1231-0952-4c6d-9a30-9de441cfa3a0', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 48),
('6bb026fc-ae0f-4a87-b0e3-845b3d55e05b', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 62),
('6bb026fc-ae0f-4a87-b0e3-845b3d55e05b', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 54),
('09cfb881-d707-49e5-a2c1-730ce136b710', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 53),
('297af444-055f-4b76-a3ee-fbe65b9752f6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 65);

-- Suppliers
INSERT INTO [Suppliers] ([Id], [Name], [Address], [Email], [WebSite], [CreatedOn], [Service])
VALUES
('CF2052B1-39AD-4D88-9DB8-0D17E7A81D45', N'Нова Линия', N'Тернопольская обл., с. Чистилов', 'office@novaliniya.com.ua', 'https://novaliniya.com.ua/', GETDATE(), N'Носки')

INSERT INTO [ContactPersons] ([Id], [Name], [Phones], [SupplierId])
VALUES
('B3B024EB-D8FB-4F0A-B6A3-1D2CF0784617', N'Леся', '0984162095', 'CF2052B1-39AD-4D88-9DB8-0D17E7A81D45'),
('E944B628-61FE-412D-9D5A-B9C2EA748E06', N'Степан', '0676748494', 'CF2052B1-39AD-4D88-9DB8-0D17E7A81D45')

-- Deliveries
INSERT INTO [Deliveries] ([Id], [BankFee], [Cost], [DeliveryDate], [RequestDate], [SupplierId], [TransferFee])
VALUES
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 222.0, 41319.0, '2018-04-05T00:00:00.0000000', '2018-02-13T00:00:00.0000000', NULL, 940.0),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', 90.0, 16500.0, '2018-06-01T00:00:00.0000000', '2018-05-03T00:00:00.0000000', NULL, 90.0),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', 110.0, 39720.0, '2018-09-26T00:00:00.0000000', '2018-08-26T00:00:00.0000000', NULL, 810.0);


-- Delivery Products
INSERT INTO [DeliveryProducts] ([ProductId], [DeliveryId], [SizeId], [PriceForItem], [Quantity])
VALUES 
('a9ab38d1-c2b2-4c50-9ab9-80335f4561f8', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 25.0, 39),
('a9ab38d1-c2b2-4c50-9ab9-80335f4561f8', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '6e519491-8fd8-45f2-992e-270b01f25971', 25.0, 38),
('5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 27.0, 47),
('5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 27.0, 46),
('85ceb6f2-c29b-4809-b30a-5ccf427a0447', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 30.0, 53),
('85ceb6f2-c29b-4809-b30a-5ccf427a0447', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 30.0, 13),
('09cfb881-d707-49e5-a2c1-730ce136b710', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 30.0, 63),
('09cfb881-d707-49e5-a2c1-730ce136b710', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 30.0, 53),
('84c601ce-a32d-432d-99e2-c23916cf4d1f', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 30.0, 48),
('84c601ce-a32d-432d-99e2-c23916cf4d1f', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 30.0, 51),
('6bb026fc-ae0f-4a87-b0e3-845b3d55e05b', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 20.0, 62),
('6bb026fc-ae0f-4a87-b0e3-845b3d55e05b', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 20.0, 54),
('b62555e5-e51b-41e2-9bf8-6a750edc0d8a', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 20.0, 65),
('b62555e5-e51b-41e2-9bf8-6a750edc0d8a', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 20.0, 66),
('304af5df-1d03-40c3-af40-9c6259898f75', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 20.0, 57),
('304af5df-1d03-40c3-af40-9c6259898f75', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 20.0, 60),
('574f3353-0c6e-4148-a8ef-0db9559f3864', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.0, 64),
('574f3353-0c6e-4148-a8ef-0db9559f3864', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.0, 48),
('32ae1bae-c186-4e7c-a6af-d683e10d1480', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.0, 48),
('32ae1bae-c186-4e7c-a6af-d683e10d1480', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.0, 50),
('8636296d-e47c-4bb8-a6fd-e0cc01d4e27a', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.0, 53),
('8636296d-e47c-4bb8-a6fd-e0cc01d4e27a', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.0, 53),
('db645119-1b9f-4161-966d-97a7cca8d2c7', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 27.0, 52),
('db645119-1b9f-4161-966d-97a7cca8d2c7', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 27.0, 49),
('297af444-055f-4b76-a3ee-fbe65b9752f6', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.0, 65),
('297af444-055f-4b76-a3ee-fbe65b9752f6', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.0, 132),
('1054708a-aa30-4ba6-84f7-321eac6aa041', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 18.7, 160),
('1054708a-aa30-4ba6-84f7-321eac6aa041', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 18.7, 110),
('8823f027-9074-4fa9-a5ef-552a5b08ef5e', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.5, 114),
('8823f027-9074-4fa9-a5ef-552a5b08ef5e', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.5, 115),
('76f1b29c-edac-4ca9-b529-da383c04905b', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 19.6, 112),
('76f1b29c-edac-4ca9-b529-da383c04905b', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 19.6, 105),
('eabc3ce7-3c55-465e-9f27-11033bcc4f33', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.0, 92),
('eabc3ce7-3c55-465e-9f27-11033bcc4f33', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.0, 98),
('d772b195-65e3-4250-8b2c-e2d59e7d24da', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 19.6, 111),
('d772b195-65e3-4250-8b2c-e2d59e7d24da', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 19.6, 108),
('380e3a08-08c5-40b1-b401-ec6b57d2e549', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 21.31, 115),
('380e3a08-08c5-40b1-b401-ec6b57d2e549', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 21.31, 107),
('e536c61e-c2c5-41ec-9205-660726baa18b', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.35, 122),
('e536c61e-c2c5-41ec-9205-660726baa18b', 'b2d8b13b-aa82-4820-ba85-e23501869c3a', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.35, 110),
('5bf3988b-ba17-4802-90ad-b77abe68677a', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 27.0, 61),
('5bf3988b-ba17-4802-90ad-b77abe68677a', '32c74ef3-adfd-4723-a319-9b8984d1b7fb', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 27.0, 57),
('a9899cd5-9b2d-4241-8e28-0d1441933bad', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.0, 44),
('a9899cd5-9b2d-4241-8e28-0d1441933bad', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.0, 43),
('f869224c-80e6-43b6-94a4-2528ecd67a75', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.0, 53),
('f869224c-80e6-43b6-94a4-2528ecd67a75', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.0, 53),
('ba641024-d50a-4f9c-bfd9-a330fe12071e', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '6e519491-8fd8-45f2-992e-270b01f25971', 25.0, 54),
('f9b055d3-5fd9-417f-b71d-0af81c821029', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '6e519491-8fd8-45f2-992e-270b01f25971', 30.0, 40),
('f9b055d3-5fd9-417f-b71d-0af81c821029', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 30.0, 53),
('bddc1231-0952-4c6d-9a30-9de441cfa3a0', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.0, 50),
('bddc1231-0952-4c6d-9a30-9de441cfa3a0', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.0, 48),
('dfa69fa0-2df3-4254-95b7-f65eb4ed6c92', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '6e519491-8fd8-45f2-992e-270b01f25971', 25.0, 56),
('dfa69fa0-2df3-4254-95b7-f65eb4ed6c92', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 25.0, 52),
('365510f0-fb1a-42cd-b249-5ad514bf2f33', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '6e519491-8fd8-45f2-992e-270b01f25971', 25.0, 46),
('365510f0-fb1a-42cd-b249-5ad514bf2f33', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 25.0, 56),
('637b0ba2-f1d9-4bf6-b1c7-1bc685033b36', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 25.0, 58),
('637b0ba2-f1d9-4bf6-b1c7-1bc685033b36', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 25.0, 57),
('55386f8f-3234-42c0-a82a-65ea7dd50b28', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '6e519491-8fd8-45f2-992e-270b01f25971', 25.0, 75),
('55386f8f-3234-42c0-a82a-65ea7dd50b28', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 25.0, 57),
('ba641024-d50a-4f9c-bfd9-a330fe12071e', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 25.0, 52),
('65801c7f-37f6-4452-a304-ceaafc940d08', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 20.0, 52);
('65801c7f-37f6-4452-a304-ceaafc940d08', '4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 20.0, 51),