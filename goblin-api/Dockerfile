FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Copy csproj and restore as distinct layers to leverage Docker cache
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code. 
# This is mainly for the initial build; the volume mount in docker-compose.yml 
# will overlay this with your local files.
COPY . ./

# Expose the port the app runs on, as defined by ASPNETCORE_URLS in docker-compose.yml
EXPOSE 8080

# Set the default command to run the app with watch for development
# Add --no-launch-profile to ignore launchSettings.json and explicitly set the URL
CMD ["dotnet", "watch", "run", "--no-restore", "--no-launch-profile", "--urls", "http://+:8080"]
