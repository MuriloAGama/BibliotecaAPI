# 1. Estágio de Build (SDK contém tudo o que precisa para compilar o projeto)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
# Usa o find aqui porque o SDK tem ferramentas de terminal para rodar o build
RUN dotnet restore "$(find . -name '*.sln')"
RUN dotnet build "$(find . -name '*.sln')" -c Release -o /app/build

# 2. Estágio de Publish (Gera os arquivos finais)
FROM build AS publish
RUN dotnet publish "$(find . -name '*.csproj')" -c Release -o /app/publish /p:UseAppHost=false

# 3. Estágio Final (Runtime - é leve, não tem SDK, por isso é mais rápido)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Nome fixo, sem comandos de busca complexos.
# O container só precisa saber o nome da DLL que foi gerada no passo anterior.
ENTRYPOINT ["dotnet", "BibliotecaAPI.dll"]