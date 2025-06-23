-- Script para inserção de dados de exemplo
-- Versão: 0.03
-- Data: $(date)
-- Descrição: Dados de exemplo para desenvolvimento e testes

USE [Pandora]
GO

-- Declarar variáveis
DECLARE @UserGuid UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111'
DECLARE @AdminUserId INT = 1

-- =============================================
-- INSERIR DADOS DE EXEMPLO - PersonalInfo
-- =============================================
IF NOT EXISTS (SELECT * FROM PersonalInfo WHERE UserID = @UserGuid)
BEGIN
    INSERT INTO PersonalInfo (
        GuidID, Name, Title, Description, Location, Phone, Email, 
        ProfileImage, YearsExperience, ProjectsCompleted, HappyClients, 
        Certifications, LinkedInUrl, GitHubUrl, PortfolioUrl, UserID
    ) VALUES (
        NEWID(),
        'Aldrin Henrique Ronchi',
        'Desenvolvedor Full Stack',
        'Desenvolvedor apaixonado por tecnologia com mais de 4 anos de experiência em desenvolvimento web. Especialista em .NET, Angular e arquiteturas modernas.',
        'Curitibanos, SC - Brasil',
        '+55 (48) 99976-9594',
        'work.aldrironchi@gmail.com',
        '/assets/img/avatars/avatar.jpg',
        4,
        13,
        10,
        10,
        'https://www.linkedin.com/in/aldrin-ronchi-538320172/',
        'https://github.com/aldrinhronchi',
        'https://aldrinronchi.com',
        @UserGuid
    )
    PRINT 'Dados pessoais inseridos com sucesso!'
END
GO

-- =============================================
-- INSERIR DADOS DE EXEMPLO - Skills
-- =============================================
IF NOT EXISTS (SELECT * FROM Skills WHERE UserID = @UserGuid)
BEGIN
    INSERT INTO Skills (GuidID, Name, Level, Category, DisplayOrder, Icon, Color, UserID) VALUES
    (NEWID(), 'C#', 95, 'Backend', 1, 'fab fa-microsoft', '#239120', @UserGuid),
    (NEWID(), '.NET Core', 90, 'Backend', 2, 'fas fa-code', '#512BD4', @UserGuid),
    (NEWID(), 'ASP.NET MVC', 85, 'Backend', 3, 'fas fa-globe', '#0078D4', @UserGuid),
    (NEWID(), 'Entity Framework', 85, 'Backend', 4, 'fas fa-database', '#FF6B35', @UserGuid),
    (NEWID(), 'SQL Server', 90, 'Database', 5, 'fas fa-database', '#CC2927', @UserGuid),
    (NEWID(), 'Angular', 85, 'Frontend', 6, 'fab fa-angular', '#DD0031', @UserGuid),
    (NEWID(), 'TypeScript', 80, 'Frontend', 7, 'fab fa-js-square', '#3178C6', @UserGuid),
    (NEWID(), 'JavaScript', 85, 'Frontend', 8, 'fab fa-js-square', '#F7DF1E', @UserGuid),
    (NEWID(), 'HTML5', 90, 'Frontend', 9, 'fab fa-html5', '#E34F26', @UserGuid),
    (NEWID(), 'CSS3', 85, 'Frontend', 10, 'fab fa-css3-alt', '#1572B6', @UserGuid),
    (NEWID(), 'Bootstrap', 80, 'Frontend', 11, 'fab fa-bootstrap', '#7952B3', @UserGuid),
    (NEWID(), 'Azure', 75, 'Cloud', 12, 'fab fa-microsoft', '#0078D4', @UserGuid),
    (NEWID(), 'Docker', 70, 'DevOps', 13, 'fab fa-docker', '#2496ED', @UserGuid),
    (NEWID(), 'Git', 85, 'Tools', 14, 'fab fa-git-alt', '#F05032', @UserGuid),
    (NEWID(), 'REST APIs', 90, 'Backend', 15, 'fas fa-exchange-alt', '#61DAFB', @UserGuid),
    (NEWID(), 'Node.js', 75, 'Backend', 16, 'fab fa-node-js', '#339933', @UserGuid),
    (NEWID(), 'Firebase', 70, 'Database', 17, 'fab fa-firebase', '#FFCA28', @UserGuid)
    
    PRINT 'Skills inseridas com sucesso!'
