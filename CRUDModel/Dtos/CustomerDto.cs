namespace CRUDModel.Dtos
{
    public class CustomerListDto { 
        public List<CustomerDto> customer { get; set; }
    }

    public class CustomerDto
    {
        public string CustId { get; set; }

        public string CustName { get; set; }

        public string CustSex { get; set; }

        public string CustBirthday { get; set; }

        public string CustPhone { get; set; }

    }
}
