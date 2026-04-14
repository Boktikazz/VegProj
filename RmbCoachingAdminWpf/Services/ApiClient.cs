using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RmbCoachingAdminWpf.Helpers;
using RmbCoachingAdminWpf.Models;

namespace RmbCoachingAdminWpf.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(AppConfig.BaseUrl)
        };
    }

    public void SetToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var content = CreateJsonContent(request);
        using var response = await _httpClient.PostAsync("api/auth/login", content);
        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(raw, response.StatusCode));
        }

        var result = JsonSerializer.Deserialize<AuthResponse>(raw, _jsonOptions);
        if (result is null)
        {
            throw new InvalidOperationException("A bejelentkezési válasz nem feldolgozható.");
        }

        return result;
    }

    public async Task<List<CourseDto>> GetAdminCoursesAsync()
    {
        using var response = await _httpClient.GetAsync("api/admin/courses");
        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(raw, response.StatusCode));
        }

        var result = JsonSerializer.Deserialize<List<CourseDto>>(raw, _jsonOptions);
        return result ?? new List<CourseDto>();
    }

    public async Task<CourseDto> CreateCourseAsync(CourseUpsertRequest request)
    {
        using var response = await _httpClient.PostAsync("api/admin/courses", CreateJsonContent(request));
        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(raw, response.StatusCode));
        }

        var result = JsonSerializer.Deserialize<CourseDto>(raw, _jsonOptions);
        return result ?? throw new InvalidOperationException("A létrehozott kurzus válasza nem feldolgozható.");
    }

    public async Task<CourseDto> UpdateCourseAsync(int id, CourseUpsertRequest request)
    {
        using var response = await _httpClient.PutAsync($"api/admin/courses/{id}", CreateJsonContent(request));
        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(raw, response.StatusCode));
        }

        var result = JsonSerializer.Deserialize<CourseDto>(raw, _jsonOptions);
        return result ?? throw new InvalidOperationException("A módosított kurzus válasza nem feldolgozható.");
    }

    public async Task DeleteCourseAsync(int id)
    {
        using var response = await _httpClient.DeleteAsync($"api/admin/courses/{id}");
        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(raw, response.StatusCode));
        }
    }

    private static StringContent CreateJsonContent<T>(T value)
    {
        var json = JsonSerializer.Serialize(value);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private string ExtractErrorMessage(string raw, HttpStatusCode statusCode)
    {
        try
        {
            var error = JsonSerializer.Deserialize<ApiError>(raw, _jsonOptions);
            if (!string.IsNullOrWhiteSpace(error?.Message))
            {
                return error.Message;
            }
        }
        catch
        {
        }

        return statusCode switch
        {
            HttpStatusCode.Forbidden => "Nincs admin jogosultságod ehhez a művelethez.",
            HttpStatusCode.Unauthorized => "Lejárt vagy hibás token. Jelentkezz be újra.",
            _ => $"Szerverhiba: {(int)statusCode}"
        };
    }

    private class ApiError
    {
        public string? Message { get; set; }
    }
}
