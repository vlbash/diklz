var target          = Argument("target", "Default");
var configuration   = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var packPathUtils      = Directory("./src/Astum.Core.Utils/Astum.Core.Utils.csproj");
var packPathData       = Directory("./src/Astum.Core.Data/Astum.Core.Data.csproj");
var packPathBusiness   = Directory("./src/Astum.Core.Business/Astum.Core.Business.csproj");

var buildArtifacts      = Directory("./artifacts/packages");

var isAppVeyor          = AppVeyor.IsRunningOnAppVeyor;
var isWindows           = IsRunningOnWindows();

///////////////////////////////////////////////////////////////////////////////
// Clean
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
{
    CleanDirectories(new DirectoryPath[] { buildArtifacts });
});

///////////////////////////////////////////////////////////////////////////////
// Build
///////////////////////////////////////////////////////////////////////////////
Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings 
    {
        Configuration = configuration
    };

    var projects = GetFiles("./src/**/*.csproj");

    foreach(var project in projects)
	{
	    DotNetCoreBuild(project.GetDirectory().FullPath, settings);
    }
});


///////////////////////////////////////////////////////////////////////////////
// Pack
///////////////////////////////////////////////////////////////////////////////
Task("Pack")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = configuration,
        OutputDirectory = buildArtifacts,
        ArgumentCustomization = args => args.Append("--include-symbols")
    };

    // add build suffix for CI builds
    if(isAppVeyor)
    {
        settings.VersionSuffix = "build" + AppVeyor.Environment.Build.Number.ToString().PadLeft(5,'0');
    }

    DotNetCorePack(packPathUtils, settings);
    DotNetCorePack(packPathData, settings);
    DotNetCorePack(packPathBusiness, settings);
});


Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("Pack");

RunTarget(target);