FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build

WORKDIR /app

COPY /BetDotNext ./

RUN dotnet restore

COPY /BetDotNext ./
WORKDIR /app

RUN dotnet publish -c Release -o out
RUN dotnet test --no-build --no-restore -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS runtime

RUN apk add --no-cache icu-libs

WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "BetDotNext.dll"]
