# 📚 BibliotecaAPI

Uma API Web robusta e escalável desenvolvida em **.NET 9** para o gerenciamento de uma biblioteca (livros, autores, empréstimos e usuários). O projeto foi construído seguindo os princípios de **Clean Code** e **Arquitetura em Camadas**, garantindo uma separação clara de responsabilidades, facilidade de manutenção e testes.

---

## 🚀 Tecnologias e Ferramentas Utilizadas

- **Runtime:** .NET 9
- **Linguagem:** C#
- **Framework Principal:** ASP.NET Core Web API
- **Documentação:** Swagger / OpenAPI (para testes rápidos de endpoints)
- **Padrão Arquitetural:** Layered Architecture (Camadas)

---

## 🏗️ Arquitetura do Projeto

Para garantir um sistema de alta demanda, modular e de fácil evolução, o projeto foi dividido nas seguintes camadas:

```text
├── BibliotecaAPI.API          # Camada de Apresentação (Controllers, Configuração do Swagger/DI)
├── BibliotecaAPI.Application  # Camada de Negócio (Interfaces, Serviços, Regras de Negócio e DTOs)
├── BibliotecaAPI.Infrastructure # Camada de Infraestrutura (Acesso a dados, Repositórios e Contexto)
└── BibliotecaAPI.Domain       # Camada de Domínio (Entidades centrais da aplicação)