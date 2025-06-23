-- Script Master para execução completa do banco de dados
-- Versão: Master
-- Data: $(date)
-- Descrição: Executa todos os scripts em ordem para criar o banco completo

USE [Pandora]
GO

PRINT '=========================================='
PRINT 'INICIANDO CRIAÇÃO COMPLETA DO BANCO PORTFOLIUM'
PRINT '=========================================='
PRINT ''

-- =============================================
-- ETAPA 1: Estrutura base (Users e Projects)
-- =============================================
PRINT '1. Executando estrutura base...'
PRINT ''

-- Executar script_v0.00.sql (Users base)
:r script_v0.00.sql

-- Executar script_v0.01_Projects.sql (Projects)
:r script_v0.01_Projects.sql

PRINT ''
PRINT '✓ Estrutura base criada com sucesso!'
PRINT ''

-- =============================================
-- ETAPA 2: Tabelas do sistema de Currículo
-- =============================================
PRINT '2. Executando criação das tabelas de currículo...'
PRINT ''

-- Executar script_v0.02_Curriculum.sql
:r script_v0.02_Curriculum.sql

PRINT ''
PRINT '✓ Tabelas de currículo criadas com sucesso!'
PRINT ''

-- =============================================
-- ETAPA 3: Atualização da tabela Users
-- =============================================
PRINT '3. Executando atualização da tabela Users...'
PRINT ''

-- Executar script_v0.04_UpdateUsers.sql
:r script_v0.04_UpdateUsers.sql

PRINT ''
PRINT '✓ Tabela Users atualizada com sucesso!'
PRINT ''

-- =============================================
-- ETAPA 4: Inserção de dados de exemplo
-- =============================================
PRINT '4. Executando inserção de dados de exemplo...'
PRINT ''

-- Executar script_v0.03_SampleData.sql
:r script_v0.03_SampleData.sql

PRINT ''
PRINT '✓ Dados de exemplo inseridos com sucesso!'
PRINT ''

-- =============================================
-- VERIFICAÇÃO FINAL
-- =============================================
PRINT '=========================================='
PRINT 'VERIFICAÇÃO FINAL DO BANCO DE DADOS'
PRINT '=========================================='
PRINT ''

-- Verificar tabelas criadas
SELECT 
    TABLE_NAME as 'Tabela',
    TABLE_TYPE as 'Tipo'
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME IN (
    'Users', 'Projects', 'PersonalInfo', 'Skills', 'Experiences', 
    'Education', 'Certifications', 'Services', 'ContactMessages'
)
ORDER BY TABLE_NAME

PRINT ''
PRINT '-- Contagem de registros por tabela --'

SELECT 'Users' as Tabela, COUNT(*) as Registros FROM Users
UNION ALL
SELECT 'Projects', COUNT(*) FROM Projects
UNION ALL
SELECT 'PersonalInfo', COUNT(*) FROM PersonalInfo
UNION ALL
SELECT 'Skills', COUNT(*) FROM Skills
UNION ALL
SELECT 'Experiences', COUNT(*) FROM Experiences
UNION ALL
SELECT 'Education', COUNT(*) FROM Education
UNION ALL
SELECT 'Certifications', COUNT(*) FROM Certifications
UNION ALL
SELECT 'Services', COUNT(*) FROM Services
UNION ALL
SELECT 'ContactMessages', COUNT(*) FROM ContactMessages

PRINT ''
PRINT '=========================================='
PRINT '✅ BANCO DE DADOS PORTFOLIUM CRIADO COM SUCESSO!'
PRINT '=========================================='
PRINT ''
PRINT 'Credenciais do usuário administrador:'
PRINT 'Email: admin@portfolium.com'
PRINT 'Senha: admin123'
PRINT ''
PRINT 'O banco está pronto para uso!'
PRINT '=========================================='
GO 