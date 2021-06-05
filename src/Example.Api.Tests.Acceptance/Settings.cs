using System.IO;

namespace Example.Api.Tests.Acceptance
{
    public static class Settings
    {
        public static string ApiProjectPath => Path.GetFullPath(@"..\Example.Api\Example.Api.csproj");

        public static string DbConnection => 
            $"Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;AttachDbFilename={Path.GetFullPath("..\\Example.Data\\DataSource\\ExampleDatabase.mdf")}";

        public static string ApiBaseUrl => "https://localhost:5001";
        public static int DotNetRunWait => 2000;
    }
}
