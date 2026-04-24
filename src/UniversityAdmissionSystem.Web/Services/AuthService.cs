using System.Net.Http.Json;
using UniversityAdmissionSystem.Web.Models;

namespace UniversityAdmissionSystem.Web.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private UserDto? _currentUser;
    private bool _isAuthenticated;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public UserDto? CurrentUser => _currentUser;
    public bool IsAuthenticated => _isAuthenticated;

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/login", new { username, password });
            
            if (response.IsSuccessStatusCode)
            {
                _currentUser = await response.Content.ReadFromJsonAsync<UserDto>();
                _isAuthenticated = true;
                return true;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }

    public void Logout()
    {
        _currentUser = null;
        _isAuthenticated = false;
    }

    public async Task<bool> RegisterAsync(string username, string password, string fullName)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", new 
            { 
                username, 
                password, 
                fullName,
                gender = "未知",
                isActive = true
            });
            
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}