namespace BlazorApp.Interface
{
    public interface ILocalStorageService
    {
        Task SetItemAsync(string key, object value);
        Task<T> GetItemAsync<T>(string key);
        Task RemoveItemAsync(string key);
        Task ClearAsync();
    }
}
