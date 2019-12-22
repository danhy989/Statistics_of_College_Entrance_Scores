FROM mcr.microsoft.com/dotnet/core/sdk:2.2

RUN mkdir /app
WORKDIR /app

COPY Statistics_College_Entrance_Scores.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

EXPOSE 5000/tcp
CMD ["dotnet", "out/Statistics_College_Entrance_Scores.dll"]
