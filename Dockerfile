# syntax=docker/dockerfile:1

# --- Сборка статического сайта ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Сначала только проекты - слой с восстановленными пакетами кэшируется
COPY PassGen.Web/PassGen.Web.csproj PassGen.Web/
COPY PasswordGenerator.cs ./
RUN dotnet restore PassGen.Web/PassGen.Web.csproj

COPY PassGen.Web/ PassGen.Web/
RUN dotnet publish PassGen.Web/PassGen.Web.csproj -c Release --no-restore -o /out

# Инлайн-блок importmap в index.html разрешается в CSP по хешу, а не через 'unsafe-inline'
COPY docker/security-headers.conf.template /tmp/security-headers.conf.template
RUN set -eu; \
    hash=$(perl -0777 -ne 'print $1 if m{<script type="importmap">(.*?)</script>}s' /out/wwwroot/index.html \
        | openssl dgst -sha256 -binary | openssl base64 -A); \
    sed "s|__IMPORTMAP_HASH__|sha256-${hash}|" /tmp/security-headers.conf.template > /tmp/security-headers.conf

# --- Раздача статики ---
FROM nginxinc/nginx-unprivileged:stable-alpine AS runtime
COPY docker/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /tmp/security-headers.conf /etc/nginx/security-headers.conf
COPY --from=build /out/wwwroot /usr/share/nginx/html
EXPOSE 8080
HEALTHCHECK --interval=30s --timeout=3s CMD wget -qO- http://127.0.0.1:8080/ >/dev/null || exit 1
