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

        /// <summary>
        /// 查詢診所客戶
        /// </summary>
        /// <param name="customerPara"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(CustomerListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getCustomer([FromQuery] CustomerParameter.getCustomerPara customerPara)
        {
            try
            {
                List<CustomerDto> customerDtos = new List<CustomerDto>();
                dynamic dynamic = _iCustomerService.GetCustomer(customerPara,customerPara.Page,customerPara.PageSize);
                foreach(var row in dynamic)
                {
                    //Console.WriteLine(row.birthday+","+row.no);

                    CustomerDto customer = new CustomerDto() {
                        CustId = row.no,
                        CustName = row.name,
                        CustSex = (row.sex == "男" ? "M" : "F"),
                        CustPhone = row.phoneno,
                        CustBirthday =(Convert.ToString(Convert.ToInt64(row.birthday.Substring(0, 3)) + 1911) + "-" + row.birthday.Substring(3, 2) + "-" + row.birthday.Substring(5, 2))
                    };
                    customerDtos.Add(customer);
                }
                CustomerListDto customerListDto = new CustomerListDto()
                {
                    customer = customerDtos
                };
                return Ok(customerListDto);
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 單筆資料在AI_Cust 資料表建檔
        /// </summary>
        /// <param name="returnToAiCustPara"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> singleReturnCustomerToAi([FromForm] CustomerParameter.returnToAiCustPara returnToAiCustPara)
        {
            try
            {
                var result =  await _iCustomerService.singleReturnCustomerToAI(returnToAiCustPara);
                return Ok(result);
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 多筆資料序列匯入AI_Cust
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> multipleReturnCustomerToAi()
        {
            try
            {
                var result = await _iCustomerService.multipleReturnCustomerToAI();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
