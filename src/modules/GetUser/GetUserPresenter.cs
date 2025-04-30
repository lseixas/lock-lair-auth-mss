
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using common;
using shared.domain.entities;

[assembly: LambdaSerializer(typeof(CustomCamelCaseLambdaJsonSerializer))]

namespace GetUser;

public class GetUserPresenter
{
    static GetUserPresenter()
    {
        AssemblyLoader.Initialize();
    }

    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        User user = new User("John Doe");
        
        GetUserViewmodel viewModel = new GetUserViewmodel(user);

        Dictionary<String, String> userInfo = viewModel.ToDict(); 

        return new APIGatewayProxyResponse()
        {
            Body = JsonSerializer.Serialize(userInfo),
            StatusCode = 200,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}