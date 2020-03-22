# eTaskAdvisor.WebApi
  
 [![Build Status](http://sedrad.com:8080/buildStatus/icon?job=eTaskAdvisor.WebApi)](http://sedrad.com:8080/job/eTaskAdvisor.WebApi/)

This project is part of a `Learning Technologies` university course.

The goal was to research the literature of `cognitive psychology` and `learning` about the effects of environmental factors on learning tasks and to develop a framework to capture and categorize the effects to feed the knowledge into a system and provide an API to access the knowledge for recommendations. Below you can see the data model. It also contains entities for managing clients and tasks (beside the knowledge entities).

This is a .NET Core 3.x Web-Api implementation for this purpose.
A web frontend is implemented at [eTaskAdvisor.App](https://github.com/srad/eTaskAdvisor.App)

## Setup

First you need to install .NET Core 3.x runtime or SDK and MongoDB, then

```bash
git clone https://github.com/srad/eTaskAdvisor.WebApi.git
cd eTaskAdvisor.WebApi
dotnet restore
```

Next, create a copy of `appsettings.Development.json`
```bash
cp appsettings.Development.json
appsettings.json
```

Define your connection string, if it's not the default localhost and define a custom `secret` value.

Now you can start:
```bash
dotnet run
```

Some basic information will be populated in the database.

## Data Model

![](https://raw.githubusercontent.com/srad/eTaskAdvisor.WebApi/master/Docs/schema.png)
