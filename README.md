# Nuclear Akinator — Gerenciador de Perguntas Educativas (.NET)

## Projeto e integrantes

**Projeto:** Nuclear Akinator

| Caio Couto | RM563452 |
| Guilherme Eustáquio | RM566784 |
| Matheus Tavares | RM566844 |

## Descrição da solução

O **Nuclear Akinator** é uma solução educativa sobre radiação e tecnologia nuclear. O usuário escolhe um objeto ou alimento, responde a perguntas para que o sistema tente adivinhar a escolha e, ao final, recebe uma explicação científica sobre a relação do item com a radiação.

A API em Java cadastra e consulta os objetos e alimentos. Este módulo em .NET é complementar: um **Gerenciador de Perguntas Educativas**, aplicação de console administrativa que cadastra e consulta perguntas de múltipla escolha (alternativas A, B, C e D, resposta correta, explicação científica, categoria e nível de dificuldade). Essas perguntas poderão ser usadas futuramente na etapa educativa do Nuclear Akinator. A aplicação não é o jogo e não é um quiz jogável.

## O que foi desenvolvido na Sprint

- Entidade `PerguntaEducativa` com enums `CategoriaEducativa` e `NivelDificuldade`.
- Menu de console com cadastro, listagem, busca por ID, busca por categoria, busca por dificuldade e exclusão.
- Validações: campos obrigatórios, resposta correta entre A e D (maiúscula ou minúscula), categoria e dificuldade válidas.
- Regra de negócio: não é permitido cadastrar perguntas duplicadas (enunciado comparado sem diferenciar maiúsculas de minúsculas e ignorando espaços externos). O ID é gerado automaticamente.
- Persistência em arquivo JSON (`dados/perguntas.json`) com `System.Text.Json`.
- Tratamento de erros com `try/catch` na leitura e gravação do arquivo, tratando arquivo inexistente, vazio ou inválido.

## Como executar

Pré-requisito: .NET SDK instalado (versão LTS). Para conferir: `dotnet --version`.

Opção 1: Realizar um clone do repositório e abrir a pasta do projeto no terminal.

Opção 2: Baixar o arquivo ZIP do repositório, descompactar e abrir a pasta do projeto na IDE.

Se o seu SDK for diferente do .NET 8, altere `<TargetFramework>` no arquivo `NuclearAkinatorDotNet.csproj` (por exemplo, `net10.0`).

Os dados ficam em `dados/perguntas.json`, dentro da pasta do projeto. O arquivo é criado automaticamente caso não exista.

## Como o .NET se aplica à solução

O .NET fornece a plataforma para construir o módulo administrativo do Nuclear Akinator como uma aplicação de console em C#. A linguagem tipada, os enums e as classes permitem modelar a pergunta educativa de forma clara. A biblioteca `System.Text.Json` serializa os dados em arquivo JSON, garantindo persistência sem banco de dados, e o tratamento de exceções com `try/catch` mantém a aplicação estável diante de erros de leitura e gravação. A separação em camadas (modelo, validador, repositório, serviço e interface de console) deixa o código organizado e pronto para ser integrado ao restante da solução.
