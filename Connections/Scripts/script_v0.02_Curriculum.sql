-- Script para criação das tabelas do sistema de Currículo
-- Versão: 0.02
-- Data: $(date)
-- Descrição: Tabelas para armazenar informações de currículo, skills, experiências, educação, certificações e serviços

USE [Pandora]
GO

-- =============================================
-- TABELA: PersonalInfo
-- Descrição: Informações pessoais do currículo
-- =============================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PersonalInfo' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[PersonalInfo](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [GuidID] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
        [Name] [nvarchar](100) NOT NULL,
        [Title] [nvarchar](100) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [Location] [nvarchar](100) NULL,
        [Phone] [nvarchar](20) NULL,
        [Email] [nvarchar](100) NULL,
        [ProfileImage] [nvarchar](255) NULL,
        [YearsExperience] [int] NOT NULL DEFAULT(0),
        [ProjectsCompleted] [int] NOT NULL DEFAULT(0),
        [HappyClients] [int] NOT NULL DEFAULT(0),
        [Certifications] [int] NOT NULL DEFAULT(0),
        [LinkedInUrl] [nvarchar](255) NULL,
        [GitHubUrl] [nvarchar](255) NULL,
        [PortfolioUrl] [nvarchar](255) NULL,
        [UserID] [uniqueidentifier] NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [DateCreated] [datetime2](7) NOT NULL DEFAULT(GETDATE()),
        [DateUpdated] [datetime2](7) NULL,
        [UserCreated] [nvarchar](255) NOT NULL DEFAULT('System'),
        [UserUpdated] [nvarchar](255) NULL,
        
        CONSTRAINT [PK_PersonalInfo] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [UQ_PersonalInfo_GuidID] UNIQUE NONCLUSTERED ([GuidID] ASC),
        CONSTRAINT [UQ_PersonalInfo_UserID] UNIQUE NONCLUSTERED ([UserID] ASC)
    ) ON [PRIMARY]

    PRINT 'Tabela PersonalInfo criada com sucesso!'
END
ELSE
BEGIN
    PRINT 'Tabela PersonalInfo já existe!'
END
GO

-- =============================================
-- TABELA: Skills
-- Descrição: Habilidades técnicas
-- =============================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Skills' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Skills](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [GuidID] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
        [Name] [nvarchar](50) NOT NULL,
        [Level] [int] NOT NULL DEFAULT(0),
        [Category] [nvarchar](20) NOT NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT(0),
        [Icon] [nvarchar](50) NULL,
        [Color] [nvarchar](7) NULL,
        [UserID] [uniqueidentifier] NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [DateCreated] [datetime2](7) NOT NULL DEFAULT(GETDATE()),
        [DateUpdated] [datetime2](7) NULL,
        [UserCreated] [nvarchar](255) NOT NULL DEFAULT('System'),
        [UserUpdated] [nvarchar](255) NULL,
        
        CONSTRAINT [PK_Skills] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [UQ_Skills_GuidID] UNIQUE NONCLUSTERED ([GuidID] ASC),
        CONSTRAINT [CK_Skills_Level] CHECK ([Level] >= 0 AND [Level] <= 100)
    ) ON [PRIMARY]

    PRINT 'Tabela Skills criada com sucesso!'
END
ELSE
BEGIN
    PRINT 'Tabela Skills já existe!'
END
GO

-- =============================================
-- TABELA: Experiences
-- Descrição: Experiências profissionais
-- =============================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Experiences' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Experiences](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [GuidID] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
        [Title] [nvarchar](100) NOT NULL,
        [Company] [nvarchar](100) NOT NULL,
        [StartDate] [datetime] NOT NULL,
        [EndDate] [datetime] NULL,
        [Location] [nvarchar](100) NULL,
        [Description] [nvarchar](1000) NULL,
        [Responsibilities] [nvarchar](max) NULL, -- JSON array
        [Technologies] [nvarchar](max) NULL, -- JSON array
        [Achievements] [nvarchar](max) NULL, -- JSON array
        [IsCurrentJob] [bit] NOT NULL DEFAULT(0),
        [DisplayOrder] [int] NOT NULL DEFAULT(0),
        [UserID] [uniqueidentifier] NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [DateCreated] [datetime2](7) NOT NULL DEFAULT(GETDATE()),
        [DateUpdated] [datetime2](7) NULL,
        [UserCreated] [nvarchar](255) NOT NULL DEFAULT('System'),
        [UserUpdated] [nvarchar](255) NULL,
        
        CONSTRAINT [PK_Experiences] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [UQ_Experiences_GuidID] UNIQUE NONCLUSTERED ([GuidID] ASC)
    ) ON [PRIMARY]

    PRINT 'Tabela Experiences criada com sucesso!'
