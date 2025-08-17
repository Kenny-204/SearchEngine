# ---------- build stage ----------
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    
    # copy everything in Backend
    COPY ./Backend ./Backend
    
    # restore using the solution file
    WORKDIR /src/Backend
    RUN dotnet restore SearchEngine.sln
    
    # publish only the API project
    RUN dotnet publish SearchEngine.API/SearchEngine.API.csproj -c Release -o /app/publish /p:UseAppHost=false
    
    # ---------- runtime stage ----------
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
    WORKDIR /app
    COPY --from=build /app/publish .
    
    # Bind to all interfaces
    ENV ASPNETCORE_URLS=http://0.0.0.0:8080
    EXPOSE 8080
    
    CMD ["dotnet", "SearchEngine.API.dll"]
    