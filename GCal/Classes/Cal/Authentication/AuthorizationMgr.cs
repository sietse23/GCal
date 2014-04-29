using System;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using GCal.Classes.Algemeen;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;

namespace GCal.Classes.Cal.Authentication
{
    /// <summary>
    /// Authorization helper for Native Applications.
    /// </summary>
    public static class AuthorizationMgr
    {
        private static readonly INativeAuthorizationFlow[] NativeFlows = new INativeAuthorizationFlow[] {new LoopbackServerAuthorizationFlow()};

        /// <summary>
        /// Requests authorization on a native client by using a predefined set of authorization flows.
        /// </summary>
        /// <param name="client">The client used for authentication.</param>
        /// <param name="authState">The requested authorization state.</param>
        /// <returns>The authorization code, or null if cancelled by the user.</returns>
        /// <exception cref="NotSupportedException">Thrown if no supported flow was found.</exception>
        public static string RequestNativeAuthorization(NativeApplicationClient client, IAuthorizationState authState)
        {
            // Try each available flow until we get an authorization / error.
            foreach (INativeAuthorizationFlow flow in NativeFlows)
            {
                try
                {
                    return flow.RetrieveAuthorization(client, authState);
                }
                catch (NotSupportedException) { /* Flow unsupported on this environment */ }
            }

            throw new NotSupportedException("Found no supported native authorization flow.");
        }

        /// <summary>
        /// Requests authorization on a native client by using a predefined set of authorization flows.
        /// </summary>
        /// <param name="client">The client used for authorization.</param>
        /// <param name="scopes">The requested set of scopes.</param>
        /// <returns>The authorized state.</returns>
        /// <exception cref="AuthenticationException">Thrown if the request was cancelled by the user.</exception>
        public static IAuthorizationState RequestNativeAuthorization(NativeApplicationClient client,
                                                                     params string[] scopes)
        {
            IAuthorizationState state = new AuthorizationState(scopes);
            string authCode = RequestNativeAuthorization(client, state);

            if (string.IsNullOrEmpty(authCode))
            {
                throw new AuthenticationException("The authentication request was cancelled by the user.");
            }
            return client.ProcessUserAuthorization(authCode, state);
        }

        /// <summary>
        /// Returns a cached refresh token for this application, or null if unavailable.
        /// </summary>
        /// <param name="storageName">The file name (without extension) used for storage.</param>
        /// <param name="key">The key to decrypt the data with.</param>
        /// <returns>The authorization state containing a Refresh Token, or null if unavailable</returns>
        public static AuthorizationState GetCachedRefreshToken(string key)
        {
            if (File.Exists(Globals.TokenFile))
            {
                byte[] contents = File.ReadAllBytes(Globals.TokenFile);

                byte[] salt = Encoding.Unicode.GetBytes(key);
                byte[] decrypted = ProtectedData.Unprotect(contents, salt, DataProtectionScope.CurrentUser);
                string[] content = Encoding.Unicode.GetString(decrypted).Split(new[] { "\r\n" }, StringSplitOptions.None);

                // Create the authorization state.
                string[] scopes = content[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string refreshToken = content[1];
                return new AuthorizationState(scopes) { RefreshToken = refreshToken };

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Saves a refresh token to the specified storage name, 
        /// and encrypts it using the specified key.
        /// </summary>
        public static void SetCachedRefreshToken(string key, IAuthorizationState state)
        {
            // Create the file content.
            string scopes = state.Scope.Aggregate("", (left, append) => left + " " + append);
            string content = scopes + "\r\n" + state.RefreshToken;

            // Encrypt it.
            byte[] salt = Encoding.Unicode.GetBytes(key);
            byte[] encrypted = ProtectedData.Protect(
                Encoding.Unicode.GetBytes(content), salt, DataProtectionScope.CurrentUser);

            File.WriteAllBytes(Globals.TokenFile, encrypted);
        }
    }

    internal interface INativeAuthorizationFlow
    {
        string RetrieveAuthorization(UserAgentClient client, IAuthorizationState authorizationState);
    }
}
