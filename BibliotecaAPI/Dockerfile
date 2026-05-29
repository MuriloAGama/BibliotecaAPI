# Etapa 1: Build da aplicação usando o SDK do .NET 10
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /src

# Copia a pasta inteira do projeto para dentro de /src/BibliotecaAPI no container
COPY BibliotecaAPI/ ./BibliotecaAPI/

# Entra na pasta onde o código realmente está para restaurar e publicar
WORKDIR /src/BibliotecaAPI
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa 2: Runtime do .NET 10 para execução (Imagem leve)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
# Busca os arquivos publicados de dentro da pasta out correta
COPY --from=build-env /src/BibliotecaAPI/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BibliotecaAPI.dll"]