END
GO

-- =============================================
-- INSERIR DADOS DE EXEMPLO - Experiences
-- =============================================
IF NOT EXISTS (SELECT * FROM Experiences WHERE UserID = @UserGuid)
BEGIN
    INSERT INTO Experiences (
        GuidID, Title, Company, StartDate, EndDate, Location, Description,
        Responsibilities, Technologies, Achievements, IsCurrentJob, DisplayOrder, UserID
    ) VALUES
    (
        NEWID(),
        'Desenvolvedor Full Stack Pleno',
        'Concept One Tecnologia',
        '2023-07-01',
        NULL,
        'Remoto, São Paulo, SP',
        'Responsável pela arquitetura e desenvolvimento de soluções corporativas escaláveis.',
        '["Definição de arquiteturas de software", "Mentoria de equipes de desenvolvimento", "Code review e padrões de qualidade", "Integração com APIs externas", "Otimização de performance"]',
        '[".NET 6+", "Angular 14+", "Azure", "SQL Server", "Docker", ]',
        '["Reduziu tempo de deploy em 40%", "Implementou arquitetura de microserviços", "Implementou Stored Procedures para Relatórios"]',
        1,
        1,
        @UserGuid
    ),
    (
        NEWID(),
        'Desenvolvedor Full Stack Júnior',
        'Concept One Tecnologia',
        '2022-06-16',
        '2023-07-30',
        'Remoto, São Paulo, SP',
        'Desenvolvimento de aplicações web e automação de fluxos de dados e integração.',
        '["Desenvolvimento frontend e backend", "Integração com APIs", "Manutenção de sistemas legados","Automação de validação NFe Concorrente","Fila autogerenciavel de envio de Arquivos"]',
        '[".NET Framework", "SQL Server", "Azure", "Entity Framework", "Node.js",".NET 6"]',
        '["Desenvolveu 10+ projetos", "Melhorou performance em 40%", "Implementou logs" ]',
        0,
        2,
        @UserGuid
    ),
    (
        NEWID(),
        'Desenvolvedor Web Júnior',
        'Revolucion Serviços de Informática',
        '2021-11-01',
        '2022-06-15',
        'Fraiburgo, SC',
        'Primeiro trabalho full time, focado em aprendizado e crescimento.',
        '["Desenvolvimento de funcionalidades", "Correção de bugs", "Documentação técnica", "Suporte a usuários"]',
        '["ASP.NET MVC", "C#", "SQL Server", "jQuery", "Bootstrap", "VB.NET", "JavaScript"]',
        '["Manutenção de sistema de gestão completo", "Desenvolvimento de telas para sites"]',
        0,
        3,
        @UserGuid
    ),
    (
        NEWID(),
        'Desenvolvedor de Aplicativos Móveis',
        'Go Party',
        '2019-10-01',
        '2020-03-30',
        'Remoto, Balneário Camboriú, SC',
        'Estagio part-time, focado em aprendizado e crescimento.',
        '["Desenvolvimento de funcionalidades", "Correção de bugs", "Documentação técnica", "Suporte a usuários"]',
        '["Python", "Flutter", "Firebase", "React Native"]',
        '["Desenvolvimento de aplicativos móveis","Scrapper em Python para coletar dados de eventos"]',
        0,
        4,
        @UserGuid
    )
    PRINT 'Experiências inseridas com sucesso!'
END
GO

