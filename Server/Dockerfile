# Use the ASP.NET Core 3.1 runtime as the base image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK to build the app
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
# Copy csproj and restore any dependencies (via NuGet)
COPY ["Server.csproj", ""]
RUN dotnet restore "./Server.csproj"
# Publish the compiled project to the /app/publish directory
COPY . .
RUN dotnet build "Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.csproj" -c Release -o /app/publish

# Final stage / image
FROM base AS final
WORKDIR /app
# Copy the publish directory from the previous stage
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.dll"]
