using System;

namespace Common.HttpService;

public interface IHttpService
{
    Task<TResponse> GetAsync<TResponse>(string url);
    Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data);
    Task PutAsync<TRequest>(string url, TRequest data);
    Task DeleteAsync(string url);
}
