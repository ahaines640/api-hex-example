namespace Example.Api.Tests.Acceptance
{
    public class DotNetRunArguments
    {
        public string Project { get; set; }
        public string Urls { get; set; }
        public string Configuration { get; set; }

        public override string ToString()
        {
            return $"run --project {Project} --urls {Urls} --configuration {Configuration}";
        }
    }
}