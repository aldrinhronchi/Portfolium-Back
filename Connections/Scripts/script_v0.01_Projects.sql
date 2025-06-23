-- Script para criação da tabela Projects
-- Versão: 0.01
-- Data: $(date)
-- Descrição: Tabela para armazenar projetos do portfólio

USE [Pandora]
GO

-- Verificar se a tabela já existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Projects' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Projects](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [GuidID] [uniqueidentifier] NOT NULL,
        [Name] [nvarchar](200) NOT NULL,
        [Description] [nvarchar](2000) NULL,
        [ShortDescription] [nvarchar](500) NULL,
        [Technologies] [nvarchar](1000) NULL,
        [ProjectUrl] [nvarchar](500) NULL,
        [DemoUrl] [nvarchar](500) NULL,
        [RepositoryUrl] [nvarchar](500) NULL,
        [MainImage] [nvarchar](500) NULL,
        [AdditionalImages] [nvarchar](2000) NULL,
        [Status] [nvarchar](50) NULL,
        [Category] [nvarchar](100) NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT(0),
        [IsFeatured] [bit] NOT NULL DEFAULT(0),
        [StartDate] [datetime] NULL,
        [EndDate] [datetime] NULL,
        [UserID] [int] NULL,
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [DateCreated] [datetime] NOT NULL DEFAULT(GETDATE()),
        [DateUpdated] [datetime] NULL,
        [UserCreated] [nvarchar](100) NULL,
        [UserUpdated] [nvarchar](100) NULL,
        
        CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [UQ_Projects_GuidID] UNIQUE NONCLUSTERED ([GuidID] ASC)
    ) ON [PRIMARY]

    PRINT 'Tabela Projects criada com sucesso!'
END
ELSE
BEGIN
    PRINT 'Tabela Projects já existe!'
END
GO

-- Verificar se existe FK para Users
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Projects_Users')
BEGIN
    ALTER TABLE [dbo].[Projects] WITH CHECK 
    ADD CONSTRAINT [FK_Projects_Users] FOREIGN KEY([UserID])
    REFERENCES [dbo].[Users] ([ID])
    
    ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Users]
    
    PRINT 'Foreign Key FK_Projects_Users criada com sucesso!'
END
ELSE
BEGIN
    PRINT 'Foreign Key FK_Projects_Users já existe!'
END
GO

-- Criar índices para melhor performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Projects_IsActive_IsFeatured')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Projects_IsActive_IsFeatured]
    ON [dbo].[Projects] ([IsActive] ASC, [IsFeatured] ASC)
    INCLUDE ([Name], [Category], [DisplayOrder])
    
    PRINT 'Índice IX_Projects_IsActive_IsFeatured criado com sucesso!'
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Projects_Category_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Projects_Category_IsActive]
    ON [dbo].[Projects] ([Category] ASC, [IsActive] ASC)
    INCLUDE ([Name], [DisplayOrder], [DateCreated])
    
    PRINT 'Índice IX_Projects_Category_IsActive criado com sucesso!'
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Projects_UserID_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Projects_UserID_IsActive]
    ON [dbo].[Projects] ([UserID] ASC, [IsActive] ASC)
    INCLUDE ([Name], [DisplayOrder], [DateCreated])
    
    PRINT 'Índice IX_Projects_UserID_IsActive criado com sucesso!'
END
GO

-- Inserir dados de exemplo (opcional)
IF NOT EXISTS (SELECT * FROM [dbo].[Projects] WHERE [Name] = 'Portfolium - Sistema de Portfólio')
BEGIN
    INSERT INTO [dbo].[Projects] (
        [GuidID], [Name], [Description], [ShortDescription], [Technologies], 
        [ProjectUrl], [DemoUrl], [RepositoryUrl], [MainImage], 
        [Status], [Category], [DisplayOrder], [IsFeatured], 
        [StartDate], [UserID], [IsActive], [UserCreated]
    ) VALUES (
        NEWID(), 
        'Portfolium - Sistema de Portfólio', 
        'Sistema completo para criação e gerenciamento de portfólios pessoais e profissionais. Desenvolvido com Angular no frontend e .NET 6 no backend, oferece uma interface moderna e responsiva para showcasing de projetos.',
        'Sistema moderno para criação de portfólios profissionais',
        'Angular, .NET 6, Entity Framework, SQL Server, Bootstrap, TypeScript',
        'https://portfolium.com',
        'https://demo.portfolium.com',
        'https://github.com/user/portfolium',
        '/images/projects/portfolium-main.jpg',
        'Em Desenvolvimento',
        'Web Application',
        1,
        1,
        GETDATE(),
        1,
        1,
        '1 - Sistema'
    )
    
    PRINT 'Projeto de exemplo inserido com sucesso!'
END
GO

PRINT 'Script executado com sucesso! Tabela Projects está pronta para uso.'
GO 