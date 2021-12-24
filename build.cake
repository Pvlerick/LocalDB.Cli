#tool "nuget:?package=GitVersion.CommandLine&version=5.8.1"

var target = Argument("target", "Build");

Task("Build")
  .DoesForEach(GetFiles("src/**/*.*proj"), file =>
  {
    DotNetCoreClean(file.FullPath);
    DotNetCoreRestore(file.FullPath);
    DotNetCoreBuild(file.FullPath, new DotNetCoreBuildSettings
    {
      Configuration = "Release"
    });
  });

Task("Test")
  .IsDependentOn("Build")
  .DoesForEach(GetFiles("test/**/*.*proj"), file =>
  {
    DotNetCoreTest(file.FullPath);
  });

Task("Pack")
  .DoesForEach(GetFiles("src/**/*.*proj"), file =>
  {
    DotNetCorePack(
      file.FullPath, 
      new DotNetCorePackSettings()
      {
        Configuration = "Release",
        ArgumentCustomization = args => args.Append("/p:Version=" + GitVersion().NuGetVersion)
      });
  });

Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("Test")
  .IsDependentOn("Pack");

RunTarget(target);