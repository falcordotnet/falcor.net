#r @"packages\FAKE.Core\tools\FakeLib.dll"
#load "BuildHelpers.fsx"

open Fake
open Fake.Testing
open Fake.AssemblyInfoFile
open BuildHelpers
open System

RestorePackages()

let version = "0.1.0-alpha"
// Core
let falcor = 
    new Project("Falcor", "Falcor.NET core library", 
                "Falcor.NET core libary providing models for both Falcor paths and JSON Graph. To use Falcor on the server, use the Falcor.Server.Owin package.", 
                "Falcor Data Web Reactive", sourcePath)
let falcorServer = 
    new Project("Falcor.Server", "Falcor.NET router implementation", 
                "Falcor.NET router implementation. To use Falcor on the server, use the Falcor.Server.Owin package.", 
                "Falcor Router Data Web API Reactive", sourcePath)
let falcorServerOwin = 
    new Project("Falcor.Server.Owin", "Falcor.NET server OWIN middleware", 
                "Falcor.NET server OWIN middleware for hosting a web-based Falcor datasource (router)", 
                "Falcor Owin Data API Reactive", sourcePath)
// Examples
let projects = [ falcor; falcorServer; falcorServerOwin ]
let context = createContext projects version

// Targets
Target "Nothing" DoNothing
Target "Default" DoNothing
Target "ContinuousIntegration" DoNothing
Target "Clean" (fun _ -> 
    for p in context.projects do
        CleanDirs [ p.binPath; p.packagingPath ]
    CleanDirs [ context.testResultsPath; context.tempPackagingPath ])
Target "AssemblyInfo" (fun _ -> 
    for p in context.projects do
        CreateCSharpAssemblyInfo p.assemblyInfoPath [ Attribute.Title p.name
                                                      Attribute.Copyright context.copyright
                                                      Attribute.Description p.description
                                                      Attribute.Product "Falcor.NET"
                                                      Attribute.Version context.version
                                                      Attribute.InternalsVisibleTo "Falcor.Tests"
                                                      Attribute.FileVersion context.version ])
Target "Compile" 
    (fun _ -> MSBuild null "Build" [ "Configuration", "Release" ] [ "./Falcor.sln" ] |> Log "Build-Output: ")
Target "CreatePackages" 
    (fun _ -> 
    let withCore = dependsOn (falcor)
    createNuGetPackage falcor context 
        (withCustomParams 
             (fun context p -> 
             { p with Dependencies = [ "Newtonsoft.Json", GetPackageVersion context.packagesDirPath "Newtonsoft.Json" ] }))
    createNuGetPackage falcorServer context 
        (withCustomParams (fun context p -> 
             { p with Dependencies = 
                          [ "Falcor", context.version
                            "Newtonsoft.Json", GetPackageVersion context.packagesDirPath "Newtonsoft.Json"
                            "Rx-Main", GetPackageVersion context.packagesDirPath "Rx-Main"
                            "Sprache", GetPackageVersion context.packagesDirPath "Sprache" ] }))
    createNuGetPackage falcorServerOwin context 
        (withCustomParams (fun context p -> 
             { p with Dependencies = 
                          [ "Falcor", context.version
                            "Falcor.Server", context.version
                            "Microsoft.Owin", GetPackageVersion context.packagesDirPath "Microsoft.Owin" ] })))
//createNuGetPackage falcorClient context withCore
//createNuGetPackage falcorRouter context withCore
//createNuGetPackage falcorWeb context withCore
//createNuGetPackage falcorWebOwin context withCore
//createNuGetPackage falcorWebRouter context withCore
Target "Test" 
    (fun _ -> 
    !!(sprintf "./src/Falcor.Tests/bin/Release/**/Falcor.Tests*.dll") 
    |> xUnit2 (fun p -> { p with ToolPath = "./build/packages/xunit.runner.console/tools/xunit.console.exe" }))
// Target dependencies
"Test" ==> "Default"
"Compile" ==> "Test"
"Clean" ==> "AssemblyInfo" ==> "Compile"
"Test" ==> "CreatePackages"
"CreatePackages" ==> "ContinuousIntegration"
RunTargetOrDefault "Default"
