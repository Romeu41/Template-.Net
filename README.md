# MEU-DOTNET-TEMPLATE

Este é o template que eu desenvolvi para permitir a criação de softwares de forma estruturada, seguindo as melhores práticas de arquitetura, organização de projetos e manutenibilidade em ecossistemas .NET.

## Guia Rápido de Uso

### Pré-requisitos
- **.NET SDK** (conforme definido no arquivo centralizado `Directory.Build.props`).
- **IDE**: Visual Studio ou VS Code com as extensões de C# recomendadas.
- **SQL Server**: É possível subir um container local rapidamente com o comando:
  ```bash
  docker run --name appproject-sqlserver -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourStrong(!)Password' -p 1433:1433 -d [mcr.microsoft.com/mssql/server:2022-latest](https://mcr.microsoft.com/mssql/server:2022-latest)

```

* **Contas Ativas**: Auth0, SendGrid e GitHub (com acesso a GitHub Models) para as integrações do sistema.

### Passo a Passo para Configuração do Ambiente

1. Clone o repositório e restaure as dependências principais:
```bash
dotnet restore AppProject.slnx

```


2. Configure o arquivo `src/AppProject.Core.API/appsettings.Development.json` apontando para o seu banco SQL Server local (String de Conexão padrão: `Server=localhost,1433;Database=AppProject;...`).
3. Realize as configurações obrigatórias do **Auth0** detalhadas na seção de integrações abaixo.
4. Execute a API:
```bash
dotnet run --project src/AppProject.Core.API

```


*(Porta padrão: `https://localhost:7121`)*
5. Execute o Frontend em Blazor:
```bash
dotnet run --project src/AppProject.Web

```


*(Porta padrão: `https://localhost:7035`)*
6. Acesse a aplicação em seu navegador via `https://localhost:7035` e explore/teste os endpoints da API através do Swagger em `https://localhost:7121/swagger`.

---

## 🛠️ Como Funciona a API (Arquitetura e Camadas)

A API foi projetada seguindo os princípios de responsabilidade única e desacoplamento, dividida de maneira modular para facilitar a manutenção e extensibilidade do ecossistema:

* **Camada de Exposição (Controllers/API):** O projeto `src/AppProject.Core.API` centraliza as configurações globais de inicialização (Bootstraps), Middlewares de exceção customizados, Rate Limiting e gerenciamento do CORS. Os endpoints REST de cada módulo ficam isolados em projetos dedicados (ex: `AppProject.Core.Controllers.General`), garantindo validação automática de modelos (`[ApiController]`) e proteção por token (`[Authorize]`).
* **Camada de Negócio (Services):** Dividida entre serviços transacionais de escrita/CRUD (`AppProject.Core.Services.<Modulo>`) e serviços focados em leitura e listagens (`AppProject.Core.Services/<Modulo>`). Os serviços de escrita gerenciam regras de negócio complexas, realizam validações de duplicidade e interagem diretamente com o repositório. Os serviços de leitura projetam dados otimizados via Mapster utilizando `SummariesResponse<T>` para alimentar os grids e comboboxes da interface sem sobrecarregar a memória com entidades complexas ou ciclos de referências.
* **Camada de Dados (Infrastructure.Database):** Centraliza o contexto do Entity Framework Core (`ApplicationDbContext`), o padrão de Repositório Genérico, o mapeamento de entidades fluentes (`EntityTypeConfiguration`) e o histórico de migrações (`Migrations`).
* **Comunicação entre APIs e Frontend:** O ecossistema se comunica utilizando a biblioteca **Refit**, transformando interfaces C# diretamente em chamadas HTTP fortemente tipadas no lado do cliente, simplificando drasticamente o consumo dos endpoints.

---

## 🔐 Integração Obrigatória: Auth0

A API e o Frontend Blazor utilizam o padrão OIDC (OpenID Connect) via Auth0 para autenticação e controle de acessos. Para que o fluxo de segurança funcione perfeitamente, siga o meu roteiro de configuração abaixo:

### 1. Aplicação Frontend (Single Page Application)

No painel do Auth0, crie uma aplicação do tipo **Single Page Application** e configure as URLs de redirecionamento (ajuste as portas caso altere o seu `launchSettings.json`):

