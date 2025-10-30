namespace httpbase
{
	/// <summary>Interface for GET operations.</summary>
	public interface ICrudGet
	{
		Task<T> GetAsync<T>(string endpoint);
	}

	/// <summary>Interface for POST operations.</summary>
	public interface ICrudPost
	{
		Task<T> PostAsync<T>(string endpoint, T payload);

		Task<TResult> PostAsync<TPayload, TResult>(string endpoint, TPayload payload);
	}

	/// <summary>Interface for PUT operations.</summary>
	public interface ICrudPut
	{
		Task<T> PutAsync<T>(string endpoint, T payload);

		Task<TResult> PutAsync<TPayload, TResult>(string endpoint, TPayload payload);
	}

	/// <summary>Interface for PATCH operations.</summary>
	public interface ICrudPatch
	{
		Task<T> PatchAsync<T>(string endpoint, T payload);

		Task<TResult> PatchAsync<TPayload, TResult>(string endpoint, TPayload payload);
	}

	/// <summary>Interface for DELETE operations.</summary>
	public interface ICrudDelete
	{
		Task<bool> DeleteAsync(string endpoint);
	}

	/// <summary>Aggregated interface for all CRUD operations.</summary>
	public interface ICrudClient : ICrudGet, ICrudPost, ICrudPut, ICrudPatch, ICrudDelete
	{
		string ClientName { get; set; }
	}
}