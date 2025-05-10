# Agendamento de Salão de Beleza API

Esta é uma API para gerenciar agendamentos em um salão de beleza. A API permite criar, atualizar, excluir e consultar usuários, funcionários, serviços e agendamentos.

## Tecnologias Utilizadas

- **.NET 8**
- **Entity Framework Core**
- **MySQL**
- **Swagger (Swashbuckle.AspNetCore)**
- **xUnit** para testes unitários
- **Injeção de Dependência**

## Estrutura do Projeto

O projeto segue uma arquitetura limpa, com separação de responsabilidades em diferentes camadas:

- **AgendamentoSalaoDeBeleza**: Camada de apresentação (controllers).
- **Application**: Contém as ViewModels e lógica de aplicação.
- **Domain**: Contém as regras de negócio encapsuladas em serviços.
- **Infrastructure**: Contém o contexto do banco de dados e a configuração do Entity Framework Core.
- **Core**: Contém as entidades principais do domínio.
- **UnityTests**: Contém os testes unitários.

## Endpoints Principais

### Usuários

- **POST /api/user**: Cria um novo usuário.
- **GET /api/user/{id}**: Obtém um usuário pelo ID.

### Funcionários

- **POST /api/employee**: Cria um novo funcionário.
- **GET /api/employee/{id}**: Obtém um funcionário pelo ID.
- **GET /api/employee**: Lista todos os funcionários.

### Serviços

- **POST /api/service**: Cria um novo serviço.
- **GET /api/service/{id}**: Obtém um serviço pelo ID.
- **GET /api/service**: Lista todos os serviços.

### Agendamentos

- **POST /api/appointments**: Cria um novo agendamento.
- **GET /api/appointments/{id}**: Obtém um agendamento pelo ID.
- **GET /api/appointments**: Lista todos os agendamentos.
- **PUT /api/appointments/{id}**: Atualiza um agendamento.
- **DELETE /api/appointments/{id}**: Exclui um agendamento.

## Configuração do Ambiente

1. **Clone o repositório**: git clone https://github.com/seu-usuario/seu-repositorio.git
   
2. **Configure o banco de dados**:
   - Atualize a string de conexão no arquivo `appsettings.json` com as credenciais do seu banco de dados MySQL.


   
