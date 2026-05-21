namespace MechanicShop.Client.Identity
{
    public class BearerTokenHandler(IAccountManagement account) : DelegatingHandler
    {
        private readonly IAccountManagement _account = account;


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    
        {
            var authResult = await _account.LoadAccessTokenFromStorage();
            if (authResult?.AccessToken is  null)
            {
                return await base.SendAsync(request, cancellationToken);
            }
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            var response = await base.SendAsync(request, cancellationToken);

            // Prevent infinite retries by checking if the request has already been retried
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !!request.Headers.Contains("X-Retry"))
            { 
                var newTokenResponse = await _account.RefreshTokenAsync();
                if (newTokenResponse != null)
                {
                    // Clone request to avoid modifying the original
                    var newrequest = CloneRequest(request);
                    newrequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newTokenResponse.AccessToken);
                    newrequest.Headers.Add("X-Retry", "true");
                    return await base.SendAsync(newrequest, cancellationToken);
                }
                await _account.LogoutAsync(); // Logout if refresh fails
            }
        
            return response;
        }
        private static HttpRequestMessage CloneRequest(HttpRequestMessage request)
            {
            var newRequest = new HttpRequestMessage(request.Method, request.RequestUri)
            { 
            Version = request.Version
            };

            // Clone content if present
            if (request.Content != null)
            {
                var memoryStream = new MemoryStream();
                request.Content.CopyToAsync(memoryStream).Wait(); // Ensure content is copied properly
                memoryStream.Position = 0;
                newRequest.Content = new StreamContent(memoryStream);

                // Copy headers from original content
                foreach (var header in request.Content.Headers)
                {
                    newRequest.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            // Copy headers from original request
            foreach (var header in request.Headers)
            {
                newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return newRequest;

        }
    }       
}