END
ELSE
BEGIN
    PRINT 'Tabela Experiences já existe!'
END
GO

-- =============================================
-- TABELA: Education
-- Descrição: Formação educacional
-- =============================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Education' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Education](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [GuidID] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
        [Degree] [nvarchar](100) NOT NULL,
        [Institution] [nvarchar](100) NOT NULL,
        [StartDate] [datetime] NOT NULL,
        [EndDate] [datetime] NULL,
        [Location] [nvarchar](100) NULL,
        [Description] [nvarchar](1000) NULL,
        [Grade] [nvarchar](10) NULL,
        [Achievements] [nvarchar](max) NULL, -- JSON array
        [DisplayOrder] [int] NOT NULL DEFAULT(0),
        [UserID] [uniqueidentifier] NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [DateCreated] [datetime2](7) NOT NULL DEFAULT(GETDATE()),
        [DateUpdated] [datetime2](7) NULL,
        [UserCreated] [nvarchar](255) NOT NULL DEFAULT('System'),
        [UserUpdated] [nvarchar](255) NULL,
        
        CONSTRAINT [PK_Education] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [UQ_Education_GuidID] UNIQUE NONCLUSTERED ([GuidID] ASC)
    ) ON [PRIMARY]

    PRINT 'Tabela Education criada com sucesso!'
END
ELSE
BEGIN
    PRINT 'Tabela Education já existe!'
END
GO

-- =============================================
-- TABELA: Certifications
-- Descrição: Certificações profissionais
-- =============================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Certifications' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Certifications](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [GuidID] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
        [Name] [nvarchar](100) NOT NULL,
        [Issuer] [nvarchar](100) NOT NULL,
        [IssueDate] [datetime] NOT NULL,
        [ExpiryDate] [datetime] NULL,
        [CredentialId] [nvarchar](100) NULL,
        [CredentialUrl] [nvarchar](255) NULL,
        [Description] [nvarchar](500) NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT(0),
        [UserID] [uniqueidentifier] NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [DateCreated] [datetime2](7) NOT NULL DEFAULT(GETDATE()),
        [DateUpdated] [datetime2](7) NULL,
        [UserCreated] [nvarchar](255) NOT NULL DEFAULT('System'),
        [UserUpdated] [nvarchar](255) NULL,
        
        CONSTRAINT [PK_Certifications] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [UQ_Certifications_GuidID] UNIQUE NONCLUSTERED ([GuidID] ASC)
    ) ON [PRIMARY]

    PRINT 'Tabela Certifications criada com sucesso!'
END
ELSE
BEGIN
    PRINT 'Tabela Certifications já existe!'
END
GO

-- =============================================
-- TABELA: Services
-- Descrição: Serviços oferecidos
-- =============================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Services' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Services](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [GuidID] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
        [Title] [nvarchar](100) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [Icon] [nvarchar](50) NULL,
        [Features] [nvarchar](max) NULL, -- JSON array
        [Price] [decimal](18,2) NULL,
        [Currency] [nvarchar](3) NULL DEFAULT('BRL'),
        [Duration] [nvarchar](50) NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT(0),
        [UserID] [uniqueidentifier] NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [DateCreated] [datetime2](7) NOT NULL DEFAULT(GETDATE()),
        [DateUpdated] [datetime2](7) NULL,
        [UserCreated] [nvarchar](255) NOT NULL DEFAULT('System'),
        [UserUpdated] [nvarchar](255) NULL,
        
        CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [UQ_Services_GuidID] UNIQUE NONCLUSTERED ([GuidID] ASC)
    ) ON [PRIMARY]

    PRINT 'Tabela Services criada com sucesso!'
