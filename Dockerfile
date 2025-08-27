# ---------- build stage ----------
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src

    # Copy solution file and project files first (for better layer caching)
    COPY Backend/*.sln ./Backend/
    COPY Backend/SearchEngine.API/*.csproj ./Backend/SearchEngine.API/
    # Copy any other project files you have in the solution
    # COPY Backend/SearchEngine.Core/*.csproj ./Backend/SearchEngine.Core/
    # COPY Backend/SearchEngine.Data/*.csproj ./Backend/SearchEngine.Data/

    # Restore dependencies
    WORKDIR /src/Backend
    RUN dotnet restore SearchEngine.sln

    # Copy the rest of the source code
    COPY Backend/ ./

    # Publish only the API project
    RUN dotnet publish SearchEngine.API/SearchEngine.API.csproj -c Release -o /app/publish /p:UseAppHost=false

    # ---------- runtime stage ----------
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
    WORKDIR /app
    COPY --from=build /app/publish .

    # Create non-root user for security
    RUN addgroup --system --gid 1001 dotnetgroup
    RUN adduser --system --uid 1001 --ingroup dotnetgroup dotnetuser
    USER dotnetuser

    ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}
    EXPOSE ${PORT:-8080}

    CMD ["dotnet", "SearchEngine.API.dll"]