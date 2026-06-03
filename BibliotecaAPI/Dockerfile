FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia tudo de uma vez. Como o comando 'restore' e 'build' 
# já vão ler o que está aqui dentro, um único COPY resolve.
COPY . .

# Faz o restore (o dotnet encontra o .csproj automaticamente na árvore de pastas)
RUN dotnet restore

# Faz o build
# RUN dotnet build "BibliotecaAPI/BibliotecaAPI.csproj" -c Release -o /app/build
RUN dotnet build "BibliotecaAPI.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "BibliotecaAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false
# RUN dotnet publish "BibliotecaAPI/BibliotecaAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Configuração final
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BibliotecaAPI.dll"]