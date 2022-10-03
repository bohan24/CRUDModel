using System.ComponentModel.DataAnnotations;

namespace CRUDModel.Parameters
{
    public class CustomerParameter
    {
        public class getCustomerPara
        {
            /// <summary>
            /// Customer Name
            /// </summary>
            public string? CustName { get; set; }

            /// <summary>
            /// Customer Birthday
            /// </summary>
            public string? CustBirthday { get; set; }

            /// <summary>
            /// Customer Phone
            /// </summary>
            public string? CustPhone { get; set; }

            /// <summary>
            /// Current Page Index
            /// </summary>
            [Range(1, short.MaxValue)]
            public short Page { get; set; } = 1;

            /// <summary>
            /// Pens per page
            /// </summary>
            [Range(1, short.MaxValue)]
            public short PageSize { get; set; } = 20;
        }

        public class returnToAiCustPara
        {
            /// <summary>
            /// Customer ID
            /// </summary>
            public string CustId { get; set; }
        }

    }
}
