<p align="center">
  <img src="https://cloud.githubusercontent.com/assets/1016365/8711049/66438ebc-2b03-11e5-8a8a-75934f7ca7ec.png">
</p>

# Falcor.NET [![Build status](https://ci.appveyor.com/api/projects/status/y7ybdqvvcrpxl1kq?svg=true)](https://ci.appveyor.com/project/CraigSmitham/falcor-net) [![NuGet package version](https://img.shields.io/nuget/v/Falcor.svg?style=flat)](https://www.nuget.org/packages/Falcor.Server.Owin)  [![Coverity](https://scan.coverity.com/projects/6781/badge.svg)](https://scan.coverity.com/projects/falcordotnet-falcor-net) 

[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/falcordotnet/falcor.net)
## What is Falcor?
Falcor is an approach pioneered by Netflix that enables you to quickly build a data-driven APIs that deliver efficient payloads to a variety of client form factors, and is able to adapt quickly to changing data access patterns as your application evolves.

Falcor provides many of the benefits of both REST and RPC, while addressing shortcomings these approaches face when building modern web applications.

## Release Status: _Developer Preview_
**Falcor.NET will remain in developer preview** until a stable client and server reference version is released from Netflix all baseline functionality is implemented. Follow progress on this GitHub issue: https://github.com/falcordotnet/falcor.net/issues/1


# Getting Started

To get started with Falcor.NET, follow these steps:

1. In your ASP.NET web project, install the _Falcor.Server.Owin_ NuGet package:

```
PM> Install-Package Falcor.Server.Owin -Pre
```
2. Create your application router to match paths to a virtual JSON Graph model:

```cs
public class HelloWorldRouter : FalcorRouter
{
    public HelloWorldRouter()
    {
        // Route to match a JSON path, in this case the 'message' member
        // of the root JSON node of the virtual JSON Graph
        Get["message"] = async _ =>
        {
            var result = await Task.FromResult(Path("message").Atom("Hello World"));
            return Complete(result);
        };
        // Define additional routes for your virtual model here
    }
}
```
**_Note_**: For a more realistic example router, see the [example Netflix router](https://github.com/falcordotnet/falcor.net/blob/master/examples/Falcor.Examples.Netflix/NetflixRouter.cs) which  is part of Falcor.Examples.Netflix.Web project that you can run yourself to see the router in action.

3. In your OWIN startup class, configure your Falcor.NET router endpoint:

```cs
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        app.UseFalcor("/helloWorldModel.json", routerFactory: config => new HelloWorldRouter());
        ...
    }
}

```
If you are using IIS (and for most development environments), ensure the web.config has a similar handler configured for your model endpoint:

```xml
<system.webServer>
  <handlers>
    <add name="NetflixFalcorModelHandler" path="model.json" verb="*" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />
  </handlers>
</system.webServer>
```
4. Using [falcor.js](https://netflix.github.io/falcor/build/falcor.browser.js) in your client, create a new Falcor model, specifying the URL of your new endpoint for your datasource:

```js
var model = new falcor.Model({
  source: new falcor.HttpDataSource('/helloWorldModel.json')
});
```
5. You've done all you need to talk to your router, call your model like so:

```js
model.get('message').then(function(json) {
  console.log(json);
});
// Result:
{
  json: {
    message: "Hello World!"
  }
}
````

<h1 id="uses">When To Use Falcor</h1>
**_Consider the Falcor approach when you are developing a client/server architecture that is intended to provide a rich client user experience._**

| *Good Fit*                                                                                     | *Poor Fit*                                                                        |
|------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------|
| Fetching _large amounts_ of _small resources_ (e.g. web components, JSON, metadata, etc.)      | Fetching _small amounts_ of _large resources_ (e.g. web pages, images, files, etc.) |
| Aggregating heterogeneous services efficiently (e.g. microservices or loosley-coupled modules) | Speaking to a single back-end data source or service provider                   |
| Delivering responsive and capable end-user client experiences                                  | Systems integration and public APIs; static websites                            |
| Heterogeneous and evolving data access patterns (multiple devices, changing use cases)         |                                                                                 |


# License
Apache 2.0
