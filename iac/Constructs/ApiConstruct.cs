using Amazon.CDK.AWS.APIGateway;
using Constructs;

namespace iac.Constructs;

public class ApiConstruct
{
    
    private RestApi RestApi { get; set; }
    public Resource ApiGatewayResource { get; set; }

    public ApiConstruct(Construct scope)
    {
        RestApi = new RestApi(scope, "LockLairAuthMssRestApi", new RestApiProps
        {
            RestApiName = "LockLairAuthMssRestApi",
            Description = "API for LockLairAuthMss",
            DefaultCorsPreflightOptions = new CorsOptions
            {
                AllowOrigins = Cors.ALL_ORIGINS,
                AllowMethods = Cors.ALL_METHODS,
                AllowHeaders = Cors.DEFAULT_HEADERS,
            }
        });
        
        ApiGatewayResource = RestApi.Root.AddResource("lock-lair-auth-mss", new RestApiProps
        {
            DefaultCorsPreflightOptions = new CorsOptions
            {
                AllowOrigins = Cors.ALL_ORIGINS,
                AllowMethods = Cors.ALL_METHODS,
                AllowHeaders = Cors.DEFAULT_HEADERS,
            }
        });

    }
}