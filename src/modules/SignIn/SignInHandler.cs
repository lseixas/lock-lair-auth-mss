using System.Text.Json;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using common;

[assembly: LambdaSerializer(typeof(CustomCamelCaseLambdaJsonSerializer))] //import CustomCamelCaseLambdaJsonSerializer from common

namespace SignIn;

public class SignInHandler
{
    private static readonly AmazonCognitoIdentityProviderClient ProviderClient;
    private static readonly string ClientId;
    static SignInHandler()
    {
        AssemblyLoader.Initialize();
        ProviderClient = new AmazonCognitoIdentityProviderClient();
        ClientId = Environment.GetEnvironmentVariable("UserPoolClientId");
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var payload = JsonSerializer.Deserialize<Dictionary<string, string>>(request.Body);
            var username = payload["username"];
            var password = payload["password"];

            var authRequest = new InitiateAuthRequest
            {
                ClientId = ClientId,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    ["USERNAME"] = username,
                    ["PASSWORD"] = password
                }
            };

            var authResponse = await ProviderClient.InitiateAuthAsync(authRequest);

            if (authResponse.AuthenticationResult != null)
            {
                var result = new
                {
                    idToken = authResponse.AuthenticationResult.IdToken,
                    accessToken = authResponse.AuthenticationResult.AccessToken,
                    refreshToken = authResponse.AuthenticationResult.RefreshToken
                };

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonSerializer.Serialize(result)
                };
            }

            // handle challenge flows if needed
            return new APIGatewayProxyResponse
            {
                StatusCode = 400,
                Body = JsonSerializer.Serialize(new
                    { error = "Challenge required", challenge = authResponse.ChallengeName })
            };
        }
        catch (Exception ex)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 500,
                Body = JsonSerializer.Serialize(new { error = ex.Message })
            };
        }
    }
}