# Como Executar

## CLI

- Execute o script em db/createdb.sql

- Inicie a API

```powershell
cd src/Shark.Api/ && dotnet run
```

## Docker

```powershell
docker-compose build && docker-compose up -d
```

# Como testar


## Testes de unidade

basta executar o projeto com os testes de unidade

```powershell
cd tests/Shark.UnitTests
dotnet test
```

## Testes de integração

- Antes de executa-los, inicie o banco de dados, e informe a string de conexão do arquivo appsettings.Development.json
- Execute os testes de integração em tests/Shark.IntegrationTests
- 
