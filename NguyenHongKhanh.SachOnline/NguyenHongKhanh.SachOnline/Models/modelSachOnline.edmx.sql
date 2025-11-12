
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/09/2025 15:18:48
-- Generated from EDMX file: C:\Users\admin\source\repos\Thuc_Hanh_Lap_Trinh_Web\NguyenHongKhanh.SachOnline\NguyenHongKhanh.SachOnline\Models\modelSachOnline.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SachOnlineData];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CTDH_DDH]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CHITIETDATHANG] DROP CONSTRAINT [FK_CTDH_DDH];
GO
IF OBJECT_ID(N'[dbo].[FK_CTDH_S]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CHITIETDATHANG] DROP CONSTRAINT [FK_CTDH_S];
GO
IF OBJECT_ID(N'[dbo].[FK_DDH_KH]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DONDATHANG] DROP CONSTRAINT [FK_DDH_KH];
GO
IF OBJECT_ID(N'[dbo].[FK_S_CD]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SACH] DROP CONSTRAINT [FK_S_CD];
GO
IF OBJECT_ID(N'[dbo].[FK_Sach_NXB]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SACH] DROP CONSTRAINT [FK_Sach_NXB];
GO
IF OBJECT_ID(N'[dbo].[FK_VS_S]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VIETSACH] DROP CONSTRAINT [FK_VS_S];
GO
IF OBJECT_ID(N'[dbo].[FK_VS_TG]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VIETSACH] DROP CONSTRAINT [FK_VS_TG];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ADMIN]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ADMIN];
GO
IF OBJECT_ID(N'[dbo].[CHITIETDATHANG]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CHITIETDATHANG];
GO
IF OBJECT_ID(N'[dbo].[CHUDE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CHUDE];
GO
IF OBJECT_ID(N'[dbo].[DONDATHANG]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DONDATHANG];
GO
IF OBJECT_ID(N'[dbo].[KHACHHANG]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KHACHHANG];
GO
IF OBJECT_ID(N'[dbo].[NHAXUATBAN]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NHAXUATBAN];
GO
IF OBJECT_ID(N'[dbo].[SACH]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SACH];
GO
IF OBJECT_ID(N'[dbo].[TACGIA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TACGIA];
GO
IF OBJECT_ID(N'[dbo].[VIETSACH]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VIETSACH];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ADMINs'
CREATE TABLE [dbo].[ADMINs] (
    [MaAd] int IDENTITY(1,1) NOT NULL,
    [HoTen] nvarchar(50)  NULL,
    [DienThoai] varchar(10)  NULL,
    [TenDN] varchar(15)  NULL,
    [MatKhau] varchar(15)  NULL,
    [Quyen] int  NULL
);
GO

-- Creating table 'CHITIETDATHANGs'
CREATE TABLE [dbo].[CHITIETDATHANGs] (
    [MaDonHang] int  NOT NULL,
    [MaSach] int  NOT NULL,
    [SoLuong] int  NULL,
    [DonGia] decimal(9,2)  NULL
);
GO

-- Creating table 'CHUDEs'
CREATE TABLE [dbo].[CHUDEs] (
    [MaCD] int IDENTITY(1,1) NOT NULL,
    [TenChuDe] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'DONDATHANGs'
CREATE TABLE [dbo].[DONDATHANGs] (
    [MaDonHang] int IDENTITY(1,1) NOT NULL,
    [DaThanhToan] bit  NULL,
    [TinhTrangGiaoHang] int  NULL,
    [NgayDat] datetime  NULL,
    [NgayGiao] datetime  NULL,
    [MaKH] int  NULL
);
GO

-- Creating table 'KHACHHANGs'
CREATE TABLE [dbo].[KHACHHANGs] (
    [MaKH] int IDENTITY(1,1) NOT NULL,
    [HoTen] nvarchar(50)  NOT NULL,
    [TaiKhoan] varchar(15)  NULL,
    [MatKhau] varchar(15)  NOT NULL,
    [Email] varchar(50)  NULL,
    [DiaChi] nvarchar(50)  NULL,
    [DienThoai] varchar(10)  NULL,
    [NgaySinh] datetime  NULL
);
GO

-- Creating table 'NHAXUATBANs'
CREATE TABLE [dbo].[NHAXUATBANs] (
    [MaNXB] int IDENTITY(1,1) NOT NULL,
    [TenNXB] nvarchar(100)  NOT NULL,
    [DiaChi] nvarchar(150)  NULL,
    [DienThoai] nvarchar(15)  NULL
);
GO

-- Creating table 'SACHes'
CREATE TABLE [dbo].[SACHes] (
    [MaSach] int IDENTITY(1,1) NOT NULL,
    [TenSach] nvarchar(100)  NOT NULL,
    [MoTa] nvarchar(max)  NULL,
    [AnhBia] varchar(50)  NULL,
    [NgayCapNhat] datetime  NULL,
    [SoLuongBan] int  NULL,
    [GiaBan] decimal(19,4)  NULL,
    [MaCD] int  NULL,
    [MaNXB] int  NULL
);
GO

-- Creating table 'TACGIAs'
CREATE TABLE [dbo].[TACGIAs] (
    [MaTG] int IDENTITY(1,1) NOT NULL,
    [TenTG] nvarchar(50)  NOT NULL,
    [DiaChi] nvarchar(100)  NULL,
    [TieuSu] nvarchar(max)  NULL,
    [DienThoai] varchar(15)  NULL
);
GO

-- Creating table 'VIETSACHes'
CREATE TABLE [dbo].[VIETSACHes] (
    [MaTG] int  NOT NULL,
    [MaSach] int  NOT NULL,
    [VaiTro] nvarchar(30)  NULL,
    [ViTri] nvarchar(30)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MaAd] in table 'ADMINs'
ALTER TABLE [dbo].[ADMINs]
ADD CONSTRAINT [PK_ADMINs]
    PRIMARY KEY CLUSTERED ([MaAd] ASC);
GO

-- Creating primary key on [MaDonHang], [MaSach] in table 'CHITIETDATHANGs'
ALTER TABLE [dbo].[CHITIETDATHANGs]
ADD CONSTRAINT [PK_CHITIETDATHANGs]
    PRIMARY KEY CLUSTERED ([MaDonHang], [MaSach] ASC);
GO

-- Creating primary key on [MaCD] in table 'CHUDEs'
ALTER TABLE [dbo].[CHUDEs]
ADD CONSTRAINT [PK_CHUDEs]
    PRIMARY KEY CLUSTERED ([MaCD] ASC);
GO

-- Creating primary key on [MaDonHang] in table 'DONDATHANGs'
ALTER TABLE [dbo].[DONDATHANGs]
ADD CONSTRAINT [PK_DONDATHANGs]
    PRIMARY KEY CLUSTERED ([MaDonHang] ASC);
GO

-- Creating primary key on [MaKH] in table 'KHACHHANGs'
ALTER TABLE [dbo].[KHACHHANGs]
ADD CONSTRAINT [PK_KHACHHANGs]
    PRIMARY KEY CLUSTERED ([MaKH] ASC);
GO

-- Creating primary key on [MaNXB] in table 'NHAXUATBANs'
ALTER TABLE [dbo].[NHAXUATBANs]
ADD CONSTRAINT [PK_NHAXUATBANs]
    PRIMARY KEY CLUSTERED ([MaNXB] ASC);
GO

-- Creating primary key on [MaSach] in table 'SACHes'
ALTER TABLE [dbo].[SACHes]
ADD CONSTRAINT [PK_SACHes]
    PRIMARY KEY CLUSTERED ([MaSach] ASC);
GO

-- Creating primary key on [MaTG] in table 'TACGIAs'
ALTER TABLE [dbo].[TACGIAs]
ADD CONSTRAINT [PK_TACGIAs]
    PRIMARY KEY CLUSTERED ([MaTG] ASC);
GO

-- Creating primary key on [MaTG], [MaSach] in table 'VIETSACHes'
ALTER TABLE [dbo].[VIETSACHes]
ADD CONSTRAINT [PK_VIETSACHes]
    PRIMARY KEY CLUSTERED ([MaTG], [MaSach] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MaDonHang] in table 'CHITIETDATHANGs'
ALTER TABLE [dbo].[CHITIETDATHANGs]
ADD CONSTRAINT [FK_CTDH_DDH]
    FOREIGN KEY ([MaDonHang])
    REFERENCES [dbo].[DONDATHANGs]
        ([MaDonHang])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [MaSach] in table 'CHITIETDATHANGs'
ALTER TABLE [dbo].[CHITIETDATHANGs]
ADD CONSTRAINT [FK_CTDH_S]
    FOREIGN KEY ([MaSach])
    REFERENCES [dbo].[SACHes]
        ([MaSach])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CTDH_S'
CREATE INDEX [IX_FK_CTDH_S]
ON [dbo].[CHITIETDATHANGs]
    ([MaSach]);
GO

-- Creating foreign key on [MaCD] in table 'SACHes'
ALTER TABLE [dbo].[SACHes]
ADD CONSTRAINT [FK_S_CD]
    FOREIGN KEY ([MaCD])
    REFERENCES [dbo].[CHUDEs]
        ([MaCD])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_S_CD'
CREATE INDEX [IX_FK_S_CD]
ON [dbo].[SACHes]
    ([MaCD]);
GO

-- Creating foreign key on [MaKH] in table 'DONDATHANGs'
ALTER TABLE [dbo].[DONDATHANGs]
ADD CONSTRAINT [FK_DDH_KH]
    FOREIGN KEY ([MaKH])
    REFERENCES [dbo].[KHACHHANGs]
        ([MaKH])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DDH_KH'
CREATE INDEX [IX_FK_DDH_KH]
ON [dbo].[DONDATHANGs]
    ([MaKH]);
GO

-- Creating foreign key on [MaNXB] in table 'SACHes'
ALTER TABLE [dbo].[SACHes]
ADD CONSTRAINT [FK_Sach_NXB]
    FOREIGN KEY ([MaNXB])
    REFERENCES [dbo].[NHAXUATBANs]
        ([MaNXB])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Sach_NXB'
CREATE INDEX [IX_FK_Sach_NXB]
ON [dbo].[SACHes]
    ([MaNXB]);
GO

-- Creating foreign key on [MaSach] in table 'VIETSACHes'
ALTER TABLE [dbo].[VIETSACHes]
ADD CONSTRAINT [FK_VS_S]
    FOREIGN KEY ([MaSach])
    REFERENCES [dbo].[SACHes]
        ([MaSach])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VS_S'
CREATE INDEX [IX_FK_VS_S]
ON [dbo].[VIETSACHes]
    ([MaSach]);
GO

-- Creating foreign key on [MaTG] in table 'VIETSACHes'
ALTER TABLE [dbo].[VIETSACHes]
ADD CONSTRAINT [FK_VS_TG]
    FOREIGN KEY ([MaTG])
    REFERENCES [dbo].[TACGIAs]
        ([MaTG])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------