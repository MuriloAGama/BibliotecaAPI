# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia tudo para o diretório de trabalho
COPY . .

# Restaura a solution (dinâmico)
RUN dotnet restore "$(find . -name '*.sln')"

# Faz o build da solution (dinâmico)
RUN dotnet build "$(find . -name '*.sln')" -c Release -o /app/build

# Estágio de Publish
FROM build AS publish
# Aqui está o segredo: não usar mais "BibliotecaAPI/BibliotecaAPI.csproj"
# Usar o find para achar o .csproj, não importa onde ele esteja
RUN dotnet publish "$(find . -name '*.csproj')" -c Release -o /app/publish /p:UseAppHost=false

# Estágio Final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "$(find . -name '*.dll' | grep -v 'obj' | head -n 1)"]