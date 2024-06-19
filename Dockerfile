# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set the working directory inside the container
WORKDIR /Portfolio-Box

COPY . ./
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory inside the container
WORKDIR /Portfolio-Box

# Copy the published output from the build stage
COPY --from=build-env /Portfolio-Box/out .

# Expose the port the app runs on
EXPOSE 5000

# Set the entry point for the container to run the application
ENTRYPOINT ["dotnet", "Portfolio-Box.dll"]
