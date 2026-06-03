FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia tudo para o diretório de trabalho
COPY . .

# Restaura a solution automaticamente, não importa onde ela esteja na estrutura
RUN dotnet restore "$(find . -name '*.sln')"

# Faz o build da solution encontrada
RUN dotnet build "$(find . -name '*.sln')" -c Release -o /app/build

# Estágio de Publish
FROM build AS publish
# já buildou, especifica o projeto principal para publicar
RUN dotnet publish "BibliotecaAPI/BibliotecaAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio Final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BibliotecaAPI.dll"]