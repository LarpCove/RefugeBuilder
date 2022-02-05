using System.IO;
using System.Threading.Tasks;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace RefugeBuilder.Build
{
    /// <summary>Manages build behavior for the solution.</summary>
    public static class Program
    {
        /// <summary>Base directory for all output.</summary>
        private static readonly string _ArtifactDir = Path.Combine(Directory.GetCurrentDirectory(), "artifacts");

        /// <summary>List of projects to include in building.</summary>
        private static readonly string[] _Projects = new[] { "Rules", "PlayerBuilder", "Terminal" };

        /// <summary>Console application entry point.</summary>
        public static async Task Main(string[] args)
        {
            Target("default", DependsOn("coverage"));
            Target("restore", Restore);
            Target("compile", DependsOn("restore"), Compile);
            Target("runConsole", DependsOn("compile"), RunConsole);
            Target("test", DependsOn("compile"), Test);
            Target("coverage", DependsOn("compile"), Coverage);
            await RunTargetsAndExitAsync(args);
        }

        /// <summary>Downloads all packages for the solution.</summary>
        private static Task Restore()
        {
            return RunAsync("dotnet", "restore");
        }

        /// <summary>Builds the solution.</summary>
        private static async Task Compile()
        {
            foreach (string project in _Projects)
            {
                await RunAsync($"dotnet", $"build src/{project} --no-restore --configuration Debug");
                await RunAsync($"dotnet", $"build src/{project} --no-restore --configuration Release");
                await RunAsync($"dotnet", $"build tests/{project}Tests --no-restore --configuration Debug");
                await RunAsync($"dotnet", $"build tests/{project}Tests --no-restore --configuration Release");
            }
        }

        /// <summary>Runs the console version.</summary>
        private static async Task RunConsole()
        {
            await RunAsync($"dotnet", $"run src/Terminal --no-restore --no-build --configuration Release");
        }

        /// <summary>Tests the solution.</summary>
        private static async Task Test()
        {
            string testArgs = "test --no-restore --no-build ";

            await RunAsync("dotnet", testArgs + "--configuration Debug");
            await RunAsync("dotnet", testArgs + "--configuration Release");
        }

        /// <summary>Tests and analyzes test code coverage.</summary>
        private static async Task Coverage()
        {
            string prefix = "coverage";
            string postfix = ".cobertura.xml";

            string coverageDir = Path.Combine(_ArtifactDir, "coverage");
            string testDir = Path.Combine(coverageDir, "testResults");
            string reportDir = Path.Combine(coverageDir, "report");

            EnsureEmpty(coverageDir);

            await RunAsync("dotnet", string.Join(' ',
                "test",
                "--no-build",
                "--no-restore",
                "--configuration Debug",
                "--collect:\"XPlat Code Coverage\"",
                $"--results-directory \"{testDir}\""));

            int count = 0;
            foreach (string result in Directory.GetFiles(testDir, $"{prefix}{postfix}", SearchOption.AllDirectories))
            {
                File.Copy(result, Path.Combine(coverageDir, $"{prefix}{count++}{postfix}"));
            }

            await RunAsync("dotnet", "tool update -g dotnet-reportgenerator-globaltool");
            await RunAsync("reportgenerator", $"-reports:{coverageDir}/*.xml -targetdir:{reportDir}");
        }

        /// <summary>Deletes and creates a directory.</summary>
        /// <param name="dir">Directory to empty.</param>
        private static void EnsureEmpty(string dir)
        {
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
            _ = Directory.CreateDirectory(dir);
        }
    }
}
