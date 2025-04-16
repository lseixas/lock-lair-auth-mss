namespace iac;

public class IacStack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CDK project initialization successful!");
            Console.WriteLine($"Current directory: {Environment.CurrentDirectory}");
            Console.WriteLine($".NET version: {Environment.Version}");
        }
    }
}