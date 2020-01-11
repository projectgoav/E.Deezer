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
    NUnit3("./build/E.Deezer.Tests/bin/" + configuration + "/net462/*.Tests.dll",
           new NUnit3Settings
            {
                Labels=NUnit3Labels.All,
                NoResults=true,
            });

    var testSettings = new DotNetCoreTestSettings()
    {
        Configuration = configuration,
        NoBuild = true,
        NoRestore = true,
    };

    /* TODO, NUnit doesn't support running NET STANDARD or NET CORE
     * tests so we have to resort to using the 'dotnet test'
     *
     * Need to find a way to get the details back out from the test
     * runner OR convert the tests to a net462 lib */
    DotNetCoreTest("./build/Integration/bin/" + configuration + "/netcoreapp2.1/*.Tests.*.dll",
                   testSettings);

    DotNetCoreTest("./build/Regression/bin/" + configuration + "/netcoreapp2.1/*Tests.*.dll",
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