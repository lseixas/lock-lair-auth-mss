using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;
using Resource = Amazon.CDK.AWS.APIGateway.Resource;

namespace iac.Constructs;

public class LambdaConstruct
{
    private readonly Resource ApiGatewayResource;
    
    public Method LoginUserLambdaFunctionIntegration { get; }
    public Method TestFunctionLambdaFunctionIntegration { get; }
    
    private BundlingOptions NewDefaultModuleBundlingOptions(string moduleName)
    {
        return new BundlingOptions()
        {
            Image = Runtime.DOTNET_8.BundlingImage,
            User = "root",
            OutputType = BundlingOutput.ARCHIVED,
            Command =
            [
                "/bin/sh",
                "-c",
                $"cd src/modules/{moduleName} && " +
                "dotnet tool install -g Amazon.Lambda.Tools" +
                " && dotnet build" +
                " && dotnet lambda package --output-package /asset-output/function.zip"
            ]
        };
    }

    private Function NewDefaultLambdaFunction(Construct scope, string moduleName, string suffix, ILayerVersion[] sharedLayers, Dictionary<string, string> environmentVariables)
    {
        return new Function(scope, $"LockLairAuthLF{moduleName}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = $"{moduleName}Assembly::{moduleName}.{moduleName}{suffix}::FunctionHandler",
            Code = Code.FromAsset(".", new AssetOptions()
            {
                Bundling = NewDefaultModuleBundlingOptions(moduleName)
            }),
            Layers = sharedLayers,
            Environment = environmentVariables,
        });
    }

    private Method LambdaFunctionApiGatewayIntegration(Resource apiGatewayResource, string pathPart, string httpMethod ,Function lambdaFunction)
    {
        return apiGatewayResource.AddResource(pathPart).AddMethod(httpMethod, integration: new LambdaIntegration(lambdaFunction));
    }

    public LambdaConstruct(Construct scope, ILayerVersion sharedLayer, Resource apiGatewayResource, Dictionary<string, string> environmentVariables)
    {
        
        ApiGatewayResource = apiGatewayResource;

        LoginUserLambdaFunctionIntegration = 
            LambdaFunctionApiGatewayIntegration(
                apiGatewayResource: ApiGatewayResource, 
                pathPart: "SignUp", 
                httpMethod: "POST",
                lambdaFunction: NewDefaultLambdaFunction(
                    scope, 
                    moduleName: "SignUp",
                    suffix: "Handler",
                    sharedLayers: [ sharedLayer ],
                    environmentVariables: environmentVariables
                    )
                );
        LoginUserLambdaFunctionIntegration.ApplyRemovalPolicy(RemovalPolicy.DESTROY);
        
        TestFunctionLambdaFunctionIntegration = 
            LambdaFunctionApiGatewayIntegration(
                apiGatewayResource: ApiGatewayResource, 
                pathPart: "test-function", 
                httpMethod: "POST",
                lambdaFunction: NewDefaultLambdaFunction(
                    scope, 
                    moduleName: "TestFunction",
                    suffix: "Presenter",
                    sharedLayers: [ sharedLayer ],
                    environmentVariables: environmentVariables
                    )
                );
        TestFunctionLambdaFunctionIntegration.ApplyRemovalPolicy(RemovalPolicy.DESTROY);
        
    }
}