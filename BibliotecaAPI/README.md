Aqui está o README.md completo, formatado e pronto para você copiar e colar no seu repositório:

Markdown
# 📚 BibliotecaAPI

Uma API Web robusta, escalável e resiliente desenvolvida em **.NET 10** para o gerenciamento de bibliotecas. O projeto foi estruturado para ser uma aplicação de nível enterprise, focada em boas práticas de engenharia de software, automação de infraestrutura e qualidade de código.

---

## 🛠️ Tecnologias e Ferramentas

- **Runtime:** .NET 10
- **Linguagem:** C#
- **Arquitetura:** Layered Architecture (Clean Architecture concepts)
- **Design Patterns:** SOLID, Injeção de Dependência
- **Automação (CI/CD):** GitHub Actions
- **Containerização:** Docker
- **Testes:** xUnit, Moq
- **Documentação:** Swagger / OpenAPI

---

## 🏗️ Estrutura do Projeto

O sistema foi modularizado para garantir a separação de responsabilidades e facilitar a manutenção e escalabilidade:

```text
├── BibliotecaAPI.API            # Camada de Apresentação (Controllers, Middleware, Configuração)
├── BibliotecaAPI.Application    # Regras de Negócio, Services, Interfaces e DTOs
├── BibliotecaAPI.Infrastructure # Acesso a dados (ORM, Repositórios, Contexto)
├── BibliotecaAPI.Domain         # Entidades de Domínio e lógica central
└── BibliotecaAPI.Tests          # Testes Unitários e de Integração (xUnit/Moq)