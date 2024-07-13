# Deploy .Net Web API Service using Entity Framework on Ubuntu

## Find .Net version of your application:

### Check target framework of your application

## Find Ubuntu OS version:

```bash
lsb_release -a
```

## Verify if .Net SDK and runtime is installed:

### To see which versions are installed:

```bash
dotnet --info
dotnet --list-sdks 
dotnet --list-runtimes 
```

## Install .Net SDK and runtime if not installed:

### Find the installation command and instructions:

 1. Search on Google by “.net sdk ubuntu”
 2. Click on the official website: https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install?tabs=dotnet8&pivots=os-linux-ubuntu-2404 
 3. Choose the appropriate Ubuntu version what is used
 4. Update system:
  ```bash
  sudo apt-get update
  sudo apt-get upgrade
  ```
  b. Install the SDK:
  ```bash
  sudo apt-get install -y dotnet-sdk-8.0
  ```
  c. Install the runtime:
  ```bash
  sudo apt-get install -y aspnetcore-runtime-8.0	
  sudo apt-get install -y dotnet-runtime-8.0
  ```
  d. Use the command if not installed:
  ```bash
  sudo apt-get update
  sudo apt-get upgrade
  sudo apt-get install -y dotnet-sdk-8.0 --fix-missing
  ```

## Clone your app on Ubuntu

```bash
git clone https://github.com/awsaf-utm/Deploy-.Net-Web-API-Service-using-Entity-Framework-on-Ubuntu.git
```

## Go to the app folder

```bash
cd Deploy-.Net-Web-API-Service-using-Entity-Framework-on-Ubuntu/BookInformationService
```

## Install dependencies:

```bash
dotnet restore
```

## Publish the Project

```bash
dotnet publish -c Release -o out
```

## Go to the publish folder “out”

```bash
cd out
```

## Add port:

```bash
sudo ufw allow 3101/tcp
```

## Restart Firewall:

```bash
sudo ufw reload
```

## Check status:

```bash
sudo ufw status
```

## Run the Published Application:

### Before running the app, set environment as Development

#### Set environment:

```bash
export ASPNETCORE_ENVIRONMENT=Development
```

#### Run the app to access from Browsers:

```bash
dotnet BookInformationService.dll
```

## Access on the Ubuntu:

http://locakhost:3101/swagger/index.html

### Or,

http://[Ubuntu IP]:3101/swagger/index.html

#### Example:

http://192.168.49.1:3101/swagger/index.html

## Access from Host PC:

### Find the physical network interface card (NIC) on the host:

#### Note: Physical NICs often have names that start with en (Ethernet) or eth (Ethernet) followed by a number (e.g., enp0s3, eth0).

```bash
ifconfig
```
### And

```bash
ip route
```

### Use the Physical NIC to access from host OS:

http://[Physical NIC]:3101/swagger/index.html

#### Example:

http://192.168.1.249:3101/swagger/index.html