-- =============================================
-- INSERIR DADOS DE EXEMPLO - Education
-- =============================================
IF NOT EXISTS (SELECT * FROM Education WHERE UserID = @UserGuid)
BEGIN
    INSERT INTO Education (
        GuidID, Degree, Institution, StartDate, EndDate, Location, Description,
        Grade, Achievements, DisplayOrder, UserID
    ) VALUES
     (
        NEWID(),
        'Tecnologo em Análise e Desenvolvimento de Sistemas',
        'UNIASSELVI',
        '2025-01-01',
        '2027-12-15',
        'São Paulo, SP',
        'Desenvolvimento de atividades técnicas, científicas e de gestão.',
        '9.2',
        '["Melhor projeto final da turma", "Certificação em Azure"]',
       1,
        @UserGuid
    ),
    (
        NEWID(),
        'Tecnologo em Análise e Desenvolvimento de Sistemas',
        'Instituto Federal Catarinense (IFC)',
        '2013-02-01',
        '2016-12-31',
        'Fraiburgo, SC',
        'Formação sólida em programação orientada a objetos, estruturas de dados e engenharia de software.',
        '8.5',
        '["Primeiro Lugar no Hackathon de Software"]',
        2,
        @UserGuid
    ),
    (
        NEWID(),
        'Tecnico em Informatica',
        'Instituto Federal Catarinense (IFC)',
        '2015-01-01',
        '2017-12-15',
        'Fraiburgo, SC',
        'Formação em fundamentos da computação, algoritmos, arquitetura de computadores e redes de computadores.',
        '9.2',
        '["Projeto de Pesquisa de Mapeamento de Dados", "Projeto de Extensão em IPv6"]',
        3,
        @UserGuid
    )
    
    PRINT 'Educação inserida com sucesso!'
END
GO

-- =============================================
-- INSERIR DADOS DE EXEMPLO - Certifications
-- =============================================
/*
IF NOT EXISTS (SELECT * FROM Certifications WHERE UserID = @UserGuid)
BEGIN
    INSERT INTO Certifications (
        GuidID, Name, Issuer, IssueDate, ExpiryDate, CredentialId, 
        CredentialUrl, Description, DisplayOrder, UserID
    ) VALUES
    (
        NEWID(),
        'Microsoft Certified: Azure Developer Associate',
        'Microsoft',
        '2023-03-15',
        '2025-03-15',
        'AZ-204-2023-001234',
        'https://learn.microsoft.com/api/credentials/share/pt-br/JoaoSilva-1234/AZ204',
        'Certificação que valida habilidades em desenvolvimento de soluções na nuvem Azure.',
        1,
        @UserGuid
    ),
    (
        NEWID(),
        'Angular Certified Developer',
        'Angular Team',
        '2022-11-20',
        NULL,
        'ANG-DEV-2022-5678',
        'https://certificates.angular.io/JoaoSilva-5678',
        'Certificação oficial do time Angular validando conhecimentos avançados no framework.',
        2,
        @UserGuid
    )
    
    PRINT 'Certificações inseridas com sucesso!'
END
GO
*/
-- =============================================
-- INSERIR DADOS DE EXEMPLO - Services
-- =============================================
IF NOT EXISTS (SELECT * FROM Services WHERE UserID = @UserGuid)
BEGIN
    INSERT INTO Services (
        GuidID, Title, Description, Icon, Features, Price, Currency, Duration, DisplayOrder, UserID
    ) VALUES
    (
        NEWID(),
        'Desenvolvimento Web Full Stack',
        'Desenvolvimento completo de aplicações web modernas e responsivas.',
        'fas fa-code',
        '["Frontend com Angular/React", "Backend com .NET Core", "Banco de dados SQL Server", "Deploy na nuvem", "Testes automatizados", "Documentação técnica"]',
        5000.00,
        'BRL',
        '2-4 semanas',
        1,
        @UserGuid
    ),
    (
        NEWID(),
        'Consultoria em Arquitetura de Software',
        'Análise e definição de arquiteturas escaláveis e performáticas.',
        'fas fa-sitemap',
        '["Análise de requisitos", "Definição de arquitetura", "Documentação técnica", "Mentoria da equipe", "Code review", "Boas práticas"]',
        3000.00,
        'BRL',
        '1-2 semanas',
        2,
        @UserGuid
    )
    
    PRINT 'Serviços inseridos com sucesso!'
END
GO

-- =============================================
-- INSERIR DADOS DE EXEMPLO - ContactMessages
-- =============================================
INSERT INTO ContactMessages (
    GuidID, Name, Email, Subject, Message, IsRead, IpAddress, UserAgent
) VALUES
(
    NEWID(),
    'Ana Costa',
    'ana.costa@tech.com',
    'Mentoria técnica',
    'Sou desenvolvedora júnior e gostaria de uma mentoria para evoluir na carreira. Qual seria o processo?',
    0,
    '172.16.0.25',
    'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36'
)

PRINT 'Mensagens de contato de exemplo inseridas com sucesso!'
GO

PRINT 'Script v0.03 executado com sucesso! Todos os dados de exemplo foram inseridos.'
GO 