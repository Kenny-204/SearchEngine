# ---------- build stage ----------
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    
    # Copy solution and project files first (for restore cache)
    COPY Backend/SearchEngine.sln .
    COPY Backend/SearchEngine.API/SearchEngine.API.csproj Backend/SearchEngine.API/
    COPY Backend/SearchEngine.Core/SearchEngine.Core.csproj Backend/SearchEngine.Core/
    COPY Backend/SearchEngine.Query/SearchEngine.Query.csproj Backend/SearchEngine.Query/
    COPY Backend/SearchEngine.Indexing/SearchEngine.Indexing.csproj Backend/SearchEngine.Indexing/
    COPY Backend/SearchEngine.DocumentProcessing/SearchEngine.DocumentProcessing.csproj Backend/SearchEngine.DocumentProcessing/
    
    # Restore dependencies
    RUN dotnet restore SearchEngine.sln
    
    # Copy everything else
    COPY Backend/ ./Backend/
    
    # Publish only the API project
    WORKDIR /src/Backend/SearchEngine.API
    RUN dotnet publish -c Release -o /app/publish --no-restore
    
    # ---------- runtime stage ----------
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
    WORKDIR /app
    COPY --from=build /app/publish .
    ENV ASPNETCORE_URLS=http://0.0.0.0:8080
    EXPOSE 8080
    ENTRYPOINT ["dotnet", "SearchEngine.API.dll"]
    