using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace DebaduttaGiri_C_Assignment
{
    internal class DataAccess
    {
        public async Task<Response> GetEmployees()
        {
            Response emps = new();
            try
            {
                string url = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";

                using (HttpClient client = new HttpClient())
                {
                    emps.Employees = await client.GetFromJsonAsync<List<EmployeeModel>>(url) ?? new List<EmployeeModel>() ;
                }
                return emps;
            }
            catch (Exception ex)
            {
                emps.Error = ex.Message;
                return emps;
            }
        } 
    }

    internal class Response
    {
        public List<EmployeeModel>? Employees {  get; set; }
        public string? Error {  get; set; }
    }
}
