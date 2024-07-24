# SeleniumRego - Azure Functions App using Docker

## Overview

SeleniumRego is an Azure Functions application that performs vehicle registration lookups using Selenium WebDriver to interact with the Queensland Government's vehicle search website. The application is built with .NET 6 and runs inside a Docker container.

## Features

- HTTP-triggered Azure Function for vehicle registration lookup
- Uses Selenium WebDriver to automate web interactions
- Dockerized for easy deployment and scalability
- Headless Chrome for efficient and fast web scraping

## Project Structure

The project consists of the following main components:

- **SeleniumRego.csproj**: Project file for the .NET Azure Functions app
- **RegoLookup.cs**: Main Azure Function for handling HTTP requests and performing vehicle registration lookups
- **Dockerfile**: Docker configuration for building and running the application in a container

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker](https://www.docker.com/get-started)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/your-repo/SeleniumRego.git
cd SeleniumRego
```
## Build and Run Locally

1. **Build the Docker Image**

    ```bash
    docker build -t seleniumrego .
    ```

2. **Run the Docker Container**

    ```bash
    docker run -p 8080:80 seleniumrego
    ```

3. **Test the Function**

Open your browser or use a tool like `curl` or Postman to send a request to the function endpoint:

    ```bash
    curl http://localhost:8080/api/RunLookup?plate=YOUR_PLATE_NUMBER
    ```

## Deploy to Azure

1. **Login to Azure**

    ```bash
    az login
    ```

2. **Create a Resource Group**

    ```bash
    az group create --name seleniumrego-rg --location <your-preferred-location>
    ```

3. **Create an Azure Function App**

    ```bash
    az functionapp create --resource-group seleniumrego-rg --consumption-plan-location <your-preferred-location> --runtime dotnet --functions-version 4 --name seleniumrego-app --storage-account <your-storage-account>
    ```

4. **Deploy the Docker Image to Azure**

    ```bash
    az functionapp config container set --name seleniumrego-app --resource-group seleniumrego-rg --docker-custom-image-name <your-dockerhub-username>/seleniumrego:latest
    ```

## Configuration

### local.settings.json

Ensure you have a `local.settings.json` file in the root of your project for local development:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  }
}
```

### host.json

Configure function behavior using the `host.json` file:

```json
{
  "version": "2.0",
  "logging": {
    "applicationInsights": {
      "samplingExcludedTypes": "Request",
      "samplingSettings": {
        "isEnabled": true
      }
    }
  }
}
```

### License
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


### Contact
For more information, please contact braydennepean@gmail.com
