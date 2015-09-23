#r @"packages\FAKE.Core\tools\FakeLib.dll"
#load "BuildHelpers.fsx"

open Fake
open Fake.Testing
open Fake.AssemblyInfoFile
open BuildHelpers
open System

RestorePackages()

let version = "0.0.1-pre"
let falcor = new Project("Falcor", "Falcor.NET Core API", "Falcor")
let falcorServer = new Project("Falcor.Server", "Falcor.NET Server", "")
let projects = [ falcor; falcorServer ]
let context = createContext projects version

// Targets
Target "Nothing" DoNothing
Target "Default" DoNothing
Target "ContinuousIntegration" DoNothing
Target "Clean" (fun _ -> 
    for p in context.projects do
        CleanDirs [ p.binPath; p.packagingPath ]
    CleanDirs [ context.testResultsPath; context.tempPackagingPath ])
Target "AssemblyInfo" 
    (fun _ -> 
        for p in context.projects do
            CreateCSharpAssemblyInfo p.assemblyInfoPath
                [Attribute.Title p.name
                 Attribute.Copyright context.copyright
                 Attribute.Description p.description
                 Attribute.Product "Falcor.NET"
                 Attribute.Version context.version
                 Attribute.FileVersion context.version]
)
Target "Compile" 
    (fun _ -> MSBuild null "Build" [ "Configuration",  "Release"] [ "./Falcor.sln" ] |> Log "Build-Output: ")
Target "CreatePackages" (fun _ -> 
    let withCore = dependsOn (falcor)
    createNuGetPackage falcor context useDefaults
    createNuGetPackage falcorClient context withCore
    createNuGetPackage falcorServer context withCore
    createNuGetPackage falcorRouter context withCore
    createNuGetPackage falcorWeb context withCore
    createNuGetPackage falcorWebRouter context withCore)
Target "Test" 
    (fun _ -> 
    !!(sprintf "./src/Falcor.Tests/bin/Release/**/Falcor.Tests*.dll" ) 
    |> xUnit2 (fun p -> { p with ToolPath = "./build/packages/xunit.runner.console/tools/xunit.console.exe" }))
// Target dependencies
"Test" ==> "Default"
"Compile" ==> "Test"
"Clean" ==> "AssemblyInfo" ==> "Compile"
"Test" ==> "CreatePackages"
"CreatePackages" ==> "ContinuousIntegration"
RunTargetOrDefault "Default"
