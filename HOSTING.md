# Hosting

The sections below assume you already have:

* A domain name (e.g. your own or a free subdomain from [FreeDNS](https://freedns.afraid.org/))
* A TLS certificate for that domain (e.g. using [Let's Encrypt](https://letsencrypt.org/))

## Linux with systemd

For any distro.

### Prerequisites

* [.NET 8 Runtime and ASP.NET Core Runtime](https://learn.microsoft.com/en-us/dotnet/core/install/linux)
* Packages
	* nginx

### Setup

Download the latest release and extract to `/var/www/LMUSessionTracker` or any other directory you wish to serve.

Follow the steps in the [nginx section](#nginx).

Create the systemd unit `/etc/systemd/system/lmust.service` with the following content (replace `example.com:5000` with your domain and chosen port):
```
[Unit]
Description=LMUSessionTracker

[Service]
WorkingDirectory=/var/www/LMUSessionTracker
ExecStart=/usr/bin/dotnet /var/www/LMUSessionTracker/LMUSessionTracker.Server.dll --urls="http://example.com:5000/"
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=lmust
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_NOLOGO=true

[Install]
WantedBy=multi-user.target
```

Enable and start the unit.
```
systemctl enable lmust --now
```

## [Pterodactyl](https://pterodactyl.io/)

### Prerequisites

* A configured and running Pterodactyl instance
* The [Generic C# egg](https://pterodactyleggs.com/egg/673601c04924a4e9bbd4b793) JSON file (`egg-generic-c.json`)
* Packages (on the node running the server)
	* nginx

### Setup

In Pterodactyl, create a new nest and add a new egg using the Generic C# egg JSON file.

Set the Process Management Start Configuration to the following:
```
{
	"done": [
		"Application started. Press Ctrl+C to shut down."
	]
}
```

LMUSessionTracker can now either be acquired [directly from the repo](#using-the-git-repo)
or [using the latest release](#using-the-latest-release-archive).

Once the server is running, 
follow the steps in the [nginx section](#nginx) on the node containing the server.

#### Using the git repo

If you'd like to run the project using the GitHub git repo, no more configuration is necessary.
Create a server using the .NET 8 yolk,
fill in the startup fields appropriately (Install Branch should correspond to the latest release),
and append `--urls="http://example.com:{{SERVER_PORT}}/"` (with your domain) to the Startup Command.

#### Using the latest release archive

To use the latest release archive, more egg configuration is required.

* Configuration
	* Name: `LMUSessionTracker`
	* Description: `LMUSessionTracker: session tracking and live timing for LMU, using the generic C# (dotnet) egg.`
	* Docker Images: `Dotnet_8|ghcr.io/ptero-eggs/yolks:dotnet_8`
	* Startup Command: `cd lmust; dotnet LMUSessionTracker.Server.dll --urls="http://{{SERVER_IP}}:{{SERVER_PORT}}/"`

Create a server. In the Startup configuration, set User Uploaded Files to `1`,
and in Startup Command replace `{{SERVER_IP}}` with your domain.

In the server files interface, create a directory named `lmust` and enter it.

Upload the latest release archive and unarchive the file. 

## [nginx](https://nginx.org/)

Create the file `/etc/nginx/sites-available/lmust.conf` with the content found in [nginx.conf](nginx.conf).
Configure the port in `upstream lmust`, and within the the `server` sections configure `server_name` and the TLS cert paths.

Symlink the file.
```
ln -s /etc/nginx/sites-available/lmust.conf /etc/nginx/sites-enabled/lmust`
```

Test the configuration.
```
nginx -t
```

Reload the configuration
```
systemctl reload nginx
```

## Configuration

The best way to persist changes to application configuration is by creating `appsettings.Production.json`.
This can either be a direct copy of `appsettings.json` or a mirror of its structure containing only setting you wish to be set.
An example is below.
```
{
	"Server": {
		"RejectAllClients": true
	}
}
```
