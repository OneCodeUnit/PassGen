# Онлайн-версия PassGen: сборка Blazor WebAssembly и раздача статики через Caddy.
# Контекст сборки - корень репозитория (проект ссылается на ../PasswordGenerator.cs).

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Сначала только проект и общий код - слой с restore кэшируется
COPY PasswordGenerator.cs ./
COPY PassGen.Web/PassGen.Web.csproj PassGen.Web/
RUN dotnet restore PassGen.Web/PassGen.Web.csproj

COPY PassGen.Web/ PassGen.Web/
RUN dotnet publish PassGen.Web/PassGen.Web.csproj -c Release -o /app --no-restore


FROM caddy:2-alpine
COPY docker/Caddyfile /etc/caddy/Caddyfile
COPY --from=build /app/wwwroot /srv

# Caddy внутри контейнера слушает только HTTP на 8080 (HTTPS - на внешнем прокси)
EXPOSE 8080
HEALTHCHECK --interval=60s --timeout=3s --start-period=5s --retries=3 \
    CMD wget -q --spider http://127.0.0.1:8080/ || exit 1
