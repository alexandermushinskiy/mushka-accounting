﻿USE [Mushka]

-- Sizes
INSERT INTO [Sizes] ([Id], [Name])
VALUES
('ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2', '36-39'),
('2DFA21EF-5EED-462F-B5E5-06EE31281BA2', '41-45'),
('FB8356A5-1629-4F9F-9B51-3D40E0E55F84', '39-42'),
('6E519491-8FD8-45F2-992E-270B01F25971', '43-46');

-- Categories
INSERT INTO [Categories] ([Id], [Name], [IsSizeRequired], [Order])
VALUES
('88CD0F34-9D4A-4E45-BE97-8899A97FB82C', N'Носки', 1, 1),
('0E7BE1DE-267C-4C0A-8EE9-ABA0A267F27A', N'Упаковка', 1, 2),
('B425D75B-2E72-45F0-A55D-3BA400051E5F', N'Другое', 0, 3);

-- Products
INSERT INTO [Products] ([Id], [CategoryId], [VendorCode], [CreatedOn], [Name], [SizeId], [Quantity])
VALUES
('a9ab38d1-c2b2-4c50-9ab9-80335f4561f8', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'AIR001', '2018-11-06T00:00:00.0000000', N'Airplane', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 39),
('a9ab38d1-c2b2-4c50-9ab9-80335f4561f9', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'AIR002', '2018-11-06T00:00:00.0000000', N'Airplane', '6e519491-8fd8-45f2-992e-270b01f25971', 38),
('8823f027-9074-4fa9-a5ef-552a5b08ef5e', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'FLM001', '2018-11-06T00:00:00.0000000', N'Flamingo', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 115),
('8823f027-9074-4fa9-a5ef-552a5b08ef5f', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'FLM002', '2018-11-06T00:00:00.0000000', N'Flamingo', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 114),
('76f1b29c-edac-4ca9-b529-da383c04905b', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'GRE001', '2018-11-06T00:00:00.0000000', N'Deep Green', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 105),
('76f1b29c-edac-4ca9-b529-da383c04905c', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'GRE002', '2018-11-06T00:00:00.0000000', N'Deep Green', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 112),
('eabc3ce7-3c55-465e-9f27-11033bcc4f33', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'CAC001', '2018-11-06T00:00:00.0000000', N'Cactus', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 98),
('eabc3ce7-3c55-465e-9f27-11033bcc4f34', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'CAC002', '2018-11-06T00:00:00.0000000', N'Cactus', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 92),
('d772b195-65e3-4250-8b2c-e2d59e7d24da', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BEE001', '2018-11-06T00:00:00.0000000', N'Bumble-bee', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 108),
('d772b195-65e3-4250-8b2c-e2d59e7d24db', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BEE002', '2018-11-06T00:00:00.0000000', N'Bumble-bee', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 111),
('380e3a08-08c5-40b1-b401-ec6b57d2e549', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SAI001', '2018-11-06T00:00:00.0000000', N'Sailor', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 107),
('380e3a08-08c5-40b1-b401-ec6b57d2e54a', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SAI002', '2018-11-06T00:00:00.0000000', N'Sailor', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 115),
('e536c61e-c2c5-41ec-9205-660726baa18b', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'PAS001', '2018-11-06T00:00:00.0000000', N'Royal Passion', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 110),
('e536c61e-c2c5-41ec-9205-660726baa18c', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'PAS002', '2018-11-06T00:00:00.0000000', N'Royal Passion', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 122),
('5bf3988b-ba17-4802-90ad-b77abe68677a', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'LAM001', '2018-11-06T00:00:00.0000000', N'Lamp', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 57),
('5bf3988b-ba17-4802-90ad-b77abe68677b', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'LAM002', '2018-11-06T00:00:00.0000000', N'Lamp', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 61),
('db645119-1b9f-4161-966d-97a7cca8d2c7', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'PEP001', '2018-11-06T00:00:00.0000000', N'Pepper', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 52),
('db645119-1b9f-4161-966d-97a7cca8d2c8', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'PEP002', '2018-11-06T00:00:00.0000000', N'Pepper', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 49),
('5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BAN001', '2018-11-06T00:00:00.0000000', N'Banana', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 47),
('5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7e', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BAN002', '2018-11-06T00:00:00.0000000', N'Banana', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 46),
('85ceb6f2-c29b-4809-b30a-5ccf427a0447', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'YOG001', '2018-11-06T00:00:00.0000000', N'Yoga', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 13),
('85ceb6f2-c29b-4809-b30a-5ccf427a0448', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'YOG002', '2018-11-06T00:00:00.0000000', N'Yoga', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 53),
('09cfb881-d707-49e5-a2c1-730ce136b710', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'EIN001', '2018-11-06T00:00:00.0000000', N'Einstein', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 53),
('09cfb881-d707-49e5-a2c1-730ce136b711', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'EIN002', '2018-11-06T00:00:00.0000000', N'Einstein', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 63),
('84c601ce-a32d-432d-99e2-c23916cf4d1f', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'STE001', '2018-11-06T00:00:00.0000000', N'Jobsy', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 51),
('84c601ce-a32d-432d-99e2-c23916cf4d10', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'STE002', '2018-11-06T00:00:00.0000000', N'Jobsy', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 48),
('6bb026fc-ae0f-4a87-b0e3-845b3d55e05b', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SBY001', '2018-11-06T00:00:00.0000000', N'Navy', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 54),
('6bb026fc-ae0f-4a87-b0e3-845b3d55e05c', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SBY002', '2018-11-06T00:00:00.0000000', N'Navy', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 62),
('1054708a-aa30-4ba6-84f7-321eac6aa041', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'MST001', '2018-11-06T00:00:00.0000000', N'Multi stripe', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 110),
('1054708a-aa30-4ba6-84f7-321eac6aa042', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'MST002', '2018-11-06T00:00:00.0000000', N'Multi stripe', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 160),
('b62555e5-e51b-41e2-9bf8-6a750edc0d8a', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SWR001', '2018-11-06T00:00:00.0000000', N'Cherry', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 65),
('b62555e5-e51b-41e2-9bf8-6a750edc0d8b', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SWR002', '2018-11-06T00:00:00.0000000', N'Cherry', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 66),
('574f3353-0c6e-4148-a8ef-0db9559f3864', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'GAL001', '2018-11-06T00:00:00.0000000', N'Spaceman', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 48),
('574f3353-0c6e-4148-a8ef-0db9559f3865', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'GAL002', '2018-11-06T00:00:00.0000000', N'Spaceman', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 64),
('32ae1bae-c186-4e7c-a6af-d683e10d1480', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DOW001', '2018-11-06T00:00:00.0000000', N'Apelsinka', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 50),
('32ae1bae-c186-4e7c-a6af-d683e10d1481', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DOW002', '2018-11-06T00:00:00.0000000', N'Apelsinka', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 48),
('8636296d-e47c-4bb8-a6fd-e0cc01d4e27a', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRW001', '2018-11-06T00:00:00.0000000', N'Beboss dot', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 53),
('8636296d-e47c-4bb8-a6fd-e0cc01d4e27b', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRW002', '2018-11-06T00:00:00.0000000', N'Beboss dot', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 53),
('65801c7f-37f6-4452-a304-ceaafc940d08', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DGY001', '2018-11-06T00:00:00.0000000', N'Avo-avocado', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 52),
('65801c7f-37f6-4452-a304-ceaafc940d09', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DGY002', '2018-11-06T00:00:00.0000000', N'Avo-avocado', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 51),
('637b0ba2-f1d9-4bf6-b1c7-1bc685033b36', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'CFF001', '2018-11-06T00:00:00.0000000', N'CoffeeOk', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 57),
('637b0ba2-f1d9-4bf6-b1c7-1bc685033b37', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'CFF002', '2018-11-06T00:00:00.0000000', N'CoffeeOk', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 58),
('a9899cd5-9b2d-4241-8e28-0d1441933bad', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'CAM001', '2018-11-06T00:00:00.0000000', N'Smile', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 43),
('a9899cd5-9b2d-4241-8e28-0d1441933bae', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'CAM002', '2018-11-06T00:00:00.0000000', N'Smile', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 44),
('f869224c-80e6-43b6-94a4-2528ecd67a75', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BER001', '2018-11-06T00:00:00.0000000', N'Beer', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 53),
('f869224c-80e6-43b6-94a4-2528ecd67a76', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BER002', '2018-11-06T00:00:00.0000000', N'Beer', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 53),
('bddc1231-0952-4c6d-9a30-9de441cfa3a0', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BDM001', '2018-11-06T00:00:00.0000000', N'Badminton', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 50),
('bddc1231-0952-4c6d-9a30-9de441cfa3a1', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BDM002', '2018-11-06T00:00:00.0000000', N'Badminton', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 48),
('55386f8f-3234-42c0-a82a-65ea7dd50b28', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRJ001', '2018-11-06T00:00:00.0000000', N'Yoda', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 57),
('55386f8f-3234-42c0-a82a-65ea7dd50b29', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRJ002', '2018-11-06T00:00:00.0000000', N'Yoda', '6e519491-8fd8-45f2-992e-270b01f25971', 75),
('ba641024-d50a-4f9c-bfd9-a330fe12071e', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRV001', '2018-11-06T00:00:00.0000000', N'Shturmovik', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 52),
('ba641024-d50a-4f9c-bfd9-a330fe12071f', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRV002', '2018-11-06T00:00:00.0000000', N'Shturmovik', '6e519491-8fd8-45f2-992e-270b01f25971', 54),
('365510f0-fb1a-42cd-b249-5ad514bf2f33', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRT001', '2018-11-06T00:00:00.0000000', N'Dartik', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 56),
('365510f0-fb1a-42cd-b249-5ad514bf2f34', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'DRT002', '2018-11-06T00:00:00.0000000', N'Dartik', '6e519491-8fd8-45f2-992e-270b01f25971', 46),
('f9b055d3-5fd9-417f-b71d-0af81c821029', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'MOT001', '2018-11-06T00:00:00.0000000', N'Motobike', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 53),
('f9b055d3-5fd9-417f-b71d-0af81c82102a', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'MOT002', '2018-11-06T00:00:00.0000000', N'Motobike', '6e519491-8fd8-45f2-992e-270b01f25971', 40),
('dfa69fa0-2df3-4254-95b7-f65eb4ed6c92', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BIK001', '2018-11-06T00:00:00.0000000', N'Bike', 'fb8356a5-1629-4f9f-9b51-3d40e0e55f84', 52),
('dfa69fa0-2df3-4254-95b7-f65eb4ed6c93', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'BIK002', '2018-11-06T00:00:00.0000000', N'Bike', '6e519491-8fd8-45f2-992e-270b01f25971', 56),
('304af5df-1d03-40c3-af40-9c6259898f75', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SWY001', '2018-11-06T00:00:00.0000000', N'Limono', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 60),
('304af5df-1d03-40c3-af40-9c6259898f76', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'SWY002', '2018-11-06T00:00:00.0000000', N'Limono', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 57),
('297af444-055f-4b76-a3ee-fbe65b9752f6', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'ORA001', '2018-11-06T00:00:00.0000000', N'Orange mood', 'eccef8a9-2c41-4270-9001-d0eb7e21b9e2', 132),
('297af444-055f-4b76-a3ee-fbe65b9752f7', '88cd0f34-9d4a-4e45-be97-8899a97fb82c', N'ORA002', '2018-11-06T00:00:00.0000000', N'Orange mood', '2dfa21ef-5eed-462f-b5e5-06ee31281ba2', 65),
-- Business card
('4B307570-7250-4867-B7C0-EC1DB6475D5B', 'B425D75B-2E72-45F0-A55D-3BA400051E5F', N'BCARD1', '2019-01-04', N'Визитка', NULL, 0),
-- Post card
('07DF9000-2680-43E7-BA2C-D4F0C48A8CB5', 'B425D75B-2E72-45F0-A55D-3BA400051E5F', N'PCARD1', '2019-01-04', N'Открытка', NULL, 0);

-- Suppliers
-- Нова Линия
INSERT INTO [Suppliers] ([Id], [Name], [Address], [Email], [WebSite], [CreatedOn], [Service])
VALUES
('CF2052B1-39AD-4D88-9DB8-0D17E7A81D45', N'Нова Линия', N'Тернопольская обл., с. Чистилов', 'office@novaliniya.com.ua', 'https://novaliniya.com.ua/', GETDATE(), N'Носки')
INSERT INTO [ContactPersons] ([Id], [Name], [Phones], [SupplierId])
VALUES
('B3B024EB-D8FB-4F0A-B6A3-1D2CF0784617', N'Леся', '0984162095', 'CF2052B1-39AD-4D88-9DB8-0D17E7A81D45'),
('E944B628-61FE-412D-9D5A-B9C2EA748E06', N'Степан', '0676748494', 'CF2052B1-39AD-4D88-9DB8-0D17E7A81D45')
INSERT INTO [PaymentCards] ([Id], [Number], [Owner], [SupplierId])
VALUES
('DCC06950-941E-4D1E-A2A4-8347C287775B', '5363542600860200', N'Чубак Степан', 'CF2052B1-39AD-4D88-9DB8-0D17E7A81D45')

-- ЮРГ
INSERT INTO [Suppliers] ([Id], [Name], [Address], [Email], [WebSite], [CreatedOn], [Service])
VALUES
('D70A24DC-BC90-414F-B79A-725920CC5470', N'ЮРГ', N'Одесса, ул. Малая Арнаутская, 39', NULL, 'http://yrg.com.ua/', GETDATE(), N'Визитки')
INSERT INTO [ContactPersons] ([Id], [Name], [Phones], [SupplierId])
VALUES
('36C6230B-0C40-4997-A2D5-0CC6B6D59DA1', N'Алина Вергелес', '048 734 40 52', 'D70A24DC-BC90-414F-B79A-725920CC5470')

-- PrintOk
INSERT INTO [Suppliers] ([Id], [Name], [Address], [Email], [WebSite], [CreatedOn], [Service])
VALUES
('2DAD45C3-4129-474B-B880-AD5221E0D1E8', N'PrintOk', N'Львов', 'vlapash@gmail.com', NULL, GETDATE(), N'Бирки, коробки-улыбка')
INSERT INTO [ContactPersons] ([Id], [Name], [Phones], [Email], [SupplierId])
VALUES
('1D6F552F-E331-4ED2-A4D2-B78435DFB317', N'Владимир Папаш', '0984162095', NULL, '2DAD45C3-4129-474B-B880-AD5221E0D1E8')
INSERT INTO [PaymentCards] ([Id], [Number], [Owner], [SupplierId])
VALUES
('C14C7859-4F27-49F7-A560-7A3603D29970', '5168755532242266', N'Папаш Володомир Миронович', '2DAD45C3-4129-474B-B880-AD5221E0D1E8'),
('7F71AFB7-5C9B-458F-A218-7FF9CA6D09E5', '5168755420757193', N'Тиндик Ігор Миколаєвич', '2DAD45C3-4129-474B-B880-AD5221E0D1E8')

-- Supplies
INSERT INTO [Supplies] ([Id], [BankFee], [Cost], [CostMethod], [ReceivedDate], [RequestDate], [SupplierId], [DeliveryCost], [DeliveryCostMethod], [TotalCost])
VALUES
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 222.0, 41319.0, 2, '2018-04-05T00:00:00.0000000', '2018-02-13T00:00:00.0000000', 'CF2052B1-39AD-4D88-9DB8-0D17E7A81D45', 940.0, 1, 42481),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', 90.0, 16500.0, 2, '2018-06-01T00:00:00.0000000', '2018-05-03T00:00:00.0000000', 'CF2052B1-39AD-4D88-9DB8-0D17E7A81D45', 90.0, 1, 16680),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', 110.0, 39720.0, 2, '2018-09-26T00:00:00.0000000', '2018-08-26T00:00:00.0000000', 'CF2052B1-39AD-4D88-9DB8-0D17E7A81D45', 810.0, 1, 40640);


-- Supply Products

INSERT INTO [SupplyProducts] ([SupplyId], [ProductId], [CostForItem], [Quantity])
VALUES
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'a9ab38d1-c2b2-4c50-9ab9-80335f4561f8', 25.0, 39),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'a9ab38d1-c2b2-4c50-9ab9-80335f4561f9', 25.0, 38),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '6bb026fc-ae0f-4a87-b0e3-845b3d55e05b', 20.0, 54),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '6bb026fc-ae0f-4a87-b0e3-845b3d55e05c', 20.0, 62),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'b62555e5-e51b-41e2-9bf8-6a750edc0d8a', 20.0, 65),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'b62555e5-e51b-41e2-9bf8-6a750edc0d8b', 20.0, 66),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '304af5df-1d03-40c3-af40-9c6259898f75', 20.0, 60),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '304af5df-1d03-40c3-af40-9c6259898f76', 20.0, 57),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '574f3353-0c6e-4148-a8ef-0db9559f3864', 25.0, 48),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '574f3353-0c6e-4148-a8ef-0db9559f3865', 25.0, 64),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '32ae1bae-c186-4e7c-a6af-d683e10d1480', 25.0, 50),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '32ae1bae-c186-4e7c-a6af-d683e10d1481', 25.0, 48),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '8636296d-e47c-4bb8-a6fd-e0cc01d4e27a', 25.0, 53),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '8636296d-e47c-4bb8-a6fd-e0cc01d4e27b', 25.0, 53),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'a9899cd5-9b2d-4241-8e28-0d1441933bad', 25.0, 43),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'a9899cd5-9b2d-4241-8e28-0d1441933bae', 25.0, 44),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'f869224c-80e6-43b6-94a4-2528ecd67a75', 25.0, 53),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'f869224c-80e6-43b6-94a4-2528ecd67a76', 25.0, 53),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'ba641024-d50a-4f9c-bfd9-a330fe12071e', 25.0, 52),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'ba641024-d50a-4f9c-bfd9-a330fe12071f', 25.0, 54),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'f9b055d3-5fd9-417f-b71d-0af81c821029', 30.0, 53),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'f9b055d3-5fd9-417f-b71d-0af81c82102a', 30.0, 40),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'bddc1231-0952-4c6d-9a30-9de441cfa3a0', 25.0, 50),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'bddc1231-0952-4c6d-9a30-9de441cfa3a1', 25.0, 48),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'dfa69fa0-2df3-4254-95b7-f65eb4ed6c92', 25.0, 52),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', 'dfa69fa0-2df3-4254-95b7-f65eb4ed6c93', 25.0, 56),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '365510f0-fb1a-42cd-b249-5ad514bf2f33', 25.0, 56),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '365510f0-fb1a-42cd-b249-5ad514bf2f34', 25.0, 46),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '637b0ba2-f1d9-4bf6-b1c7-1bc685033b36', 25.0, 57),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '637b0ba2-f1d9-4bf6-b1c7-1bc685033b37', 25.0, 58),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '55386f8f-3234-42c0-a82a-65ea7dd50b28', 25.0, 57),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '55386f8f-3234-42c0-a82a-65ea7dd50b29', 25.0, 75),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '65801c7f-37f6-4452-a304-ceaafc940d08', 20.0, 52),
('4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6', '65801c7f-37f6-4452-a304-ceaafc940d09', 20.0, 51),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', '5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d', 27.0, 47),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', '5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7e', 27.0, 46),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', '85ceb6f2-c29b-4809-b30a-5ccf427a0447', 30.0, 13),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', '85ceb6f2-c29b-4809-b30a-5ccf427a0448', 30.0, 53),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', '09cfb881-d707-49e5-a2c1-730ce136b710', 30.0, 53),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', '09cfb881-d707-49e5-a2c1-730ce136b711', 30.0, 63),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', '84c601ce-a32d-432d-99e2-c23916cf4d1f', 30.0, 51),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', '84c601ce-a32d-432d-99e2-c23916cf4d10', 30.0, 48),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', 'db645119-1b9f-4161-966d-97a7cca8d2c7', 27.0, 52),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', 'db645119-1b9f-4161-966d-97a7cca8d2c8', 27.0, 49),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', '5bf3988b-ba17-4802-90ad-b77abe68677a', 27.0, 57),
('32c74ef3-adfd-4723-a319-9b8984d1b7fb', '5bf3988b-ba17-4802-90ad-b77abe68677b', 27.0, 61),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', '297af444-055f-4b76-a3ee-fbe65b9752f6', 25.0, 132),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', '297af444-055f-4b76-a3ee-fbe65b9752f7', 25.0, 65),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', '1054708a-aa30-4ba6-84f7-321eac6aa041', 18.7, 110),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', '1054708a-aa30-4ba6-84f7-321eac6aa042', 18.7, 160),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', '8823f027-9074-4fa9-a5ef-552a5b08ef5e', 25.5, 115),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', '8823f027-9074-4fa9-a5ef-552a5b08ef5f', 25.5, 114),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', '76f1b29c-edac-4ca9-b529-da383c04905b', 19.6, 105),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', '76f1b29c-edac-4ca9-b529-da383c04905c', 19.6, 112),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', 'eabc3ce7-3c55-465e-9f27-11033bcc4f33', 25.0, 98),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', 'eabc3ce7-3c55-465e-9f27-11033bcc4f34', 25.0, 92),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', 'd772b195-65e3-4250-8b2c-e2d59e7d24da', 19.6, 108),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', 'd772b195-65e3-4250-8b2c-e2d59e7d24db', 19.6, 111),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', '380e3a08-08c5-40b1-b401-ec6b57d2e549', 21.31, 107),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', '380e3a08-08c5-40b1-b401-ec6b57d2e54a', 21.31, 115),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', 'e536c61e-c2c5-41ec-9205-660726baa18b', 25.35, 110),
('b2d8b13b-aa82-4820-ba85-e23501869c3a', 'e536c61e-c2c5-41ec-9205-660726baa18c', 25.35, 122);