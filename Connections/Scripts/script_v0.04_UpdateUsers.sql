-- Script para atualização da tabela Users
-- Versão: 0.04
-- Data: $(date)
-- Descrição: Atualização da tabela Users para compatibilidade com o novo sistema

USE [Pandora]
GO

-- =============================================
-- ATUALIZAR TABELA USERS
-- =============================================

-- Verificar se a coluna Token existe, se não, adicionar
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'Token')
BEGIN
    ALTER TABLE [dbo].[Users] ADD [Token] [nvarchar](500) NULL
    PRINT 'Coluna Token adicionada à tabela Users!'
END
ELSE
BEGIN
    PRINT 'Coluna Token já existe na tabela Users!'
END
GO

-- Verificar se a coluna TokenExpiry existe, se não, adicionar
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'TokenExpiry')
BEGIN
    ALTER TABLE [dbo].[Users] ADD [TokenExpiry] [datetime] NULL
    PRINT 'Coluna TokenExpiry adicionada à tabela Users!'
END
ELSE
BEGIN
    PRINT 'Coluna TokenExpiry já existe na tabela Users!'
END
GO

-- Verificar se a coluna IsAdmin existe, se não, adicionar
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'IsAdmin')
BEGIN
    ALTER TABLE [dbo].[Users] ADD [IsAdmin] [bit] NOT NULL DEFAULT(0)
    PRINT 'Coluna IsAdmin adicionada à tabela Users!'
END
ELSE
BEGIN
    PRINT 'Coluna IsAdmin já existe na tabela Users!'
END
GO

-- Verificar se a coluna LastLogin existe, se não, adicionar
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'LastLogin')
BEGIN
    ALTER TABLE [dbo].[Users] ADD [LastLogin] [datetime] NULL
    PRINT 'Coluna LastLogin adicionada à tabela Users!'
END
ELSE
BEGIN
    PRINT 'Coluna LastLogin já existe na tabela Users!'
END
GO

-- Verificar se a coluna ProfileImage existe, se não, adicionar
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'ProfileImage')
BEGIN
    ALTER TABLE [dbo].[Users] ADD [ProfileImage] [nvarchar](255) NULL
    PRINT 'Coluna ProfileImage adicionada à tabela Users!'
END
ELSE
BEGIN
    PRINT 'Coluna ProfileImage já existe na tabela Users!'
END
GO

-- Atualizar tipo da coluna GuidID se necessário
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'GuidID' AND DATA_TYPE = 'uniqueidentifier')
BEGIN
    PRINT 'Coluna GuidID já está no tipo correto!'
END
ELSE
BEGIN
    -- Se GuidID não for uniqueidentifier, precisamos converter
    IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'GuidID')
    BEGIN
        PRINT 'ATENÇÃO: Coluna GuidID existe mas não é uniqueidentifier. Conversão manual necessária!'
    END
    ELSE
    BEGIN
        ALTER TABLE [dbo].[Users] ADD [GuidID] [uniqueidentifier] NOT NULL DEFAULT NEWID()
        PRINT 'Coluna GuidID adicionada à tabela Users!'
    END
END
GO

-- =============================================
-- INSERIR USUÁRIO ADMIN DE EXEMPLO
-- =============================================
IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'admin@portfolium.com')
BEGIN
    INSERT INTO Users (
        GuidID, Name, Email, Password, Role, IsAdmin, IsActive, 
        ProfileImage, UserCreated
    ) VALUES (
        '11111111-1111-1111-1111-111111111111',
        'Administrador',
        'admin@portfolium.com',
        'AQAAAAEAACcQAAAAEK8H+8Jz9n0/9XzY2Q1L3mF5K9X8YrZ3Vw4N2Jh7Bg5Fq8Dp6Cs9Et1Xu3Nv2Mp8Rw==', -- senha: admin123
        'admin',
        1,
        1,
        '/assets/img/avatars/admin.jpg',
        'System'
    )
    PRINT 'Usuário administrador criado com sucesso!'
    PRINT 'Email: admin@portfolium.com'
    PRINT 'Senha: admin123'
END
ELSE
BEGIN
    -- Atualizar usuário existente para ser admin
    UPDATE Users 
    SET IsAdmin = 1, 
        Role = 'admin',
        GuidID = '11111111-1111-1111-1111-111111111111'
    WHERE Email = 'admin@portfolium.com'
    
    PRINT 'Usuário admin@portfolium.com atualizado para administrador!'
