FROM mcr.microsoft.com/dotnet/sdk:6.0 AS installer-env
# Build requires 3.1 SDK
COPY --from=mcr.microsoft.com/dotnet/core/sdk:3.1 /usr/share/dotnet /usr/share/dotnet

COPY ["SeleniumRego/SeleniumRego.csproj", "/src/dotnet-function-app/SeleniumRego/"]
COPY . /src/dotnet-function-app
RUN cd /src/dotnet-function-app/SeleniumRego && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish SeleniumRego.csproj --output /home/site/wwwroot


FROM mcr.microsoft.com/azure-functions/dotnet:4-appservice
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - \
  && echo "deb http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google-chrome.list \
  && apt-get update -qqy \
  && apt-get -qqy install \
    google-chrome-stable \
  && rm /etc/apt/sources.list.d/google-chrome.list \
  && rm -rf /var/lib/apt/lists/* /var/cache/apt/*

RUN apt-get update \
    && apt-get install -y \
        build-essential \
        cmake \
        git \
        wget \
        unzip \
        unixodbc-dev \
        libglib2.0-0 \
        libnss3 \
        libxss1 \
        libasound2 \
        libappindicator3-1 \
        fonts-liberation \
        xdg-utils \
    && rm -rf /var/lib/apt/lists/*

COPY . /home/site/wwwroot
    
COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]