* **Allowed Callback URLs**: `https://localhost:7035/authentication/login-callback`, `https://localhost:7121/swagger/oauth2-redirect.html`
* **Allowed Logout URLs**: `https://localhost:7035`, `https://localhost:7121/swagger/`
* **Allowed Web Origins**: `https://localhost:7035`, `https://localhost:7121`

### 2. Configuração da API no Auth0

Crie uma **API** no menu correspondente do Auth0. Use como o campo *Identifier* o exato valor que definirá no seu `Auth0:Audience` (por padrão, eu utilizo `https://appproject.api`). Entre em **Access Settings** e marque a opção **Allow Offline Access** para habilitar o uso de tokens de atualização.

### 3. Injeção de Claims customizadas (Roles, Name e Email)

Para garantir que o JWT (JSON Web Token) trafegue as informações corretas de permissões do usuário para a minha API, crie uma **Action de Post-Login** (`post_login`) com o script JavaScript abaixo:

```javascript
if (api.accessToken) {
  if (event.user && event.user.email) {
    api.accessToken.setCustomClaim("email", event.user.email);
  }

  if (event.user && event.user.name) {
    api.accessToken.setCustomClaim("name", event.user.name);
  }

  if (event.authorization && event.authorization.roles) {
    api.accessToken.setCustomClaim("roles", event.authorization.roles);
  }
}

```

### 4. Atualização dos Arquivos de Configuração (`appsettings.json`)

Por fim, copie as credenciais geradas para as chaves correspondentes nos seus arquivos de configuração locais:

**No Backend (`src/AppProject.Core.API/appsettings.json`):**

```json
"Auth0": {
  "Authority": "[https://seu-dominio-auth0.us.auth0.com](https://seu-dominio-auth0.us.auth0.com)",
  "ClientId": "seu_client_id_da_spa",
  "Audience": "[https://appproject.api](https://appproject.api)"
}

```

---

## 🧩 Demais Integrações Externas

### SendGrid

* Utilizado para o envio de e-mails transacionais do sistema utilizando templates Razor compilados dinamicamente.
* Crie uma conta no portal do SendGrid, valide a identidade do remetente e gere uma chave de API para preencher em `SendEmail:ApiKey`.

### GitHub AI Models

* Integração nativa para execução de inteligência artificial baseada em LLMs providas pelo GitHub.
* Gere um token clássico com acesso aos modelos em [GitHub Settings Tokens](https://github.com/settings/tokens) e preencha as propriedades `AI:Endpoint` e `AI:Token`.

---

## 🏗️ Estrutura de Código de Exemplo (CRUD)

Caso precise adicionar novos módulos de negócio à solução, tome o meu módulo **General** (que implementa Países, Estados e Cidades) como referência absoluta de design de software:

1. **DTOs de API vs Modelos Observáveis do Frontend:** As DTOs da API implementam `IEntity` ou `ISummary`. No frontend Blazor, os modelos herdam de `ObservableModel` para habilitar notificações automáticas de mudança de propriedades na interface (`INotifyPropertyChanged`).
2. **Validações:** Exceções de validação disparam respostas padronizadas baseadas em `ExceptionDetail`. Validações customizadas de banco de dados e regras de negócio ficam contidas unicamente nos serviços.
3. **Mapeamento:** O Mapster realiza a conversão automática entre entidades (`Tb[Nome]`) e DTOs, estendido por classes de configuração explícitas (`IRegisterMapsterConfig`) quando propriedades agregadas complexas são necessárias.
4. **Persistência de Dados & Migrations:** As tabelas utilizam mapeamentos fluentes e convenções plurais (ex: `[Table("Countries")]`). Adicione novas migrações via CLI apontando explicitamente os projetos corretos:
```bash
dotnet ef migrations add NomeDaSuaMigration --project AppProject.Core.Infrastructure.Database --startup-project AppProject.Core.API --output-dir Migrations

```


*(As migrações são aplicadas automaticamente de forma segura assim que a API é inicializada)*.

---

## 🧪 Executando os Testes Unitários

O projeto conta com uma robusta suíte de testes unitários baseados em **NUnit**, **Moq**, **Shouldly** e **Bogus** para geração de dados fictícios, cobrindo fluxos felizes e validação de exceções de negócio nos serviços.
Para rodar toda a suíte de testes da solução, execute o comando abaixo no terminal de sua preferência:

```bash
dotnet test AppProject.slnx