END
GO

-- =============================================
-- CRIAR ÍNDICES ADICIONAIS PARA USERS
-- =============================================

-- Índice para Email (único)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Email_Unique')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email_Unique]
    ON [dbo].[Users] ([Email] ASC)
    WHERE [IsActive] = 1
    
    PRINT 'Índice único IX_Users_Email_Unique criado com sucesso!'
END
GO

-- Índice para Token
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Token_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Users_Token_IsActive]
    ON [dbo].[Users] ([Token] ASC, [IsActive] ASC)
    INCLUDE ([GuidID], [Name], [Email], [Role], [IsAdmin])
    
    PRINT 'Índice IX_Users_Token_IsActive criado com sucesso!'
END
GO

-- Índice para IsAdmin
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_IsAdmin_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Users_IsAdmin_IsActive]
    ON [dbo].[Users] ([IsAdmin] ASC, [IsActive] ASC)
    INCLUDE ([GuidID], [Name], [Email])
    
    PRINT 'Índice IX_Users_IsAdmin_IsActive criado com sucesso!'
END
GO

-- =============================================
-- CRIAR FOREIGN KEYS PARA AS NOVAS TABELAS
-- =============================================

-- FK PersonalInfo -> Users
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_PersonalInfo_Users')
BEGIN
    ALTER TABLE [dbo].[PersonalInfo] WITH CHECK 
    ADD CONSTRAINT [FK_PersonalInfo_Users] FOREIGN KEY([UserID])
    REFERENCES [dbo].[Users] ([GuidID])
    
    ALTER TABLE [dbo].[PersonalInfo] CHECK CONSTRAINT [FK_PersonalInfo_Users]
    PRINT 'Foreign Key FK_PersonalInfo_Users criada com sucesso!'
END
GO

-- FK Skills -> Users
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Skills_Users')
BEGIN
    ALTER TABLE [dbo].[Skills] WITH CHECK 
    ADD CONSTRAINT [FK_Skills_Users] FOREIGN KEY([UserID])
    REFERENCES [dbo].[Users] ([GuidID])
    
    ALTER TABLE [dbo].[Skills] CHECK CONSTRAINT [FK_Skills_Users]
    PRINT 'Foreign Key FK_Skills_Users criada com sucesso!'
END
GO

-- FK Experiences -> Users
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Experiences_Users')
BEGIN
    ALTER TABLE [dbo].[Experiences] WITH CHECK 
    ADD CONSTRAINT [FK_Experiences_Users] FOREIGN KEY([UserID])
    REFERENCES [dbo].[Users] ([GuidID])
    
    ALTER TABLE [dbo].[Experiences] CHECK CONSTRAINT [FK_Experiences_Users]
    PRINT 'Foreign Key FK_Experiences_Users criada com sucesso!'
END
GO

-- FK Education -> Users
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Education_Users')
BEGIN
    ALTER TABLE [dbo].[Education] WITH CHECK 
    ADD CONSTRAINT [FK_Education_Users] FOREIGN KEY([UserID])
    REFERENCES [dbo].[Users] ([GuidID])
    
    ALTER TABLE [dbo].[Education] CHECK CONSTRAINT [FK_Education_Users]
    PRINT 'Foreign Key FK_Education_Users criada com sucesso!'
END
GO

-- FK Certifications -> Users
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Certifications_Users')
BEGIN
    ALTER TABLE [dbo].[Certifications] WITH CHECK 
    ADD CONSTRAINT [FK_Certifications_Users] FOREIGN KEY([UserID])
    REFERENCES [dbo].[Users] ([GuidID])
    
    ALTER TABLE [dbo].[Certifications] CHECK CONSTRAINT [FK_Certifications_Users]
    PRINT 'Foreign Key FK_Certifications_Users criada com sucesso!'
END
GO

-- FK Services -> Users
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Services_Users')
BEGIN
    ALTER TABLE [dbo].[Services] WITH CHECK 
    ADD CONSTRAINT [FK_Services_Users] FOREIGN KEY([UserID])
    REFERENCES [dbo].[Users] ([GuidID])
    
    ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_Services_Users]
    PRINT 'Foreign Key FK_Services_Users criada com sucesso!'
END
GO

PRINT 'Script v0.04 executado com sucesso! Tabela Users atualizada e foreign keys criadas.'
GO 