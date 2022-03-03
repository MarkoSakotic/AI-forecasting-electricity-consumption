
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/24/2022 03:52:08
-- Generated from EDMX file: C:\Users\Marko\Desktop\ISIS - projekat\PrognozaPotrosnjeElEnergije\DataLayer\WeatherConditions.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WeatherConditions];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[WeatherSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WeatherSet];
GO
IF OBJECT_ID(N'[dbo].[WeatherForecastSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WeatherForecastSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'WeatherSet'
CREATE TABLE [dbo].[WeatherSet] (
    [WeatherId] int IDENTITY(1,1) NOT NULL,
    [AirTemperature] float  NOT NULL,
    [AtmosphericPressure] float  NOT NULL,
    [PressureTendency] float  NOT NULL,
    [RelativeHumidity] float  NOT NULL,
    [Pressure] float  NOT NULL,
    [CloudCover] float  NOT NULL,
    [LocalTime] datetime  NOT NULL,
    [LoadMWh] float  NOT NULL,
    [DewPointTemperature] float  NOT NULL,
    [MeanWindSpeed] float  NOT NULL,
    [Day] float  NOT NULL,
    [Month] float  NOT NULL,
    [Hour] float  NOT NULL,
    [TypeOfDay] float  NOT NULL
);
GO

-- Creating table 'WeatherForecastSet'
CREATE TABLE [dbo].[WeatherForecastSet] (
    [ForecastId] int IDENTITY(1,1) NOT NULL,
    [AtmosphericPressure] float  NOT NULL,
    [AirTemperature] float  NOT NULL,
    [PressureTendency] float  NOT NULL,
    [RelativeHumidity] float  NOT NULL,
    [Pressure] float  NOT NULL,
    [CloudCover] float  NOT NULL,
    [LocalTime] datetime  NOT NULL,
    [DewPointTemperature] float  NOT NULL,
    [MeanWindSpeed] float  NOT NULL,
    [Month] float  NOT NULL,
    [Day] float  NOT NULL,
    [Hour] float  NOT NULL,
    [TypeOfDay] float  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [WeatherId] in table 'WeatherSet'
ALTER TABLE [dbo].[WeatherSet]
ADD CONSTRAINT [PK_WeatherSet]
    PRIMARY KEY CLUSTERED ([WeatherId] ASC);
GO

-- Creating primary key on [ForecastId] in table 'WeatherForecastSet'
ALTER TABLE [dbo].[WeatherForecastSet]
ADD CONSTRAINT [PK_WeatherForecastSet]
    PRIMARY KEY CLUSTERED ([ForecastId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------