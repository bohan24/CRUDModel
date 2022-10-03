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

        public IEnumerable<Customer> GetCustomer(CustomerParameter.getCustomerPara customerPara, int page, int pageSize)
        {
            IEnumerable<Customer> customer = null;
            var birthday = "";
            if (customerPara.CustBirthday != null)
            {
                var birthYear = Convert.ToString(Convert.ToDateTime(customerPara.CustBirthday).Year - 1911);
                var birthMonth = customerPara.CustBirthday.Substring(5, 2);
                var birthDay = customerPara.CustBirthday.Substring(8, 2);
                birthday = birthYear.PadLeft(3, '0') + birthMonth + birthDay;
            }

            using (var conn = new MySqlConnection(_database.GetDefaultConnectionString()))
            {
                string sql = String.Format(@"select * from Customer where birthday !='' and birthday !='0' and no in (select no from customer where name like @name and birthday like @birthday and phoneno like @phone) and ((phoneno like '09%' and CHAR_LENGTH(phoneno) =10) or phoneno like '+%' or phoneno like '1%') LIMIT {0} OFFSET {1}", pageSize, (page - 1) * pageSize);
                customer = conn.Query<Customer>(sql, new { name = "%" + customerPara.CustName + "%", birthday = "%" + customerPara.CustBirthday == null ? customerPara.CustBirthday : birthday + "%", phone = "%" + customerPara.CustPhone + "%" });
            }
            return customer;
        }

        public async Task<dynamic> singleReturnCustomerToAI(CustomerParameter.returnToAiCustPara returnToAiCustPara)
        {
            try
            {
                Customer customer = null;
                using (var conn = new MySqlConnection(_database.GetDefaultConnectionString()))
                {
                    string sql = String.Format(@"select * from Customer where no = @no");
                    customer = conn.QueryFirstOrDefault<Customer>(sql, new { no=returnToAiCustPara.CustId });
                }
                dynamic auth = new Dictionary<string,string>();
                auth.Add("account", "test");
                auth.Add("password", "1234");
                var authResult = JObject.Parse(await _httpRequest.post("https://localhost:7117/api/User/Login",auth));
                dynamic apiHeaders = new Dictionary<string,string>();
                apiHeaders.Add("Authorization","Bearer "+authResult.token);
                var dataRow = new Dictionary<string,string>();
                dataRow.Add("CustName",customer.name);
                dataRow.Add("CustSex",customer.sex == "男" ? "M" : "F");
                dataRow.Add("CustPhone",customer.phoneno);
                dataRow.Add("CustBirthday",(Convert.ToString(Convert.ToInt64(customer.birthday.Substring(0, 3)) + 1911) + "-" + customer.birthday.Substring(3, 2) + "-" + customer.birthday.Substring(5, 2)));
                _httpRequest.post("https://localhost:7117/api/Customers",dataRow,apiHeaders);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<dynamic> multipleReturnCustomerToAI()
        {
            try
            {
                IEnumerable<Customer> customer = null;
                using (var conn = new MySqlConnection(_database.GetDefaultConnectionString()))
                {
                    string sql = String.Format(@"select * from Customer where birthday !='' and birthday !='0' and ((phoneno like '09%' and CHAR_LENGTH(phoneno) =10) or phoneno like '+%' or phoneno like '1%')");
                    customer = conn.Query<Customer>(sql);
                }
                dynamic auth = new Dictionary<string, string>();
                auth.Add("account", "test");
                auth.Add("password", "1234");
                var authResult = JObject.Parse(await _httpRequest.post("https://localhost:7117/api/User/Login", auth));
                dynamic apiHeaders = new Dictionary<string, string>();
                apiHeaders.Add("Authorization", "Bearer " + authResult.token);
                var dataRow = new Dictionary<string, string>();
                foreach(var row in customer)
                {
                    dataRow.Add("CustName", row.name);
                    dataRow.Add("CustSex",row.sex == "男" ? "M" : "F");
                    dataRow.Add("CustPhone", row.phoneno);
                    dataRow.Add("CustBirthday", (Convert.ToString(Convert.ToInt64(row.birthday.Substring(0, 3)) + 1911) + "-" + row.birthday.Substring(3, 2) + "-" + row.birthday.Substring(5, 2)));
                    await _httpRequest.post("https://localhost:7117/api/Customers", dataRow, apiHeaders);
                    dataRow = new Dictionary<string, string>();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
