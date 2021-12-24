#tool "nuget:?package=GitVersion.CommandLine&version=5.8.1"

var target = Argument("target", "Build");

Task("Build")
  .DoesForEach(GetFiles("src/**/*.*proj"), file =>
  {
    DotNetClean(file.FullPath);
    DotNetRestore(file.FullPath);
    DotNetBuild(file.FullPath, new DotNetCoreBuildSettings
    {
      Configuration = "Release"
    });
  });

Task("Test")
  .IsDependentOn("Build")
  .DoesForEach(GetFiles("test/**/*.*proj"), file =>
  {
    DotNetTest(file.FullPath);
  });

Task("Pack")
  .DoesForEach(GetFiles("src/**/*.*proj"), file =>
  {
    DotNetPack(
      file.FullPath, 
      new DotNetPackSettings()
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