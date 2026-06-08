# 1. Estágio de Build (Compila a solução inteira)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia os arquivos do projeto para o container
COPY . .

# Restaura e Builda a solução (o .sln resolve as dependências entre BackEnd, FrontEnd e API)
RUN dotnet restore "$(find . -name '*.sln')"
RUN dotnet build "$(find . -name '*.sln')" -c Release -o /app/build

# 2. Estágio de Publish (Publica apenas a API, que é o que o Render vai subir)
FROM build AS publish
# Aqui apontamos o caminho exato que identificamos no seu log anterior
RUN dotnet publish "BibliotecaAPI/BibliotecaAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. Estágio Final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Copia apenas o resultado do publish para o ambiente final
COPY --from=publish /app/publish .

# Define o ponto de entrada usando o nome fixo da sua DLL
ENTRYPOINT ["dotnet", "BibliotecaAPI.dll"]