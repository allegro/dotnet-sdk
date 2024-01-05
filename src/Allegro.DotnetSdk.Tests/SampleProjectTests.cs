using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Allegro.DotnetSdk.Tests;

public class SampleProjectTests
{
    [Theory, MemberData(nameof(ProjectNames))]
    public async Task ValidateProject(string projectName)
    {
        var projectPath = Path.Combine(ProjectsDirectory, projectName);
        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = projectPath,
            FileName = OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet",
            ArgumentList = {
                "build",
                "--nologo",
                "--no-incremental"
            },
        };
        // MSBuild variables like MSBuildExtensionsPath, MSBuildSDKsPath cause issues
        // with correct SDK resolution based on global.json in sample projects.
        // Also remove any locally set AllegroDotnetSdk* variables
        static bool IsFilteredEnvVar(string value) =>
            value.StartsWith("MSBuild", StringComparison.OrdinalIgnoreCase) ||
            value.StartsWith("AllegroDotnetSdk", StringComparison.OrdinalIgnoreCase);

        foreach (var key in startInfo.Environment.Keys.Where(IsFilteredEnvVar).ToArray())
        {
            startInfo.Environment.Remove(key);
        }
        // simulate CI builds, that changes some switches in Sdk
        startInfo.Environment["CI"] = "true";
        var process = Process.Start(startInfo);
        await process!.WaitForExitAsync();

        var msgPath = Path.Combine(projectPath, "message.txt");
        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        if (Path.Exists(msgPath))
        {
            var msg = File.ReadAllText(msgPath);
            Assert.Contains(msg, stderr + stdout);
        }

        if (projectName.EndsWith("Error"))
        {
            Assert.NotEqual(0, process.ExitCode);
        }
        else
        {
            Assert.Empty(stderr);
            Assert.Equal(0, process.ExitCode);
        }
    }

    public static IEnumerable<object[]> ProjectNames
    {
        get
        {
            var dirs = Directory.EnumerateDirectories(ProjectsDirectory);
            return dirs.Select(x => new object[] { Path.GetFileName(x)! });
        }
    }

    private static Lazy<string> ThisFileDirectoryLazy { get; } =
    new(() =>
    {
        return Directory.GetParent(GetCallerFilePath())!.FullName;

        static string GetCallerFilePath([CallerFilePath] string? path = null) => path ?? "";
    });

    private static string ThisFileDirectory => ThisFileDirectoryLazy.Value;
    private static string ProjectsDirectory { get; } = Path.Combine(ThisFileDirectory, "projects");
}