using System.Net;
using System.Text;
using System.Text.Json;

namespace httpbase.Tests
{
	/// <summary>
	/// A mock HTTP message handler used for unit testing HTTP client behavior.
	/// It simulates responses based on the provided payload and status code.
	/// </summary>
	public class MockHttpMessageHandler : HttpMessageHandler
	{
		private readonly object _response;
		private readonly HttpStatusCode _statusCode;

		/// <summary>
		/// Initializes a new instance of the <see cref="MockHttpMessageHandler"/> class.
		/// </summary>
		/// <param name="response">The object to serialize as the HTTP response body.</param>
		/// <param name="statusCode">The HTTP status code to return. Defaults to 200 OK.</param>
		public MockHttpMessageHandler(object response, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			_response = response;
			_statusCode = statusCode;
		}

		/// <summary>
		/// Sends a mock HTTP response based on the configured payload and status code.
		/// </summary>
		/// <param name="request">The incoming HTTP request.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>A simulated <see cref="HttpResponseMessage"/>.</returns>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var json = _response != null ? JsonSerializer.Serialize(_response) : "null";
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var responseMessage = new HttpResponseMessage(_statusCode)
			{
				Content = content
			};

			return Task.FromResult(responseMessage);
		}
	}
}