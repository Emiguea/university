using System.Net.Http.Json;

namespace UniversityAdmissionSystem.Web.Services;

public class StatisticsService
{
    private readonly HttpClient _httpClient;

    public StatisticsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<StatisticsSummary?> GetSummaryAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<StatisticsSummary>("api/statistics/summary");
        }
        catch
        {
            return null;
        }
    }

    public async Task<int> GetTotalAdmittedStudentsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<int>("api/statistics/totaladmitted");
        }
        catch
        {
            return 0;
        }
    }

    public async Task<int> GetTotalRegisteredStudentsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<int>("api/statistics/totalregistered");
        }
        catch
        {
            return 0;
        }
    }

    public async Task<decimal> GetRegistrationRateAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<decimal>("api/statistics/registrationrate");
        }
        catch
        {
            return 0;
        }
    }

    public async Task<int> GetTotalPaidStudentsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<int>("api/statistics/totalpaid");
        }
        catch
        {
            return 0;
        }
    }

    public async Task<decimal> GetPaymentRateAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<decimal>("api/statistics/paymentrate");
        }
        catch
        {
            return 0;
        }
    }

    public async Task<decimal> GetTotalPaymentAmountAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<decimal>("api/statistics/totalpaymentamount");
        }
        catch
        {
            return 0;
        }
    }

    public async Task<Dictionary<string, int>?> GetAdmissionsByDepartmentAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Dictionary<string, int>>("api/statistics/admissionsbydepartment");
        }
        catch
        {
            return null;
        }
    }
}

public class StatisticsSummary
{
    public int TotalAdmittedStudents { get; set; }
    public int TotalRegisteredStudents { get; set; }
    public decimal RegistrationRate { get; set; }
    public int TotalPaidStudents { get; set; }
    public decimal PaymentRate { get; set; }
    public decimal TotalPaymentAmount { get; set; }
}