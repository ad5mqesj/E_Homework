#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["E_Homework/E_Homework.csproj", "E_Homework/"]
COPY ["E_Homework.DTO/E_Homework.DTO.csproj", "E_Homework.DTO/"]
RUN dotnet restore "E_Homework/E_Homework.csproj"
COPY . .
WORKDIR "/src/E_Homework"
RUN dotnet build "E_Homework.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "E_Homework.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E_Homework.dll"]