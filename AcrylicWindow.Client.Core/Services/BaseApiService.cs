using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Models;
using IdentityModel.Client;
using System;
using System.Net.Http;

namespace AcrylicWindow.Client.Core.Services
{
    public abstract class BaseApiService : IDisposable
    {
        private bool _disposed = false;

        private readonly ITokenStorage _tokenStorage;

        protected readonly HttpClient HttpClient;

        public BaseApiService(ITokenStorage tokenStorage, HttpClient httpClient)
        {
            _tokenStorage = Has.NotNull(tokenStorage);
            HttpClient = Has.NotNull(httpClient);

            HttpClient.SetBearerToken(_tokenStorage[Tokens.Access]);

            _tokenStorage.TokenStateChanged += OnTokentChanged;
            _tokenStorage.Cleared += OnCleared;
        }

        private void OnTokentChanged(object s, TokenEventArgs e) 
        {
            if (e.Key.Equals(Tokens.Access))
                HttpClient.SetBearerToken(e.Value);
        }

        private void OnCleared(object s, EventArgs e) =>
            HttpClient.DefaultRequestHeaders.Authorization = null;

        ~BaseApiService() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            _tokenStorage.TokenStateChanged -= OnTokentChanged;
            _tokenStorage.Cleared -= OnCleared;
            _disposed = true;
        }
    }
}
