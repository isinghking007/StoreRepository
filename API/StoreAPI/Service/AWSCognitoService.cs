using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider.Model.Internal.MarshallTransformations;
using Amazon.Runtime;

namespace StoreAPI.Service
{
    public class AWSCognitoService
    {
        private readonly string _clientId_user_pool;
        private readonly string _clientId;
        private readonly string _userPoolId;
        private readonly AmazonCognitoIdentityProviderClient _provider;
        private readonly ILogger _log;
        public AWSCognitoService(IConfiguration config,ILogger<AWSCognitoService> log)
        {
            _userPoolId = Environment.GetEnvironmentVariable("UserPoolId");
            _clientId = Environment.GetEnvironmentVariable("clientId");
            _clientId_user_pool = Environment.GetEnvironmentVariable("ClientID_new_userpool");
            _provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), Amazon.RegionEndpoint.APSouth1);
            _log = log;

        }

        
        public async Task<string> ListUserPoolDetails()
        {
            _log.LogInformation("Inside List User Pool Details Method in AWSCognitoService");
            try
            {
                var listUserPoolsRequest = new ListUserPoolsRequest
                {
                    MaxResults = 10 // Adjust as needed
                };

                var userPoolsResponse = await _provider.ListUserPoolsAsync(listUserPoolsRequest);

                foreach (var userPool in userPoolsResponse.UserPools)
                {
                    _log.LogInformation($"User Pool ID: {userPool.Id}, Name: {userPool.Name}");

                    // Retrieve the App Clients for this User Pool
                    var listUserPoolClientsRequest = new ListUserPoolClientsRequest
                    {
                        UserPoolId = userPool.Id,
                        MaxResults = 10
                    };

                    var clientsResponse = await _provider.ListUserPoolClientsAsync(listUserPoolClientsRequest);

                    foreach (var client in clientsResponse.UserPoolClients)
                    {
                        _log.LogInformation($" - Client Name: {client.ClientName}, Client ID: {client.ClientId}");
                    }
                }
                return $"Details logged in Log file";
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return ex.Message ;
            }
        }

        public async Task<string> RegisterUser(string email, string phone, string username,string password)
        {
            string phonenumber = FormatPhoneNumber(phone);
            var signupreq = new SignUpRequest
            {
                ClientId = _clientId_user_pool,
                Username= phonenumber,
                Password = password,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "phone_number", Value = phonenumber },  // Must be in E.164 format (+1...)
                    new AttributeType { Name = "email", Value = email },
                   
                }
            };
            try
            {
                var result = await _provider.SignUpAsync(signupreq);
                if(result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    /* await _provider.AdminConfirmSignUpAsync(new AdminConfirmSignUpRequest
                     {
                         Username = username,
                         UserPoolId = _userPoolId
                     });*/
                    return result.UserSub;
                }
                else
                {
                    return "Unable to confirm user in AWS Congito, Please confirm manually";
                }
            }
            catch(Exception e)
            {
                return e.Message;
            }

        }

        public async Task<string> LoginUser(string username, string password)
        {
            // var srpHelper=new CognitoSRPHelper
            username = FormatPhoneNumber(username);
            var authrequest = new InitiateAuthRequest
            {
                ClientId = _clientId_user_pool,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    {"USERNAME",username },
                    {"PASSWORD",password }
                }
            };
            try
            {
                var request = await _provider.InitiateAuthAsync(authrequest);
                if(request.AuthenticationResult!=null)
                {
                    return request.AuthenticationResult.IdToken;
                }
                else
                {
                    throw new Exception("Authentication failed, no token received.");
                }
            }
            catch (NotAuthorizedException)
            {
                throw new Exception("Invalid username or password.");
            }
            catch (UserNotFoundException)
            {
                throw new Exception("User does not exist.");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        #region RESET USER JIRA STOREREPO-14 START
        public async Task<string> ResetUser(string username)
        {
            _log.LogInformation($"Inside Reset User Method in AWSCognitoService for {username}");
            username =FormatPhoneNumber(username);
            var request = new ForgotPasswordRequest
            {
                ClientId = _clientId_user_pool,
                Username = username
            };
            try
            {
                var response = await _provider.ForgotPasswordAsync(request);
                _log.LogInformation($"PassCode sent to user's email: {response.CodeDeliveryDetails.Destination}");
                return response.CodeDeliveryDetails.Destination;
            }
            catch (Exception ex)
            {
                _log.LogWarning($"Error in Reset User Method in AWSCognitoService for {username} : {ex.Message}");
                return ex.Message;
            }
        }
        public async Task<string> ConfirmForgotPassword(string username,string passcode,string newPassword)
        {
            _log.LogInformation($"Inside Confirm Forgot Password Method in AWSCognitoService for {username}");
           
            try
            {
                username = FormatPhoneNumber(username);
                var request = new ConfirmForgotPasswordRequest
                {
                    ClientId = _clientId_user_pool,
                    Username = username,
                    ConfirmationCode = passcode,
                    Password = newPassword
                };
                try
                {
                    var response = await _provider.ConfirmForgotPasswordAsync(request);
                    _log.LogInformation($"Password had been sucessfully reset for user: {username}");
                    return response.HttpStatusCode.ToString();
                }
                catch (Exception ex)
                {
                    _log.LogWarning($"Error in Confirm Forgot Password Method in AWSCognitoService for {username} : {ex.Message}");
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                _log.LogWarning($"Error in Confirm Forgot Password Method in AWSCognitoService for {username} : {ex.Message}");
                return ex.Message;
            }
        }

        #endregion RESET USER JIRA STOREREPO-14 END

        #region Helper Methods

        static string FormatPhoneNumber(string phone)
        {
            string phonenumber = new string(phone.Where(char.IsDigit).ToArray());
            if(phonenumber.Length ==10)
            {
                return $"+91{phonenumber}";
            }
            else if(phone.StartsWith("91") && phonenumber.Length ==12)
            {
                return $"+{phonenumber}";
            }
            return "Invalid Phone Number";
        }

        #endregion Helper Methods

    }
}
