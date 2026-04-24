using System.Net.Http.Json;
using UniversityAdmissionSystem.Web.Models;

namespace UniversityAdmissionSystem.Web.Services;

public class AdmissionService
{
    private readonly HttpClient _httpClient;

    public AdmissionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AdmissionDto>?> GetAllAdmissionsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<AdmissionDto>>("api/admissions");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<AdmissionDto>?> GetPublishedAdmissionsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<AdmissionDto>>("api/admissions/published");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<AdmissionDto>?> GetUnpublishedAdmissionsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<AdmissionDto>>("api/admissions/unpublished");
        }
        catch
        {
            return null;
        }
    }

    public async Task<AdmissionDto?> GetAdmissionByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<AdmissionDto>($"api/admissions/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<AdmissionDto?> CreateAdmissionAsync(AdmissionDto admission)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/admissions", admission);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AdmissionDto>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateAdmissionAsync(int id, AdmissionDto admission)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/admissions/{id}", admission);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAdmissionAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/admissions/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> PublishAdmissionAsync(int admissionId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/admissions/{admissionId}/publish", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> PrintAdmissionNoticeAsync(int admissionId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/admissions/{admissionId}/printnotice", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> PrintMailingLabelAsync(int admissionId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/admissions/{admissionId}/printmailinglabel", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> GenerateAdmissionNumberAsync(int admissionId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/admissions/{admissionId}/generateadmissionnumber", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}