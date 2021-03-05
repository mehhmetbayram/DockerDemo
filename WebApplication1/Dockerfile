FROM mcr.microsoft.com/dotnet/sdk:5.0 as firstimage
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish WebApplication1.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=firstimage /app/out .
ENV ASPNETCORE_URLS="http://*:1453"
ENTRYPOINT ["dotnet","WebApplication1.dll"]