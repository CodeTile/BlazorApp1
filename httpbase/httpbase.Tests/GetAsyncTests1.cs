using System.Net;

using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace httpbase.Tests
{
	[TestClass]
	public class GetAsyncTests
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
		public async Task GetAsync_ShouldReturnModel_WhenSuccessful()
		{
			var expected = new FooBar { Id = 1, Name = "Test" };
			ConfigureClient(expected);
			var result = await _client.GetAsync<FooBar>("http://test.com");
			result.ShouldNotBeNull();
			result.Name.ShouldBe("Test");
		}

		[TestMethod]
		public async Task GetAsync_ShouldThrow_WhenNotFound()
		{
			ConfigureClient(null, HttpStatusCode.NotFound);
			await Should.ThrowAsync<HttpRequestException>(() => _client.GetAsync<FooBar>("http://test.com/notfound"));
		}

		[TestMethod]
		public async Task GetAsync_ShouldThrow_WhenServerError()
		{
			ConfigureClient(null, HttpStatusCode.InternalServerError);
			await Should.ThrowAsync<HttpRequestException>(() => _client.GetAsync<FooBar>("http://test.com/error"));
		}

		[TestMethod]
		public async Task GetAsync_ShouldHandleTimeout()
		{
			var httpClient = new HttpClient(new HttpClientHandler()) { Timeout = TimeSpan.FromMilliseconds(1) };
			_factoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
			_client = new GenericCrudClient(_factoryMock.Object, _loggerMock.Object, "TestClient");

			await Should.ThrowAsync<TaskCanceledException>(() => _client.GetAsync<FooBar>("http://test.com/timeout"));
		}

		[TestMethod]
		public async Task GetAsync_ShouldReturnNull_WhenResponseIsNull()
		{
			ConfigureClient("null");
			var result = await _client.GetAsync<FooBar>("http://test.com/empty");
			result.ShouldBeNull();
		}
	}
}