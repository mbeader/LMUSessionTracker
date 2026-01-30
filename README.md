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

## Features

* Unified view of active sessions
	* Live session conditions (data provided using websockets)
	* Live standings (data provided using websockets)
		* Including standard timing information, fuel, lap progess, and car status
	* Live lap time tracking (data provided using websockets)
	* Entry list with flags, badges, and roles for humans
	* Multiplayer event resilience
		* Identification of clients connected to the same session
			* Whether as drivers or race engineers on the same team or competing teams
		* Continuous data collection when a client leaves a session where other clients were present
* Historical view of old sessions
	* Last known conditions
	* Final standings based on recorded lap data (not equivalent to game results)
	* Lap times for each car
* Live active sessions list (data provided using websockets)
* Best laps per driver per car per track
	* Prominent display of drivers marked as "known"
	* Highlighting of best sector times
	* Filters
		* Track
		* Date cutoff
		* Car class
		* Session type
		* Known drivers only
		* Online/offline

### Planned features

* Session tracking
	* Cars
		* Best laps/sectors within session per class/car/driver
		* Pit status
	* Data
		* Chat
		* Tires/fuel/VE from history
* Front-end
	* Page for full live timing
	* Track map
* Accept data from rF2 plugin

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

* Run `LMUSessionTracker.Client.exe` at any time
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
* Configure `appsettings.json` if desired

### Usage

* Run `dotnet LMUSessionTracker.Server.dll`
* The web interface is `http://localhost:5000` by default

See [HOSTING.md](HOSTING.md) for more information.

## Notes

### Event support

* Online
	* Daily/Weekly/Special Events: supported
	* Practice sessions: supported
	* Hosted: supported
	* Championships: untested
* Offline
	* Race Weekend: supported
	* Coop: untested

### LMU REST API

Since the client uses the REST API, LMU updates are likely to break things.
The expected JSON schema is found in `Core/Json/Schema`.
Validation can be enabled via appsettings to log violations.

### Known issues

* Game API responses
	* Fuel for the player car is almost always reported by the game as 100%
	* Some fields may not be updated in realtime, unclear when they actually do
		* Fuel updates after a completed sector
		* Car pit status may be stuck on `EXITING` until next pit
	* Sectors may have no time despite the lap still being valid
* Session tracking
	* During session transitions, the phase changing before the session type may cause an empty new session to be created
	* When a client already exists in a session, another client joining the server may not match its state by the time it starts sending data
	* Entry list similarity checking is currently very lenient and may result in strange behavior when a client leaves and rejoins an online practice session

## Building from source

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [Node.js](https://nodejs.org/) v22+ with npm
* [Angular CLI](https://angular.dev/)
	* `npm install -g @angular/cli`

### Build

From the root of the project directory, run:
```
publish.bat
```
The 64-bit Windows native AOT client can be found in `Client\out`.

The framework-dependent server can be found in `Server\out`.
