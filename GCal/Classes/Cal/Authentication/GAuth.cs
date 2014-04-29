using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Util;
using System;
using GCal.Classes.Algemeen;

namespace GCal.Classes.Cal.Authentication
{
    public class GAuth
    {
        public static IAuthenticator CreateAuthenticator()
        {
            try
            {
                NativeApplicationClient myClient = new NativeApplicationClient(GoogleAuthenticationServer.Description);
                myClient.ClientIdentifier = Globals.CLIENT_ID;
                myClient.ClientSecret = Globals.CLIENT_SECRET;

                return new OAuth2Authenticator<NativeApplicationClient>(myClient, GetAuthorization);
            }
            catch (Exception ex)
            {
                
                //ErrorDump.AddError(System.IntPtr.Zero, "GAuth.cs", "CreateAuthenticator", ex);
                return null;
            }
        }

        public static IAuthorizationState GetAuthorization(NativeApplicationClient Client)
        {
            try
            {
                const string KEY = "z},drdzf11x9;87";
                string myScope = CalendarService.Scopes.Calendar.GetStringValue();

                // Check if there is a cached refresh token available.
                IAuthorizationState myState = AuthorizationMgr.GetCachedRefreshToken(KEY);
                if (myState != null)
                {
                    try
                    {
                        Client.RefreshToken(myState);
                        return myState; // Yes - we are done.
                    }
                    catch (DotNetOpenAuth.Messaging.ProtocolException ex)
                    {
                        //ErrorDump.AddError(System.IntPtr.Zero, "GAuth.cs", "GetAuthorization", ex, "Using existing refresh token failed");
                    }
                }

                // If we get here, there is no stored token. Retrieve the authorization from the user.
                myState = AuthorizationMgr.RequestNativeAuthorization(Client, myScope);
                AuthorizationMgr.SetCachedRefreshToken(KEY, myState);
                return myState;
            }
            catch (Exception ex)
            {
                //ErrorDump.AddError(System.IntPtr.Zero, "GAuth.cs", "GetAuthorization", ex);
                return null;
            }            
        }
    }
}
