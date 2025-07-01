using BlazorApp.Interface;
using Microsoft.JSInterop;
using System.Text.Json;
namespace BlazorApp.Services
{
    public class LocalStorageService:ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetItemAsync(string key, object value)
        {
            var serialized = JsonSerializer.Serialize(value);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, serialized);
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            var serialized = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (string.IsNullOrEmpty(serialized))
                return default;

            return JsonSerializer.Deserialize<T>(serialized);
        }

        public async Task RemoveItemAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async Task ClearAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.clear");

        }
    }
}
