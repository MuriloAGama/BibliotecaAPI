FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copia o arquivo .csproj de dentro da subpasta para a estrutura do Docker
COPY ["BibliotecaAPI/BibliotecaAPI.csproj", "BibliotecaAPI/"]
RUN dotnet restore "BibliotecaAPI/BibliotecaAPI.csproj"

# 2. Copia o restante dos arquivos do repositório
COPY . .

# 3. Entra na pasta do projeto para rodar o build e publish
WORKDIR "/src/BibliotecaAPI"
RUN dotnet build "BibliotecaAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BibliotecaAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. Configura o container final para rodar a API de forma contínua
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Garante que o ASP.NET vai escutar a porta que o Render mandar
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "BibliotecaAPI.dll"]