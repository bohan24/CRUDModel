using CRUDModel.Models;
using CRUDModel.Parameters;

namespace CRUDModel.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomer(CustomerParameter.getCustomerPara customerPara,int page,int pageCount);

        Task<dynamic> singleReturnCustomerToAI(CustomerParameter.returnToAiCustPara returnToAiCustPara);

        Task<dynamic> multipleReturnCustomerToAI();
    }
}