using CRUDModel.Dtos;
using CRUDModel.Parameters;
using CRUDModel.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUDModel.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _iCustomerService;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _iCustomerService = customerService;
        }


    }
}
