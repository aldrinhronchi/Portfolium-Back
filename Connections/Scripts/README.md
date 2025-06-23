# Scripts do Banco de Dados - Portfolium

Este diretório contém todos os scripts SQL necessários para criar e configurar o banco de dados do sistema Portfolium.

## 📁 Estrutura dos Scripts

### Scripts Principais

| Script | Versão | Descrição |
|--------|--------|-----------|
| `ExecuteAll.sql` | Master | **Script principal** - Executa todos os scripts em ordem |
| `script_v0.00.sql` | 0.00 | Estrutura base (Users e tabelas legadas) |
| `script_v0.01_Projects.sql` | 0.01 | Tabela Projects com índices e dados de exemplo |
| `script_v0.02_Curriculum.sql` | 0.02 | Tabelas do sistema de currículo |
| `script_v0.03_SampleData.sql` | 0.03 | Dados de exemplo para desenvolvimento |
| `script_v0.04_UpdateUsers.sql` | 0.04 | Atualização da tabela Users e foreign keys |

## 🚀 Como Executar

### Opção 1: Execução Completa (Recomendado)
```sql
-- No SQL Server Management Studio ou Azure Data Studio
-- Abrir o arquivo ExecuteAll.sql e executar
:r ExecuteAll.sql
```

### Opção 2: Execução Individual
Execute os scripts na seguinte ordem:
1. `script_v0.00.sql`
2. `script_v0.01_Projects.sql`
3. `script_v0.02_Curriculum.sql`
4. `script_v0.04_UpdateUsers.sql`
5. `script_v0.03_SampleData.sql`

## 📊 Estrutura do Banco

### Tabelas Criadas

#### 👤 **Users**
- Usuários do sistema
- Campos: ID, GuidID, Name, Email, Password, Role, IsAdmin, Token, etc.

#### 🎯 **Projects**
- Projetos do portfólio
- Campos: Name, Description, Technologies, URLs, Status, Category, etc.

#### 👨‍💼 **PersonalInfo**
- Informações pessoais do currículo
- Campos: Name, Title, Description, Contact, Statistics, etc.

#### 🛠️ **Skills**
- Habilidades técnicas
- Campos: Name, Level, Category, Icon, Color, etc.

#### 💼 **Experiences**
- Experiências profissionais
- Campos: Title, Company, Dates, Description, Technologies, etc.

#### 🎓 **Education**
- Formação educacional
- Campos: Degree, Institution, Dates, Grade, etc.

#### 🏆 **Certifications**
- Certificações profissionais
- Campos: Name, Issuer, Dates, Credential, etc.

#### 🔧 **Services**
- Serviços oferecidos
- Campos: Title, Description, Price, Features, etc.

#### 📧 **ContactMessages**
- Mensagens de contato
- Campos: Name, Email, Subject, Message, Status, etc.

## 🔐 Credenciais Padrão

Após a execução dos scripts, será criado um usuário administrador:

- **Email:** `admin@portfolium.com`
- **Senha:** `admin123`
- **Perfil:** Administrador

## 📈 Índices e Performance

Todos os scripts incluem índices otimizados para:
- Consultas por usuário
- Filtros por categoria
- Ordenação por data
- Busca por status
- Performance geral

## 🔗 Relacionamentos

```
Users (1) ←→ (1) PersonalInfo
Users (1) ←→ (*) Skills
Users (1) ←→ (*) Experiences
Users (1) ←→ (*) Education
Users (1) ←→ (*) Certifications
Users (1) ←→ (*) Services
Users (1) ←→ (*) Projects
```

## 🛡️ Segurança

- Foreign Keys para integridade referencial
- Constraints para validação de dados
- Índices únicos para campos críticos
- Campos de auditoria (DateCreated, UserCreated, etc.)

## 📝 Dados de Exemplo

O script inclui dados de exemplo para:
- ✅ Usuário administrador
- ✅ Informações pessoais completas
- ✅ 15 skills técnicas
- ✅ 3 experiências profissionais
- ✅ 2 formações educacionais
- ✅ 3 certificações
- ✅ 4 serviços oferecidos
- ✅ 3 mensagens de contato
- ✅ 1 projeto de exemplo

## 🔧 Configuração do Ambiente

### Pré-requisitos
- SQL Server 2019+ ou Azure SQL Database
- Banco de dados "Pandora" criado
- Permissões de DDL (CREATE, ALTER, DROP)

### Verificação Pós-Execução
```sql
-- Verificar tabelas criadas
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME IN ('Users', 'Projects', 'PersonalInfo', 'Skills', 'Experiences', 'Education', 'Certifications', 'Services', 'ContactMessages')

-- Verificar dados inseridos
SELECT 'Users' as Tabela, COUNT(*) as Registros FROM Users
UNION ALL SELECT 'Projects', COUNT(*) FROM Projects
UNION ALL SELECT 'PersonalInfo', COUNT(*) FROM PersonalInfo
UNION ALL SELECT 'Skills', COUNT(*) FROM Skills
-- ... etc
```

## 🐛 Troubleshooting

### Problemas Comuns

1. **Erro: Database 'Pandora' not found**
   - Criar o banco de dados antes de executar os scripts

2. **Erro: Permission denied**
   - Verificar permissões de DDL no banco

3. **Erro: Foreign key constraint**
   - Executar scripts na ordem correta

4. **Erro: Unique constraint violation**
   - Limpar dados existentes antes de re-executar

### Limpeza do Banco
```sql
-- CUIDADO: Remove todos os dados!
DROP TABLE IF EXISTS ContactMessages
DROP TABLE IF EXISTS Services
DROP TABLE IF EXISTS Certifications
DROP TABLE IF EXISTS Education
DROP TABLE IF EXISTS Experiences
DROP TABLE IF EXISTS Skills
DROP TABLE IF EXISTS PersonalInfo
DROP TABLE IF EXISTS Projects
-- Users mantém-se para não quebrar outras funcionalidades
```

## 📞 Suporte

Para dúvidas ou problemas:
1. Verificar este README
2. Consultar logs de execução
3. Validar pré-requisitos
4. Contactar equipe de desenvolvimento

---

**Nota:** Sempre faça backup do banco antes de executar os scripts em produção! 