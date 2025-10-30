using System.Net;

using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace httpbase.Tests;

[TestClass]
public class DeleteAsyncTests
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
	public async Task DeleteAsync_ShouldReturnTrue_WhenSuccessful()
	{
		ConfigureClient(null, HttpStatusCode.NoContent);
		var result = await _client.DeleteAsync("http://test.com/resource/1");
		result.ShouldBeTrue();
	}

	[TestMethod]
	public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
	{
		ConfigureClient(null, HttpStatusCode.NotFound);
		var result = await _client.DeleteAsync("http://test.com/missing");
		result.ShouldBeFalse();
	}

	[TestMethod]
	public async Task DeleteAsync_ShouldReturnFalse_WhenServerError()
	{
		ConfigureClient(null, HttpStatusCode.InternalServerError);
		var result = await _client.DeleteAsync("http://test.com/fail");
		result.ShouldBeFalse();
	}

	[TestMethod]
	public async Task DeleteAsync_ShouldHandleTimeout()
	{
		var httpClient = new HttpClient(new HttpClientHandler()) { Timeout = TimeSpan.FromMilliseconds(1) };
		_factoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
		_client = new GenericCrudClient(_factoryMock.Object, _loggerMock.Object, "TestClient");

		await Should.ThrowAsync<TaskCanceledException>(() =>
			_client.DeleteAsync("http://test.com/timeout"));
	}

	[TestMethod]
	public async Task DeleteAsync_ShouldReturnFalse_WhenResponseIsNull()
	{
		// Simulate a 204 No Content with no body
		ConfigureClient(null, HttpStatusCode.NoContent);
		var result = await _client.DeleteAsync("http://test.com/empty");
		result.ShouldBeTrue(); // Still true because 204 is a success status
	}
}