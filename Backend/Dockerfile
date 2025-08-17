# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first (for restore cache)
COPY *.sln .
COPY SearchEngine.API/SearchEngine.API.csproj SearchEngine.API/
COPY SearchEngine.Core/SearchEngine.Core.csproj SearchEngine.Core/
COPY SearchEngine.Query/SearchEngine.Query.csproj SearchEngine.Query/
COPY SearchEngine.Indexing/SearchEngine.Indexing.csproj SearchEngine.Indexing/
COPY SearchEngine.DocumentProcessing/SearchEngine.DocumentProcessing.csproj SearchEngine.DocumentProcessing/

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Publish only the API project
WORKDIR /src/SearchEngine.API
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Configure for Render hosting
# Render will set PORT environment variable
ENV PORT=8080
ENV ASPNETCORE_URLS=http://+:${PORT}

# Make sure we listen on all interfaces
EXPOSE ${PORT}

# Copy .env file for local development (will be overridden by Render environment variables)
COPY --from=build /src/SearchEngine.API/.env .

ENTRYPOINT ["dotnet", "SearchEngine.API.dll"]
    