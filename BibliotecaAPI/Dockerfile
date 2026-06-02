FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Como o "Build Context" é a pasta BibliotecaAPI, 
# o .csproj já está na raiz do contexto.
COPY . .
RUN dotnet restore
# Copia o restante dos arquivos
COPY . .

RUN find . -name "*.csproj"
# Faz o build (já estamos em /src)
RUN dotnet build "BibliotecaAPI/BibliotecaAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BibliotecaAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Configuração final
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BibliotecaAPI.dll"]