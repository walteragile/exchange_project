CREATE DATABASE ExchangeDb
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

CREATE TABLE [ExchangePurchases] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Currency] nvarchar(3) NOT NULL,
    [Date] datetime2 NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_ExchangePurchases] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200705183416_InitialMigration', N'3.1.5');
GO
