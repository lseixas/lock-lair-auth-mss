﻿using System.Text.Json;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using common;

[assembly: LambdaSerializer(typeof(CustomCamelCaseLambdaJsonSerializer))] //import CustomCamelCaseLambdaJsonSerializer from common

namespace SignUp;

public class SignUpHandler
{
    private static readonly AmazonCognitoIdentityProviderClient ProviderClient;
    private static readonly string ClientId;
    static SignUpHandler()
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
            var email = payload["email"];

            var signUpRequest = new SignUpRequest
            {
                ClientId = ClientId,
                Username = username,
                Password = password,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType() { Name = "email", Value = email }
                }
            };
            
            var signUpResponse = await ProviderClient.SignUpAsync(signUpRequest);

            if (signUpResponse.UserConfirmed == true)
            {
                return new APIGatewayProxyResponse()
                {
                    StatusCode = 201,
                    Body = JsonSerializer.Serialize(new { message = "User created and confirmed." }),
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" }
                    }
                };
            }
            else
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonSerializer.Serialize(new {
                        message        = "User created; confirmation required.",
                        userSub        = signUpResponse.UserSub,
                        codeDelivery   = signUpResponse.CodeDeliveryDetails
                    })
                };
            }
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