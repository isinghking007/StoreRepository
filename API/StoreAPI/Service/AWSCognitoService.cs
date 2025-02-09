using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider.Model.Internal.MarshallTransformations;
using Amazon.Runtime;

namespace StoreAPI.Service
{
    public class AWSCognitoService
    {
        private readonly string _clientId;
        private readonly string _userPoolId;
        private readonly AmazonCognitoIdentityProviderClient _provider;
        public AWSCognitoService(IConfiguration config)
        {
            _userPoolId = Environment.GetEnvironmentVariable("UserPoolId");
            _clientId = Environment.GetEnvironmentVariable("clientId");
            _provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), Amazon.RegionEndpoint.USEast1);
        }

        public async Task<string> RegisterUser(string email,string username,string password)
        {
            var signupreq = new SignUpRequest
            {
                ClientId = _clientId,
                Username = username,
                Password = password,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType{Name="email",Value=email }
                }
            };

            var result = await _provider.SignUpAsync(signupreq);
            return result.UserSub;
        }

    }
}
