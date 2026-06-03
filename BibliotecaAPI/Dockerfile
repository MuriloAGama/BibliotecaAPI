FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia tudo (incluindo a solution)
COPY . .

# Restaura a solução inteira
RUN dotnet restore "BibliotecaAPI.sln"

# Faz o build da solução inteira
RUN dotnet build "BibliotecaAPI.sln" -c Release -o /app/build

FROM build AS publish
# Faz o publish apontando para o projeto da API, mas já aproveitando o build da solução
RUN dotnet publish "BibliotecaAPI/BibliotecaAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final continua igual
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BibliotecaAPI.dll"]