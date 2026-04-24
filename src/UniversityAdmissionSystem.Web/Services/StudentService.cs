using System.Net.Http.Json;
using UniversityAdmissionSystem.Web.Models;

namespace UniversityAdmissionSystem.Web.Services;

public class StudentService
{
    private readonly HttpClient _httpClient;

    public StudentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<StudentDto>?> GetAllStudentsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<StudentDto>>("api/students");
        }
        catch
        {
            return null;
        }
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<StudentDto>($"api/students/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<StudentDto?> CreateStudentAsync(StudentDto student)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/students", student);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<StudentDto>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateStudentAsync(int id, StudentDto student)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/students/{id}", student);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/students/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AssignClassAsync(int studentId, int classId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/students/{studentId}/assignclass/{classId}", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AssignDormitoryAsync(int studentId, int dormitoryId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/students/{studentId}/assigndormitory/{dormitoryId}", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> GenerateStudentNumberAsync(int studentId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/students/{studentId}/generatestudentnumber", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}