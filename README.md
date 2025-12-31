# LMUSessionTracker

LMUSessionTracker is a distributed system that tracks live sessions in Le Mans Ultimate (LMU).
Without a dedicated server and with the growing significance of official endurance events,
there is a need for a system that uses game clients to provide data for something
that was once ubiquitous to self-hosted game servers.
LMUSessionTracker is more than just live timing, it is a collaborative means for the aggregation and display of session data.

This repo contains a [client](#client) and a [server](#server).

> [!WARNING]
> This project is under active development and may change significantly between `0.x` releases

### Goal

The objective of this project is to achieve a unified and consistent view of live team endurance races.
To achieve this, each member of a team can run the client which will send session data to the server.
Regardless of any members role or whether they are currently connected to the server,
as long as one exists, data will continue to be collected uninterrupted.

## Client

The client is intended for those playing the game, whether you're driving or spectating.
It acquires data from the LMU REST API and simply sends it to an LMUSessionTracker server.

### Prerequisites

* 64-bit Windows 10 or 11
* Le Mans Ultimate V1.2+
* A recent version of the Microsoft Visual C++ 2015-2022 (v14) Redistributable (required by LMU anyway)
* Compatible LMUSessionTracker server to connect to

### Setup

* Download latest release
* Set target server (defined by `LMU.BaseUri`) in `appsettings.json`
* Run `LMUSessionTracker.Client.exe` once

### Usage

* Run `LMUSessionTracker.Client.exe`
* Start Le Mans Ultimate
* Join a session to start collecting data
* Close the console window to stop collecting data at any time

## Server

The server is intended to run on a separate machine from those running the game,
in a location accessible to clients over the network.
It receives data from the LMUSessionTracker client, provides a web interface, and stores data using SQLite.

### Prerequisites

* [.NET 8 Runtime and ASP.NET Core Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Setup

* Download latest release
* Configure `appsettings.json`

### Usage

* Run `dotnet LMUSessionTracker.Server.dll`
* The web interface is `http://localhost:5000` by default

See [HOSTING.md](HOSTING.md) for more information.

## Notes

### Event support

* Online
	* Daily/Weekly/Special Events: supported
	* Practice sessions: supported
	* Hosted: untested
	* Championships: untested
* Offline
	* Race Weekend: supported
	* Coop: untested

### LMU REST API

Since the client uses the REST API, LMU updates are likely to break things.
The expected JSON schema is found in `Core/Json/Schema`.
Validation can be enabled via appsettings to log violations.
