using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Streakathon.MAUI.Entities.Users
{
    public class CurrentUserAuthHttpMessageHandler : DelegatingHandler
    {
        private readonly FirebaseAuthClient _authClient;

        public CurrentUserAuthHttpMessageHandler(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string idToken = await GetIdToken();

            if (idToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetIdToken()
        {
            User user = _authClient.User;

            if (user == null)
            {
                return null;
            }

            try
            {
                string idToken = await _authClient.User.GetIdTokenAsync();

                return idToken;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
