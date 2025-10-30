using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.Logging;

namespace httpbase
{
	public class GenericCrudClient : ICrudClient
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<GenericCrudClient> _logger;

		public string ClientName { get; set; }

		/// <summary>Initializes a new instance of the GenericCrudClient.</summary>
		public GenericCrudClient(IHttpClientFactory httpClientFactory, ILogger<GenericCrudClient> logger, string clientName)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
			ClientName = clientName;
		}

		private HttpClient CreateClient() => _httpClientFactory.CreateClient(ClientName);

		public async Task<T> GetAsync<T>(string endpoint)
		{
			var client = CreateClient();
			var response = await client.GetAsync(endpoint);
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(json) || json.Equals("\"null\""))
				return default!;
			return JsonSerializer.Deserialize<T>(json);
		}

		public async Task<T> PostAsync<T>(string endpoint, T payload)
		{
			var client = CreateClient();
			var response = await client.PostAsJsonAsync(endpoint, payload);
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(json))
				return default!;
			return JsonSerializer.Deserialize<T>(json);
		}

		public async Task<TResult> PostAsync<TPayload, TResult>(string endpoint, TPayload payload)
		{
			var client = CreateClient();
			var response = await client.PostAsJsonAsync(endpoint, payload);
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(json))
				return default!;
			return JsonSerializer.Deserialize<TResult>(json);
		}

		public async Task<T> PutAsync<T>(string endpoint, T payload)
		{
			var client = CreateClient();
			var response = await client.PutAsJsonAsync(endpoint, payload);
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(json))
				return default!;
			return JsonSerializer.Deserialize<T>(json);
		}

		public async Task<TResult> PutAsync<TPayload, TResult>(string endpoint, TPayload payload)
		{
			var client = CreateClient();
			var response = await client.PutAsJsonAsync(endpoint, payload);
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(json))
				return default!;
			return JsonSerializer.Deserialize<TResult>(json);
		}

		public async Task<T> PatchAsync<T>(string endpoint, T payload)
		{
			var client = CreateClient();
			var request = new HttpRequestMessage(HttpMethod.Patch, endpoint)
			{
				Content = JsonContent.Create(payload)
			};
			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(json))
				return default!;
			return JsonSerializer.Deserialize<T>(json);
		}

		public async Task<TResult> PatchAsync<TPayload, TResult>(string endpoint, TPayload payload)
		{
			var client = CreateClient();
			var request = new HttpRequestMessage(HttpMethod.Patch, endpoint)
			{
				Content = JsonContent.Create(payload)
			};
			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(json))
				return default!;
			return JsonSerializer.Deserialize<TResult>(json);
		}

		public async Task<bool> DeleteAsync(string endpoint)
		{
			var client = CreateClient();
			var response = await client.DeleteAsync(endpoint);
			_logger.LogInformation("DELETE response status: {StatusCode}", response.StatusCode);

			return response.IsSuccessStatusCode;
		}
	}
}