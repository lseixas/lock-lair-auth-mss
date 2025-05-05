using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using common;

[assembly: LambdaSerializer(typeof(CustomCamelCaseLambdaJsonSerializer))] //import CustomCamelCaseLambdaJsonSerializer from common

namespace CapModuleName;
public class CapModuleNamePresenter
{
    static CapModuleNamePresenter()
    {
        AssemblyLoader.Initialize();
    }
    
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        return new APIGatewayProxyResponse()
        {
            Body = JsonSerializer.Serialize("Hello World!"),
            StatusCode = 200,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}