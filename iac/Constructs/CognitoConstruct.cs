using Amazon.CDK;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace iac.Constructs;

public class CognitoConstruct
{
    public CognitoConstruct(Construct scope)
    {
        var UserPool = new UserPool(scope, "TestUserPoolId", new UserPoolProps
        {
            UserPoolName = "TestUserPoolName",
            
            // Sign-in with email only
            SignInAliases = new SignInAliases
            {
                Email = true,
                Username = false
            },

            // Automatically verify email addresses
            AutoVerify = new AutoVerifiedAttrs
            {
                Email = true
            },

            // Password policy
            PasswordPolicy = new PasswordPolicy
            {
                MinLength = 8,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireDigits = true,
                RequireSymbols = false,
                TempPasswordValidity = Duration.Days(3)
            },

            // Self sign-up enabled
            SelfSignUpEnabled = true,

            // Email will be used as the verifiable attribute
            StandardAttributes = new StandardAttributes
            {
                Email = new StandardAttribute { Required = true, Mutable = false }
            }
        });

        // Create an App Client
        var UserPoolClient = new UserPoolClient(scope, "UserPoolClientId", new UserPoolClientProps
        {
            UserPool = UserPool,
            GenerateSecret = false,

            // Enable authentication flows
            AuthFlows = new AuthFlow
            {
                UserPassword = true,       // USER_PASSWORD_AUTH
                Custom = true,             // REFRESH_TOKEN_AUTH
                UserSrp = true             // SRP (Secure Remote Password) flow
            },

            // Token expiration settings
            AccessTokenValidity = Duration.Minutes(60),
            IdTokenValidity = Duration.Minutes(60),
            RefreshTokenValidity = Duration.Days(30)
        });
    }
}