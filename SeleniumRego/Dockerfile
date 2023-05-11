# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:4 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80
EXPOSE 99

# 0. Install essential packages
RUN apt-get update \
    && apt-get install -y \
        build-essential \
        cmake \
        git \
        wget \
        unzip \
        unixodbc-dev \
    && rm -rf /var/lib/apt/lists/*

# 1. Install Chrome (root image is debian)
# See https://stackoverflow.com/questions/49132615/installing-chrome-in-docker-file
ARG CHROME_VERSION="google-chrome-stable"
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - \
  && echo "deb http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google-chrome.list \
  && apt-get update -qqy \
  && apt-get -qqy install \
    ${CHROME_VERSION:-google-chrome-stable} \
  && rm /etc/apt/sources.list.d/google-chrome.list \
  && rm -rf /var/lib/apt/lists/* /var/cache/apt/*

# 2. Install Chrome driver used by Selenium
RUN LATEST=$(wget -q -O - http://chromedriver.storage.googleapis.com/LATEST_RELEASE) && \
    wget http://chromedriver.storage.googleapis.com/$LATEST/chromedriver_linux64.zip && \
    unzip chromedriver_linux64.zip && ln -s $PWD/chromedriver /usr/local/bin/chromedriver

ENV PATH="/usr/local/bin/chromedriver:${PATH}"





FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SeleniumRego/SeleniumRego.csproj", "SeleniumRego/"]
RUN dotnet restore "SeleniumRego/SeleniumRego.csproj"
COPY . .
WORKDIR "/src/SeleniumRego"
RUN dotnet build "SeleniumRego.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeleniumRego.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

    
#RUN apt-get update && \
    #apt-get install -y gnupg wget curl unzip --no-install-recommends && \
    #&& apt-get install -y libglib2.0-0 libsm6 libxrender1 libxext6 \
    #wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - && \
    #echo "deb http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list && \
    #apt-get update -y && \
    #apt-get install -y google-chrome-stable && \
    #CHROMEVER=$(google-chrome --product-version | grep -o "[^\.]*\.[^\.]*\.[^\.]*") && \
    #DRIVERVER=$(curl -s "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_$CHROMEVER") && \
    #wget -q --continue -P /chromedriver "http://chromedriver.storage.googleapis.com/$DRIVERVER/chromedriver_linux64.zip" && \
    #unzip /chromedriver/chromedriver* -d /chromedriver








## install essential tools
#RUN apt-get update && apt-get install unzip
#
## install Chrome
#RUN wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb \
    #&& apt-get install ./google-chrome-stable_current_amd64.deb -y
#
## download matching Chrome Driver
## https://stackoverflow.com/a/61928952/167920
#RUN chromeVersion=$(google-chrome --product-version) \
    #&& chromeMajorVersion=${chromeVersion%%.*} \
    #&& latestDriverReleaseURL=https://chromedriver.storage.googleapis.com/LATEST_RELEASE_$chromeMajorVersion \
    #&& wget $latestDriverReleaseURL \
    #&& latestDriverVersionFileName="LATEST_RELEASE_"$chromeMajorVersion \
    #&& latestFullDriverVersion=$(cat $latestDriverVersionFileName) \
    #&& rm $latestDriverVersionFileName \
    #&& finalURL="http://chromedriver.storage.googleapis.com/"$latestFullDriverVersion"/chromedriver_linux64.zip" \
    #&& wget $finalURL \
    #&& unzip chromedriver_linux64.zip \
    #&& rm chromedriver_linux64.zip
    #
## Copy chromedriver to the desired folder
#RUN mkdir --parents /home/site/wwwroot/bin/Debug/net6.0 \
#&& mv chromedriver /home/site/wwwroot/bin/Debug/net6.0/












## Install unzip
#RUN apt-get install -yqq unzip
#
#RUN apt-get update \
    #&& apt-get install -y libglib2.0-0 libsm6 libxrender1 libxext6 \
    #&& apt-get install -y wget gnupg \
    #&& ln -s /lib/x86_64-linux-gnu/libglib-2.0.so.0 /usr/lib/libglib-2.0.so.0 \
    #&& export LD_LIBRARY_PATH=/usr/lib/x86_64-linux-gnu:$LD_LIBRARY_PATH \
    #&& apt-get install -y libqt5core5a \
    #&& rm -rf /var/lib/apt/lists/*
## Install driver dependencies
#RUN apt-get update && \
    #apt-get -qq -y install  libxpm4 libxrender1 libgtk2.0-0 libnss3\ 
       #libgconf-2-4  libpango1.0-0 libxss1 libxtst6 fonts-liberation\ 
       #libappindicator1 xdg-utils
#
## Install font lib dependencies
#RUN apt-get -y install \
               #xvfb gtk2-engines-pixbuf \
               #xfonts-cyrillic xfonts-100dpi xfonts-75dpi xfonts-base xfonts-scalable \
               #imagemagick x11-apps zip
#
## Install Chrome WebDriver
#RUN wget -O /tmp/chromedriver.zip https://chromedriver.storage.googleapis.com/2.46/chromedriver_linux64.zip \
    #&& unzip /tmp/chromedriver.zip -d /usr/local/bin \
    #&& rm /tmp/chromedriver.zip
#ENV PATH="/usr/local/bin:${PATH}"
#
## Install Google Chrome
#RUN curl -sS -o - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - && \
    #echo "deb http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google-chrome.list && \
    #apt-get -yqq update && \
    #apt-get -yqq install google-chrome-stable && \
    #rm -rf /var/lib/apt/lists/*
#
#
##&& strip --remove-section=.note.ABI-tag /usr/lib/x86_64-linux-gnu/libQt5Core.so.5 \
    ##&& apt-get install -y google-chrome-stable fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf libxss1 \
      ##--no-install-recommends \