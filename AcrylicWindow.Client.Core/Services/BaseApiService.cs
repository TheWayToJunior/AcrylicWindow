using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using IdentityModel.Client;
using System;
using System.Net.Http;

namespace AcrylicWindow.Client.Core.Services
{
    public abstract class BaseApiService : IDisposable
    {
        private readonly ITokenStorage _tokenStorage;

        protected readonly HttpClient HttpClient;

        public BaseApiService(ITokenStorage tokenStorage, HttpClient httpClient)
        {
            _tokenStorage = Has.NotNull(tokenStorage);
            HttpClient = Has.NotNull(httpClient);

            HttpClient.SetBearerToken(_tokenStorage[Tokens.Access]);

            _tokenStorage.TokenStateChanged += (s, e) => 
            {
                if (e.Key.Equals(Tokens.Access)) 
                    HttpClient.SetBearerToken(e.Value);
            };

            _tokenStorage.Cleared += (s, e) => 
                HttpClient.DefaultRequestHeaders.Authorization = null;
        }

        public void Dispose()
        {
        }
    }
}
