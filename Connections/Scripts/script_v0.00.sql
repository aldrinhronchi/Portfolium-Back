-- Script database version 0.00

/*
-- Entity

IF OBJECT_ID('dbo.tbl_name', 'U') IS NULL
	CREATE TABLE tbl_name (
	ID int IDENTITY(1,1) NOT NULL,
	GuidID uniqueidentifier NOT NULL DEFAULT NEWID(),
	--Fields
	IsActive bit NOT NULL DEFAULT 1,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateUpdated datetime2(7) NULL,
	UserCreated nvarchar(255) NOT NULL DEFAULT 'System',
	UserUpdated int NULL DEFAULT 0,
	CONSTRAINT PK_Users_ID PRIMARY KEY (ID)
);
GO
*/


-- USER
IF OBJECT_ID('dbo.Users', 'U') IS NULL
	CREATE TABLE Users (
	ID int IDENTITY(1,1) NOT NULL,
	GuidID uniqueidentifier NOT NULL DEFAULT NEWID(),
	Name nvarchar(255) NOT NULL,
	Email nvarchar(255) NOT NULL,
	Password nvarchar(255) NOT NULL,
	Role nvarchar(25) NULL DEFAULT 'admin',
	IsActive bit NOT NULL DEFAULT 1,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateUpdated datetime2(7) NULL,
	UserCreated nvarchar(255) NOT NULL DEFAULT 'System',
	UserUpdated int NULL DEFAULT 0,
	CONSTRAINT PK_Users_ID PRIMARY KEY (ID)
);


-- Receitas
IF OBJECT_ID('dbo.Receitas_Receita', 'U') IS NULL
	CREATE TABLE Receitas_Receita (
	ID int IDENTITY(1,1) NOT NULL,
	GuID uniqueidentifier NOT NULL,
	Nome NVARCHAR(255) NOT NULL,
	Descricao NVARCHAR(255) NOT NULL,
	Etapas TEXT NULL,
	Rendimentos int NULL,
	TempoPreparo int NULL,
	IsActive bit NOT NULL DEFAULT 1,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateUpdated datetime2(7) NULL,
	UserCreated int NOT NULL DEFAULT 0,
	UserUpdated int NULL DEFAULT 0,
	CONSTRAINT Receitas_Receita_ID PRIMARY KEY (ID)
);
GO

-- Ingredientes 
IF OBJECT_ID('dbo.Ingredientes_Receita', 'U') IS NULL
	CREATE TABLE Ingredientes_Receita (
	ID int IDENTITY(1,1) NOT NULL,
	GuID uniqueidentifier NOT NULL,
	Nome NVARCHAR(255) NOT NULL,
	Quantidade DECIMAL(20,2) NOT NULL,
	Medida NVARCHAR(15) NOT NULL,
	Opcional bit NOT NULL DEFAULT 1,
	IDReceita INT NOT NULL,
	IsActive bit NOT NULL DEFAULT 1,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateUpdated datetime2(7) NULL,
	UserCreated int NOT NULL DEFAULT 0,
	UserUpdated int NULL DEFAULT 0,
	CONSTRAINT Ingredientes_Receita_ID PRIMARY KEY (ID)
);
GO

