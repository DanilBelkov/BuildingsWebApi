
CREATE DATABASE [Buildings] 
    ON (NAME = N'Buildings', FILENAME = N'D:\Other\SQLServer\MSSQL16.SQLEXPRESS\MSSQL\DATA\Buildings.mdf', SIZE = 1024MB, FILEGROWTH = 256MB)
LOG ON (NAME = N'Buildings_log', FILENAME = N'D:\Other\SQLServer\MSSQL16.SQLEXPRESS\MSSQL\DATA\Buildings_log.ldf', SIZE = 512MB, FILEGROWTH = 125MB)
GO