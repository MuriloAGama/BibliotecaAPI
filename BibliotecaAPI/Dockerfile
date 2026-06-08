# 1. Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .

# Restaura e Builda via Solution (funciona para qualquer estrutura de pastas)
RUN dotnet restore "$(find . -name '*.sln')"
RUN dotnet build "$(find . -name '*.sln')" -c Release -o /app/build

# 2. Estágio de Publish
FROM build AS publish
WORKDIR /src
# Comando de segurança: se o publish falhar, ele vai listar onde esta o csproj para ajudar a diagnosticar o problema
RUN echo "Estou na pasta:" && pwd && echo "Listando arquivos:" && ls -R | grep BibliotecaAPI.csproj || true
# Publica usando o caminho completo encontrado pelo find
RUN dotnet publish "$(find . -name 'BibliotecaAPI.csproj' | head -n 1)" -c Release -o /app/publish /p:UseAppHost=false

# 3. Estágio Final
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BibliotecaAPI.dll"]