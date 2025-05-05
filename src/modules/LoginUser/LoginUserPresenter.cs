using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using common;
using shared.domain.entities;

[assembly: LambdaSerializer(typeof(CustomCamelCaseLambdaJsonSerializer))] //import CustomCamelCaseLambdaJsonSerializer from common

namespace LoginUser;

public class LoginUserPresenter
{
    static LoginUserPresenter()
    {
        AssemblyLoader.Initialize();
    }
    
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        
        var user = new User("John Doe");
        
        return new APIGatewayProxyResponse()
        {
            Body = JsonSerializer.Serialize("Hello World!"),
            StatusCode = 200,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}