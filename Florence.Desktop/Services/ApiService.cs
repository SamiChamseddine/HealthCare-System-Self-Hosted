using Florence.Desktop.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Florence.Desktop.Services
{
    public class ApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5005/api/")
        };

        public async Task<List<PatientDto>> GetPatientsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PatientDto>>("patients") ?? new List<PatientDto>();
        }

        public async Task<List<InvoiceDto>> GetInvoicesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<InvoiceDto>>("invoices") ?? new List<InvoiceDto>();
        }

        public async Task<List<NurseDto>> GetNursesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<NurseDto>>("nurses") ?? new List<NurseDto>();
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto customer)
        {
            var response = await _httpClient.PostAsJsonAsync("customers", customer);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerDto>() ?? new CustomerDto();
        }

        public async Task<List<CustomerDto>> GetCustomersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CustomerDto>>("customers") ?? new List<CustomerDto>();
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto invoice)
        {
            var response = await _httpClient.PostAsJsonAsync("invoices", invoice);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<InvoiceDto>() ?? new InvoiceDto();
        }

        public async Task DeleteInvoiceAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"invoices/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"customers/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<InvoiceDto>> GetCustomerInvoicesAsync(int customerId)
        {
            return await _httpClient.GetFromJsonAsync<List<InvoiceDto>>($"customers/{customerId}/invoices")
                   ?? new List<InvoiceDto>();
        }

        public async Task<InvoiceDto> UpdateInvoiceAsync(int id, CreateInvoiceDto invoice)
        {
            var response = await _httpClient.PutAsJsonAsync($"invoices/{id}", invoice);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<InvoiceDto>() ?? new InvoiceDto();
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CustomerDto>($"customers/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<InvoiceDto?> MarkInvoicePaidAsync(int id, string paymentMethod = "Cash")
        {
            var response = await _httpClient.PatchAsync($"invoices/{id}/pay?paymentMethod={paymentMethod}", null);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<InvoiceDto>();
        }

        public async Task CheckLateInvoicesAsync()
        {
            var response = await _httpClient.PatchAsync("invoices/check-late", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto patient)
        {
            var response = await _httpClient.PostAsJsonAsync("patients", patient);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PatientDto>() ?? new PatientDto();
        }

        public async Task<PatientDto> UpdatePatientAsync(int id, CreatePatientDto patient)
        {
            var response = await _httpClient.PutAsJsonAsync($"patients/{id}", patient);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PatientDto>() ?? new PatientDto();
        }

        public async Task DeletePatientAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"patients/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<NurseDto> CreateNurseAsync(CreateNurseDto nurse)
        {
            var response = await _httpClient.PostAsJsonAsync("nurses", nurse);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NurseDto>() ?? new NurseDto();
        }

        public async Task<NurseDto> UpdateNurseAsync(int id, CreateNurseDto nurse)
        {
            var response = await _httpClient.PutAsJsonAsync($"nurses/{id}", nurse);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NurseDto>() ?? new NurseDto();
        }

        public async Task DeleteNurseAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"nurses/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<PatientDtoFull?> GetPatientByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PatientDtoFull>($"patients/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<NurseDtoFull?> GetNurseByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<NurseDtoFull>($"nurses/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<CustomerDto> UpdateCustomerAsync(int id, CreateCustomerDto customer)
        {
            var response = await _httpClient.PutAsJsonAsync($"customers/{id}", customer);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerDto>() ?? new CustomerDto();
        }

        public async Task<List<ExpenseReportDto>> GetExpenseReportsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ExpenseReportDto>>("expensereports")
                   ?? new List<ExpenseReportDto>();
        }

        public async Task<ExpenseReportDto?> GetExpenseReportByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ExpenseReportDto>($"expensereports/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<ExpenseReportDto> CreateExpenseReportAsync(CreateExpenseReportDto report)
        {
            var response = await _httpClient.PostAsJsonAsync("expensereports", report);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ExpenseReportDto>() ?? new ExpenseReportDto();
        }

        public async Task<ExpenseReportDto> UpdateExpenseReportAsync(int id, CreateExpenseReportDto report)
        {
            var response = await _httpClient.PutAsJsonAsync($"expensereports/{id}", report);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ExpenseReportDto>() ?? new ExpenseReportDto();
        }

        public async Task DeleteExpenseReportAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"expensereports/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
