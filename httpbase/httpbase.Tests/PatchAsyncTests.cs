using System.Net;

using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace httpbase.Tests
{
	[TestClass]
	public class PatchAsyncTests
	{
		private GenericCrudClient _client;
		private Mock<IHttpClientFactory> _factoryMock;
		private Mock<ILogger<GenericCrudClient>> _loggerMock;

		[TestInitialize]
		public void Setup()
		{
			_factoryMock = new Mock<IHttpClientFactory>();
			_loggerMock = new Mock<ILogger<GenericCrudClient>>();
		}

		private void ConfigureClient(object response, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			var handler = new MockHttpMessageHandler(response, statusCode);
			var httpClient = new HttpClient(handler);
			_factoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
			_client = new GenericCrudClient(_factoryMock.Object, _loggerMock.Object, "TestClient");
		}

		[TestMethod]
		public async Task PatchAsync_ShouldReturnUpdatedModel()
		{
			var patched = new FooBar { Id = 1, Name = "Patched" };
			ConfigureClient(patched);
			var result = await _client.PatchAsync("http://test.com", patched);
			result.ShouldNotBeNull();
			result.Name.ShouldBe("Patched");
		}

		[TestMethod]
		public async Task PatchAsync_ShouldHandleTimeout()
		{
			var httpClient = new HttpClient(new HttpClientHandler()) { Timeout = TimeSpan.FromMilliseconds(1) };
			_factoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
			_client = new GenericCrudClient(_factoryMock.Object, _loggerMock.Object, "TestClient");
			await Should.ThrowAsync<TaskCanceledException>(() => _client.PatchAsync("http://test.com", new FooBar()));
		}

		[TestMethod]
		public async Task PatchAsync_ShouldHandleNullResponse()
		{
			ConfigureClient(null);
			var result = await _client.PatchAsync("http://test.com", new FooBar());
			result.ShouldBeNull();
		}

		[TestMethod]
		public async Task PatchAsync_ShouldHandleIncorrectUrl()
		{
			ConfigureClient(null, HttpStatusCode.NotFound);
			await Should.ThrowAsync<HttpRequestException>(() => _client.PatchAsync("http://badurl.com", new FooBar()));
		}

		[TestMethod]
		public async Task PatchGenericAsync_ShouldReturnResult()
		{
			var payload = new FooBar { Id = 2, Name = "PatchPayload" };
			var resultModel = new FooBar { Id = 3, Name = "PatchResult" };
			ConfigureClient(resultModel);
			var result = await _client.PatchAsync<FooBar, FooBar>("http://test.com", payload);
			result.ShouldNotBeNull();
			result.Name.ShouldBe("PatchResult");
		}
	}
}