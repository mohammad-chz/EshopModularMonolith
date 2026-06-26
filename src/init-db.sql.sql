-- init-database.sql
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'EShopDb')
BEGIN
    CREATE DATABASE EShopDb;
    PRINT 'Database EShopDb created successfully.';
END
ELSE
BEGIN
    PRINT 'Database EShopDb already exists.';
END
GO