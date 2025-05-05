using Constructs;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using iac.Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;

namespace iac;

public class Stack
{
    public class StackClass : Amazon.CDK.Stack
    {
        
        internal StackClass(Construct scope, string id, StackProps? props = null) : base(scope, id, props)
        {

            var sharedLayer = new LayerVersion(this, "LockLairAuthMssLayer", new LayerVersionProps
            {
                CompatibleRuntimes = new[] { Runtime.DOTNET_8 },
                Code = Code.FromAsset("./layer-package"),
                Description = "Lambda Layer for lock-lair-auth-mss project",
                RemovalPolicy = RemovalPolicy.DESTROY,
            });
            
            var apiConstruct = new ApiConstruct(this);
            var lambdaConstruct = new LambdaConstruct(this, sharedLayer, apiConstruct.ApiGatewayResource);
            var cognitoConstruct = new CognitoConstruct(this);
        }
    }
}