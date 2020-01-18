#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./build/");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./E.Deezer.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    // Use MSBuild
    MSBuild("./E.Deezer.sln", settings =>
    {
       settings.Restore = true;

       settings.SetRestoreLockedMode(true);
       settings.SetConfiguration(configuration);
    });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var testSettings = new NUnit3Settings
        {
            Labels=NUnit3Labels.All,
            NoResults=true,
        };

    NUnit3("./build/E.Deezer.Tests/bin/" + configuration + "/net462/*.Tests.dll",
           testSettings);

    NUnit3("./build/Integration/bin/" + configuration + "/net462/*.Tests.*.dll",
           testSettings);

    NUnit3("./build/Regression/bin/" + configuration + "/net462/*Tests.*.dll",
           testSettings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);