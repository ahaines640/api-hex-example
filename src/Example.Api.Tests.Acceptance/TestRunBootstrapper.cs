using System.Diagnostics;
using System.Threading;
using TechTalk.SpecFlow;

namespace Example.Api.Tests.Acceptance
{
    [Binding]
    public class TestRunBootstrapper
    {
        private static Process _dotnetProcess;

        [BeforeTestRun]
        public static void StartServer()
        {
            _dotnetProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = new DotNetRunArguments
                    {
                        Project = Settings.ApiProjectPath,
                        Urls = Settings.ApiBaseUrl,
                        Configuration = "Debug"
                    }.ToString(),
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = false
                }
            };

            _dotnetProcess.Start();
            Thread.Sleep(Settings.DotNetRunWait);
        }

        [AfterTestRun]
        public static void StopServer()
        {
            _dotnetProcess.Kill();
        }
    }
}