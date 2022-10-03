using CRUDModel.Dtos;
using CRUDModel.Models;
using CRUDModel.Parameters;
using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Dynamic;
using static CRUDModel.Parameters.CustomerParameter;

namespace CRUDModel.Services
{
    public class CustomerService : ICustomerService
    {
        private Database _database;
        private HttpRequestService _httpRequest;
        public CustomerService(IServiceProvider services)
        {
            _database = services.GetService<Database>();
            _httpRequest = services.GetService<HttpRequestService>();
        }

 
    }
}
