all : cleanup restore build

cleanup:
		dotnet clean DataIntegrationChallenge.API/
		 
restore:
		dotnet restore DataIntegrationChallenge.API/
		 
build:
		dotnet build DataIntegrationChallenge.API/ -c Release -o ./build
		cp DataIntegrationChallenge.API/appsettings.json DataIntegrationChallenge.API/../
		 
run:
		dotnet run -p DataIntegrationChallenge.API/DataIntegrationChallenge.API.csproj
