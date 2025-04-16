using Amazon.CDK;

namespace Iac
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            var awsRegion = Environment.GetEnvironmentVariable("AWS_REGION");
            var awsAccountId = Environment.GetEnvironmentVariable("AWS_ACCOUNT_ID");
            var stackName = Environment.GetEnvironmentVariable("STACK_NAME") ?? "LockLairStackAuthdev";
            var githubRef = Environment.GetEnvironmentVariable("GITHUB_REF_NAME") ?? "dev";

            string stage = githubRef switch
            {
                "prod" => "PROD",
                "homolog" => "HOMOLOG",
                "dev" => "DEV",
                _ => "TEST"
            };

            var tags = new Dictionary<string, string>
            {
                { "project", "Reservation Courts and Schedule MSS" },
                { "stage", stage },
                { "stack", "BACK" },
                { "owner", "DevCommunity" }
            };

            app.Synth();
        }
    }
}