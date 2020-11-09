FROM golang:alpine as cert

RUN apk update \
  && apk add git openssl nss-tools \
  && rm -rf /var/cache/apk/*

RUN cd /go && \
  go get -u github.com/FiloSottile/mkcert && \
  cd src/github.com/FiloSottile/mkcert && \
  go build -o /bin/mkcert

WORKDIR /root/.local/share/mkcert

COPY certs/rootCA*.pem /root/.local/share/mkcert/

ARG DNS_NAMES
ARG CERT_NAME

RUN mkcert -install \
  && mkcert -key-file https-${CERT_NAME}.key -cert-file https-${CERT_NAME}.pem ${DNS_NAMES} \
  && openssl pkcs12 -export -out https-${CERT_NAME}.pfx -inkey https-${CERT_NAME}.key -in https-${CERT_NAME}.pem -certfile rootCA.pem -passout pass:https-${CERT_NAME}


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
ENV PATH="$PATH:/root/.dotnet/tools"
WORKDIR /src
COPY ["src/Services/Identity/Identity.API/Identity.API.csproj", "src/Services/Identity/Identity.API/"]
RUN dotnet restore "src/Services/Identity/Identity.API/Identity.API.csproj"

COPY . .
WORKDIR "/src/src/Services/Identity/Identity.API"

RUN dotnet publish -c Debug -o /dist

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim

RUN apt-get update
RUN apt-get -y install libnss3-tools

ARG CERT_NAME

RUN curl -L https://github.com/FiloSottile/mkcert/releases/download/v1.4.2/mkcert-v1.4.2-linux-amd64 > mkcert
RUN chmod +x mkcert
RUN cp mkcert /usr/local/bin/

COPY --from=cert /root/.local/share/mkcert/rootCA.pem /root/.local/share/mkcert/rootCA.pem
COPY --from=cert /root/.local/share/mkcert/https-${CERT_NAME}.pfx /app/

ENV Kestrel__Certificates__Default__Path=/app/https-${CERT_NAME}.pfx
ENV Kestrel__Certificates__Default__Password=https-${CERT_NAME}

RUN mkcert -install
RUN apt-get -y remove libnss3-tools
RUN rm -rf /usr/local/bin/mkcert

WORKDIR /app
COPY --from=build /dist /app

EXPOSE 80
EXPOSE 443

CMD ["dotnet", "Identity.API.dll"]