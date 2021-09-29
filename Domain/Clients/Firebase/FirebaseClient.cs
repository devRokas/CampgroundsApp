using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain.Clients.Firebase.Models;
using Domain.Clients.Firebase.Options;
using Microsoft.Extensions.Options;

namespace Domain.Clients.Firebase
{
    public class FirebaseClient : IFirebaseClient
    {
        private readonly HttpClient _httpClient;
        private readonly FirebaseOptions _firebaseOptions;

        public FirebaseClient(HttpClient httpClient, IOptions<FirebaseOptions> firebaseOptions)
        {
            _httpClient = httpClient;
            _firebaseOptions = firebaseOptions.Value;
        }

        public async Task<SignUpResponse> SignUpAsync(string email, string password)
        {
            var url = $"{_firebaseOptions.BaseAddress}/v1/accounts:signUp?key={_firebaseOptions.ApiKey}";

            var request = new SignUpRequest
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync(url, request);

            return await response.Content.ReadFromJsonAsync<SignUpResponse>();
        }

        public async Task<SignInResponse> SignInAsync(string email, string password)
        {
            var url = $"{_firebaseOptions.BaseAddress}/v1/accounts:signInWithPassword?key={_firebaseOptions.ApiKey}";

            var request = new SignInRequest
            {
                Email = email,
                Password = password,
                
            };

            var response = await _httpClient.PostAsJsonAsync(url, request);

            return await response.Content.ReadFromJsonAsync<SignInResponse>();
        }
    }
}