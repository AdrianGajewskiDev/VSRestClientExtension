using VSRESTClient.UI.Utils;

namespace VSRESTClient.UI.Models
{
    public class AuthorizationModel
    {
        public AuthorizationType AuthorizationType { get; set; }
        public AuthorizationAttachMethod AuthorizationAttachMethod { get; set; }

        public string BasicAuthorizationHeaderOrParamName { get; set;}
        public string BasicAuthorizationHeaderOrParamValue { get; set; }
        public string ApiKeyAuthorizationHeaderOrParamName { get; set; }
        public string ApiKeyAuthorizationHeaderOrParamValue { get; set; }

        public string Token { get; set; }

        public AuthorizationModel()
        {
            AuthorizationType = AuthorizationType.NoAuth;
            AuthorizationAttachMethod = AuthorizationAttachMethod.Headers;
        }

        public void SetCurrentAuthorizationType(AuthorizationType type)
        {
            AuthorizationType = type;
        }

        public void SetAuthorizationAttachMethod(AuthorizationAttachMethod type)
        {
            AuthorizationAttachMethod = type;
        }

    }
}
