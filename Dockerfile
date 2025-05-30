# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy file project và restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy toàn bộ source và publish bản release
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy output từ build stage sang
COPY --from=build /app/out ./

# Expose cổng app sẽ chạy
EXPOSE 5000

# Thiết lập URL app lắng nghe (cổng 5000)
ENV ASPNETCORE_URLS=http://+:5000

# Lệnh chạy app
ENTRYPOINT ["dotnet", "cybersoft-final-project.dll"]
