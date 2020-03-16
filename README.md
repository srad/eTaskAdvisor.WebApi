# eTaskAdvisor.WebApi
 
 [![Build Status](http://sedrad.com:8080/buildStatus/icon?job=eTaskAdvisor.WebApi)](http://sedrad.com:8080/job/eTaskAdvisor.WebApi/)

This is a .NET Core 3.x Web-Api implementation of [eTaskAdvisor.API](https://github.com/srad/eTaskAdvisor.API) for [eTaskAdvisor.App](https://github.com/srad/eTaskAdvisor.App)

This implementation is more lightweighted, statically types and faster, but uses also JWT and MongoDB.

## Setup

First you need to install .NET Core 3.x and MongoDB, then

```bash
git clone https://github.com/srad/eTaskAdvisor.WebApi.git
cd eTaskAdvisor.WebApi.git
dotnet restore
```

Next, create a copy of `appsettings.Development.json`
```bash
cp appsettings.Development.json
appsettings.json
```

Define your connection string, if it's not the default localhost and define a custom `secret`.

Now you can start:
```bash
dotnet run
```

Some basic information will be populated in the database.

## Design

A basic overview of the entities show following diagram.

![](https://raw.githubusercontent.com/srad/eTaskAdvisor.WebApi/master/Docs/schema.png)
