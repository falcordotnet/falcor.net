#r @"temp\tools\FAKE.Core\tools\FakeLib.dll"
#r @"temp\tools\FSharp.Data\lib\net40\FSharp.Data.dll"

open Fake
open Fake.Testing
open Fake.AppVeyor
open Fake.SemVerHelper
open Fake.AssemblyInfoFile
open FSharp.Data
open FSharp.Data.JsonExtensions
open System
open System.IO

exception InvalidSemVer of string

exception EnvironmentVariableAlreadyExists of string

type Project(name : string, description : string, tags : string) = 
    let packagingRootPath = "temp" @@ "packaging"
    member this.summary = ""
    member this.authors = [ "falcor.net" ]
    member this.name = name
    member this.description = name
    member this.tags = name
    member this.assemblyInfoPath = "src" @@ name @@ "Properties" @@ "AssemblyInfo.cs"
    member this.binPath = "src" @@ name @@ "bin"
    member this.dllPath = this.binPath @@ "Release" @@ name + ".dll"
    member this.packagingPath = packagingRootPath @@ name
    member this.net45packagePath = this.packagingPath @@ "lib" @@ "net45"

type BuildContext = 
    { copyright: string
      packagesDirPath : string
      projects : List<Project>
      publishUrl : string
      publishApiKey : string
      tempPackagingPath : string
      testResultsPath : string
      version : string }

let falcorNetAuthor = [ "falcor.net" ]
let falcorClient = new Project("Falcor.Client", "Falcor .NET Client", "")
let falcorRouter = new Project("Falcor.Router", "Falcor .NET Router", "")
let falcorWebRouter = new Project("Falcor.Web.Router", "Falcor .NET Web Router", "")
let falcorWeb = new Project("Falcor.Web", "Falcor .NET Web", "")

let createContext (baseProjects : List<Project>) (version : string) = 
    let projects = baseProjects @ [ falcorClient; falcorRouter; falcorWebRouter; falcorWeb ]
    // Initialize local environment variables 
    if File.Exists "local.json" then 
        let localVarProps = JsonValue.Parse(File.ReadAllText"local.json").Properties
        for key, jsonValue in localVarProps do
            let value = jsonValue.AsString()
            if (environVarOrNone(key).IsSome) then raise (EnvironmentVariableAlreadyExists(key))
            setEnvironVar key value
            printfn "loaded local config %A" key
    let buildNumber = 
        match environVarOrNone "APPVEYOR_BUILD_NUMBER" with
        | Some(appVeyorBuildNumber) -> Some(appVeyorBuildNumber)
        | None -> environVarOrNone "buildNumber"
    
    let buildVersion = 
        match buildNumber with
        | Some(build) -> version + "-" + build
        | None -> version
    
    if (isValidSemVer buildVersion = false) then raise (InvalidSemVer(version))
    { copyright = sprintf "%A Falcor.NET" DateTime.UtcNow.Year
      packagesDirPath = "packages"
      projects = projects
      publishUrl = environVarOrDefault "publishUrl" ""
      publishApiKey = environVarOrDefault "publishApiKiey" ""
      tempPackagingPath = "temp" @@ "packaging"
      testResultsPath = "tests"
      version = buildVersion }

let createNuGetPackage (project : Project) (context : BuildContext) 
    (customParams : (BuildContext -> NuGetParams -> NuGetParams) option) = 
    CopyFiles project.net45packagePath [ project.dllPath; "LICENSE" ]
    NuGet ((fun p -> 
        { p with Project = project.name
                 Authors = project.authors
                 Description = project.description
                 OutputPath = context.packagesDirPath
                 Summary = project.summary
                 WorkingDir = project.packagingPath
                 Version = context.version
                 Tags = project.tags
                 Copyright = context.copyright
                 PublishUrl = context.publishUrl
                 AccessKey = context.publishApiKey
                 Publish = context.publishUrl <> "" }
        |> match customParams with
           | Some(customParams) -> customParams (context)
           | None -> (fun p -> p))) "./base.nuspec"

let useDefaults = None
let withCustomParams (configuration : BuildContext -> NuGetParams -> NuGetParams) = Some(configuration)
let withPackage (package : string) = 
    withCustomParams 
        (fun context p -> { p with Dependencies = [ package, GetPackageVersion context.packagesDirPath package ] })
let dependsOn (project : Project) = 
    withCustomParams (fun context p -> { p with Dependencies = [ project.name, context.version ] })
