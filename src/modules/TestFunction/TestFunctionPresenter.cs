
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using common;
using shared.domain.entities;

[assembly: LambdaSerializer(typeof(CustomCamelCaseLambdaJsonSerializer))]

namespace TestFunction;

public class TestFunctionPresenter
{
    static TestFunctionPresenter()
    {
        AssemblyLoader.Initialize();
    }

    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        User user = new User("John Doe");
        
        TestFunctionViewmodel viewModel = new TestFunctionViewmodel(user);

        Dictionary<String, String> userInfo = viewModel.ToDict(); 

        return new APIGatewayProxyResponse()
        {
            Body = JsonSerializer.Serialize(userInfo),
            StatusCode = 200,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}