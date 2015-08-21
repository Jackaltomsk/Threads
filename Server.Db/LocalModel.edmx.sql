
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/21/2015 20:21:06
-- Generated from EDMX file: D:\projects\Test\Server.Db\LocalModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UsersCoordinates]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Coordinates] DROP CONSTRAINT [FK_UsersCoordinates];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Coordinates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Coordinates];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Coordinates'
CREATE TABLE [dbo].[Coordinates] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Latitude] decimal(9,6)  NOT NULL,
    [Longtitude] decimal(9,6)  NOT NULL,
    [UserId] bigint  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Password] uniqueidentifier  NOT NULL,
    [Name] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Coordinates'
ALTER TABLE [dbo].[Coordinates]
ADD CONSTRAINT [PK_Coordinates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'Coordinates'
ALTER TABLE [dbo].[Coordinates]
ADD CONSTRAINT [FK_UsersCoordinates]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersCoordinates'
CREATE INDEX [IX_FK_UsersCoordinates]
ON [dbo].[Coordinates]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------