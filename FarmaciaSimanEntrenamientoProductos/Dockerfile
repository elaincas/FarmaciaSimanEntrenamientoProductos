FROM mcr.microsoft.com/dotnet/aspnet:2.1
WORKDIR /app    
COPY ./out .
ENV TZ=America/Tegucigalpa
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "/app/FarmaciaSimanEntrenamientoProductos.dll"]