END
ELSE
BEGIN
    PRINT 'Tabela Services já existe!'
END
GO

-- =============================================
-- TABELA: ContactMessages
-- Descrição: Mensagens de contato recebidas
-- =============================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ContactMessages' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[ContactMessages](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [GuidID] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
        [Name] [nvarchar](100) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [Subject] [nvarchar](200) NOT NULL,
        [Message] [nvarchar](2000) NOT NULL,
        [IsRead] [bit] NOT NULL DEFAULT(0),
        [ReadAt] [datetime] NULL,
        [Response] [nvarchar](max) NULL,
        [ResponseAt] [datetime] NULL,
        [IpAddress] [nvarchar](15) NULL,
        [UserAgent] [nvarchar](200) NULL,
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [DateCreated] [datetime2](7) NOT NULL DEFAULT(GETDATE()),
        [DateUpdated] [datetime2](7) NULL,
        [UserCreated] [nvarchar](255) NOT NULL DEFAULT('System'),
        [UserUpdated] [nvarchar](255) NULL,
        
        CONSTRAINT [PK_ContactMessages] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [UQ_ContactMessages_GuidID] UNIQUE NONCLUSTERED ([GuidID] ASC)
    ) ON [PRIMARY]

    PRINT 'Tabela ContactMessages criada com sucesso!'
END
ELSE
BEGIN
    PRINT 'Tabela ContactMessages já existe!'
END
GO

-- =============================================
-- CRIAÇÃO DE ÍNDICES PARA PERFORMANCE
-- =============================================

-- Índices para Skills
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Skills_UserID_Category_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Skills_UserID_Category_IsActive]
    ON [dbo].[Skills] ([UserID] ASC, [Category] ASC, [IsActive] ASC)
    INCLUDE ([Name], [Level], [DisplayOrder])
    
    PRINT 'Índice IX_Skills_UserID_Category_IsActive criado com sucesso!'
END
GO

-- Índices para Experiences
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Experiences_UserID_IsActive_StartDate')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Experiences_UserID_IsActive_StartDate]
    ON [dbo].[Experiences] ([UserID] ASC, [IsActive] ASC, [StartDate] DESC)
    INCLUDE ([Title], [Company], [DisplayOrder])
    
    PRINT 'Índice IX_Experiences_UserID_IsActive_StartDate criado com sucesso!'
END
GO

-- Índices para Education
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Education_UserID_IsActive_StartDate')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Education_UserID_IsActive_StartDate]
    ON [dbo].[Education] ([UserID] ASC, [IsActive] ASC, [StartDate] DESC)
    INCLUDE ([Degree], [Institution], [DisplayOrder])
    
    PRINT 'Índice IX_Education_UserID_IsActive_StartDate criado com sucesso!'
END
GO

-- Índices para Certifications
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Certifications_UserID_IsActive_IssueDate')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Certifications_UserID_IsActive_IssueDate]
    ON [dbo].[Certifications] ([UserID] ASC, [IsActive] ASC, [IssueDate] DESC)
    INCLUDE ([Name], [Issuer], [DisplayOrder])
    
    PRINT 'Índice IX_Certifications_UserID_IsActive_IssueDate criado com sucesso!'
END
GO

-- Índices para Services
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Services_UserID_IsActive_DisplayOrder')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Services_UserID_IsActive_DisplayOrder]
    ON [dbo].[Services] ([UserID] ASC, [IsActive] ASC, [DisplayOrder] ASC)
    INCLUDE ([Title], [Price])
    
    PRINT 'Índice IX_Services_UserID_IsActive_DisplayOrder criado com sucesso!'
END
GO

-- Índices para ContactMessages
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ContactMessages_DateCreated_IsRead')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ContactMessages_DateCreated_IsRead]
    ON [dbo].[ContactMessages] ([DateCreated] DESC, [IsRead] ASC)
    INCLUDE ([Name], [Email], [Subject])
    
    PRINT 'Índice IX_ContactMessages_DateCreated_IsRead criado com sucesso!'
END
GO

PRINT 'Script v0.02 executado com sucesso! Todas as tabelas do sistema de Currículo estão prontas para uso.'
GO 